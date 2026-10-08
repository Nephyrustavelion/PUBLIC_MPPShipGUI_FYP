import { useState } from "react";
import { MessageSquarePlus } from "lucide-react";
import { formatNumber } from "../../domain/numeric";
import "./ResultGrid.css";
export interface ResultItem {
  key: string;
  label: string;
  value: number | string | null | undefined;
  unit?: string;
  digits?: number;
}
interface Props {
  items: ResultItem[];
  comments: Record<string, string>;
  onComment: (key: string, value: string) => void;
}
export function ResultGrid({ items, comments, onComment }: Props) {
  const [open, setOpen] = useState<string | null>(null);
  return (
    <div className="result-grid">
      {items.map((item) => (
        <div className="result-item" key={item.key}>
          <div className="result-label">
            <span>{item.label}</span>
            <button
              aria-label={`Comment on ${item.label}`}
              aria-expanded={open === item.key}
              onClick={() => setOpen(open === item.key ? null : item.key)}
            >
              <MessageSquarePlus size={13} />
            </button>
          </div>
          <div
            className={`result-value ${typeof item.value === "string" ? "result-text" : ""} ${item.value === "SATISFACTORY" ? "result-passed" : item.value === "UNSATISFACTORY" ? "result-failed" : ""}`}
          >
            {typeof item.value === "string"
              ? item.value
              : formatNumber(item.value, item.digits ?? 3)}
            {item.unit && <small>{item.unit}</small>}
          </div>
          {open === item.key && (
            <textarea
              aria-label={`${item.label} comment`}
              value={comments[item.key] ?? ""}
              rows={2}
              onChange={(e) => onComment(item.key, e.target.value)}
              placeholder="Report note…"
            />
          )}
        </div>
      ))}
    </div>
  );
}
