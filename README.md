# Propulsion Studio

A browser-based migration of the supplied C# minimum propulsion power application, built with React, TypeScript, Vite and Tailwind CSS. All seven calculation sections appear on one scrolling page in the original order. The interface follows the desktop application's grey background, blue labels, pale input colours, Calibri/Arial Narrow font families, and coloured controls, with rounder buttons and responsive layouts. There is no sidebar, page switching or calculation pagination.

## Start locally

Use Node.js 22.12 or newer. From this folder:

```sh
corepack enable
pnpm install
pnpm dev
```

Open the local address displayed in your terminal. The included pnpm lockfile records the tested dependency versions. If you prefer npm, `npm install` and `npm run dev` also work; use one package manager consistently and commit its lockfile.

```sh
pnpm test       # Formula and workflow checks
pnpm build      # TypeScript checks and production build into dist/
pnpm preview    # Serve that production build locally
pnpm format     # Format source files
```

## Deploy to Netlify

Push the contents of this folder to your Git repository, then connect that repository to Netlify. `netlify.toml` supplies the settings:

| Setting           | Value           |
| ----------------- | --------------- |
| Build command     | `npm run build` |
| Publish directory | `dist`          |
| Node version      | `22`            |

If this project is placed inside a larger repository, set Netlify's base directory to that project folder. For a manual deployment, upload the contents of `dist`, or extract the supplied deployment ZIP and upload its folder. The assessment is a single page without URL routing, so no URL rewrite is required. No backend, database, API keys or environment variables are needed.

