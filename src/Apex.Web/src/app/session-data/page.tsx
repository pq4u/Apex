'use client'
import { useEffect, useState } from 'react'
import * as Chart from 'chart.js'
import { SessionSelectors } from './components/SessionSelectors'
import { LapTimesChart } from './components/LapTimesChart'
import { LapTimesTable } from './components/LapTimesTable'
import { useSessionData } from './hooks/useSessionData'
import { useTelemetryData } from './hooks/useTelemetryData'

export default function SessionDataPage() {
  const [showTable, setShowTable] = useState(false);

  useEffect(() => {
    Chart.Chart.register(...Chart.registerables);
  }, [])

  // custom hooks for data
  const {
    meetings,
    sessions,
    drivers,
    allDriverLaps,
    allDriverStints,
    selectedMeeting,
    selectedSession,
    selectedDrivers,
    setSelectedMeeting,
    setSelectedSession,
    setSelectedDrivers
  } = useSessionData();

  const {
    expandedRows,
    telemetryData,
    fetchTelemetryForLap,
    getTelemetryForLap
  } = useTelemetryData();

  const handleTelemetryToggle = (lapNumber: number) => {
    fetchTelemetryForLap(lapNumber, selectedDrivers, allDriverLaps, selectedSession)
  };

  const hasSelectedDrivers = selectedDrivers.length > 0;

  return (
    <div className="container mx-auto p-4">
      <h2 className="text-xl font-bold pt-2 pb-2">Session data</h2>

      <SessionSelectors
        meetings={meetings}
        sessions={sessions}
        drivers={drivers}
        selectedMeeting={selectedMeeting}
        selectedSession={selectedSession}
        selectedDrivers={selectedDrivers}
        onMeetingChange={setSelectedMeeting}
        onSessionChange={setSelectedSession}
        onDriversChange={setSelectedDrivers}
      />

      {hasSelectedDrivers && (
        <div className="mt-6 space-y-6">
          <LapTimesChart
            allDriverLaps={allDriverLaps}
            allDriverStints={allDriverStints}
            selectedDrivers={selectedDrivers}
            drivers={drivers}
          />

          <div>
            <button
              onClick={() => setShowTable(!showTable)}
              className="mb-4 px-4 py-2 bg-blue-500 text-white rounded hover:bg-blue-600 transition-colors"
            >
              {showTable ? 'Hide lap times table' : 'Show lap times table'}
            </button>

            {showTable && (
              <LapTimesTable
                allDriverLaps={allDriverLaps}
                allDriverStints={allDriverStints}
                selectedDrivers={selectedDrivers}
                drivers={drivers}
                expandedRows={expandedRows}
                telemetryData={telemetryData}
                onToggleTelemetry={handleTelemetryToggle}
              />
            )}
          </div>
        </div>
      )}
    </div>
  )
}