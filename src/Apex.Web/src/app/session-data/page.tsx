'use client'
import * as Chart from 'chart.js'
import { useState, useEffect, useRef } from 'react'

export default function Stats() {
  const [meetings, setMeetings] = useState([])
  const [sessions, setSessions] = useState([])
  const [drivers, setDrivers] = useState([])
  const [allDriverLaps, setAllDriverLaps] = useState({}) 
  const [selectedMeeting, setSelectedMeeting] = useState('')
  const [selectedSession, setSelectedSession] = useState('')
  const [selectedDrivers, setSelectedDrivers] = useState([])
  
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
    if (Object.keys(allDriverLaps).length > 0 && chartRef.current){
      if (chartInstanceRef.current){
        chartInstanceRef.current.destroy()
      }

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
      let allLapNumbers = new Set()

      Object.entries(allDriverLaps).forEach(([driverId, laps], index) => {
        var filteredLaps = laps.filter(x => x.lapDurationMs > 1)

        // Remove anomalies (outliers) using IQR method
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
        const driverName = driver ? `${driver.firstName} ${driver.lastName}` : `Driver ${driverId}`

        datasets.push({
          label: driverName,
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
  }, [allDriverLaps, drivers])

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
            <div className="border border-slate-200 rounded p-3 bg-white">
              {drivers.map((driver) => (
                <label key={driver.id} className="flex items-center space-x-2 mb-2 cursor-pointer">
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
                  <span className="text-sm text-slate-700">{driver.firstName} {driver.lastName}</span>
                </label>
              ))}
            </div>
          </>
          )}

        {selectedDrivers.length > 0 && (
          <>
            <div className="mt-6 mb-6">
              <canvas ref={chartRef} width="800" height="400"></canvas>
            </div>
            
            <table className="w-full text-left table-auto min-w-max">
              <thead>
                <tr>
                  <th className="p-4 border-b border-blue-gray-100 bg-blue-gray-50">Driver</th>
                  <th className="p-4 border-b border-blue-gray-100 bg-blue-gray-50">Lap number</th>
                  <th className="p-4 border-b border-blue-gray-100 bg-blue-gray-50">Lap time</th>
                </tr>
              </thead>
              <tbody>
                {Object.entries(allDriverLaps).flatMap(([driverId, laps]) => {
                  const driver = drivers.find(d => d.id === parseInt(driverId))
                  const driverName = driver ? `${driver.firstName} ${driver.lastName}` : `Driver ${driverId}`
                  return laps.map((lap) => (
                    <tr key={`${driverId}-${lap.lapNumber}`}>
                      <td className="p-4 border-b border-blue-gray-50">{driverName}</td>
                      <td className="p-4 border-b border-blue-gray-50">{lap.lapNumber}</td>
                      <td className="p-4 border-b border-blue-gray-50">{millisecondsToMinuteFormat(lap.lapDurationMs)}</td>
                    </tr>
                  ))
                })}
              </tbody>
            </table>
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