'use client'
import { Driver, Lap, Stint, TyreInfo, TelemetryPoint } from '../types'
import { TelemetryChart } from './TelemetryChart'

interface LapTimesTableProps {
  allDriverLaps: Record<string, Lap[]>;
  allDriverStints: Record<string, Stint[]>;
  selectedDrivers: number[];
  drivers: Driver[];
  expandedRows: Set<number>;
  telemetryData: Record<number, Record<string, TelemetryPoint[]>>;
  onToggleTelemetry: (lapNumber: number) => void;
}

export function LapTimesTable({
  allDriverLaps,
  allDriverStints,
  selectedDrivers,
  drivers,
  expandedRows,
  telemetryData,
  onToggleTelemetry
}: LapTimesTableProps) {

  const getTyreInfoForLap = (driverId: number, lapNumber: number): TyreInfo | null => {
    const driverStints = allDriverStints[driverId] || []
    const stint = driverStints.find(s => lapNumber >= s.startLap && lapNumber <= s.endLap)
    return stint ? {
      compound: stint.compound,
      stintNumber: stint.stintNumber,
      tyreAge: stint.startTyreAge + (lapNumber - stint.startLap)
    } : null;
  };

  const millisecondsToMinuteFormat = (ms: number): string => {
    const totalSeconds = ms / 1000;
    const minutes = Math.floor(totalSeconds / 60)
    const seconds = Math.floor(totalSeconds % 60);
    const milliseconds = ms % 1000;

    return `${minutes}:${seconds.toString().padStart(2, '0')}.${milliseconds.toString().padStart(3, '0')}`;
  };
 
  const allLapNumbers = new Set<number>();
  Object.values(allDriverLaps).forEach(laps => {
    laps.forEach(lap => allLapNumbers.add(lap.lapNumber));
  });
  const sortedLapNumbers = Array.from(allLapNumbers).sort((a, b) => a - b);


  return (
    <div className="overflow-x-auto">
      <table className="w-full text-left table-auto">
        <thead>
          <tr>
            <th className="p-2 md:p-4 border-b border-blue-gray-100 bg-blue-gray-50 sticky left-0 bg-blue-gray-50 min-w-[60px]">
              Lap
            </th>
            {selectedDrivers.map(driverId => {
              const driver = drivers.find(d => d.id === driverId)
              return (
                <th
                  key={driverId}
                  className="p-2 md:p-4 border-b border-blue-gray-100 bg-blue-gray-50 min-w-[100px] text-center"
                >
                  <div className="flex flex-col items-center space-y-1">
                    {driver?.headshotUrl && (
                      <img
                        src={driver.headshotUrl}
                        alt={driver.nameAcronym}
                        className="w-8 h-8 rounded-full object-cover"
                      />
                    )}
                    <span className="text-xs font-medium">{driver?.nameAcronym}</span>
                  </div>
                </th>
              )
            })}
            <th className="p-2 md:p-4 border-b border-blue-gray-100 bg-blue-gray-50 min-w-[120px] text-center">
              Actions
            </th>
          </tr>
        </thead>
        <tbody>
          {sortedLapNumbers.flatMap(lapNumber => {
            const rows = []

            // main lap row
            rows.push(
              <tr key={lapNumber}>
                <td className="p-2 md:p-4 border-b border-blue-gray-50 font-medium sticky left-0 bg-white min-w-[60px]">
                  {lapNumber}
                </td>
                {selectedDrivers.map(driverId => {
                  const driverLaps = allDriverLaps[driverId] || []
                  const lap = driverLaps.find(l => l.lapNumber === lapNumber)
                  const tyreInfo = getTyreInfoForLap(driverId, lapNumber)

                  return (
                    <td
                      key={`${lapNumber}-${driverId}`}
                      className="p-2 md:p-4 border-b border-blue-gray-50 text-center text-sm">

                      {lap ? (
                        <div className="flex flex-col items-center space-y-1">
                          <span className="font-medium">
                            {millisecondsToMinuteFormat(lap.lapDurationMs)}
                          </span>

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
                      ) : (
                        '-'
                      )}
                    </td>
                  )
                })}

                <td className="p-2 md:p-4 border-b border-blue-gray-50 text-center">
                  <button
                    onClick={() => onToggleTelemetry(lapNumber)}
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

            // telemetry
            if (expandedRows.has(lapNumber) && telemetryData[lapNumber]) {
              rows.push(
                <tr key={`${lapNumber}-telemetry`}>
                  <td
                    colSpan={selectedDrivers.length + 2}
                    className="p-4 bg-gray-50 border-b border-blue-gray-50"
                  >
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
          })}
        </tbody>
      </table>
    </div>
  )
}