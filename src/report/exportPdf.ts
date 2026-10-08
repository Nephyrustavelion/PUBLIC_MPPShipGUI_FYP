import { jsPDF } from "jspdf";
import { autoTable } from "jspdf-autotable";
import Chart from "chart.js/auto";
import { chartSpec, type ChartId } from "../components/Charts/chartConfig";
import type { AssessmentState } from "../domain/types";
import { formatNumber } from "../domain/numeric";
import { reportSections } from "./reportModel";

const fontCache = new Map<string, string>();
async function loadFont(name: string) {
  if (fontCache.has(name)) return fontCache.get(name)!;
  const response = await fetch(`${import.meta.env.BASE_URL}fonts/${name}.ttf`);
  if (!response.ok)
    throw new Error(
      "The report font could not be loaded. Please refresh and try again.",
    );
  const bytes = new Uint8Array(await response.arrayBuffer());
  let binary = "";
  for (let i = 0; i < bytes.length; i += 8192)
    binary += String.fromCharCode(...bytes.subarray(i, i + 8192));
  const encoded = btoa(binary);
  fontCache.set(name, encoded);
  return encoded;
}
function renderChart(state: AssessmentState, id: ChartId): string {
  const canvas = document.createElement("canvas");
  canvas.width = 1200;
  canvas.height = 580;
  const spec = chartSpec(state, id, true);
  let chart: Chart<"line" | "bar"> | undefined;
  try {
    chart = new Chart(canvas, {
      type: spec.type,
      data: spec.data,
      options: spec.options,
      plugins: [
        {
          id: "report-background",
          beforeDraw(instance) {
            const ctx = instance.ctx;
            ctx.save();
            ctx.globalCompositeOperation = "destination-over";
            ctx.fillStyle = "#ffffff";
            ctx.fillRect(0, 0, instance.width, instance.height);
            ctx.restore();
          },
        },
      ],
    });
    chart.update("none");
    return canvas.toDataURL("image/png");
  } finally {
    chart?.destroy();
  }
}
export async function createReportPdf(state: AssessmentState): Promise<jsPDF> {
  const doc = new jsPDF({
    orientation: "portrait",
    unit: "mm",
    format: "a4",
    compress: true,
  });
  const [regular, bold] = await Promise.all([
    loadFont("NotoSans-Regular"),
    loadFont("NotoSans-Bold"),
  ]);
  doc.addFileToVFS("NotoSans-Regular.ttf", regular);
  doc.addFont("NotoSans-Regular.ttf", "NotoSans", "normal");
  doc.addFileToVFS("NotoSans-Bold.ttf", bold);
  doc.addFont("NotoSans-Bold.ttf", "NotoSans", "bold");
  doc.setFont("NotoSans");
  doc.setProperties({
    title: `Minimum propulsion power — ${state.form.VesselName || "Vessel assessment"}`,
    subject: "2013 minimum propulsion power assessment",
    author: "Propulsion Studio",
    creator: "React + TypeScript ship assessment",
  });
  const width = doc.internal.pageSize.getWidth(),
    height = doc.internal.pageSize.getHeight();
  const left = 17,
    right = width - 17;
  let y = 27;
  const newPage = () => {
    doc.addPage();
    y = 27;
  };
  const ensure = (needed: number) => {
    if (y + needed > height - 22) newPage();
  };
  const text = (content: string, size = 9, bold = false) => {
    doc.setFont("NotoSans", bold ? "bold" : "normal");
    doc.setFontSize(size);
    doc.setTextColor(62, 84, 92);
    const lines = doc.splitTextToSize(content, right - left) as string[];
    ensure(lines.length * size * 0.42 + 6);
    doc.text(lines, left, y);
    y += lines.length * size * 0.42 + 5;
  };
  const table = (head: string[], body: string[][]) => {
    autoTable(doc, {
      head: [head],
      body,
      startY: y,
      margin: { left, right: 17, top: 25, bottom: 22 },
      styles: {
        font: "NotoSans",
        fontSize: 8,
        cellPadding: 1.7,
        textColor: [59, 81, 89],
        lineColor: [218, 229, 231],
        lineWidth: 0.12,
        overflow: "linebreak",
      },
      headStyles: {
        fillColor: [37, 106, 119],
        textColor: [255, 255, 255],
        fontStyle: "bold",
        fontSize: 8,
      },
      alternateRowStyles: { fillColor: [243, 248, 248] },
      columnStyles:
        head.length === 4
          ? {
              0: { cellWidth: 64 },
              1: { cellWidth: 37 },
              2: { cellWidth: 23 },
              3: { cellWidth: 52 },
            }
          : undefined,
      showHead: "everyPage",
      rowPageBreak: "avoid",
    });
    y =
      (doc as jsPDF & { lastAutoTable: { finalY: number } }).lastAutoTable
        .finalY + 8;
  };
  text("Minimum propulsion power", 20, true);
  text(
    "Following MEPC.232(65): 2013 Interim guidelines for determining minimum propulsion power to maintain the manoeuvrability of ships in adverse conditions.",
    8,
  );
  text(`Vessel: ${state.form.VesselName || "Not entered"}`, 10, true);
  if (state.dirty)
    text(
      "Inputs have changed since calculations were last run. Calculated results below retain the last run state.",
      8,
    );
  const sections = reportSections(state);
  for (const section of sections) {
    if (
      [
        "engine",
        "level1",
        "geometry",
        "resistance",
        "waves",
        "curves",
      ].includes(section.id)
    )
      newPage();
    ensure(25);
    text(section.title, 12, true);
    table(
      ["Parameter", "Value", "Unit", "Comment"],
      section.rows.map((r) => [r.parameter, r.value, r.unit, r.comment]),
    );
    if (section.narrative) text(section.narrative, 8);
    if (section.id === "waves")
      table(
        ["Tp (s)", "Raw (kN)", "Total (kN)", "Thrust (kN)"],
        state.waveRows.map((r) => [
          formatNumber(r.period, 1),
          formatNumber(r.added, 2),
          formatNumber(r.total, 0),
          formatNumber(r.thrust, 0),
        ]),
      );
    if (section.id === "curves" && state.propellerPoints.length)
      table(
        ["J", "KT", "10KQ", "η₀", "KTship"],
        state.propellerPoints.map((r) => [
          formatNumber(r.J, 2),
          formatNumber(r.KT),
          formatNumber(r.KQ),
          formatNumber(r.efficiency),
          formatNumber(r.ship),
        ]),
      );
    if (section.chart && section.ready) {
      ensure(101);
      if (y === 27) text(chartSpec(state, section.chart, true).title, 12, true);
      doc.addImage(
        renderChart(state, section.chart),
        "PNG",
        left,
        y,
        right - left,
        85,
      );
      y += 94;
    }
  }
  const pages = doc.getNumberOfPages();
  const generated = new Date().toLocaleString("en-GB");
  for (let page = 1; page <= pages; page++) {
    doc.setPage(page);
    doc.setDrawColor(210, 228, 231);
    doc.line(left, 16, right, 16);
    doc.line(left, height - 16, right, height - 16);
    doc.setFont("NotoSans", "normal");
    doc.setFontSize(7);
    doc.setTextColor(105, 133, 142);
    doc.text("PROPULSION STUDIO  /  CALCULATION REPORT", left, 12);
    doc.text(state.form.VesselName || "Vessel assessment", right, 12, {
      align: "right",
    });
    doc.text(`2013 methodology · Generated ${generated}`, left, height - 10);
    doc.text(`${page} / ${pages}`, right, height - 10, { align: "right" });
  }
  return doc;
}
export async function downloadReport(state: AssessmentState): Promise<void> {
  const doc = await createReportPdf(state);
  const vessel = (state.form.VesselName || "vessel")
    .replace(/[^\p{L}\p{N}_-]+/gu, "-")
    .slice(0, 60);
  doc.save(`${vessel}-propulsion-assessment.pdf`);
}
