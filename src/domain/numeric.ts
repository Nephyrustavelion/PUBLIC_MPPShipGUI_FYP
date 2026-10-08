/** .NET Math.Round uses round-to-even at midpoint; JS Math.round does not. */
export function roundEven(value: number, digits = 0): number {
  if (!Number.isFinite(value)) return value;
  const scale = 10 ** digits;
  const scaled = value * scale;
  const floor = Math.floor(scaled);
  const fraction = scaled - floor;
  return (
    (fraction === 0.5
      ? floor % 2 === 0
        ? floor
        : floor + 1
      : Math.round(scaled)) / scale
  );
}
export function formatNumber(
  value: number | null | undefined,
  digits = 3,
): string {
  if (value === null || value === undefined) return "—";
  if (!Number.isFinite(value))
    return Number.isNaN(value) ? "NaN" : String(value);
  return roundEven(value, digits).toLocaleString("en-GB", {
    maximumFractionDigits: digits,
  });
}
export function legacyFloat(value: string): number {
  return Math.fround(Number(value));
}
