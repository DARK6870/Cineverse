export interface Hall{
  id: string,
  name: string,
  seats: Seat[]
}

export interface Seat{
  seatId: string,
  row: number,
  number: number
}
