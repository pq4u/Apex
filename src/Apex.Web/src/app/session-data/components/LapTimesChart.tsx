'use client'
import { useEffect, useRef } from 'react'
import * as Chart from 'chart.js'
import { Driver, Lap, Stint, TyreInfo } from '../types'

interface LapTimesChartProps {
  allDriverLaps: Record<string, Lap[]>;
  allDriverStints: Record<string, Stint[]>;
  selectedDrivers: number[];
  drivers: Driver[];
}

const CHART_COLORS = [  // to do: handle teams colors
  'rgba(93, 1, 146, 1)',
  'rgba(255, 99, 132, 1)',
  'rgba(54, 162, 235, 1)',
  'rgba(255, 206, 86, 1)',
  'rgba(75, 192, 192, 1)',
  'rgba(153, 102, 255, 1)',
  'rgba(255, 159, 64, 1)'
];

export function LapTimesChart({ allDriverLaps, allDriverStints, selectedDrivers, drivers }: LapTimesChartProps) {
  const chartRef = useRef<HTMLCanvasElement>(null);
  const chartInstanceRef = useRef<Chart.Chart | null>(null);

  const getTyreInfoForLap = (driverId: number, lapNumber: number): TyreInfo | null => {
    const driverStints = allDriverStints[driverId] || [];
    const stint = driverStints.find(s => lapNumber >= s.startLap && lapNumber <= s.endLap);
    return stint ? {
      compound: stint.compound,
      stintNumber: stint.stintNumber,
      tyreAge: stint.startTyreAge + (lapNumber - stint.startLap)
    } : null;
  };

  const millisecondsToMinuteFormat = (ms: number): string => {
    const totalSeconds = ms / 1000;
    const minutes = Math.floor(totalSeconds / 60);
    const seconds = Math.floor(totalSeconds % 60);
    const milliseconds = ms % 1000;

    return `${minutes}:${seconds.toString().padStart(2, '0')}.${milliseconds.toString().padStart(3, '0')}`;
  };

  useEffect(() => {
    if (!chartRef.current || Object.keys(allDriverLaps).length === 0) return;

    if (chartInstanceRef.current) {
      chartInstanceRef.current.destroy();
    }

    const datasets: Chart.ChartDataset<'line'>[] = [];
    const allLapNumbers = new Set<number>();

    Object.entries(allDriverLaps).forEach(([driverId, laps], index) => {
      let filteredLaps = laps.filter(x => x.lapDurationMs > 1);

      // exclude invalid laptimes using iqr method
      if (filteredLaps.length > 4) {
        const sortedTimes = filteredLaps.map(x => x.lapDurationMs).sort((a, b) => a - b);
        const q1Index = Math.floor(sortedTimes.length * 0.25);
        const q3Index = Math.floor(sortedTimes.length * 0.75);
        const q1 = sortedTimes[q1Index];
        const q3 = sortedTimes[q3Index];
        const iqr = q3 - q1;
        const lowerBound = q1 - (1.5 * iqr);
        const upperBound = q3 + (1.5 * iqr);

        filteredLaps = filteredLaps.filter(x => x.lapDurationMs >= lowerBound && x.lapDurationMs <= upperBound);
      }

      filteredLaps.forEach(lap => allLapNumbers.add(lap.lapNumber));

      const driver = drivers.find(d => d.id === parseInt(driverId));

      datasets.push({
        label: driver?.nameAcronym || `Driver ${driverId}`,
        data: filteredLaps.map(lap => ({ x: lap.lapNumber, y: lap.lapDurationMs })),
        borderColor: CHART_COLORS[index % CHART_COLORS.length],
        backgroundColor: CHART_COLORS[index % CHART_COLORS.length].replace('1)', '0.2)'),
        tension: 0.1,
        fill: false
      });
    })

    const sortedLapNumbers = Array.from(allLapNumbers).sort((a, b) => a - b);

    const context = chartRef.current.getContext('2d')
    if (context) {
      chartInstanceRef.current = new Chart.Chart(context, {
        type: 'line',
        data: {
          labels: sortedLapNumbers,
          datasets: datasets
        },
        options: {
          responsive: true,
          interaction: {
            intersect: false,
            mode: 'index'
          },
          plugins: {
            tooltip: {
              callbacks: {
                afterLabel: function(context: any) {
                  const lapNumber = context.parsed.x
                  const datasetIndex = context.datasetIndex
                  const driverId = selectedDrivers[datasetIndex]
                  const tyreInfo = getTyreInfoForLap(driverId, lapNumber)

                  if (tyreInfo) {
                    return [
                      `Tyre: ${tyreInfo.compound}`,
                      `Stint: ${tyreInfo.stintNumber}`,
                      `Tyre age: ${tyreInfo.tyreAge} laps`
                    ];
                  }
                  return [];
                }
              }
            }
          },
          scales: {
            y: {
              beginAtZero: false,
              title: {
                display: true,
                text: 'Time (minutes)'
              },
              ticks: {
                callback: function(value: any) {
                  return millisecondsToMinuteFormat(value);
                }
              }
            },
            x: {
              title: {
                display: true,
                text: 'Lap number'
              }
            }
          }
        }
      })
    }

    return () => {
      if (chartInstanceRef.current) {
        chartInstanceRef.current.destroy();
      }
    };
  }, [allDriverLaps, allDriverStints, drivers, selectedDrivers]);

  return (
    <div className="mt-6 mb-6">
      <canvas ref={chartRef} width="800" height="400" />
    </div>
  )
}