'use client'
import { Meeting, Session, Driver } from '../types'

interface SessionSelectorsProps {
  meetings: Meeting[];
  sessions: Session[];
  drivers: Driver[];
  selectedMeeting: string;
  selectedSession: string;
  selectedDrivers: number[];
  onMeetingChange: (meetingId: string) => void;
  onSessionChange: (sessionId: string) => void;
  onDriversChange: (driverIds: number[]) => void;
}

export function SessionSelectors({
  meetings,
  sessions,
  drivers,
  selectedMeeting,
  selectedSession,
  selectedDrivers,
  onMeetingChange,
  onSessionChange,
  onDriversChange
}: SessionSelectorsProps) {

  const handleDriverToggle = (driverId: number, checked: boolean) => {
    if (checked) {
      onDriversChange([...selectedDrivers, driverId]);
    } else {
      onDriversChange(selectedDrivers.filter(id => id !== driverId));
    }

  };

  return (
    <div className="space-y-4">

      {/* meeting */}
      <div>
        <label className="block text-sm font-medium mb-2">Select round:</label>
        <select
          value={selectedMeeting}
          onChange={(e) => onMeetingChange(e.target.value)}
          className="w-full bg-transparent placeholder:text-slate-400 text-slate-700 text-sm border border-slate-200 rounded pl-3 pr-8 py-2 transition duration-300 ease focus:outline-none focus:border-slate-400 hover:border-slate-400 shadow-sm focus:shadow-md appearance-none cursor-pointer"
        >
          <option value="">Choose round</option>
          {meetings.map((meeting) => (
            <option key={meeting.id} value={meeting.id}>
              {meeting.startDate} - {meeting.name}
            </option>
          ))}
        </select>
      </div>

      {/* session */}
      {selectedMeeting && (
        <div>
          <label className="block text-sm font-medium mb-2">Select session:</label>
          <select
            value={selectedSession}
            onChange={(e) => onSessionChange(e.target.value)}
            className="w-full bg-transparent placeholder:text-slate-400 text-slate-700 text-sm border border-slate-200 rounded pl-3 pr-8 py-2 transition duration-300 ease focus:outline-none focus:border-slate-400 hover:border-slate-400 shadow-sm focus:shadow-md appearance-none cursor-pointer"
          >
            <option value="">Choose session</option>
            {sessions.map((session) => (
              <option key={session.id} value={session.id}>
                {session.name}
              </option>
            ))}
          </select>
        </div>
      )}

      {/* driver */}
      {selectedSession && (
        <div>
          <label className="block text-sm font-medium mb-2">Select drivers:</label>
          <div className="border border-slate-200 rounded p-4 bg-white">
            <div className="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-3">
              {drivers.map((driver) => (
                <label
                  key={driver.id}
                  className="flex items-center space-x-2 p-2 rounded hover:bg-gray-50 cursor-pointer border border-transparent hover:border-gray-200 transition-colors"
                >
                  <input
                    type="checkbox"
                    checked={selectedDrivers.includes(driver.id)}
                    onChange={(e) => handleDriverToggle(driver.id, e.target.checked)}
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
                      <div className="text-xs text-slate-500 truncate">
                        {driver.firstName} {driver.lastName}
                      </div>
                    </div>
                  </div>
                </label>
              ))}
            </div>
          </div>
        </div>
      )}

    </div>
  )
}