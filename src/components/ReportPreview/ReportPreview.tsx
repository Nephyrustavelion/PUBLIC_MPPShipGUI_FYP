import { Download, FileText } from "lucide-react";
import { reportSections } from "../../report/reportModel";
import { DataTable } from "../DataTable/DataTable";
import { AssessmentChart } from "../Charts/AssessmentChart";
import { formatNumber } from "../../domain/numeric";
import type { AssessmentState } from "../../domain/types";
import "./ReportPreview.css";
interface Props {
  state: AssessmentState;
  onExport: () => void;
  exporting: boolean;
}
export function ReportPreview({ state, onExport, exporting }: Props) {
  return (
    <section className="report-preview assessment-panel">
      <header className="report-preview-header">
        <div>
          <div className="flex items-center gap-2">
            <FileText size={16} />
            <span>CALCULATION REPORT</span>
          </div>
          <h2>{state.form.VesselName || "Vessel assessment"}</h2>
          <p>Minimum propulsion power · 2013 methodology · A4 PDF</p>
        </div>
        <button
          className="button button-primary"
          onClick={onExport}
          disabled={exporting}
        >
          <Download size={14} />
          {exporting ? "Preparing…" : "Download PDF"}
        </button>
      </header>
      <div className="report-body">
        {state.dirty && (
          <p className="report-stale">
            Inputs have changed. Run the affected sections to refresh their
            calculated results before export.
          </p>
        )}
        {reportSections(state).map((section) => (
          <article key={section.id} className="report-section">
            <h3>{section.title}</h3>
            {section.narrative && <p>{section.narrative}</p>}
            <DataTable
              title={`${section.title} values`}
              headers={["Parameter", "Value", "Unit", "Comment"]}
              rows={section.rows.map((r) => [
                r.parameter,
                r.value,
                r.unit,
                r.comment,
              ])}
            />
            {section.id === "waves" && (
              <DataTable
                title="Peak wave period table"
                headers={["Tp (s)", "Raw (kN)", "Total (kN)", "Thrust (kN)"]}
                rows={state.waveRows.map((r) => [
                  formatNumber(r.period, 1),
                  formatNumber(r.added, 2),
                  formatNumber(r.total, 0),
                  formatNumber(r.thrust, 0),
                ])}
              />
            )}
            {section.id === "curves" && (
              <DataTable
                title="Propeller coefficient table"
                headers={["J", "KT", "10KQ", "η₀", "KTship"]}
                rows={state.propellerPoints.map((r) => [
                  formatNumber(r.J, 2),
                  formatNumber(r.KT),
                  formatNumber(r.KQ),
                  formatNumber(r.efficiency),
                  formatNumber(r.ship),
                ])}
              />
            )}
            {section.chart && (
              <AssessmentChart
                state={state}
                id={section.chart}
                ready={section.ready}
              />
            )}
          </article>
        ))}
      </div>
    </section>
  );
}
