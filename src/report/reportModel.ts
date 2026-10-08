import { fields } from "../domain/fields";
import { formatNumber } from "../domain/numeric";
import type {
  AssessmentState,
  CalculationSection,
  InputKey,
} from "../domain/types";
import type { ChartId } from "../components/Charts/chartConfig";
export interface ReportRow {
  parameter: string;
  value: string;
  unit: string;
  comment: string;
}
export interface ReportSection {
  id: string;
  title: string;
  rows: ReportRow[];
  narrative?: string;
  chart?: ChartId;
  ready: boolean;
}
export function reportSections(state: AssessmentState): ReportSection[] {
  const p = state.parameters;
  const inputRows = (keys: InputKey[]) =>
    keys.map((key) => {
      const f = Object.values(fields)
        .flat()
        .find((f) => f.key === key)!;
      return {
        parameter: f.label,
        value: state.form[key] || "—",
        unit: f.unit ?? "",
        comment: state.comments[key] ?? "",
      };
    });
  const outputRow = (
    key: string,
    label: string,
    value: number | string,
    section: CalculationSection,
    unit = "",
    digits = 3,
  ): ReportRow => ({
    parameter: label,
    value: state.completed[section]
      ? typeof value === "string"
        ? value
        : formatNumber(value, digits)
      : "Not calculated",
    unit,
    comment: state.comments[key] ?? "",
  });
  return [
    {
      id: "ship",
      title: "Ship data",
      rows: inputRows([
        "DNVGL",
        "IMONum",
        "VesselName",
        "ST",
        "BuildingYard",
        "HullNum",
      ]),
      ready: true,
    },
    {
      id: "environmental",
      title: "Environmental factors",
      rows: inputRows(["RowAir", "RowH2o", "Temp", "ViscH2o", "g"]),
      ready: true,
    },
    {
      id: "particulars",
      title: "Principal particulars",
      rows: inputRows([
        "Lpp",
        "BWL",
        "Tm",
        "LSW",
        "DWT",
        "BlockC",
        "S",
        "FWA",
        "LWA",
        "AR",
      ]),
      ready: true,
    },
    {
      id: "engine",
      title: "Main engine particulars",
      rows: inputRows([
        "NumEng",
        "EngineManufacturer",
        "EngineType",
        "MCR",
        "nrpm",
      ]),
      ready: true,
    },
    {
      id: "propeller-data",
      title: "Propeller particulars",
      rows: inputRows(["PropellerType", "NumBlades", "Dp", "Layout", "RReff"]),
      ready: true,
    },
    {
      id: "level1",
      title: "Assessment Level 1 — Minimum power line",
      ready: !!state.completed.level1,
      chart: "power",
      rows: [
        ...inputRows(["DWT", "MCR"]),
        outputRow("a", "Coefficient a", p.a, "level1", "", 4),
        outputRow("b", "Coefficient b", p.b, "level1", "", 1),
        outputRow("MPP", "Minimum propulsion power", p.MPP, "level1", "kW", 0),
        outputRow("Result", "Assessment result", p.Result, "level1"),
      ],
      narrative: state.completed.level1
        ? `MPP = a × DWT + b = ${p.a} × ${p.DWT} + ${p.b} = ${formatNumber(p.MPP, 0)} kW. The assessment is ${p.Result.toLowerCase()}.`
        : "Run Assessment Level 1 to calculate the minimum power line.",
    },
    {
      id: "geometry",
      title: "Assessment Level 2 — Hull & rudder",
      ready: !!state.completed.geometry,
      rows: [
        ...inputRows(["BWL", "Tm", "Lpp", "AR"]),
        outputRow(
          "ALScor",
          "Corrected submerged lateral area",
          p.ALScor,
          "geometry",
          "m²",
          0,
        ),
        outputRow(
          "PerALS",
          "Rudder / corrected lateral area",
          p.PerALS,
          "geometry",
          "%",
        ),
      ],
    },
    {
      id: "speed",
      title: "Required speed of advance",
      ready: !!state.completed.speed,
      rows: [
        ...inputRows(["FWA", "LWA", "Vnav"]),
        outputRow(
          "RatioFL",
          "Frontal / lateral windage ratio",
          p.RatioFL,
          "speed",
        ),
        outputRow(
          "Vckref",
          "Reference course keeping speed",
          p.Vckref,
          "speed",
          "knots",
        ),
        outputRow(
          "Vck",
          "Minimum course keeping speed",
          p.Vck,
          "speed",
          "knots",
        ),
        outputRow("Vs", "Required advance speed", p.Vs, "speed", "m/s"),
      ],
    },
    {
      id: "resistance",
      title: "Procedure of assessment of installed power",
      ready: !!state.completed.environment,
      chart: "subtotal",
      rows: [
        ...inputRows([
          "NumEng",
          "BlockC",
          "Temp",
          "RowAir",
          "ViscH2o",
          "RowH2o",
          "S",
          "K",
          "Rapp",
        ]),
        outputRow(
          "Rey",
          "Reynolds number",
          p.Rey / 1e5,
          "environment",
          "×10⁵",
          5,
        ),
        outputRow("Vw", "Mean wind speed", p.Vw, "environment", "m/s", 2),
        outputRow(
          "Cf",
          "Frictional resistance coefficient",
          p.Cf,
          "environment",
          "",
          6,
        ),
        outputRow("Rcw", "Calm-water resistance", p.Rcw, "environment", "kN"),
        outputRow(
          "Cair",
          "Aerodynamic resistance coefficient",
          p.Cair,
          "environment",
        ),
        outputRow(
          "Rair",
          "Aerodynamic resistance",
          p.Rair,
          "environment",
          "kN",
        ),
        outputRow(
          "Rsubtotal",
          "Resistance subtotal",
          p.Rsubtotal,
          "environment",
          "kN",
          0,
        ),
      ],
    },
    {
      id: "waves",
      title: "Total resistance and thrust",
      ready: !!state.completed.waves,
      chart: "waves",
      rows: [
        ...inputRows(["g"]),
        outputRow("Fr", "Froude number", p.Fr, "waves", "", 6),
        outputRow("Hs", "Significant wave height", p.Hs, "waves", "m"),
        outputRow("TDF", "Thrust deduction factor", p.TDF, "waves"),
      ],
      narrative:
        "The resistance table uses manually entered wave resistance at each peak period.",
    },
    {
      id: "curves",
      title: "Propeller analysis",
      ready: !!state.completed.propeller,
      chart: "propeller",
      rows: [
        ...inputRows([
          "Dp",
          "Layout",
          "RReff",
          "nrpm",
          "KTa",
          "KTb",
          "KTc",
          "KQa",
          "KQb",
          "KQc",
        ]),
        outputRow("Wake", "Wake fraction", p.Wake, "propeller"),
        outputRow("TDF", "Thrust deduction factor", p.TDF, "propeller"),
        outputRow("Ua", "Propeller advance speed", p.Ua, "propeller", "m/s", 2),
        outputRow(
          "TransEff",
          "Transmission efficiency",
          p.TransEff,
          "propeller",
        ),
        outputRow(
          "intercept",
          "KT intersection (J, KT)",
          `${formatNumber(p.J_at_KTintercept)}, ${formatNumber(p.KTship0)}`,
          "propeller",
        ),
        outputRow(
          "optimal",
          "Optimal shaft speed coordinates",
          `${formatNumber(p.J_at_KTintercept)}, ${formatNumber(p.eta0)}`,
          "propeller",
        ),
      ],
      narrative: state.propellerPoints.some((r) => !Number.isFinite(r.ship))
        ? "The retained legacy wave model produces non-finite KTship and operating-point values for this assessment. These are shown as NaN, matching the source behavior."
        : undefined,
    },
  ];
}
