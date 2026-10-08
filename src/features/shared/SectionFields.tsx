import { Field } from "../../components/Field/Field";
import { fields } from "../../domain/fields";
import type { useAssessment } from "../../hooks/useAssessment";
import type { CalculationSection } from "../../domain/types";
export type AssessmentController = ReturnType<typeof useAssessment>;
export interface SectionProps {
  assessment: AssessmentController;
}
export function SectionFields({
  assessment,
  section,
}: {
  assessment: AssessmentController;
  section: CalculationSection;
}) {
  return (
    <div className="grid gap-x-6 gap-y-5 sm:grid-cols-2 xl:grid-cols-3">
      {fields[section].map((field) => (
        <Field
          key={field.key}
          definition={field}
          value={assessment.state.form[field.key]}
          comment={assessment.state.comments[field.key] ?? ""}
          onChange={(value) => assessment.updateField(field.key, value)}
          onComment={(value) => assessment.updateComment(field.key, value)}
        />
      ))}
    </div>
  );
}
