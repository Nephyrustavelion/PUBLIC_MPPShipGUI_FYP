// components/AssessmentForm.tsx
import { useState } from 'react';
import { calculateLevel1, Level1Input, Level1Result } from '../lib/parameters';
import { Line } from 'react-chartjs-2';

export default function AssessmentForm() {
  const [input, setInput] = useState<Level1Input>({ dwt: 0, mcr: 0 });
  const [result, setResult] = useState<Level1Result | null>(null);

  const runLevel1 = () => {
    const res = calculateLevel1(input);
    setResult(res);
  };

  return (
    <div>
      <div>
        <label>DWT: </label>
        <input type="number" value={input.dwt} onChange={e => setInput({...input, dwt: +e.target.value})} />
      </div>
      <div>
        <label>MCR: </label>
        <input type="number" value={input.mcr} onChange={e => setInput({...input, mcr: +e.target.value})} />
      </div>
      <button onClick={runLevel1}>Run Level 1</button>

      {result && (
        <div style={{ marginTop: '1rem' }}>
          <h2>Result: {result.status}</h2>
          <Line
            data={{
              labels: result.chart.map(p => p.x),
              datasets: [{ label: 'Power vs Speed', data: result.chart.map(p => p.y) }]
            }}
          />
        </div>
      )}
    </div>
  );
}