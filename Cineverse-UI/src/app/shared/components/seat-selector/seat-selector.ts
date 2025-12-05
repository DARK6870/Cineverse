import {
  Component,
  computed,
  EventEmitter,
  Input,
  Output,
} from '@angular/core';
import { Hall, Seat } from '../../../features/hall/api/hall.graphql.types';
import { Button } from 'primeng/button';

@Component({
  selector: 'app-seat-selector',
  imports: [Button],
  templateUrl: 'seat-selector.html',
  styleUrl: 'seat-selector.css',
})
export class SeatSelector {
  @Input() hall!: Hall;
  @Input() bookedSeatsIds!: string[];
  @Input() ticketPrice!: number;

  @Output() confirm = new EventEmitter<string[]>();

  selectedSeats = new Set<string>();

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
    if (this.bookedSeatsIds.includes(id)) {
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
