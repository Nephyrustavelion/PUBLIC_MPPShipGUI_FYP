import { Download, FlaskConical, Play, RotateCcw } from "lucide-react";
import "./Toolbar.css";
interface Props {
  vessel: string;
  onRunAll: () => void;
  onClearAll: () => void;
  onSample: (sample: "A" | "B") => void;
  onExport: () => void;
  exporting: boolean;
}
export function Toolbar({
  vessel,
  onRunAll,
  onClearAll,
  onSample,
  onExport,
  exporting,
}: Props) {
  return (
    <header className="assessment-header">
      <h1>Minimum Propulsion Power Assessment</h1>
      <p className="methodology">
        Following MEPC.232(65): 2013 Interim guidelines for determining minimum
        propulsion power to maintain the manoeuvrability of ships in adverse
        conditions.
      </p>
      {vessel && (
        <p className="assessment-vessel">
          Vessel: <strong>{vessel}</strong>
        </p>
      )}
      <div className="toolbar-actions" aria-label="Assessment controls">
        <button className="button button-run-all" onClick={onRunAll}>
          <Play size={15} />
          Run all
        </button>
        <button className="button button-clear" onClick={onClearAll}>
          <RotateCcw size={15} />
          Clear all
        </button>
        <button className="button button-sample" onClick={() => onSample("A")}>
          <FlaskConical size={15} />
          Sample A
        </button>
        <button className="button button-sample" onClick={() => onSample("B")}>
          <FlaskConical size={15} />
          Sample B
        </button>
        <button
          className="button button-export export-button"
          onClick={onExport}
          disabled={exporting}
        >
          <Download size={15} />
          {exporting ? "Preparing PDF…" : "Export PDF"}
        </button>
      </div>
    </header>
  );
}
