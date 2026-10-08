## Live Website: 

[Open the Ship Assessment website](https://mppshipgui.netlify.app/)

# Minimum Propulsion Power Assessment

A ship-engineering Final Year Project originally developed in C# during 2020–2021 and refactored into a React and TypeScript website in 2026.

The project began with an Excel calculation workflow. Its purpose was to bring ship parameters, propulsion calculations, tables, and charts into one application, reducing repetitive spreadsheet work and making it easier to generate a PDF calculation report.

The current website uses **React, TypeScript, Vite, and Tailwind CSS**. It keeps the original calculation logic and familiar controls while making the application accessible through a browser.

## Purpose and intended audience

This application is intended for people who record, measure, or assess ship parameters, including engineering students, researchers, and personnel working with vessel dimensions, engine particulars, resistance, and propeller data.

The workflow starts with entered measurements and vessel information. It then calculates the assessment results, generates tables and charts, and assembles a report that can be exported as PDF. It supports the assessment of recorded parameters; it does not collect measurements directly from instruments.

The aim of replacing the Excel workflow was to:

- Keep vessel inputs and related calculations together.
- Reduce repeated formula entry and manual spreadsheet preparation.
- Generate calculation charts and diagrams from the entered parameters more quickly.
- Make it easier to compare results and review the relationships between inputs and outputs.
- Produce a consistent report containing calculations, charts, and supporting comments.

## The original Final Year Project: 2020–2021

The first approximately four months were spent learning about ships and developing the calculations in Excel. This provided the engineering foundation for the application: understanding the parameters, working through the equations, and identifying how the different calculations depended on one another.

The next approximately three to four months focused on turning that workflow into a C# Windows Forms application, alongside two other academic modules. About a month involved intensive self-directed learning through LinkedIn videos, Stack Overflow, and other programming resources, studying most days of the week.

The original implementation was developed without AI coding assistance. Learning the language and delivering the application happened together. The challenges included:

- Translating spreadsheet formulas into C# methods while keeping track of units and calculation order.
- Learning Windows Forms controls, button events, input handling, and application state.
- Connecting user-entered ship parameters to calculated results, tables, and charts.
- Adapting examples from different resources into a single application for a specific engineering problem.
- Integrating PDF generation so the assessment could be documented outside the application.
- Troubleshooting unfamiliar code and libraries within the available project time.

The resulting application combined vessel particulars, minimum-power assessment, geometry and speed calculations, resistance analysis, propeller charts, sample inputs, comments, and PDF export. Its structure reflects a first substantial programming project developed while learning: much of the workflow lived in the form, with the equations grouped in a separate calculation class.

### Original C# interface

The screenshot below shows the original Windows Forms interface across its assessment pages.

![Original 2021 C# Windows Forms interface showing ship inputs, resistance charts, and propeller analysis](Screenshots%20of%20C%23%20interface.jpg)

## Revisiting the project in 2026

Five years later, while studying AI engineering and learning Java, the project was revisited to bring the existing application to the web. The 2026 refactor used AI coding assistance to migrate the C# calculation methods and application workflow into TypeScript, organize the code, and check the migration against reference results from the original calculation class.

The objective was to preserve the engineering work and functionality while improving accessibility and maintainability:

- **React and TypeScript** provide the browser interface and typed application code.
- **Vite** supplies the development server and production build.
- **Tailwind CSS and component-local CSS** handle layout and styling.
- **Chart.js** renders the calculation charts.
- **jsPDF and AutoTable** generate the PDF calculation report in the browser.
- **Netlify** can host the built website without a separate application server.

All seven calculation sections now appear on one scrolling page. The interface keeps the original grey background, blue labels, pale input colours, coloured controls, and Calibri/Arial Narrow font preferences, with rounder buttons and layouts that adapt to smaller screens.

This refactor also separates the calculation engine, interface components, application state, and PDF formatting. The original equations and workflow remain the compatibility baseline; moving to TypeScript does not, by itself, revise the engineering methodology.

### React and TypeScript interface

The screenshot below shows the browser version, including the main controls, vessel particulars, and minimum-power chart.

![2026 React and TypeScript interface showing rounded controls, vessel particulars, and the minimum-power assessment chart](Screenshots%20of%20TS%20interface.png)

## Assessment features

| Section | Purpose |
| --- | --- |
| Vessel particulars | Record vessel identity and engine information. |
| Minimum power line | Compare installed power with the calculated minimum-power line. |
| Geometry | Calculate corrected submerged lateral area and rudder-area relationships. |
| Required speed | Calculate navigation and course-keeping speed relationships. |
| Environment and resistance | Calculate the associated environmental and resistance quantities. |
| Waves | Build the wave-period, resistance, and thrust table and charts. |
| Propeller | Generate thrust, torque, efficiency, and ship-curve calculations and charts. |

The application also includes Sample A and Sample B, section-level Run and Clear controls, field comments, adjustable chart limits, a report preview, and PDF export.

## Run locally

Use Node.js 22.12 or newer. From the project root:

```sh
corepack enable
pnpm install
pnpm dev
```

Open the local address displayed in the terminal. The included pnpm lockfile records the dependency versions. If using npm instead, run `npm install` and `npm run dev`; use one package manager consistently and retain its lockfile.

```sh
pnpm test       # Run formula and workflow checks
pnpm build      # Check TypeScript and build into dist/
pnpm preview    # Preview the production build locally
pnpm format     # Format source files
```

## Use the application and export PDF

1. Load a sample or enter the vessel particulars and assessment inputs.
2. Run the relevant sections in their calculation order. If an input changes, rerun the affected sections to update the results.
3. In the wave section, enter added resistance and select **Add resistance** to fill the next peak period. **Remove last row** reverses the last entry. The table covers 7 to 15.5 seconds in half-second increments.
4. Run the propeller section to populate its coefficient table and charts. Adjust the chart limits as needed.
5. Add supporting notes using the field comment controls.
6. Review **Preview calculation report** at the bottom of the page, then select **Export PDF**.

The PDF contains calculation tables, available charts, comments, selectable text, embedded fonts, and page numbers. The website uses a single calculation page; the report uses normal A4 page breaks for printing.

**Run all** retains the original sequence: vessel particulars, the minimum-power assessment, and propeller analysis. Run the remaining sections individually as needed. Sample A fills the wave table and runs its sample workflow; Sample B updates only its original subset of inputs.

Previously entered wave rows retain their saved resistance values. Remove and re-enter rows when changing that dataset. Inputs and comments remain in browser memory and reset when the page is refreshed.

## Deploy to Netlify

Connect the repository to Netlify. The included `netlify.toml` defines the build settings:

| Setting | Value |
| --- | --- |
| Base directory | Leave blank when this application is at the repository root. |
| Build command | `npm run build` |
| Publish directory | `dist` |
| Node version | `22` |

If the application is moved into a subfolder, use that folder as the base directory. For a manual deployment, build the application and upload the contents of `dist`.

Calculations and PDF generation run in the visitor's browser. No backend, database, API keys, or environment variables are required. The single-page assessment has no URL routing and does not require routing rewrites.

## Code organization

```text
src/
  App.tsx                    Application composition
  App.css                    Tailwind import and shared application styles
  main.tsx                   React entry point
  domain/                    Calculation engine, inputs, samples, and types
    __fixtures__/            Captured original C# calculation results
    *.test.ts                Formula and workflow tests
  hooks/                     Assessment state and action bindings
  components/                Reusable components with adjacent CSS files
  features/                  The seven assessment sections and their local CSS
  report/                    Report data model and PDF export
public/fonts/                Report fonts and their license
netlify.toml                 Hosting configuration
```

`App.tsx` holds the global React composition; its `.tsx` extension allows JSX. `App.css` holds shared styles. Each component or feature keeps its specific CSS alongside its TypeScript React files. Calculations are kept separate from interface rendering and PDF formatting.

## Calculation compatibility and known limitations

The migration uses the saved C# `Parameters` class and the original Windows Forms action bindings as its reference. The live application requires only the JavaScript toolchain; it does not require a C# runtime. The original project is retained in the historical archive and Git history.

The calculation approach follows the original project's 2013 methodology. Reference-based tests check that the TypeScript port reproduces the supplied C# calculation behavior for the tested cases. These migration tests were added during the refactor and are separate from the original 2021 work. They establish compatibility, rather than independent validation of every engineering equation or possible input.

Some original behaviors are intentionally retained and should be considered when reviewing results:

- Bulk-carrier minimum power returns zero at exactly 20,000 and 145,000 tonnes because of the original strict inequalities.
- The original second speed conversion in the Froude calculation remains.
- Some wake-calculation branches retain an earlier value rather than assigning a new one.
- The propeller Run action does not bind the RPM textbox back to the engine.
- Sample A sets the maximum-beam input but does not assign the calculation engine's `Bmax`. Its empirical ship curve and operating-point calculations therefore produce `NaN`; available thrust, torque, and efficiency curves still render.
- The empirical propeller ship curve uses the original seven-second period. Manually entered wave-resistance rows follow a separate calculation path.
- Inactive wave-model bindings remain reference values, and the entrance-angle helper uses the saved engine state.

The original Excel-import and engine-load-chart handlers were commented out in the supplied C# source and are not implemented in the web version. Presentation improvements include clearer input errors, bounded chart generation, a new PDF layout, and labels aligned with the displayed units.

The tests and reference results are under `src/domain/`, including `__fixtures__/csharp-reference.json`.

## Historical files

- `20210517_FYP_C#.zip` — archived original C# project.
- `Sample Report.pdf` — example report from the original application.
- The interface screenshots above document the desktop application and its browser refactor.

## Font attribution

PDF reports embed Noto Sans Regular and Bold, distributed under the SIL Open Font License. The license is included in `public/fonts/LICENSE.txt`. The report fonts are served locally and do not require a third-party font service.
