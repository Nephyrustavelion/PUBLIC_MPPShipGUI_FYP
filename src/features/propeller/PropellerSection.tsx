import { Info } from "lucide-react";
import { SectionCard } from "../../components/SectionCard/SectionCard";
import { ResultGrid } from "../../components/ResultGrid/ResultGrid";
import { DataTable } from "../../components/DataTable/DataTable";
import { AssessmentChart } from "../../components/Charts/AssessmentChart";
import { SectionFields, type SectionProps } from "../shared/SectionFields";
import { formatNumber } from "../../domain/numeric";
import "./PropellerSection.css";
export function PropellerSection({ assessment: a }: SectionProps) {
  const p = a.state.parameters,
    ready = !!a.state.completed.propeller;
  const hasNonFinite = a.state.propellerPoints.some(
    (r) => !Number.isFinite(r.ship),
  );
  return (
    <SectionCard
      number="06"
      title="Propeller analysis"
      subtitle="Original open-water coefficient curves and operating point calculations."
      onRun={() => a.run("propeller")}
      onClear={() => a.clear("propeller")}
    >
      <SectionFields assessment={a} section="propeller" />
      <ResultGrid
        comments={a.state.comments}
        onComment={a.updateComment}
        items={[
          { key: "Wake", label: "Wake fraction", value: ready ? p.Wake : null },
          {
            key: "TDF",
            label: "Thrust deduction factor",
            value: ready ? p.TDF : null,
          },
          {
            key: "Ua",
            label: "Advance speed at propeller",
            value: ready ? p.Ua : null,
            unit: "m/s",
            digits: 2,
          },
          {
            key: "TransEff",
            label: "Transmission efficiency",
            value: ready ? p.TransEff : null,
          },
          {
            key: "intercept",
            label: "KT intersection (J, KT)",
            value: ready
              ? `${formatNumber(p.J_at_KTintercept)}, ${formatNumber(p.KTship0)}`
              : null,
          },
          {
            key: "optimal",
            label: "Optimal shaft speed coordinates",
            value: ready
              ? `${formatNumber(p.J_at_KTintercept)}, ${formatNumber(p.eta0)}`
              : null,
          },
        ]}
      />
      {hasNonFinite && (
        <div className="propeller-note">
          <Info size={16} />
          <p>
            The original model returns NaN for the ship curve and operating
            point with these saved wave inputs. The available KT, 10KQ and
            efficiency curves are shown.
          </p>
        </div>
      )}
      <AssessmentChart state={a.state} id="propeller" ready={ready} />
      <DataTable
        title="Propeller coefficient table"
        headers={["J", "KT", "10KQ", "η₀", "KTship"]}
        rows={a.state.propellerPoints.map((r) => [
          formatNumber(r.J, 2),
          formatNumber(r.KT),
          formatNumber(r.KQ),
          formatNumber(r.efficiency),
          formatNumber(r.ship),
        ])}
      />
    </SectionCard>
  );
}
