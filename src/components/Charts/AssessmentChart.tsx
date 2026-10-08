import "chart.js/auto";
import { Chart } from "react-chartjs-2";
import { BarChart3 } from "lucide-react";
import type { AssessmentState } from "../../domain/types";
import { chartSpec, type ChartId } from "./chartConfig";
import "./AssessmentChart.css";
interface Props {
  state: AssessmentState;
  id: ChartId;
  ready: boolean;
}
export function AssessmentChart({ state, id, ready }: Props) {
  const spec = chartSpec(state, id);
  return (
    <div className="chart-panel">
      <header>
        <span className="chart-icon">
          <BarChart3 size={16} />
        </span>
        <div>
          <h3>{spec.title}</h3>
          <p>{spec.subtitle}</p>
        </div>
      </header>
      <div className="chart-canvas" role="img" aria-label={spec.title}>
        {ready ? (
          <Chart type={spec.type} data={spec.data} options={spec.options} />
        ) : (
          <div className="chart-empty">
            <BarChart3 size={30} strokeWidth={1} />
            <span>Your chart will appear here</span>
            <small>Run the section or load Sample A to begin.</small>
          </div>
        )}
      </div>
    </div>
  );
}
