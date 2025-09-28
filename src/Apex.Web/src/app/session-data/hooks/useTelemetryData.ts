'use client'
import { useState } from 'react'
import { TelemetryPoint, Lap } from '../types'

const API_BASE_URL = 'http://localhost:5001'

export function useTelemetryData() {
  const [expandedRows, setExpandedRows] = useState<Set<number>>(new Set());
  const [telemetryData, setTelemetryData] = useState<Record<number, Record<string, TelemetryPoint[]>>>({});

  const fetchTelemetryForLap = async (
    lapNumber: number,
    selectedDrivers: number[],
    allDriverLaps: Record<string, Lap[]>,
    selectedSession: string
  ) => {
    const newExpandedRows = new Set(expandedRows);

    // toggle expansion
    if (expandedRows.has(lapNumber)) {
      newExpandedRows.delete(lapNumber);
      setExpandedRows(newExpandedRows);
      return;
    }

    newExpandedRows.add(lapNumber);
    setExpandedRows(newExpandedRows);

    try {
      const fetchPromises = selectedDrivers.map(async (driverId) => {
        const driverLaps = allDriverLaps[driverId] || [];
        const currentLap = driverLaps.find(l => l.lapNumber === lapNumber);

        if (!currentLap) {
          return { driverId, data: [] };
        }

        const nextLap = driverLaps.find(l => l.lapNumber === lapNumber + 1);
        const dateFrom = currentLap.startDate;
        let dateTo: string;

        if (nextLap) {
          const nextLapDate = new Date(nextLap.startDate);
          nextLapDate.setMilliseconds(nextLapDate.getMilliseconds() - 1);
          dateTo = nextLapDate.toISOString();
        } else {
          const currentLapDate = new Date(currentLap.startDate);
          currentLapDate.setMinutes(currentLapDate.getMinutes() + 3);
          dateTo = currentLapDate.toISOString();
        }

        const telemetryUrl = `${API_BASE_URL}/telemetry?sessionId=${selectedSession}&driverId=${driverId}&dateFrom=${dateFrom}&dateTo=${dateTo}`;

        try {
          const response = await fetch(telemetryUrl);
          const data = await response.json();
          return { driverId, data };
        } catch (error) {
          console.error(`Failed to fetch telemetry for driver ${driverId}:`, error);
          return { driverId, data: [] };
        }
      });

      const results = await Promise.all(fetchPromises);
      const lapTelemetryData: Record<string, TelemetryPoint[]> = {};

      results.forEach(({ driverId, data }) => {
        lapTelemetryData[driverId] = data
      });

      setTelemetryData(prev => ({
        ...prev,
        [lapNumber]: lapTelemetryData
      }))
    } catch (error) {
      console.error('failed to fetch telemetry data:', error);
    }
  };

  const isRowExpanded = (lapNumber: number) => expandedRows.has(lapNumber);

  const getTelemetryForLap = (lapNumber: number) => telemetryData[lapNumber] || {};

  return {
    expandedRows,
    telemetryData,
    fetchTelemetryForLap,
    isRowExpanded,
    getTelemetryForLap
  };
}