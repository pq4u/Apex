'use client'
import { useEffect, useRef } from 'react'
import * as Chart from 'chart.js'
import 'chartjs-adapter-date-fns'
import { Driver, TelemetryPoint } from '../types'

interface TelemetryChartProps {
  lapNumber: number;
  telemetryData: Record<string, TelemetryPoint[]>;
  selectedDrivers: number[];
  drivers: Driver[];
}

const CHART_COLORS = [  // to do: handle teams colours
  'rgba(93, 1, 146, 1)',
  'rgba(255, 99, 132, 1)',
  'rgba(54, 162, 235, 1)',
  'rgba(255, 206, 86, 1)',
  'rgba(75, 192, 192, 1)',
  'rgba(153, 102, 255, 1)',
  'rgba(255, 159, 64, 1)'
];

export function TelemetryChart({ lapNumber, telemetryData, selectedDrivers, drivers }: TelemetryChartProps) {
  const chartRef = useRef<HTMLCanvasElement>(null);
  const chartInstanceRef = useRef<Chart.Chart | null>(null);

  useEffect(() => {
    if (!chartRef.current || !telemetryData) return;

    if (chartInstanceRef.current) {
      chartInstanceRef.current.destroy();
    }

    console.log('chart telemetry data:', telemetryData);

    const datasets: Chart.ChartDataset<'line'>[] = [];
    const allTimestamps: Date[] = [];

    selectedDrivers.forEach((driverId, index) => {
      const driverData = telemetryData[driverId] || [];
      const driver = drivers.find(d => d.id === driverId);

      if (driverData.length > 0) {
        const chartData = driverData
          .filter(point => point.speed !== null && point.speed !== undefined)
          .map(point => {
            const timestamp = new Date(point.time);
            allTimestamps.push(timestamp);
            return {
              x: timestamp,
              y: point.speed
            }
          });

        if (chartData.length > 0) {
          datasets.push({
            label: driver?.nameAcronym || `Driver ${driverId}`,
            data: chartData,
            borderColor: CHART_COLORS[index % CHART_COLORS.length],
            backgroundColor: CHART_COLORS[index % CHART_COLORS.length].replace('1)', '0.2)'),
            tension: 0.1,
            fill: false,
            pointRadius: 1,
            borderWidth: 2
          });
        }
      }
    })

    if (datasets.length > 0 && allTimestamps.length > 0) {
      const minTime = new Date(Math.min(...allTimestamps.map(t => t.getTime())));
      const maxTime = new Date(Math.max(...allTimestamps.map(t => t.getTime())));

      const context = chartRef.current.getContext('2d');
      if (context) {
        chartInstanceRef.current = new Chart.Chart(context, {
          type: 'line',
          data: {
            datasets: datasets
          },
          options: {
            responsive: true,
            maintainAspectRatio: false,
            interaction: {
              intersect: false,
              mode: 'index'
            },
            plugins: {
              title: {
                display: true,
                text: `Speed - lap ${lapNumber}`
              },
              legend: {
                display: true,
                position: 'top'
              }
            },
            scales: {
              x: {
                type: 'time',
                min: minTime,
                max: maxTime,
                time: {
                  displayFormats: {
                    millisecond: 'HH:mm:ss.SSS',
                    second: 'HH:mm:ss',
                    minute: 'HH:mm'
                  }
                },
                title: {
                  display: true,
                  text: 'Time'
                }
              },
              y: {
                beginAtZero: false,
                title: {
                  display: true,
                  text: 'Speed (km/h)'
                }
              }
            }
          }
        })
      }
    }

    return () => {
      if (chartInstanceRef.current) {
        chartInstanceRef.current.destroy();
      }
    };
  }, [telemetryData, selectedDrivers, drivers, lapNumber])

  const hasData = telemetryData && Object.keys(telemetryData).length > 0;

  return (
    <div className="w-full">
      <div style={{ height: '400px' }}>
        <canvas ref={chartRef} />
      </div>
      {!hasData && (
        <div className="text-center text-gray-500 py-8">
          Loading telemetry data...
        </div>
      )}
    </div>
  )
}