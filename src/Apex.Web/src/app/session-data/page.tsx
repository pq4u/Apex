'use client'
import * as Chart from 'chart.js'
import 'chartjs-adapter-date-fns'
import { useState, useEffect, useRef } from 'react'

function TelemetryChart({ lapNumber, telemetryData, selectedDrivers, drivers }) {
  const chartRef = useRef(null)
  const chartInstanceRef = useRef(null)

  useEffect(() => {
    if (chartRef.current && telemetryData) {
      if (chartInstanceRef.current) {
        chartInstanceRef.current.destroy()
      }

      console.log('chart telemetry data:', telemetryData)

      const colors = [
        'rgba(93, 1, 146, 1)',
        'rgba(255, 99, 132, 1)',
        'rgba(54, 162, 235, 1)',
        'rgba(255, 206, 86, 1)',
        'rgba(75, 192, 192, 1)',
        'rgba(153, 102, 255, 1)',
        'rgba(255, 159, 64, 1)'
      ]

      const datasets = []
      let allTimestamps = []

      selectedDrivers.forEach((driverId, index) => {
        const driverData = telemetryData[driverId] || []
        const driver = drivers.find(d => d.id === parseInt(driverId))

        console.log(`Driver ${driverId} data:`, driverData)

        if (driverData.length > 0) {
          const chartData = driverData
            .filter(point => point.speed !== null && point.speed !== undefined)
            .map(point => {
              const timestamp = new Date(point.time)
              allTimestamps.push(timestamp)
              return {
                x: timestamp,
                y: point.speed
              }
            })

          console.log(`Driver ${driverId} chart data:`, chartData)

          if (chartData.length > 0) {
            datasets.push({
              label: driver?.nameAcronym || `Driver ${driverId}`,
              data: chartData,
              borderColor: colors[index % colors.length],
              backgroundColor: colors[index % colors.length].replace('1)', '0.2)'),
              tension: 0.1,
              fill: false,
              pointRadius: 1,
              borderWidth: 2
            })
          }
        }
      })

      if (datasets.length > 0 && allTimestamps.length > 0) {
        const minTime = new Date(Math.min(...allTimestamps))
        const maxTime = new Date(Math.max(...allTimestamps))

        const context = chartRef.current.getContext('2d')
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
                text: `Speed - Lap ${lapNumber}`
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
      } else {
        console.log('No valid data for chart')
      }
    }

    return () => {
      if (chartInstanceRef.current) {
        chartInstanceRef.current.destroy()
      }
    }
  }, [telemetryData, selectedDrivers, drivers, lapNumber])

  return (
    <div className="w-full">
      <div style={{ height: '400px' }}>
        <canvas ref={chartRef}></canvas>
      </div>
      {(!telemetryData || Object.keys(telemetryData).length === 0) && (
        <div className="text-center text-gray-500 py-8">
          Loading telemetry data...
        </div>
      )}
    </div>
  )
}

