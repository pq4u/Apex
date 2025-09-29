export interface Meeting {
  id: number
  name: string
  startDate: string
}

export interface Session {
  id: number
  name: string
}

export interface Driver {
  id: number
  nameAcronym: string
  firstName: string
  lastName: string
  headshotUrl?: string
}

export interface Lap {
  lapNumber: number
  lapDurationMs: number
  startDate: string
}

export interface Stint {
  startLap: number
  endLap: number
  compound: string
  stintNumber: number
  startTyreAge: number
}

export interface TelemetryPoint {
  time: string
  speed: number
  throttle: number
  brake: number
}

export interface TyreInfo {
  compound: string
  stintNumber: number
  tyreAge: number
}