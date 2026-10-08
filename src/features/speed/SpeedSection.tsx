import { SectionCard } from "../../components/SectionCard/SectionCard";
import { ResultGrid } from "../../components/ResultGrid/ResultGrid";
import { SectionFields, type SectionProps } from "../shared/SectionFields";
import "./SpeedSection.css";
export function SpeedSection({ assessment: a }: SectionProps) {
  const p = a.state.parameters,
    ready = !!a.state.completed.speed;
  return (
    <SectionCard
      number="03"
      title="Speed of advance"
      subtitle="Required ship speed from windage and course keeping criteria."
      onRun={() => a.run("speed")}
      onClear={() => a.clear("speed")}
    >
      <SectionFields assessment={a} section="speed" />
      <p className="speed-note">
        Hull and rudder values are refreshed when you run this section.
      </p>
      <ResultGrid
        comments={a.state.comments}
        onComment={a.updateComment}
        items={[
          {
            key: "RatioFL",
            label: "Frontal / lateral windage",
            value: ready ? p.RatioFL : null,
          },
          {
            key: "Vckref",
            label: "Reference course keeping speed",
            value: ready ? p.Vckref : null,
            unit: "knots",
          },
          {
            key: "Vck",
            label: "Minimum course keeping speed",
            value: ready ? p.Vck : null,
            unit: "knots",
          },
          {
            key: "Vs",
            label: "Required speed of advance",
            value: ready ? p.Vs : null,
            unit: "m/s",
          },
        ]}
      />
    </SectionCard>
  );
}
