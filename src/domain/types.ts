import type { Parameters } from "./Parameters";

export type InputKey =
  | "DNVGL"
  | "IMONum"
  | "VesselName"
  | "BuildingYard"
  | "HullNum"
  | "LSW"
  | "EngineManufacturer"
  | "EngineType"
  | "DWT"
  | "MCR"
  | "ST"
  | "BWL"
  | "Tm"
  | "Lpp"
  | "AR"
  | "FWA"
  | "LWA"
  | "Vnav"
  | "NumEng"
  | "BlockC"
  | "Temp"
  | "RowAir"
  | "ViscH2o"
  | "RowH2o"
  | "S"
  | "K"
  | "Rapp"
  | "g"
  | "Raw"
  | "Bmax"
  | "TA"
  | "TF"
  | "WaveAmp"
  | "kyy"
  | "Iyy"
  | "E_para"
  | "PropellerType"
  | "NumBlades"
  | "Dp"
  | "Layout"
  | "RReff"
  | "nrpm"
  | "J_limit"
  | "J_interval"
  | "KTa"
  | "KTb"
  | "KTc"
  | "KQa"
  | "KQb"
  | "KQc"
  | "chartXmax"
  | "chartYmax";
export type FormValues = Record<InputKey, string>;
export type CalculationSection =
  | "vessel"
  | "level1"
  | "geometry"
  | "speed"
  | "environment"
  | "waves"
  | "propeller";
export interface FieldDefinition {
  key: InputKey;
  label: string;
  symbol?: string;
  unit?: string;
  type?: "text" | "number" | "select";
  options?: string[];
  placeholder?: string;
  hint?: string;
}
export interface WaveRow {
  period: number;
  added: number | null;
  total: number | null;
  thrust: number | null;
  calm: number | null;
  air: number | null;
  appendage: number | null;
}
export interface PropellerPoint {
  J: number;
  KT: number;
  KQ: number;
  efficiency: number;
  ship: number;
}
export interface AssessmentState {
  form: FormValues;
  comments: Record<string, string>;
  parameters: Parameters;
  completed: Partial<Record<CalculationSection, boolean>>;
  waveRows: WaveRow[];
  propellerPoints: PropellerPoint[];
  dirty: boolean;
  dirtySections: CalculationSection[];
  revision: number;
  message: string;
  errors: string[];
}
