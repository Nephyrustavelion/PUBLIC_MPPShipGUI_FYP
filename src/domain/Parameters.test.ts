import { describe, expect, it } from "vitest";
import { Parameters } from "./Parameters";
import fixture from "./__fixtures__/csharp-reference.json";

type Value = number | string | number[];
function assertEquivalent(actual: Value, expected: Value): void {
  if (Array.isArray(expected)) {
    expect(actual).toBeInstanceOf(Array);
    expected.forEach((v, i) => assertEquivalent((actual as number[])[i], v));
    return;
  }
  if (typeof expected === "string") {
    expect(String(actual)).toBe(expected);
    return;
  }
  expect(typeof actual).toBe("number");
  expect(Math.abs(Number(actual) - expected)).toBeLessThanOrEqual(
    Math.max(1, Math.abs(expected)) * 1e-11,
  );
}
describe("Provided C# formula reference", () => {
  for (const testCase of fixture.cases) {
    it(testCase.name, () => {
      const engine = Object.assign(new Parameters(), testCase.input);
      let actual: Value = 0;
      for (const call of testCase.calls) {
        const method = (
          engine as unknown as Record<string, (...args: number[]) => Value>
        )[call.method];
        actual = method.apply(engine, call.args);
      }
      assertEquivalent(actual, testCase.expected);
    });
  }
  it("retains the legacy wake state across successive calls", () => {
    const p = new Parameters();
    p.NumEng = 1;
    p.BlockC = 0.55;
    expect(p.Eqn_Wake()).toBe(0);
    p.BlockC = 0.8;
    expect(p.Eqn_Wake()).toBe(0.35);
    p.BlockC = 0.55;
    expect(p.Eqn_Wake()).toBe(0.35);
  });
});
