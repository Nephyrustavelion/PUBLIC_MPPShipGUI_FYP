import { describe, expect, it } from "vitest";
import {
  addWave,
  clearSection,
  initialAssessment,
  loadSample,
  removeWave,
  runAll,
  runSection,
  updateField,
} from "./assessment";
import { sampleAForm } from "./samples";
import { roundEven } from "./numeric";
import { reportSections } from "../report/reportModel";

describe("Legacy assessment workflows", () => {
  it("loads Sample A, calculates Level 1, and fills all 18 wave periods", () => {
    const state = loadSample(initialAssessment(), "A");
    expect(state.errors).toEqual([]);
    expect(state.parameters.MPP).toBeCloseTo(9632.8075, 8);
    expect(state.parameters.Result).toBe("SATISFACTORY");
    expect(state.waveRows.every((r) => r.added !== null)).toBe(true);
    expect(state.waveRows[0].added).toBe(774.46);
    expect(state.waveRows[17].added).toBe(569.71);
    expect(state.waveRows[0].total).toBe(1037);
    expect(state.waveRows[0].thrust).toBe(1373);
    expect(state.parameters.Bmax).toBe(0);
    expect(Number.isNaN(state.parameters.J_at_KTintercept)).toBe(true);
    expect(state.propellerPoints[0].KT).toBeCloseTo(0.477629, 6);
  });
  it("keeps Sample B partial, matching its original action", () => {
    const state = loadSample(initialAssessment(), "B");
    expect(state.form.DWT).toBe("208001");
    expect(state.form.Dp).toBe("8.4");
    expect(state.form.KTa).toBe("");
    expect(state.completed.level1).toBeUndefined();
    expect(state.dirty).toBe(true);
  });
  it("uses the existing vessel and coefficient fields when loading B after A", () => {
    const a = loadSample(initialAssessment(), "A");
    const b = loadSample(a, "B");
    expect(b.form.VesselName).toBe(a.form.VesselName);
    expect(b.form.KTa).toBe(a.form.KTa);
    const run = runSection(b, "level1");
    expect(run.parameters.MPP).toBeCloseTo(17521.049, 6);
    expect(run.parameters.Result).toBe("UNSATISFACTORY");
  });
  it("adds/removes the last wave row without changing prior rows", () => {
    const original = loadSample(initialAssessment(), "A");
    const removed = removeWave(original);
    expect(original.waveRows[17].added).toBe(569.71);
    expect(removed.waveRows[17].added).toBeNull();
    removed.form.Raw = "700";
    const added = addWave(removed);
    expect(added.waveRows[17].added).toBe(700);
    expect(added.waveRows[16]).toEqual(original.waveRows[16]);
    expect(addWave(added).errors[0]).toContain("18");
  });
  it("runs the original Run0, Run1, Run6 sequence", () => {
    const state = initialAssessment();
    state.form = sampleAForm();
    const next = runAll(state);
    expect(next.completed.vessel).toBe(true);
    expect(next.completed.level1).toBe(true);
    expect(next.completed.propeller).toBe(true);
    expect(next.completed.environment).toBeUndefined();
    expect(state.completed.level1).toBeUndefined();
  });
  it("accepts MCR exactly equal to minimum power", () => {
    const state = initialAssessment();
    state.form = {
      ...state.form,
      ST: "Bulk Carrier",
      DWT: "84000",
      MCR: "9783.5",
    };
    const next = runSection(state, "level1");
    expect(next.parameters.Result).toBe("SATISFACTORY");
  });
  it("keeps the legacy boundary result instead of changing the formula", () => {
    const state = initialAssessment();
    state.form = {
      ...state.form,
      ST: "Bulk Carrier",
      DWT: "145000",
      MCR: "1000",
    };
    const next = runSection(state, "level1");
    expect(next.parameters.MPP).toBe(0);
    expect(next.parameters.Result).toBe("SATISFACTORY");
  });
  it("reports missing inputs and prevents an unbounded chart loop", () => {
    expect(runSection(initialAssessment(), "level1").errors[0]).toContain(
      "DWT",
    );
    const state = loadSample(initialAssessment(), "A");
    state.form.J_interval = "0";
    expect(runSection(state, "propeller").errors[0]).toContain(
      "positive J interval",
    );
  });
  it("preserves comments in report tables and distinguishes uncalculated results", () => {
    const state = initialAssessment();
    state.comments.VesselName = "Owner review";
    const sections = reportSections(state);
    expect(
      sections[0].rows.find((r) => r.parameter === "Vessel name")?.comment,
    ).toBe("Owner review");
    expect(
      sections
        .find((s) => s.id === "level1")
        ?.rows.find((r) => r.parameter === "Minimum propulsion power")?.value,
    ).toBe("Not calculated");
  });
  it("clears wave output and retains the gravity default", () => {
    const state = clearSection(loadSample(initialAssessment(), "A"), "waves");
    expect(state.waveRows.every((r) => r.added === null)).toBe(true);
    expect(state.form.g).toBe("9.807");
  });
  it("uses .NET round-to-even for display midpoints", () => {
    expect(roundEven(2.5)).toBe(2);
    expect(roundEven(3.5)).toBe(4);
    expect(roundEven(-2.5)).toBe(-2);
    expect(roundEven(1.125, 2)).toBe(1.12);
  });
  it("keeps changed calculations marked stale after saving unrelated vessel details", () => {
    const changed = updateField(
      loadSample(initialAssessment(), "A"),
      "MCR",
      "9500",
    );
    const saved = runSection(changed, "vessel");
    expect(saved.dirty).toBe(true);
    expect(saved.parameters.MCR).toBe(9801);
    const calculated = runSection(saved, "level1");
    expect(calculated.dirty).toBe(false);
    expect(calculated.parameters.Result).toBe("UNSATISFACTORY");
  });
  it("retains affected hull calculations as stale after the original partial Run All", () => {
    const changed = updateField(
      loadSample(initialAssessment(), "A"),
      "DWT",
      "84000",
    );
    const partial = runAll(changed);
    expect(partial.dirty).toBe(true);
    expect(partial.dirtySections).toContain("waves");
    expect(runSection(partial, "waves").dirty).toBe(false);
  });
});
