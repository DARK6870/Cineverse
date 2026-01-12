import {
  Component,
  computed,
  EventEmitter,
  inject,
  Input,
  OnInit,
  Output, signal,
} from '@angular/core';
import { Hall, Seat } from '../../../features/hall/api/hall.graphql.types';
import { Button } from 'primeng/button';
import { Screening } from '../../../features/screening/api/screening.graphql.types';
import { BookingGraphqlService } from '../../../features/booking/api/booking.graphql.service';
import { firstValueFrom } from 'rxjs';
import { formatDate } from '@cineverse/infrastructure-common';
import { Booking } from '../../../features/booking/api/booking.graphql.types';

@Component({
  selector: 'app-seat-selector',
  imports: [Button],
  templateUrl: 'seat-selector.html',
  styleUrl: 'seat-selector.css',
})
export class SeatSelector implements OnInit {
  private bookingGraphQlService = inject(BookingGraphqlService);
  protected readonly formatDate = formatDate;

  @Input() hall!: Hall;
  @Input() screening!: Screening;
  @Input() booking: Booking | null = null;
  @Output() confirm = new EventEmitter<string[]>();

  bookedSeats = signal<string[]>([]);
  selectedSeats = new Set<string>();

  async ngOnInit() {
    if (this.booking) {
      this.bookedSeats.set(this.booking.seatIds);
      this.selectedSeats = new Set(this.booking.seatIds);
    }
    else
    {
      const bookedSeats = await firstValueFrom(
        this.bookingGraphQlService.getBookedSeats(this.screening.id),
      );
      this.bookedSeats.set(bookedSeats);
    }
  }


  seatMatrix = computed(() => {
    if (!this.hall || !this.hall.seats) return [];
    return this.groupSeatsByRow(this.hall.seats);
  });

  private groupSeatsByRow(seats: Seat[]) {
    const rows = new Map<number, Seat[]>();

    seats.forEach((seat) => {
      if (!rows.has(seat.row)) rows.set(seat.row, []);
      rows.get(seat.row)!.push(seat);
    });

    return Array.from(rows.entries())
      .map(([row, seats]) => ({
        row,
        seats: seats.sort((a, b) => a.number - b.number),
      }))
      .sort((a, b) => a.row - b.row);
  }

  toggleSeat(id: string) {
    if (this.bookedSeats().includes(id)) {
      return;
    }

    this.selectedSeats.has(id)
      ? this.selectedSeats.delete(id)
      : this.selectedSeats.add(id);
  }

  onConfirmClick() {
    this.confirm.emit([...this.selectedSeats]);
  }
}
