import { Anchor } from "lucide-react";
import { SectionCard } from "../../components/SectionCard/SectionCard";
import { SectionFields, type SectionProps } from "../shared/SectionFields";
import "./VesselSection.css";
export function VesselSection({ assessment }: SectionProps) {
  return (
    <SectionCard
      number="00"
      title="Vessel particulars"
      subtitle="Start with your vessel’s identity and main engine details."
      onRun={() => assessment.run("vessel")}
      onClear={() => assessment.clear("vessel")}
      runLabel="Save particulars"
    >
      <SectionFields assessment={assessment} section="vessel" />
      <div className="vessel-note">
        <Anchor size={16} />
        <p>
          Your particulars and field comments will be included in the
          calculation report.
        </p>
      </div>
    </SectionCard>
  );
}
