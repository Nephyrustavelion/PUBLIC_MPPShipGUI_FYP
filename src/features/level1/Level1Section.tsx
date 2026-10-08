import { CheckCircle2, CircleDashed } from "lucide-react";
import { SectionCard } from "../../components/SectionCard/SectionCard";
import { ResultGrid } from "../../components/ResultGrid/ResultGrid";
import { AssessmentChart } from "../../components/Charts/AssessmentChart";
import { SectionFields, type SectionProps } from "../shared/SectionFields";
import "./Level1Section.css";
export function Level1Section({ assessment: a }: SectionProps) {
  const p = a.state.parameters,
    ready = !!a.state.completed.level1;
  return (
    <SectionCard
      number="01"
      title="Minimum power line"
      subtitle="Assessment Level 1 · Compare installed power with the minimum power line."
      onRun={() => a.run("level1")}
      onClear={() => a.clear("level1")}
    >
      <div className="level1-layout">
        <div className="level1-values">
          <SectionFields assessment={a} section="level1" />
          <div className="power-equation">
            <div>
              <span>MINIMUM PROPULSION POWER</span>
              <strong>MPP = a × DWT + b</strong>
            </div>
            <div
              className={`power-status ${ready && p.Result === "SATISFACTORY" ? "passed" : ""}`}
            >
              {ready ? <CheckCircle2 size={15} /> : <CircleDashed size={15} />}
              <span>{ready ? p.Result : "Awaiting calculation"}</span>
            </div>
          </div>
          <ResultGrid
            comments={a.state.comments}
            onComment={a.updateComment}
            items={[
              {
                key: "a",
                label: "Coefficient a",
                value: ready ? p.a : null,
                digits: 4,
              },
              {
                key: "b",
                label: "Coefficient b",
                value: ready ? p.b : null,
                digits: 1,
              },
              {
                key: "MPP",
                label: "Minimum propulsion power",
                value: ready ? p.MPP : null,
                unit: "kW",
                digits: 0,
              },
              {
                key: "Result",
                label: "Assessment result",
                value: ready ? p.Result : null,
              },
            ]}
          />
        </div>
        <AssessmentChart
          state={a.state}
          id="power"
          ready={ready && p.DWT >= 20000}
        />
      </div>
    </SectionCard>
  );
}
