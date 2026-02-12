export interface Booking{
  id: string;
  userId: string;
  screeningId: string;
  seatIds: string[];
  totalPrice: number;
}

export interface CreateBookingRequestInput{
  screeningId: string;
  seatsIds: string[];
}
