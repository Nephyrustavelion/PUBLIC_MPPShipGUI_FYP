import { Parameters } from "./Parameters";
import { defaultForm, fields } from "./fields";
import { legacyFloat, roundEven } from "./numeric";
import { sampleA, sampleB, sampleResistance } from "./samples";
import type {
  AssessmentState,
  CalculationSection,
  InputKey,
  WaveRow,
} from "./types";

export function emptyWaveRows(): WaveRow[] {
  return Array.from({ length: 18 }, (_, i) => ({
    period: 7 + i * 0.5,
    added: null,
    total: null,
    thrust: null,
    calm: null,
    air: null,
    appendage: null,
  }));
}
export function initialAssessment(): AssessmentState {
  const parameters = Object.assign(new Parameters(), {
    RowAir: 1.225,
    RowH2o: 1025.8,
    Temp: 15,
    ViscH2o: 1.184,
    g: 9.807,
    RReff: 1,
    chartXmax: 1.1,
    chartYmax: 0.9,
  });
  return {
    form: defaultForm(),
    parameters,
    comments: {},
    completed: {},
    waveRows: emptyWaveRows(),
    propellerPoints: [],
    dirty: false,
    dirtySections: [],
    revision: 0,
    errors: [],
    message: "",
  };
}
export function cloneAssessment(state: AssessmentState): AssessmentState {
  return {
    ...state,
    form: { ...state.form },
    comments: { ...state.comments },
    parameters: Object.assign(new Parameters(), state.parameters),
    completed: { ...state.completed },
    dirtySections: [...state.dirtySections],
    waveRows: state.waveRows.map((r) => ({ ...r })),
    propellerPoints: [...state.propellerPoints],
    errors: [],
  };
}
export function updateField(
  state: AssessmentState,
  key: InputKey,
  value: string,
): AssessmentState {
  const next = cloneAssessment(state);
  next.form[key] = value;
  const owner = (Object.keys(fields) as CalculationSection[]).find((section) =>
    fields[section].some((field) => field.key === key),
  );
  const affected: Record<CalculationSection, CalculationSection[]> = {
    vessel: ["vessel"],
    level1:
      key === "MCR"
        ? ["level1"]
        : ["level1", "geometry", "speed", "environment", "waves", "propeller"],
    geometry: ["geometry", "speed", "environment", "waves", "propeller"],
    speed: ["speed", "environment", "waves", "propeller"],
    environment: ["environment", "waves", "propeller"],
    waves: ["waves", "propeller"],
    propeller: ["propeller"],
  };
  if (owner && !["chartXmax", "chartYmax"].includes(key)) {
    next.dirtySections = [
      ...new Set([...next.dirtySections, ...affected[owner]]),
    ];
  }
  next.dirty = next.dirtySections.length > 0;
  next.message = "";
  return next;
}
function bind(state: AssessmentState, keys: InputKey[], section: string) {
  const missing = keys.filter(
    (key) =>
      state.form[key].trim() === "" ||
      !Number.isFinite(Number(state.form[key])),
  );
  if (missing.length)
    throw new Error(
      `${section}: enter valid values for ${missing.join(", ")}.`,
    );
  for (const key of keys) {
    // Mirrors the Windows Forms float.Parse input path; the formula class retains double operations.
    (state.parameters as unknown as Record<string, number>)[key] = legacyFloat(
      state.form[key],
    );
  }
}
function execute(state: AssessmentState, section: CalculationSection): void {
  const p = state.parameters;
  if (section === "vessel") {
    for (const field of fields.vessel)
      (p as unknown as Record<string, string>)[field.key] =
        state.form[field.key] || "nil";
  }
  if (section === "level1") {
    bind(state, ["DWT", "MCR"], "Minimum power line");
    if (!state.form.ST)
      throw new Error("Minimum power line: select a ship type.");
    p.ST = state.form.ST;
    p.Eqn_MPP();
    p.Result =
      p.MCR < p.MPP || p.DWT < 20000 ? "UNSATISFACTORY" : "SATISFACTORY";
  }
  if (section === "geometry") {
    bind(state, ["BWL", "Tm", "Lpp", "AR"], "Hull & rudder");
    p.Eqn_ALScor();
    p.Eqn_PerALS();
    p.Eqn_HsVw();
    state.form.Bmax = state.form.BWL;
  }
  if (section === "speed") {
    execute(state, "geometry");
    bind(state, ["FWA", "LWA", "Vnav", "NumEng", "BlockC"], "Speed of advance");
    p.Eqn_RatioFL();
    p.Eqn_Vckref();
    p.Eqn_Vck();
    p.Eqn_Vs();
  }
  if (section === "environment") {
    execute(state, "speed");
    bind(
      state,
      ["RowAir", "ViscH2o", "RowH2o", "S", "Rapp"],
      "Environment & resistance",
    );
    p.Temp = Number(state.form.Temp);
    if (state.form.K.trim() === "" || state.form.K.toLowerCase() === "nil") {
      p.Eqn_K();
      state.form.K = String(roundEven(p.K, 5));
    } else bind(state, ["K"], "Form factor");
    p.Eqn_Rey();
    p.Eqn_HsVw();
    p.Eqn_Cf();
    p.Eqn_Rcw();
    p.Eqn_Rair();
    p.Eqn_Rsubtotal();
  }
  if (section === "waves") {
    execute(state, "environment");
    bind(state, ["g"], "Wave resistance");
    p.Eqn_Fr();
    p.Eqn_HsVw();
  }
  if (section === "propeller") {
    bind(
      state,
      [
        "Dp",
        "RReff",
        "J_limit",
        "J_interval",
        "KTa",
        "KTb",
        "KTc",
        "KQa",
        "KQb",
        "KQc",
      ],
      "Propeller analysis",
    );
    if (!state.form.Layout)
      throw new Error("Propeller analysis: select an engine layout.");
    if (p.J_interval <= 0 || p.J_limit <= 0 || p.J_limit / p.J_interval > 10000)
      throw new Error(
        "Use a positive J interval and limit, with at most 10,000 rows.",
      );
    p.Layout = state.form.Layout;
    p.PropellerType = state.form.PropellerType;
    p.NumBlades = state.form.NumBlades;
    // The source does not re-bind RPM in RunInput6. Preserve its sample-seeded engine value.
    p.Eqn_Fr();
    p.Eqn_HsVw();
    state.propellerPoints = [];
    for (let i = 0; i < Math.trunc(p.J_limit / p.J_interval); i++) {
      const J = roundEven(i * p.J_interval, 2);
      const KT = p.Eqn_KT(J);
      const KQ = p.Eqn_KQ(J);
      const efficiency = p.Eqn_eta0(J);
      // Preserve the original empirical resistance path and seven-second period.
      const ship = p.Eqn_KTship0(p.Eqn_Rtotal(7), J);
      state.propellerPoints.push({ J, KT, KQ, efficiency, ship });
      if (i > 2 && efficiency < 0) break;
    }
    p.Eqn_Wake();
    p.Eqn_TDF();
    p.Eqn_Ua();
    p.Eqn_TransEff();
    p.J_at_KTintercept = p.Eqn_SolveQuad(p.KTa - p.c7, p.KTb, p.KTc);
    // These are the displayed coordinates in Run6, rather than a new shaft-speed formula.
    p.KTship0 = p.Eqn_KT(p.J_at_KTintercept);
    p.eta0 = p.Eqn_eta0(p.J_at_KTintercept);
  }
  state.completed[section] = true;
  state.dirtySections = state.dirtySections.filter((id) => id !== section);
}
export function runSection(
  state: AssessmentState,
  section: CalculationSection,
): AssessmentState {
  const next = cloneAssessment(state);
  try {
    execute(next, section);
    next.message = `${section === "vessel" ? "Vessel particulars saved" : "Calculation complete"}.`;
    next.revision++;
    next.dirty = next.dirtySections.length > 0;
  } catch (error) {
    next.errors = [
      error instanceof Error ? error.message : "Check the section inputs.",
    ];
    next.message = "";
  }
  return next;
}
export function runAll(state: AssessmentState): AssessmentState {
  let next = cloneAssessment(state);
  // Form1.RunALL calls Run0, Run1, Run6. Keep its action sequence unchanged.
  for (const section of ["vessel", "level1", "propeller"] as const) {
    const result = runSection(next, section);
    next = { ...result, errors: [...next.errors, ...result.errors] };
  }
  if (!next.errors.length) next.message = "Run All complete.";
  return next;
}
export function addWave(state: AssessmentState): AssessmentState {
  const next = runSection(state, "waves");
  if (next.errors.length) return next;
  const index = next.waveRows.findIndex((row) => row.added === null);
  if (index < 0)
    return {
      ...next,
      errors: [
        "All 18 peak-period rows are filled. Remove a row to enter another value.",
      ],
    };
  if (
    next.form.Raw.trim() === "" ||
    !Number.isFinite(Number(next.form.Raw)) ||
    Number(next.form.Raw) === 0
  )
    return {
      ...next,
      errors: ["Enter a non-zero added resistance for the next row."],
    };
  const p = next.parameters;
  const added = legacyFloat(next.form.Raw);
  const total = p.Eqn_Rsubtotal() + added;
  const thrust = total / (1 - p.Eqn_TDF());
  next.waveRows[index] = {
    period: 7 + index * 0.5,
    added: roundEven(added, 2),
    total: roundEven(total),
    thrust: roundEven(thrust),
    calm: p.Rcw,
    air: p.Rair,
    appendage: p.Rapp,
  };
  p.count = index + 1;
  p.Raw = added;
  p.RawHeight = Math.max(p.RawHeight, added);
  next.message = `Added resistance at ${next.waveRows[index].period} s.`;
  return next;
}
export function removeWave(state: AssessmentState): AssessmentState {
  const next = cloneAssessment(state);
  const index = next.waveRows.findLastIndex((row) => row.added !== null);
  if (index < 0)
    return { ...next, errors: ["There are no resistance rows to remove."] };
  next.waveRows[index] = emptyWaveRows()[index];
  next.parameters.count = index;
  next.revision++;
  next.message = "Last resistance row removed.";
  return next;
}
export function loadSample(
  state: AssessmentState,
  sample: "A" | "B",
): AssessmentState {
  let next = cloneAssessment(state);
  Object.assign(next.form, sample === "A" ? sampleA : sampleB);
  const p = next.parameters;
  const data = sample === "A" ? sampleA : sampleB;
  for (const [key, value] of Object.entries(data)) {
    if (typeof (p as unknown as Record<string, unknown>)[key] === "string") {
      (p as unknown as Record<string, string>)[key] = value;
    }
    if (
      typeof (p as unknown as Record<string, unknown>)[key] === "number" &&
      value !== ""
    )
      (p as unknown as Record<string, number>)[key] = Number(value);
  }
  // SAMPLE_A/B set the beam textbox, but never assign Parameters.Bmax.
  p.Bmax = state.parameters.Bmax;
  p.Eqn_K();
  next.form.K = String(roundEven(p.K, 5));
  if (sample === "A") {
    next.waveRows = emptyWaveRows();
    for (let i = 0; i < 18; i++) {
      next.form.Raw = String(roundEven(sampleResistance(7 + i * 0.5), 3));
      next = addWave(next);
    }
    next = runAll(next);
  } else {
    // The original Sample B fills a partial form without calling RunALL.
    p.Eqn_E_para();
    next.form.E_para = String(roundEven(p.E_para, 6));
    next.dirty = true;
    next.dirtySections = [
      "level1",
      "geometry",
      "speed",
      "environment",
      "waves",
      "propeller",
    ];
  }
  next.message =
    sample === "A"
      ? "Sample A loaded and calculated."
      : "Sample B loaded. Run the sections you want to refresh.";
  return next;
}
export function clearSection(
  state: AssessmentState,
  section: CalculationSection,
): AssessmentState {
  const next = cloneAssessment(state);
  const defaults = defaultForm();
  for (const { key } of fields[section]) next.form[key] = defaults[key];
  delete next.completed[section];
  if (section === "waves") {
    next.waveRows = emptyWaveRows();
    next.parameters.count = 0;
    next.parameters.Raw = 0;
    next.parameters.RawHeight = 0;
  }
  if (section === "propeller") next.propellerPoints = [];
  next.dirty = true;
  next.dirtySections = [...new Set([...next.dirtySections, section])];
  next.revision++;
  next.message = "Section cleared.";
  return next;
}
export function entranceAngle(state: AssessmentState): AssessmentState {
  const next = cloneAssessment(state);
  if (!next.form.Bmax || !next.form.Lpp)
    return {
      ...next,
      errors: ["Enter maximum beam and length between perpendiculars first."],
    };
  next.form.E_para = String(
    roundEven((180 / Math.PI) * next.parameters.Eqn_E_para(), 2),
  );
  next.message = "Entrance angle calculated using the saved engine values.";
  return next;
}
