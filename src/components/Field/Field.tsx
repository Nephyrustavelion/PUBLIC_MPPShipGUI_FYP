import { useId, useState } from "react";
import { MessageSquarePlus } from "lucide-react";
import type { FieldDefinition } from "../../domain/types";
import "./Field.css";

interface Props {
  definition: FieldDefinition;
  value: string;
  comment: string;
  onChange: (value: string) => void;
  onComment: (value: string) => void;
}
export function Field({
  definition: f,
  value,
  comment,
  onChange,
  onComment,
}: Props) {
  const id = useId();
  const [showComment, setShowComment] = useState(false);
  return (
    <div className="field">
      <div className="field-label-row">
        <label htmlFor={id}>{f.label}</label>
        <button
          type="button"
          className={`comment-toggle ${comment ? "has-comment" : ""}`}
          aria-label={`Comment on ${f.label}`}
          aria-expanded={showComment}
          onClick={() => setShowComment(!showComment)}
        >
          <MessageSquarePlus size={14} />
        </button>
      </div>
      <div className="field-control">
        {f.type === "select" ? (
          <select
            id={id}
            value={value}
            onChange={(e) => onChange(e.target.value)}
          >
            <option value="">Select {f.label.toLowerCase()}</option>
            {f.options?.map((option) => (
              <option key={option} value={option}>
                {option}
              </option>
            ))}
          </select>
        ) : (
          <input
            id={id}
            type={f.type === "text" ? "text" : "number"}
            step="any"
            value={value}
            placeholder={f.placeholder ?? "Enter value"}
            onChange={(e) => onChange(e.target.value)}
          />
        )}
        {f.unit && <span className="field-unit">{f.unit}</span>}
      </div>
      {f.symbol && <span className="field-symbol">{f.symbol}</span>}
      {f.hint && <p className="field-hint">{f.hint}</p>}
      {showComment && (
        <textarea
          aria-label={`${f.label} comment`}
          className="field-comment"
          rows={2}
          value={comment}
          placeholder="Add a note for the report…"
          onChange={(e) => onComment(e.target.value)}
        />
      )}
    </div>
  );
}
