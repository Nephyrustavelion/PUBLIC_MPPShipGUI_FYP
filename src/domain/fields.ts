import type { CalculationSection, FieldDefinition, FormValues } from "./types";

export const fields: Record<CalculationSection, FieldDefinition[]> = {
  vessel: [
    {
      key: "VesselName",
      label: "Vessel name",
      type: "text",
      placeholder: "Enter vessel name",
    },
    {
      key: "IMONum",
      label: "IMO number",
      type: "text",
      placeholder: "e.g. 9123123",
    },
    {
      key: "DNVGL",
      label: "DNVGL reference",
      type: "text",
      placeholder: "Optional reference",
    },
    { key: "BuildingYard", label: "Building yard", type: "text" },
    { key: "HullNum", label: "Hull number", type: "text" },
    { key: "LSW", label: "Light ship weight", type: "text", unit: "tonnes" },
    { key: "EngineManufacturer", label: "Engine manufacturer", type: "text" },
    { key: "EngineType", label: "Engine type", type: "text" },
  ],
  level1: [
    {
      key: "ST",
      label: "Ship type",
      type: "select",
      options: ["Bulk Carrier", "Tanker/Combination Carrier"],
    },
    { key: "DWT", label: "Deadweight tonnage", symbol: "DWT", unit: "tonnes" },
    {
      key: "MCR",
      label: "Maximum continuous rating",
      symbol: "MCR",
      unit: "kW",
    },
  ],
  geometry: [
    {
      key: "Lpp",
      label: "Length between perpendiculars",
      symbol: "LPP",
      unit: "m",
    },
    { key: "BWL", label: "Breadth on water line", symbol: "BWL", unit: "m" },
    { key: "Tm", label: "Scantling draft at midship", symbol: "Tm", unit: "m" },
    { key: "AR", label: "Total rudder area", symbol: "AR", unit: "m²" },
  ],
  speed: [
    { key: "FWA", label: "Frontal windage area", symbol: "FWA", unit: "m²" },
    { key: "LWA", label: "Lateral windage area", symbol: "LWA", unit: "m²" },
    {
      key: "Vnav",
      label: "Minimum navigation speed",
      symbol: "Vnav",
      unit: "knots",
    },
    {
      key: "NumEng",
      label: "Number of engines",
      type: "select",
      options: ["1", "2"],
    },
    { key: "BlockC", label: "Block coefficient", symbol: "Cb", unit: "—" },
  ],
  environment: [
    { key: "Temp", label: "Temperature", unit: "°C" },
    { key: "RowAir", label: "Density of air", symbol: "ρair", unit: "kg/m³" },
    {
      key: "RowH2o",
      label: "Density of water",
      symbol: "ρwater",
      unit: "kg/m³",
    },
    {
      key: "ViscH2o",
      label: "Kinematic viscosity",
      symbol: "ν",
      unit: "×10⁻⁶ m²/s",
    },
    { key: "S", label: "Wetted surface area", symbol: "S", unit: "m²" },
    {
      key: "K",
      label: "Form factor",
      symbol: "k",
      placeholder: "Auto",
      hint: "Leave blank to calculate the empirical form factor.",
    },
    { key: "Rapp", label: "Appendage resistance", symbol: "Rapp", unit: "kN" },
  ],
  waves: [
    { key: "g", label: "Gravity", symbol: "g", unit: "m/s²" },
    {
      key: "Raw",
      label: "Added resistance for next row",
      symbol: "Raw",
      unit: "kN",
    },
    { key: "Bmax", label: "Maximum beam", symbol: "Bmax", unit: "m" },
    {
      key: "TA",
      label: "Draft at fore perpendicular",
      symbol: "TA",
      unit: "m",
    },
    { key: "TF", label: "Draft at aft perpendicular", symbol: "TF", unit: "m" },
    { key: "WaveAmp", label: "Incident wave amplitude", unit: "m" },
    {
      key: "kyy",
      label: "Pitch radius coefficient",
      symbol: "kyy",
      placeholder: "e.g. 0.25",
    },
    {
      key: "Iyy",
      label: "Pitch moment of inertia",
      symbol: "Iyy",
      placeholder: "Optional when kyy is known",
    },
    {
      key: "E_para",
      label: "Waterline entrance angle",
      unit: "degrees",
      placeholder: "Calculate using the button",
    },
  ],
  propeller: [
    {
      key: "PropellerType",
      label: "Propeller type",
      type: "text",
      placeholder: "e.g. FPP",
    },
    { key: "NumBlades", label: "Number of blades", type: "text" },
    { key: "Dp", label: "Propeller diameter", symbol: "Dp", unit: "m" },
    {
      key: "Layout",
      label: "Engine layout",
      type: "select",
      options: ["aft engine", "midship"],
    },
    { key: "RReff", label: "Relative rotative efficiency", symbol: "ηR" },
    { key: "nrpm", label: "Engine speed at MCR", unit: "rev/min" },
    { key: "J_limit", label: "Advance coefficient limit", symbol: "J max" },
    {
      key: "J_interval",
      label: "Advance coefficient interval",
      symbol: "ΔJ",
      type: "select",
      options: ["0.01", "0.025", "0.05", "0.1", "0.2"],
    },
    { key: "KTa", label: "Thrust coefficient a", symbol: "KT a" },
    { key: "KTb", label: "Thrust coefficient b", symbol: "KT b" },
    { key: "KTc", label: "Thrust coefficient c", symbol: "KT c" },
    { key: "KQa", label: "Torque coefficient a", symbol: "10KQ a" },
    { key: "KQb", label: "Torque coefficient b", symbol: "10KQ b" },
    { key: "KQc", label: "Torque coefficient c", symbol: "10KQ c" },
    { key: "chartXmax", label: "Chart X maximum", symbol: "J" },
    { key: "chartYmax", label: "Chart Y maximum" },
  ],
};
export const allFields = Object.values(fields).flat();
export function defaultForm(): FormValues {
  const form = Object.fromEntries(
    allFields.map((f) => [f.key, ""]),
  ) as FormValues;
  Object.assign(form, {
    RowAir: "1.225",
    RowH2o: "1025.8",
    Temp: "15",
    ViscH2o: "1.184",
    g: "9.807",
    Rapp: "0",
    RReff: "1",
    chartXmax: "1.1",
    chartYmax: "0.9",
  });
  return form;
}
