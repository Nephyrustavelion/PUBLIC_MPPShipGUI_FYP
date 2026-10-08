import { useState } from "react";
import {
  addWave,
  clearSection,
  entranceAngle,
  initialAssessment,
  loadSample,
  removeWave,
  runAll,
  runSection,
  updateField,
} from "../domain/assessment";
import type { CalculationSection, InputKey } from "../domain/types";

export function useAssessment() {
  const [state, setState] = useState(initialAssessment);
  return {
    state,
    updateField: (key: InputKey, value: string) =>
      setState((s) => updateField(s, key, value)),
    updateComment: (key: string, value: string) =>
      setState((s) => ({ ...s, comments: { ...s.comments, [key]: value } })),
    run: (section: CalculationSection) =>
      setState((s) => runSection(s, section)),
    runAll: () => setState(runAll),
    clear: (section: CalculationSection) =>
      setState((s) => clearSection(s, section)),
    clearAll: () => setState(initialAssessment()),
    loadSample: (sample: "A" | "B") => setState((s) => loadSample(s, sample)),
    addWave: () => setState(addWave),
    removeWave: () => setState(removeWave),
    entranceAngle: () => setState(entranceAngle),
  };
}
