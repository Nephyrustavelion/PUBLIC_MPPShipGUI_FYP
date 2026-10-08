import { useState } from "react";
import { Check, Info, X } from "lucide-react";
import { useAssessment } from "./hooks/useAssessment";
import { Toolbar } from "./components/Toolbar/Toolbar";
import { VesselSection } from "./features/vessel/VesselSection";
import { Level1Section } from "./features/level1/Level1Section";
import { GeometrySection } from "./features/geometry/GeometrySection";
import { SpeedSection } from "./features/speed/SpeedSection";
import { EnvironmentSection } from "./features/environment/EnvironmentSection";
import { WavesSection } from "./features/waves/WavesSection";
import { PropellerSection } from "./features/propeller/PropellerSection";
import { ReportPreview } from "./components/ReportPreview/ReportPreview";
import "./App.css";

export default function App() {
  const assessment = useAssessment();
  const { state } = assessment;
  const [reportOpen, setReportOpen] = useState(false);
  const [exporting, setExporting] = useState(false);
  const [exportError, setExportError] = useState("");
  const exportPdf = async () => {
    setExporting(true);
    setExportError("");
    try {
      const { downloadReport } = await import("./report/exportPdf");
      await downloadReport(state);
    } catch (error) {
      setExportError(
        error instanceof Error
          ? error.message
          : "Unable to create the report. Please try again.",
      );
    } finally {
      setExporting(false);
    }
  };
  return (
    <div className="app-shell">
      <a href="#assessment-content" className="skip-link">
        Skip to assessment
      </a>
      <main className="app-main" id="assessment-content">
        <Toolbar
          vessel={state.form.VesselName}
          onRunAll={assessment.runAll}
          onClearAll={assessment.clearAll}
          onSample={assessment.loadSample}
          onExport={exportPdf}
          exporting={exporting}
        />
        {(state.errors.length > 0 || exportError) && (
          <div className="notice error-notice" role="alert">
            <X size={16} />
            <div>
              {state.errors.map((error, i) => (
                <p key={i}>{error}</p>
              ))}
              {exportError && <p>{exportError}</p>}
            </div>
          </div>
        )}
        {state.message && (
          <div className="notice success-notice" role="status">
            <Check size={14} />
            {state.message}
          </div>
        )}
        {state.dirty && (
          <div className="notice dirty-notice">
            <Info size={14} />
            Inputs changed. Run the affected sections to update their results.
          </div>
        )}
        <div className="assessment-sections">
          <VesselSection assessment={assessment} />
          <Level1Section assessment={assessment} />
          <GeometrySection assessment={assessment} />
          <SpeedSection assessment={assessment} />
          <EnvironmentSection assessment={assessment} />
          <WavesSection assessment={assessment} />
          <PropellerSection assessment={assessment} />
        </div>
        <details
          className="report-disclosure"
          onToggle={(event) => setReportOpen(event.currentTarget.open)}
        >
          <summary>Preview calculation report</summary>
          {reportOpen && (
            <ReportPreview
              state={state}
              onExport={exportPdf}
              exporting={exporting}
            />
          )}
        </details>
        <footer className="app-footer">
          <span>Minimum propulsion power assessment · MEPC.232(65)</span>
          <span>Calculations and PDF export run in your browser.</span>
        </footer>
      </main>
    </div>
  );
}