These build settings follow [Netlify's Vite deployment guide](https://docs.netlify.com/build/frameworks/framework-setup-guides/vite/). The app calculates and creates PDFs in the visitor's browser. Inputs remain in memory and reset on page refresh.

## Use the assessment

1. Load Sample A to reproduce the original sample workflow, or enter vessel details.
2. Use each section's Run button to update the relevant calculations. Section dependencies follow the desktop workflow.
3. Enter added wave resistance and select Add resistance to fill the next peak period. Remove last row reverses the last entry. The table covers 7 to 15.5 seconds in half-second increments.
4. Run the propeller section to populate its coefficient table and charts. Chart axis limits can be changed independently.
5. Use the comment icons to attach notes to inputs or results.
6. Select Export PDF in the top control row. Expand Preview calculation report at the bottom of the same page to review the tables first if needed. Export downloads an A4 report with selectable text, embedded fonts, comments, charts, page numbers and repeated table headings. Website calculations have no pagination; the PDF keeps normal A4 page breaks for printing.

The top buttons retain their original order: Run all, Clear all, Sample A, Sample B, with Export PDF on the right on desktop. Each section keeps Run/Save followed by Clear beside its heading. Calibri and Arial Narrow are used when installed, with standard browser font fallbacks on other systems.

Run all preserves the original `Run0`, `Run1`, `Run6` sequence: vessel details, Level 1 and propeller analysis. It does not calculate every Level 2 section. Sample A fills all 18 wave rows and runs the sample calculations; Sample B changes only its original subset of inputs. Changed dependent sections remain marked as out of date until run. Previously entered wave rows retain their saved resistance values; remove/re-enter rows to change that dataset.

## File organization

```text
src/
  App.tsx                         Single-page application composition
  App.css                         Tailwind import, theme and shared layout styles
  main.tsx                        React entry point
  domain/
    Parameters.ts                 Port of all 48 C# formula methods
    assessment.ts                 Original action order, binding and sample workflow
    fields.ts                     Input definitions, labels, units and defaults
    numeric.ts                    C# float input and midpoint rounding helpers
    samples.ts                    Original sample values and resistance polynomial
    types.ts                      Shared domain types
    __fixtures__/                 Results captured by executing the original C# class
    *.test.ts                     Formula and workflow tests
  hooks/useAssessment.ts          React state and action bindings
  components/
    Field/                        Field.tsx + Field.css
    ResultGrid/                   ResultGrid.tsx + ResultGrid.css
    SectionCard/                  SectionCard.tsx + SectionCard.css
    Toolbar/                      Toolbar.tsx + Toolbar.css
    DataTable/                    DataTable.tsx + DataTable.css
    Charts/                       AssessmentChart.tsx + AssessmentChart.css
    ReportPreview/                ReportPreview.tsx + ReportPreview.css
  features/
    vessel/ level1/ geometry/ speed/ environment/ waves/ propeller/
                                  Each section has its own .tsx and .css
    shared/SectionFields.tsx       Shared section field renderer
  report/
    reportModel.ts                Structured report data
    exportPdf.ts                  PDF formatting and export
public/fonts/                     Locally hosted report fonts and license
netlify.toml                      Deployment settings
```

`App.tsx` is the TypeScript React equivalent of the requested App.ts: JSX uses the `.tsx` extension. Component-specific CSS stays beside each component. Calculation code is separate from UI and PDF formatting. Tailwind utilities handle reusable grids and spacing; local CSS defines the original desktop palette and component details.

## Calculation compatibility

The calculation baseline is the supplied `20250515_FYP_revamp/Parameters.cs`, with action order and inputs taken from the original WinForms `Form1.cs`. The web project contains TypeScript application code and captured JSON reference results; the legacy source remains in your existing Git history. The port keeps the original formulas, constants, state mutations, units inside the engine, and action dependencies. Float literals/input parsing and round-to-even display helpers mirror the original numeric paths. No C# runtime or PowerShell helper is needed to develop, test, build or deploy this project.

Retained source behavior includes:

- Bulk-carrier minimum power returns zero at exactly 20,000 and 145,000 tonnes because the original uses strict inequalities.
- The original second speed conversion in the Froude calculation remains.
- Wake values retain earlier engine state for branches that do not assign a value.
- Sample B is a partial form update and does not automatically run all calculations.
- The RPM textbox is not rebound by the original propeller Run action; the sample-seeded engine value remains.
- Sample A fills the maximum-beam textbox but does not assign the engine's `Bmax`. Its empirical ship curve and operating-point calculations therefore produce `NaN`. The port retains and explains these results rather than changing the equations. Available thrust, torque and efficiency curves still render.
- The empirical propeller ship curve uses the original seven-second period; manually entered wave-resistance rows are a separate path.
- Inactive wave-model bindings remain reference values. The entrance-angle helper uses the saved engine state.

Excel import and the engine load-chart handlers were commented out in the supplied source, so no new implementations were invented. The original application's installed designer/runtime was not launched; parity checks target the supplied formula class and the mapped action workflows.

Presentation changes include a new report layout, clearer empty/error states, input validation, bounded chart generation, and matching gravity/knots labels to their values. These do not revise the calculation equations. The methodology remains the source application's 2013 methodology.

## Verification

- 71 automated checks: 57 reference cases generated by invoking the original C# class, a stateful wake test, and 13 workflow/report/rounding checks.
- All 48 formula methods are covered by the C# reference cases; finite results are compared at relative tolerance `1e-11`, with explicit handling of non-finite and tuple results.
- Production build and TypeScript checks pass.
- Isolated browser checks cover all seven sections being present on one page, original button order, samples, comments, charts, PDF download, clear/run actions and mobile overflow at 390 pixels.
- The exported Sample A report was rendered and visually checked across all nine A4 pages.

The captured reference results are stored in `src/domain/__fixtures__/csharp-reference.json`. The TypeScript tests read these results directly, so testing requires only the JavaScript toolchain.

These checks establish migration compatibility for the tested cases; they do not independently validate the engineering methodology for every possible input.

## Font attribution

PDF reports embed Noto Sans Regular and Bold from the Noto project, distributed under the SIL Open Font License. The license is included at `public/fonts/LICENSE.txt`. Font files are served locally, so PDF generation does not require a third-party font service.
