import type { ReactNode } from "react";
import "./DataTable.css";
interface Props {
  title: string;
  headers: string[];
  rows: ReactNode[][];
  empty?: string;
}
export function DataTable({
  title,
  headers,
  rows,
  empty = "Run this section to see calculated values.",
}: Props) {
  return (
    <div className="data-table-wrap">
      <table>
        <caption>{title}</caption>
        <thead>
          <tr>
            {headers.map((header, i) => (
              <th scope="col" key={i}>
                {header}
              </th>
            ))}
          </tr>
        </thead>
        <tbody>
          {rows.length ? (
            rows.map((row, i) => (
              <tr key={i}>
                {row.map((cell, j) => (
                  <td key={j}>{cell}</td>
                ))}
              </tr>
            ))
          ) : (
            <tr>
              <td className="empty-table" colSpan={headers.length}>
                {empty}
              </td>
            </tr>
          )}
        </tbody>
      </table>
    </div>
  );
}