export default function Stats() {
  const [meetings, setMeetings] = useState([])
  const [sessions, setSessions] = useState([])
  const [drivers, setDrivers] = useState([])
  const [allDriverLaps, setAllDriverLaps] = useState({})
  const [allDriverStints, setAllDriverStints] = useState({})
  const [selectedMeeting, setSelectedMeeting] = useState('')
  const [selectedSession, setSelectedSession] = useState('')
  const [selectedDrivers, setSelectedDrivers] = useState([])
  const [showTable, setShowTable] = useState(false)
  const [expandedRows, setExpandedRows] = useState(new Set())
  const [telemetryData, setTelemetryData] = useState({})
  
  const chartRef = useRef(null)
  const chartInstanceRef = useRef(null)

  useEffect(() => {
    Chart.Chart.register(...Chart.registerables)
  }, []);

  useEffect(() => {
    fetch('http://localhost:5001/meetings')
      .then(res => res.json())
      .then(setMeetings)
      .catch(console.error)
  }, [])
  
  useEffect(() => {
    if (selectedMeeting) {
      fetch(`http://localhost:5001/sessions?meetingId=${selectedMeeting}`)
        .then(res => res.json())
        .then(setSessions)
        .catch(console.error)
    }
    else {
      setSessions([])
    }
    setSelectedSession('')
    setSelectedDrivers([])
  }, [selectedMeeting])

  useEffect(() => {
    if (selectedSession) {
      fetch(`http://localhost:5001/drivers?sessionId=${selectedSession}`)
        .then(res => res.json())
        .then(setDrivers)
        .catch(console.error)
    } else {
      setDrivers([])
    }
    setSelectedDrivers([])
  }, [selectedSession])

  useEffect(() => {
    if (selectedDrivers.length > 0){
      const fetchPromises = selectedDrivers.map(driverId =>
        fetch(`http://localhost:5001/laps?sessionId=${selectedSession}&driverId=${driverId}`)
          .then(res => res.json())
          .then(laps => ({ driverId, laps }))
      )

      Promise.all(fetchPromises)
        .then(results => {
          const lapsByDriver = {}
          results.forEach(({ driverId, laps }) => {
            lapsByDriver[driverId] = laps
          })
          setAllDriverLaps(lapsByDriver)
        })
        .catch(console.error)
    } else {
      setAllDriverLaps({})
    }
  }, [selectedDrivers, selectedSession])

  useEffect(() => {
    if (selectedDrivers.length > 0) {
      const fetchStintsPromises = selectedDrivers.map(driverId =>
        fetch(`http://localhost:5001/stints?sessionId=${selectedSession}&driverId=${driverId}`)
          .then(res => res.json())
          .then(stints => ({ driverId, stints }))
      )

      Promise.all(fetchStintsPromises)
        .then(results => {
          const stintsByDriver = {}
          results.forEach(({ driverId, stints }) => {
            stintsByDriver[driverId] = stints
          })
          setAllDriverStints(stintsByDriver)
        })
        .catch(console.error)
    } else {
      setAllDriverStints({})
    }
  }, [selectedDrivers, selectedSession])
  
  
  const getTyreInfoForLap = (driverId: number, lapNumber: number) => {
    const driverStints = allDriverStints[driverId] || []
    const stint = driverStints.find(s => lapNumber >= s.startLap && lapNumber <= s.endLap)
    return stint ? {
      compound: stint.compound,
      stintNumber: stint.stintNumber,
      tyreAge: stint.startTyreAge + (lapNumber - stint.startLap)
    } : null
  }

  const handleShowTelemetry = (lapNumber: number) => {
    const newExpandedRows = new Set(expandedRows)

    if (expandedRows.has(lapNumber)) {
      newExpandedRows.delete(lapNumber)
      setExpandedRows(newExpandedRows)
      return
    }

    newExpandedRows.add(lapNumber)
    setExpandedRows(newExpandedRows)

    const fetchPromises = selectedDrivers.map(driverId => {
      const driverLaps = allDriverLaps[driverId] || []
      const currentLap = driverLaps.find(l => l.lapNumber === lapNumber)

      if (currentLap) {
        const nextLap = driverLaps.find(l => l.lapNumber === lapNumber + 1)
        const dateFrom = currentLap.startDate
        let dateTo

        if (nextLap) {
          const nextLapDate = new Date(nextLap.startDate)
          nextLapDate.setMilliseconds(nextLapDate.getMilliseconds() - 1)
          dateTo = nextLapDate.toISOString()
        } else {
          const currentLapDate = new Date(currentLap.startDate)
          currentLapDate.setMinutes(currentLapDate.getMinutes() + 3)
          dateTo = currentLapDate.toISOString()
        }

        const telemetryUrl = `http://localhost:5001/telemetry?sessionId=${selectedSession}&driverId=${driverId}&dateFrom=${dateFrom}&dateTo=${dateTo}`

        return fetch(telemetryUrl)
          .then(res => res.json())
          .then(data => ({ driverId, data }))
          .catch(error => {
            console.error(error)
            return { driverId, data: [] }
          })
      }
      return Promise.resolve({ driverId, data: [] })
    })

    Promise.all(fetchPromises)
      .then(results => {
        const lapTelemetryData = {}
        results.forEach(({ driverId, data }) => {
          lapTelemetryData[driverId] = data
        })

        setTelemetryData(prev => ({
          ...prev,
          [lapNumber]: lapTelemetryData
        }))
      })
  }

  useEffect(() => {
    if (Object.keys(allDriverLaps).length > 0 && chartRef.current){
      if (chartInstanceRef.current){
        chartInstanceRef.current.destroy()
      }

      const colors = [ // fix - get teams colours
        'rgba(93, 1, 146, 1)',
        'rgba(255, 99, 132, 1)',
        'rgba(54, 162, 235, 1)',
        'rgba(255, 206, 86, 1)',
        'rgba(75, 192, 192, 1)',
        'rgba(153, 102, 255, 1)',
        'rgba(255, 159, 64, 1)'
      ]

      const datasets = []
      let allLapNumbers = new Set()

      Object.entries(allDriverLaps).forEach(([driverId, laps], index) => {
        var filteredLaps = laps.filter(x => x.lapDurationMs > 1)

        // IQR method
        if (filteredLaps.length > 4) {
          const sortedTimes = filteredLaps.map(x => x.lapDurationMs).sort((a, b) => a - b)
          const q1Index = Math.floor(sortedTimes.length * 0.25)
          const q3Index = Math.floor(sortedTimes.length * 0.75)
          const q1 = sortedTimes[q1Index]
          const q3 = sortedTimes[q3Index]
          const iqr = q3 - q1
          const lowerBound = q1 - (1.5 * iqr)
          const upperBound = q3 + (1.5 * iqr)

          filteredLaps = filteredLaps.filter(x => x.lapDurationMs >= lowerBound && x.lapDurationMs <= upperBound)
        }

        filteredLaps.forEach(lap => allLapNumbers.add(lap.lapNumber))

        const driver = drivers.find(d => d.id === parseInt(driverId))

        datasets.push({
          label: driver.nameAcronym,
          data: filteredLaps.map(lap => ({x: lap.lapNumber, y: lap.lapDurationMs})),
          borderColor: colors[index % colors.length],
          backgroundColor: colors[index % colors.length].replace('1)', '0.2)'),
          tension: 0.1,
          fill: false
        })
      })

      const sortedLapNumbers = Array.from(allLapNumbers).sort((a, b) => a - b)

      const context = chartRef.current.getContext('2d')
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
                      `Tyre Age: ${tyreInfo.tyreAge} laps`
                    ]
                  }
                  return []
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
                text: 'Lap Number'
              }
            }
          }}
      })
    }
    return () => {
      if (chartInstanceRef.current){
        chartInstanceRef.current.destroy()
      }
    }
  }, [allDriverLaps, allDriverStints, drivers, selectedDrivers])

  return (
      <div>
        <h2 className="text-xl font-bold pt-2 pb-2">Session data</h2>

        Select round:
        <select
          value={selectedMeeting}
          onChange={(e) => setSelectedMeeting(e.target.value)}
          className="w-full bg-transparent placeholder:text-slate-400 text-slate-700 text-sm border border-slate-200 rounded pl-3 pr-8 py-2 transition duration-300 ease focus:outline-none focus:border-slate-400 hover:border-slate-400 shadow-sm focus:shadow-md appearance-none cursor-pointer">
          <option value="">Choose round</option>
          {meetings.map((meeting) => (
            <option key={meeting.id} value={meeting.id}>{meeting.startDate} - {meeting.name}</option>
          ))}
        </select>

        {selectedMeeting && (
            <>
              Select session:
              <select
                value={selectedSession}
                onChange={(e) => setSelectedSession(e.target.value)}
                className="w-full bg-transparent placeholder:text-slate-400 text-slate-700 text-sm border border-slate-200 rounded pl-3 pr-8 py-2 transition duration-300 ease focus:outline-none focus:border-slate-400 hover:border-slate-400 shadow-sm focus:shadow-md appearance-none cursor-pointer">
                
                <option value="">Choose session</option>
                {sessions.map((session) => (
                  <option key={session.id} value={session.id}>{session.name}</option>
                ))}
              </select>
            </>
          )}

        {selectedSession && (
          <>
            Select drivers:
            <div className="border border-slate-200 rounded p-4 bg-white">
              <div className="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-3">
                {drivers.map((driver) => (
                  <label key={driver.id} className="flex items-center space-x-2 p-2 rounded hover:bg-gray-50 cursor-pointer border border-transparent hover:border-gray-200 transition-colors">
                    <input
                      type="checkbox"
                      checked={selectedDrivers.includes(driver.id)}
                      onChange={(e) => {
                        if (e.target.checked) {
                          setSelectedDrivers([...selectedDrivers, driver.id])
                        } else {
                          setSelectedDrivers(selectedDrivers.filter(id => id !== driver.id))
                        }
                      }}
                      className="rounded"
                    />
                    <div className="flex items-center space-x-2 min-w-0">
                      {driver.headshotUrl && (
                        <img
                          src={driver.headshotUrl}
                          alt={driver.nameAcronym}
                          className="w-6 h-6 rounded-full object-cover flex-shrink-0"
                        />
                      )}
                      <div className="min-w-0">
                        <div className="text-sm font-medium text-slate-700">{driver.nameAcronym}</div>
                        <div className="text-xs text-slate-500 truncate">{driver.firstName} {driver.lastName}</div>
                      </div>
                    </div>
                  </label>
                ))}
              </div>
            </div>
          </>
          )}

        {selectedDrivers.length > 0 && (
          <>
            <div className="mt-6 mb-6">
              <canvas ref={chartRef} width="800" height="400"></canvas>
            </div>

            <button
              onClick={() => setShowTable(!showTable)}
              className="mb-4 px-4 py-2 bg-blue-500 text-white rounded hover:bg-blue-600 transition-colors"
            >
              {showTable ? 'Hide lap times table' : 'Show lap times table'}
            </button>

            {showTable && (
            <div className="overflow-x-auto">
              <table className="w-full text-left table-auto">
                <thead>
                  <tr>
                    <th className="p-2 md:p-4 border-b border-blue-gray-100 bg-blue-gray-50 sticky left-0 bg-blue-gray-50 min-w-[60px]">Lap</th>
                    {selectedDrivers.map(driverId => {
                      const driver = drivers.find(d => d.id === parseInt(driverId))
                      return (
                        <th key={driverId} className="p-2 md:p-4 border-b border-blue-gray-100 bg-blue-gray-50 min-w-[100px] text-center">
                          <div className="flex flex-col items-center space-y-1">
                            {driver?.headshotUrl && (
                              <img
                                src={driver.headshotUrl}
                                alt={driver.nameAcronym}
                                className="w-8 h-8 rounded-full object-cover"
                              />
                            )}
                            <span className="text-xs font-medium">{driver.nameAcronym}</span>
                          </div>
                        </th>
                      )
                    })}
                    <th className="p-2 md:p-4 border-b border-blue-gray-100 bg-blue-gray-50 min-w-[120px] text-center">Actions</th>
                  </tr>
                </thead>
                <tbody>
                  {(() => {
                      
                    const allLapNumbers = new Set()
                    Object.values(allDriverLaps).forEach(laps => {
                      laps.forEach(lap => allLapNumbers.add(lap.lapNumber))
                    })
                    const sortedLapNumbers = Array.from(allLapNumbers).sort((a, b) => a - b)

                    return sortedLapNumbers.flatMap(lapNumber => {
                      const rows = []

                      rows.push(
                        <tr key={lapNumber}>
                          <td className="p-2 md:p-4 border-b border-blue-gray-50 font-medium sticky left-0 bg-white min-w-[60px]">{lapNumber}</td>
                          {selectedDrivers.map(driverId => {
                            const driverLaps = allDriverLaps[driverId] || []
                            const lap = driverLaps.find(l => l.lapNumber === lapNumber)
                            const tyreInfo = getTyreInfoForLap(driverId, lapNumber)

                            return (
                              <td key={`${lapNumber}-${driverId}`} className="p-2 md:p-4 border-b border-blue-gray-50 text-center text-sm">
                                {lap ? (
                                  <div className="flex flex-col items-center space-y-1">
                                    <span className="font-medium">{millisecondsToMinuteFormat(lap.lapDurationMs)}</span>
                                    {tyreInfo && (
                                      <div className="text-xs text-gray-600 flex items-center space-x-1">
                                        <span className="px-1 py-0.5 bg-gray-100 rounded text-xs font-mono">
                                          {tyreInfo.compound}
                                        </span>
                                        <span>S{tyreInfo.stintNumber}</span>
                                        <span>{tyreInfo.tyreAge}L</span>
                                      </div>
                                    )}
                                  </div>
                                ) : '-'}
                              </td>
                            )
                          })}
                          <td className="p-2 md:p-4 border-b border-blue-gray-50 text-center">
                            <button
                              onClick={() => handleShowTelemetry(lapNumber)}
                              className={`px-3 py-1 text-white text-xs rounded transition-colors ${
                                expandedRows.has(lapNumber)
                                  ? 'bg-red-500 hover:bg-red-600'
                                  : 'bg-green-500 hover:bg-green-600'
                              }`}
                            >
                              {expandedRows.has(lapNumber) ? 'Hide Telemetry' : 'Show Telemetry'}
                            </button>
                          </td>
                        </tr>
                      )

                      if (expandedRows.has(lapNumber) && telemetryData[lapNumber]) {
                        rows.push(
                          <tr key={`${lapNumber}-telemetry`}>
                            <td colSpan={selectedDrivers.length + 2} className="p-4 bg-gray-50 border-b border-blue-gray-50">
                              <TelemetryChart
                                lapNumber={lapNumber}
                                telemetryData={telemetryData[lapNumber]}
                                selectedDrivers={selectedDrivers}
                                drivers={drivers}
                              />
                            </td>
                          </tr>
                        )
                      }

                      return rows
                    })
                  })()}
                </tbody>
              </table>
            </div>
            )}
          </>
        )}
      </div>
  )

  function millisecondsToMinuteFormat(ms: number): string {
    const totalSeconds = ms / 1000;
    const minutes = Math.floor(totalSeconds / 60);
    const seconds = Math.floor(totalSeconds % 60);
    const milliseconds = ms % 1000;
    
    return `${minutes}:${seconds.toString().padStart(2, '0')}.${milliseconds.toString().padStart(3, '0')}`;
  }
}