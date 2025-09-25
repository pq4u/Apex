'use client'
import { useState, useEffect } from 'react'

export default function Stats() {
  const [meetings, setMeetings] = useState([])
  const [sessions, setSessions] = useState([])
  const [selectedMeeting, setSelectedMeeting] = useState('')
  
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
                className="w-full bg-transparent placeholder:text-slate-400 text-slate-700 text-sm border border-slate-200 rounded pl-3 pr-8 py-2 transition duration-300 ease focus:outline-none focus:border-slate-400 hover:border-slate-400 shadow-sm focus:shadow-md appearance-none cursor-pointer">
                <option value="">Choose session</option>
                {sessions.map((session) => (
                  <option key={session.id} value={session.id}>{session.name}</option>
                ))}
              </select>
            </>
          )}
      </div>
    </div>
    </div>
  )
}