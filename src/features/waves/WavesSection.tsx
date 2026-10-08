import { Plus, Minus, Calculator } from "lucide-react";
import { Field } from "../../components/Field/Field";
import { SectionCard } from "../../components/SectionCard/SectionCard";
import { ResultGrid } from "../../components/ResultGrid/ResultGrid";
import { DataTable } from "../../components/DataTable/DataTable";
import { AssessmentChart } from "../../components/Charts/AssessmentChart";
import { fields } from "../../domain/fields";
import { formatNumber } from "../../domain/numeric";
import type { SectionProps } from "../shared/SectionFields";
import "./WavesSection.css";
export function WavesSection({ assessment: a }: SectionProps) {
  const p = a.state.parameters,
    ready = !!a.state.completed.waves;
  const renderField = (key: string) => {
    const f = fields.waves.find((f) => f.key === key)!;
    return (
      <Field
        key={key}
        definition={f}
        value={a.state.form[f.key]}
        comment={a.state.comments[f.key] ?? ""}
        onChange={(value) => a.updateField(f.key, value)}
        onComment={(value) => a.updateComment(f.key, value)}
      />
    );
  };
  return (
    <SectionCard
      number="05"
      title="Wave resistance"
      subtitle="Build the original 18-row resistance and thrust assessment."
      onRun={() => a.run("waves")}
      onClear={() => a.clear("waves")}
    >
      <div className="grid gap-6 sm:grid-cols-2">
        {renderField("g")}
        {renderField("Raw")}
      </div>
      <div className="wave-actions">
        <span>
          Next period:{" "}
          <strong>
            {a.state.waveRows.find((r) => r.added === null)?.period ??
              "All rows filled"}{" "}
            s
          </strong>
        </span>
        <div className="flex flex-wrap gap-2">
          <button className="button button-quiet" onClick={a.removeWave}>
            <Minus size={14} />
            Remove last row
          </button>
          <button className="button button-secondary" onClick={a.addWave}>
            <Plus size={14} />
            Add resistance
          </button>
        </div>
      </div>
      <details className="wave-reference">
        <summary>Wave model reference values</summary>
        <p>
          These values retain the desktop model’s saved state. The resistance
          table uses your manually entered Raw values.
        </p>
        <dl>
          {["Bmax", "TA", "TF", "WaveAmp", "kyy", "Iyy", "E_para"].map(
            (key) => {
              const field = fields.waves.find((f) => f.key === key)!;
              return (
                <div key={key}>
                  <dt>{field.label}</dt>
                  <dd>
                    {a.state.form[field.key] || "—"} {field.unit}
                  </dd>
                </div>
              );
            },
          )}
        </dl>
        <button className="button button-quiet" onClick={a.entranceAngle}>
          <Calculator size={13} />
          Calculate entrance angle
        </button>
      </details>
      <ResultGrid
        comments={a.state.comments}
        onComment={a.updateComment}
        items={[
          {
            key: "Fr",
            label: "Froude number",
            value: ready ? p.Fr : null,
            digits: 6,
          },
          {
            key: "Hs",
            label: "Significant wave height",
            value: ready ? p.Hs : null,
            unit: "m",
          },
          {
            key: "TDF",
            label: "Thrust deduction factor",
            value: ready ? p.TDF : null,
          },
        ]}
      />
      <DataTable
        title="Peak period · resistance · thrust"
        headers={["Tp (s)", "Raw (kN)", "Total (kN)", "Thrust (kN)"]}
        rows={a.state.waveRows.map((r) => [
          formatNumber(r.period, 1),
          formatNumber(r.added, 2),
          formatNumber(r.total, 0),
          formatNumber(r.thrust, 0),
        ])}
      />
      <AssessmentChart
        state={a.state}
        id="waves"
        ready={a.state.waveRows.some((r) => r.added !== null)}
      />
    </SectionCard>
  );
}
