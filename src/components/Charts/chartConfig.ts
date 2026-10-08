import type { ChartData, ChartOptions } from "chart.js";
import type { AssessmentState } from "../../domain/types";
export type ChartId = "power" | "subtotal" | "waves" | "propeller";
export interface ChartSpec {
  title: string;
  subtitle: string;
  type: "line" | "bar";
  data: ChartData<"line" | "bar">;
  options: ChartOptions<"line" | "bar">;
}
const teal = "#0000ff",
  blue = "#0080ff",
  amber = "#ff8c00",
  green = "#00b000";
const finite = (v: number) => (Number.isFinite(v) ? v : null);
const base: ChartOptions<"line" | "bar"> = {
  font: { family: "Calibri, Segoe UI, Arial, sans-serif", size: 12 },
  responsive: true,
  maintainAspectRatio: false,
  animation: false,
  interaction: { mode: "index", intersect: false },
  plugins: {
    legend: {
      position: "bottom",
      labels: {
        boxWidth: 9,
        boxHeight: 9,
        usePointStyle: true,
        padding: 20,
        font: { family: "Calibri, Segoe UI, Arial, sans-serif", size: 12 },
        color: "#333333",
      },
    },
  },
  scales: {
    x: {
      grid: { display: false },
      ticks: { color: "#333333", font: { size: 11 } },
      border: { display: false },
    },
    y: {
      grid: { color: "#dddddd" },
      ticks: { color: "#333333", font: { size: 11 } },
      border: { display: false },
    },
  },
};
export function chartSpec(
  state: AssessmentState,
  id: ChartId,
  print = false,
): ChartSpec {
  const p = state.parameters;
  const options: ChartOptions<"line" | "bar"> = structuredClone(base);
  if (print) {
    options.responsive = false;
    options.devicePixelRatio = 1;
  }
  if (id === "power") {
    const labels = [20000, p.DWT];
    options.scales!.x = {
      type: "linear",
      grid: { display: false },
      ticks: { color: "#333333", font: { size: 11 } },
      border: { display: false },
      title: {
        display: true,
        text: "Deadweight tonnage (tonnes)",
        color: "#333333",
      },
    };
    options.scales!.y = {
      ...options.scales!.y,
      title: { display: true, text: "Power (kW)", color: "#333333" },
    };
    return {
      title: "Installed vs. minimum power",
      subtitle: "Minimum power line and the vessel’s MCR",
      type: "line",
      options,
      data: {
        labels,
        datasets: [
          {
            label: "Minimum power line",
            data: labels.map((x) => ({ x, y: p.a * x + p.b })),
            borderColor: teal,
            backgroundColor: teal,
            pointRadius: 0,
            borderWidth: 2,
          },
          {
            label: "Installed power (MCR)",
            data: [{ x: p.DWT, y: p.MCR }],
            borderColor: amber,
            backgroundColor: amber,
            pointRadius: 5,
            showLine: false,
          },
        ],
      },
    };
  }
  if (id === "subtotal") {
    options.indexAxis = "y";
    options.scales!.x = {
      ...options.scales!.x,
      stacked: true,
      title: { display: true, text: "Resistance (kN)", color: "#333333" },
    };
    options.scales!.y = { ...options.scales!.y, stacked: true };
    return {
      title: "Resistance breakdown",
      subtitle: "Calm water, air and appendage resistance",
      type: "bar",
      options,
      data: {
        labels: ["Subtotal"],
        datasets: [
          { label: "Calm water", data: [finite(p.Rcw)], backgroundColor: blue },
          { label: "Air", data: [finite(p.Rair)], backgroundColor: amber },
          {
            label: "Appendages",
            data: [finite(p.Rapp)],
            backgroundColor: teal,
          },
        ],
      },
    };
  }
  if (id === "waves") {
    options.scales!.x = {
      ...options.scales!.x,
      stacked: true,
      title: { display: true, text: "Peak wave period (s)", color: "#333333" },
    };
    options.scales!.y = {
      ...options.scales!.y,
      stacked: true,
      title: { display: true, text: "Resistance (kN)", color: "#333333" },
    };
    return {
      title: "Total resistance by wave period",
      subtitle: "Added resistance and the saved subtotal at each period",
      type: "bar",
      options,
      data: {
        labels: state.waveRows.map((r) => String(r.period)),
        datasets: [
          {
            label: "Calm water",
            data: state.waveRows.map((r) => r.calm),
            backgroundColor: blue,
          },
          {
            label: "Air",
            data: state.waveRows.map((r) => r.air),
            backgroundColor: amber,
          },
          {
            label: "Appendages",
            data: state.waveRows.map((r) => r.appendage),
            backgroundColor: teal,
          },
          {
            label: "Added wave resistance",
            data: state.waveRows.map((r) => r.added),
            backgroundColor: green,
          },
        ],
      },
    };
  }
  const xmax = Number(state.form.chartXmax),
    ymax = Number(state.form.chartYmax);
  options.scales!.x = {
    type: "linear",
    grid: { display: false },
    ticks: { color: "#333333", font: { size: 11 } },
    border: { display: false },
    min: 0,
    max: xmax > 0 ? xmax : undefined,
    title: { display: true, text: "Advance coefficient (J)", color: "#333333" },
  };
  options.scales!.y = {
    ...options.scales!.y,
    min: 0,
    max: ymax > 0 ? ymax : undefined,
  };
  return {
    title: "Open-water propeller curves",
    subtitle: "Thrust, torque, efficiency and the ship curve",
    type: "line",
    options,
    data: {
      datasets: [
        {
          label: "KT",
          data: state.propellerPoints.map((r) => ({ x: r.J, y: r.KT })),
          borderColor: blue,
          pointRadius: 0,
          borderWidth: 2,
        },
        {
          label: "10KQ",
          data: state.propellerPoints.map((r) => ({ x: r.J, y: r.KQ })),
          borderColor: amber,
          pointRadius: 0,
          borderWidth: 2,
        },
        {
          label: "Efficiency (η₀)",
          data: state.propellerPoints.map((r) => ({ x: r.J, y: r.efficiency })),
          borderColor: teal,
          pointRadius: 0,
          borderWidth: 2,
        },
        {
          label: "KTship",
          data: state.propellerPoints
            .filter((r) => Number.isFinite(r.ship))
            .map((r) => ({ x: r.J, y: r.ship })),
          borderColor: green,
          pointRadius: 0,
          borderWidth: 2,
        },
        {
          label: "KT intersection",
          data: Number.isFinite(p.J_at_KTintercept)
            ? [{ x: p.J_at_KTintercept, y: p.KTship0 }]
            : [],
          borderColor: "#ba7580",
          backgroundColor: "#ba7580",
          pointRadius: 4,
          showLine: false,
        },
        {
          label: "Optimal shaft speed coordinates",
          data: Number.isFinite(p.J_at_KTintercept)
            ? [{ x: p.J_at_KTintercept, y: p.eta0 }]
            : [],
          borderColor: "#94708f",
          backgroundColor: "#94708f",
          pointRadius: 4,
          showLine: false,
        },
      ],
    },
  };
}
