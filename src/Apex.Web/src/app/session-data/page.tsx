'use client'
import { useState, useEffect } from 'react'

export default function Stats() {
  const [meetings, setMeetings] = useState([])
  const [sessions, setSessions] = useState([])
  const [drivers, setDrivers] = useState([])
  const [laps, setLaps] = useState([]) 
  const [selectedMeeting, setSelectedMeeting] = useState('')
  const [selectedSession, setSelectedSession] = useState('')
  const [selectedDriver, setSelectedDriver] = useState('')
  
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
  }, [selectedSession])

  useEffect(() => {
    if (selectedDriver){
      fetch(`http://localhost:5001/laps?sessionId=${selectedSession}&driverId=${selectedDriver}`)
        .then(res => res.json())
        .then(setLaps)
        .catch(console.error)
    } else {
      setLaps([])
    }
  }, [selectedDriver])
  
  return (
    <div>
      <h1>Apex</h1>
      <div className="w-full max-w-sm min-w-[200px]">
      <div className="relative">
        Select round:
        <select
          value={selectedMeeting}
          onChange={(e) => setSelectedMeeting(e.target.value)}
          className="w-full bg-transparent placeholder:text-slate-400 text-slate-700 text-sm border border-slate-200 rounded pl-3 pr-8 py-2 transition duration-300 ease focus:outline-none focus:border-slate-400 hover:border-slate-400 shadow-sm focus:shadow-md appearance-none cursor-pointer">
          <option value="">Choose round</option>
          {meetings.map((meeting) => (
            <option key={meeting.id} value={meeting.id}>{meeting.name}</option>
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
            Select driver:
            <select
              value={selectedDriver}
              onChange={(e) => setSelectedDriver(e.target.value)}
              className="w-full bg-transparent placeholder:text-slate-400 text-slate-700 text-sm border border-slate-200 rounded pl-3 pr-8 py-2 transition duration-300 ease focus:outline-none focus:border-slate-400 hover:border-slate-400 shadow-sm focus:shadow-md appearance-none cursor-pointer">
              <option value="">Choose driver</option>
              {drivers.map((driver) => (
                <option key={driver.id} value={driver.id}>{driver.firstName} {driver.lastName}</option>
              ))}
            </select>
          </>
          )}

        {selectedDriver && (
          <table className="w-full text-left table-auto min-w-max">
            <thead>
              <tr>
                <th className="p-4 border-b border-blue-gray-100 bg-blue-gray-50">Lap number</th>
                <th className="p-4 border-b border-blue-gray-100 bg-blue-gray-50">Lap time</th>
              </tr>
            </thead>
            <tbody>
              {laps.map((lap) => (
                <tr key={lap.lapNumber}>
                  <td className="p-4 border-b border-blue-gray-50">{lap.lapNumber}</td>
                  <td className="p-4 border-b border-blue-gray-50">{millisecondsToMinuteFormat(lap.lapDurationMs)}</td>
                </tr>
              ))}
            </tbody>
          </table>
        )}
      </div>
    </div>
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