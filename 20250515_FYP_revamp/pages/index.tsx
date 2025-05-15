// lib/parameters.ts

// pages/index.tsx
import React from 'react';
import Head from 'next/head';
import AssessmentForm from '../components/AssessmentForm';

const Home: React.FC = () => {
  return (
    <>
      <Head>
        <title>Ship Assessment App</title>
        <meta name="viewport" content="width=device-width, initial-scale=1" />
      </Head>
      <main style={{ padding: '2rem' }}>
        <h1>Minimum Propulsion Power Assessment</h1>
        <AssessmentForm />
      </main>
    </>
  );
};

export default Home;


/**
 * Stubbed port of your C# Parameters.cs logic.
 * Add your actual formulas below.
 */

export interface Level1Input {
    dwt: number;
    mcr: number;
  }
  export interface ChartPoint { x: number; y: number; }
  export interface Level1Result {
    status: 'SATISFACTORY' | 'UNSATISFACTORY';
    chart: ChartPoint[];
  }
  
  export function calculateLevel1(input: Level1Input): Level1Result {
    // Example placeholder logic:
    const mpp = input.dwt * 0.1 + input.mcr * 0.05;
    const status = mpp > 5000 ? 'SATISFACTORY' : 'UNSATISFACTORY';
    const chart: ChartPoint[] = Array.from({ length: 10 }, (_, i) => ({
      x: i * 2,
      y: mpp * (1 + i / 20)
    }));
    return { status, chart };
  }