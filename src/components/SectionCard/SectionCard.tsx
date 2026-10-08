import type { ReactNode } from "react";
import { Play, RotateCcw } from "lucide-react";
import "./SectionCard.css";
interface Props {
  number: string;
  title: string;
  subtitle: string;
  children: ReactNode;
  onRun?: () => void;
  onClear?: () => void;
  runLabel?: string;
}
export function SectionCard({
  number,
  title,
  subtitle,
  children,
  onRun,
  onClear,
  runLabel = "Run section",
}: Props) {
  return (
    <section className="section-card assessment-panel" aria-label={title}>
      <header className="section-card-header">
        <div className="section-number">{number}</div>
        <div className="section-heading">
          <h2>{title}</h2>
          <p>{subtitle}</p>
        </div>
        <div className="section-actions">
          {onRun && (
            <button
              className={`button ${number === "00" ? "button-save" : "button-run"}`}
              onClick={onRun}
            >
              <Play size={13} />
              {runLabel}
            </button>
          )}
          {onClear && (
            <button className="button button-clear" onClick={onClear}>
              <RotateCcw size={14} />
              Clear
            </button>
          )}
        </div>
      </header>
      <div className="section-card-body">{children}</div>
    </section>
  );
}
