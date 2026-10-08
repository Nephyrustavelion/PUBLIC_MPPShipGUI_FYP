import { SectionCard } from "../../components/SectionCard/SectionCard";
import { ResultGrid } from "../../components/ResultGrid/ResultGrid";
import { SectionFields, type SectionProps } from "../shared/SectionFields";
import "./GeometrySection.css";
export function GeometrySection({ assessment: a }: SectionProps) {
  const p = a.state.parameters,
    ready = !!a.state.completed.geometry;
  return (
    <SectionCard
      number="02"
      title="Hull & rudder"
      subtitle="Assessment Level 2 · Principal particulars and submerged lateral area."
      onRun={() => a.run("geometry")}
      onClear={() => a.clear("geometry")}
    >
      <SectionFields assessment={a} section="geometry" />
      <div
        className="hull-illustration"
        aria-label="Simplified ship dimensions"
      >
        <svg viewBox="0 0 680 125" role="img">
          <path
            d="M68 63h540l-58 34H126z M126 63V34h354v29 M210 34V20h63v14"
            fill="none"
            stroke="#555555"
            strokeWidth="2"
          />
          <path d="M80 113H595 M80 108v10 M595 108v10" stroke="#777777" />
          <text x="325" y="119" fill="#333333" fontSize="10">
            LPP
          </text>
          <path d="M635 62v35 M630 62h10 M630 97h10" stroke="#777777" />
          <text x="645" y="83" fill="#333333" fontSize="10">
            Tm
          </text>
        </svg>
        <span>Principal ship dimensions · schematic</span>
      </div>
      <ResultGrid
        comments={a.state.comments}
        onComment={a.updateComment}
        items={[
          {
            key: "ALScor",
            label: "Corrected submerged lateral area",
            value: ready ? p.ALScor : null,
            unit: "m²",
            digits: 0,
          },
          {
            key: "PerALS",
            label: "Rudder / corrected lateral area",
            value: ready ? p.PerALS : null,
            unit: "%",
          },
        ]}
      />
    </SectionCard>
  );
}
