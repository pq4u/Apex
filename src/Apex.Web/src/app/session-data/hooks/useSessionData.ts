'use client'
import { useState, useEffect } from 'react'
import { Meeting, Session, Driver, Lap, Stint } from '../types'

const API_BASE_URL = 'http://localhost:5000'

export function useSessionData() {
  const [meetings, setMeetings] = useState<Meeting[]>([]);
  const [sessions, setSessions] = useState<Session[]>([]);
  const [drivers, setDrivers] = useState<Driver[]>([]);
  const [allDriverLaps, setAllDriverLaps] = useState<Record<string, Lap[]>>({});
  const [allDriverStints, setAllDriverStints] = useState<Record<string, Stint[]>>({});

  const [selectedMeeting, setSelectedMeeting] = useState('');
  const [selectedSession, setSelectedSession] = useState('');
  const [selectedDrivers, setSelectedDrivers] = useState<number[]>([]);

  useEffect(() => {
    const fetchMeetings = async () => {
      try {
        const response = await fetch(`${API_BASE_URL}/meetings`);
        const data = await response.json();
        setMeetings(data);
      } catch (error) {
        console.error('Failed to fetch meetings:', error);
      }
    };

    fetchMeetings()
  }, [])

  useEffect(() => {
    const fetchSessions = async () => {
      if (!selectedMeeting) {
        setSessions([]);
        return;
      }

      try {
        const response = await fetch(`${API_BASE_URL}/sessions?meetingId=${selectedMeeting}`);
        const data = await response.json();
        setSessions(data);
      } catch (error) {
        console.error('Failed to fetch sessions:', error);
      }
    }

    fetchSessions();
    setSelectedSession('');
    setSelectedDrivers([]);
  }, [selectedMeeting]);

  useEffect(() => {
    const fetchDrivers = async () => {
      if (!selectedSession) {
        setDrivers([]);
        return;
      }

      try {
        const response = await fetch(`${API_BASE_URL}/drivers?sessionId=${selectedSession}`)
        const data = await response.json()
        setDrivers(data)
      } catch (error) {
        console.error('Failed to fetch drivers:', error)
      }
    };

    fetchDrivers();
    setSelectedDrivers([]);
  }, [selectedSession]);

  useEffect(() => {
    const fetchLaps = async () => {
      if (selectedDrivers.length === 0) {
        setAllDriverLaps({});
        return;
      }

      try {
        const fetchPromises = selectedDrivers.map(driverId =>
          fetch(`${API_BASE_URL}/laps?sessionId=${selectedSession}&driverId=${driverId}`)
            .then(res => res.json())
            .then(laps => ({ driverId, laps }))
        );

        const results = await Promise.all(fetchPromises);
        const lapsByDriver: Record<string, Lap[]> = {};

        results.forEach(({ driverId, laps }) => {
          lapsByDriver[driverId] = laps
        });

        setAllDriverLaps(lapsByDriver);
      } catch (error) {
        console.error('Failed to fetch laps:', error);
      }
    }

    fetchLaps();
  }, [selectedDrivers, selectedSession]);

  useEffect(() => {
    const fetchStints = async () => {
      if (selectedDrivers.length === 0) {
        setAllDriverStints({});
        return;
      }

      try {
        const fetchPromises = selectedDrivers.map(driverId =>
          fetch(`${API_BASE_URL}/stints?sessionId=${selectedSession}&driverId=${driverId}`)
            .then(res => res.json())
            .then(stints => ({ driverId, stints }))
        );

        const results = await Promise.all(fetchPromises);
        const stintsByDriver: Record<string, Stint[]> = {};

        results.forEach(({ driverId, stints }) => {
          stintsByDriver[driverId] = stints;
        })

        setAllDriverStints(stintsByDriver);
      } catch (error) {
        console.error('Failed to fetch stints:', error);
      }
    }

    fetchStints()
  }, [selectedDrivers, selectedSession]);

  return {
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
  };
}