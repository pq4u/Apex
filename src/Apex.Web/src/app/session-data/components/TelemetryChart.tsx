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

    selectedDrivers.forEach((driverId, index) => {
      const driverData = telemetryData[driverId] || [];
      const driver = drivers.find(d => d.id === driverId);

      if (driverData.length > 0) {
        const validPoints = driverData.filter(point => point.speed !== null && point.speed !== undefined);

        if (validPoints.length > 0) {
          const driverStartTime = new Date(validPoints[0].time);

          const chartData = validPoints.map(point => {
            const timestamp = new Date(point.time);
            const secondsFromStart = (timestamp.getTime() - driverStartTime.getTime()) / 1000;
            return {
              x: secondsFromStart,
              y: point.speed
            }
          });

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

    if (datasets.length > 0) {
      const maxDuration = Math.max(...datasets.flatMap(d => d.data.map((point: any) => point.x)));

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
                type: 'linear',
                min: 0,
                max: maxDuration,
                title: {
                  display: true,
                  text: 'Time (seconds)'
                },
                ticks: {
                  callback: function(value: any) {
                    const totalSeconds = Math.floor(value);
                    const minutes = Math.floor(totalSeconds / 60);
                    const seconds = totalSeconds % 60;
                    return `${minutes}:${seconds.toString().padStart(2, '0')}`;
                  }
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
    <div className="w-full space-y-6">
      <div>
        <div style={{ height: '400px' }}>
          <canvas ref={chartRef} />
        </div>
      </div>

      {hasData && (
        <>
          <ThrottleChart
            lapNumber={lapNumber}
            telemetryData={telemetryData}
            selectedDrivers={selectedDrivers}
            drivers={drivers}
          />
          <BrakeChart
            lapNumber={lapNumber}
            telemetryData={telemetryData}
            selectedDrivers={selectedDrivers}
            drivers={drivers}
          />
          <RPMChart
            lapNumber={lapNumber}
            telemetryData={telemetryData}
            selectedDrivers={selectedDrivers}
            drivers={drivers}
          />

          <GearChart
            lapNumber={lapNumber}
            telemetryData={telemetryData}
            selectedDrivers={selectedDrivers}
            drivers={drivers}
          />
        </>
      )}

      {!hasData && (
        <div className="text-center text-gray-500 py-8">
          Loading telemetry data...
        </div>
      )}
    </div>
  )
}

function ThrottleChart({ lapNumber, telemetryData, selectedDrivers, drivers }: TelemetryChartProps) {
  const chartRef = useRef<HTMLCanvasElement>(null);
  const chartInstanceRef = useRef<Chart.Chart | null>(null);

  useEffect(() => {
    if (!chartRef.current || !telemetryData) return;

    if (chartInstanceRef.current) {
      chartInstanceRef.current.destroy();
    }

    const datasets: Chart.ChartDataset<'line'>[] = [];

    selectedDrivers.forEach((driverId, index) => {
      const driverData = telemetryData[driverId] || [];
      const driver = drivers.find(d => d.id === driverId);

      if (driverData.length > 0) {
        const validPoints = driverData.filter(point => point.throttle !== null && point.throttle !== undefined);

        if (validPoints.length > 0) {
          const driverStartTime = new Date(validPoints[0].time);

          const chartData = validPoints.map(point => {
            const timestamp = new Date(point.time);
            const secondsFromStart = (timestamp.getTime() - driverStartTime.getTime()) / 1000;
            return {
              x: secondsFromStart,
              y: point.throttle
            }
          });

          datasets.push({
            label: driver?.nameAcronym || `Driver ${driverId}`,
            data: chartData,
            borderColor: CHART_COLORS[index % CHART_COLORS.length],
            backgroundColor: CHART_COLORS[index % CHART_COLORS.length].replace('1)', '0.2)'),
            tension: 0.1,
            fill: false,
            pointRadius: 0,
            borderWidth: 2
          });
        }
      }
    })

    if (datasets.length > 0) {
      const maxDuration = Math.max(...datasets.flatMap(d => d.data.map((point: any) => point.x)));

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
                text: `Throttle - lap ${lapNumber}`
              },
              legend: {
                display: true,
                position: 'top'
              }
            },
            scales: {
              x: {
                type: 'linear',
                min: 0,
                max: maxDuration,
                title: {
                  display: true,
                  text: 'Time (seconds)'
                },
                ticks: {
                  callback: function(value: any) {
                    const totalSeconds = Math.floor(value);
                    const minutes = Math.floor(totalSeconds / 60);
                    const seconds = totalSeconds % 60;
                    return `${minutes}:${seconds.toString().padStart(2, '0')}`;
                  }
                }
              },
              y: {
                min: 0,
                max: 100,
                title: {
                  display: true,
                  text: 'Throttle (%)'
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

  return (
    <div>
      <div style={{ height: '300px' }}>
        <canvas ref={chartRef} />
      </div>
    </div>
  )
}

function BrakeChart({ lapNumber, telemetryData, selectedDrivers, drivers }: TelemetryChartProps) {
  const chartRef = useRef<HTMLCanvasElement>(null);
  const chartInstanceRef = useRef<Chart.Chart | null>(null);

  useEffect(() => {
    if (!chartRef.current || !telemetryData) return;

    if (chartInstanceRef.current) {
      chartInstanceRef.current.destroy();
    }

    const datasets: Chart.ChartDataset<'line'>[] = [];

    selectedDrivers.forEach((driverId, index) => {
      const driverData = telemetryData[driverId] || [];
      const driver = drivers.find(d => d.id === driverId);

      if (driverData.length > 0) {
        const validPoints = driverData.filter(point => point.brake !== null && point.brake !== undefined);

        if (validPoints.length > 0) {
          const driverStartTime = new Date(validPoints[0].time);

          const chartData = validPoints.map(point => {
            const timestamp = new Date(point.time);
            const secondsFromStart = (timestamp.getTime() - driverStartTime.getTime()) / 1000;
            return {
              x: secondsFromStart,
              y: point.brake
            }
          });

          datasets.push({
            label: driver?.nameAcronym || `Driver ${driverId}`,
            data: chartData,
            borderColor: CHART_COLORS[index % CHART_COLORS.length],
            backgroundColor: CHART_COLORS[index % CHART_COLORS.length].replace('1)', '0.2)'),
            tension: 0.1,
            fill: false,
            pointRadius: 0,
            borderWidth: 2
          });
        }
      }
    })

    if (datasets.length > 0) {
      const maxDuration = Math.max(...datasets.flatMap(d => d.data.map((point: any) => point.x)));

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
                text: `Brake - lap ${lapNumber}`
              },
              legend: {
                display: true,
                position: 'top'
              }
            },
            scales: {
              x: {
                type: 'linear',
                min: 0,
                max: maxDuration,
                title: {
                  display: true,
                  text: 'Time (seconds)'
                },
                ticks: {
                  callback: function(value: any) {
                    const totalSeconds = Math.floor(value);
                    const minutes = Math.floor(totalSeconds / 60);
                    const seconds = totalSeconds % 60;
                    return `${minutes}:${seconds.toString().padStart(2, '0')}`;
                  }
                }
              },
              y: {
                min: 0,
                max: 100,
                title: {
                  display: true,
                  text: 'Brake (%)'
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

  return (
    <div>
      <div style={{ height: '300px' }}>
        <canvas ref={chartRef} />
      </div>
    </div>
  )
}

function RPMChart({ lapNumber, telemetryData, selectedDrivers, drivers }: TelemetryChartProps) {
  const chartRef = useRef<HTMLCanvasElement>(null);
  const chartInstanceRef = useRef<Chart.Chart | null>(null);

  useEffect(() => {
    if (!chartRef.current || !telemetryData) return;

    if (chartInstanceRef.current) {
      chartInstanceRef.current.destroy();
    }

    const datasets: Chart.ChartDataset<'line'>[] = [];

    selectedDrivers.forEach((driverId, index) => {
      const driverData = telemetryData[driverId] || [];
      const driver = drivers.find(d => d.id === driverId);

      if (driverData.length > 0) {
        const validPoints = driverData.filter(point => point.rpm !== null && point.rpm !== undefined);

        if (validPoints.length > 0) {
          const driverStartTime = new Date(validPoints[0].time);

          const chartData = validPoints.map(point => {
            const timestamp = new Date(point.time);
            const secondsFromStart = (timestamp.getTime() - driverStartTime.getTime()) / 1000;
            return {
              x: secondsFromStart,
              y: point.rpm
            }
          });

          datasets.push({
            label: driver?.nameAcronym || `Driver ${driverId}`,
            data: chartData,
            borderColor: CHART_COLORS[index % CHART_COLORS.length],
            backgroundColor: CHART_COLORS[index % CHART_COLORS.length].replace('1)', '0.2)'),
            tension: 0.1,
            fill: false,
            pointRadius: 0,
            borderWidth: 2
          });
        }
      }
    })

    if (datasets.length > 0) {
      const maxDuration = Math.max(...datasets.flatMap(d => d.data.map((point: any) => point.x)));

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
                text: `RPM - lap ${lapNumber}`
              },
              legend: {
                display: true,
                position: 'top'
              }
            },
            scales: {
              x: {
                type: 'linear',
                min: 0,
                max: maxDuration,
                title: {
                  display: true,
                  text: 'Time (seconds)'
                },
                ticks: {
                  callback: function(value: any) {
                    const totalSeconds = Math.floor(value);
                    const minutes = Math.floor(totalSeconds / 60);
                    const seconds = totalSeconds % 60;
                    return `${minutes}:${seconds.toString().padStart(2, '0')}`;
                  }
                }
              },
              y: {
                beginAtZero: false,
                title: {
                  display: true,
                  text: 'RPM'
                },
                ticks: {
                  callback: function(value: any) {
                    return Math.round(value).toLocaleString();
                  }
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

  return (
    <div>
      <div style={{ height: '300px' }}>
        <canvas ref={chartRef} />
      </div>
    </div>
  )
}

function GearChart({ lapNumber, telemetryData, selectedDrivers, drivers }: TelemetryChartProps){
  const chartRef  = useRef<HTMLCanvasElement>(null);
  const chartInstanceRef = useRef<Chart.Chart | null>(null);

  useEffect(() => {
    if (!chartRef.current || !telemetryData) return;

    if (chartInstanceRef.current){
      chartInstanceRef.current.destroy();
    }

    const datasets: Chart.ChartDataset<'line'>[] = [];

    selectedDrivers.forEach((driverId, index) => {
      const driverData = telemetryData[driverId] || [];
      const driver = drivers.find(x => x.id === driverId);

      if (driverData.length > 0){
        const validPoints = driverData.filter(x => x.gear !== null && x.gear !== undefined);

        if (validPoints.length > 0) {
          const driverStartTime = new Date(validPoints[0].time);

          const chartData = validPoints.map(point => {
            const timestamp = new Date(point.time);
            const secondsFromStart = (timestamp.getTime() - driverStartTime.getTime()) / 1000;
            return {
              x: secondsFromStart,
              y: point.gear
            }
          });

          datasets.push({
            label: driver?.nameAcronym,
            data: chartData,
            borderColor: CHART_COLORS[index % CHART_COLORS.length],
            backgroundColor: CHART_COLORS[index % CHART_COLORS.length].replace('1)', '0.2'),
            tension: 0.1,
            fill: false,
            pointRadius: 0,
            borderWidth: 2

          });
        }
      }
    })

    if (datasets.length > 0){
      const maxDuration = Math.max(...datasets.flatMap(d => d.data.map((point: any) => point.x)));

      const context = chartRef.current.getContext('2d');

      if (context){
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
                display:true,
                text: `Gear - lap ${lapNumber}`
              },
              legend: {
                display: true,
                position: 'top'
              }
            },
            scales: {
              x: {
                type: 'linear',
                min: 0,
                max: maxDuration,
                title: {
                  display: true,
                  text: 'Time (seconds)'
                },
                ticks: {
                  callback: function(value: any) {
                    const totalSeconds = Math.floor(value);
                    const minutes = Math.floor(totalSeconds / 60);
                    const seconds = totalSeconds % 60;

                    return `${minutes}:${seconds.toString().padStart(2, '0')}`;
                  }
                }
              },
              y: {
                beginAtZero: false,
                title: {
                  display: true,
                  text: 'Gear'
                },
                ticks: {
                  callback: function(value: any){
                    return Math.round(value).toLocaleString();
                  }
                }
              }
            }
          }
        })
      }
    }

    return () => {
      if (chartInstanceRef.current){
        chartInstanceRef.current.destroy();
      }
    }
  }, [telemetryData, selectedDrivers, drivers, lapNumber])

  return (
    <div>
      <div style={{ height: '300px' }}>
        <canvas ref={chartRef} />
      </div>
    </div>
  )
}