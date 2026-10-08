import { SectionCard } from "../../components/SectionCard/SectionCard";
import { ResultGrid } from "../../components/ResultGrid/ResultGrid";
import { AssessmentChart } from "../../components/Charts/AssessmentChart";
import { SectionFields, type SectionProps } from "../shared/SectionFields";
import "./EnvironmentSection.css";
export function EnvironmentSection({ assessment: a }: SectionProps) {
  const p = a.state.parameters,
    ready = !!a.state.completed.environment;
  return (
    <SectionCard
      number="04"
      title="Environment & resistance"
      subtitle="Environmental assumptions and the procedure for installed power."
      onRun={() => a.run("environment")}
      onClear={() => a.clear("environment")}
    >
      <SectionFields assessment={a} section="environment" />
      <div className="environment-results">
        <ResultGrid
          comments={a.state.comments}
          onComment={a.updateComment}
          items={[
            {
              key: "Rey",
              label: "Reynolds number",
              value: ready ? p.Rey / 1e5 : null,
              unit: "×10⁵",
              digits: 5,
            },
            {
              key: "Vw",
              label: "Mean wind speed",
              value: ready ? p.Vw : null,
              unit: "m/s",
              digits: 2,
            },
            {
              key: "Cf",
              label: "Friction coefficient",
              value: ready ? p.Cf : null,
              digits: 6,
            },
            {
              key: "Cair",
              label: "Air resistance coefficient",
              value: ready ? p.Cair : null,
            },
            {
              key: "Rcw",
              label: "Calm-water resistance",
              value: ready ? p.Rcw : null,
              unit: "kN",
            },
            {
              key: "Rair",
              label: "Aerodynamic resistance",
              value: ready ? p.Rair : null,
              unit: "kN",
            },
            {
              key: "Rsubtotal",
              label: "Resistance subtotal",
              value: ready ? p.Rsubtotal : null,
              unit: "kN",
              digits: 0,
            },
          ]}
        />
      </div>
      <AssessmentChart state={a.state} id="subtotal" ready={ready} />
    </SectionCard>
  );
}
