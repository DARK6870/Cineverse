import { CommonModule } from '@angular/common';
import { Component, computed, inject, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { AbstractControl, FormArray, FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { InputTextModule } from 'primeng/inputtext';
import { Message } from 'primeng/message';
import { ConfirmationService } from 'primeng/api';
import { ToastService, getValidationError } from '@cineverse/infrastructure-common';
import { CreateHallRequestInput, HallSeatInput } from '../../api/hall.graphql.types';
import { HallsFacade } from '../../store/halls.facade';
import { HasUnsavedChanges } from '../../../../core/guards/unsaved-changes.guard';

@Component({
  selector: 'app-hall-add-edit',
  imports: [
    CommonModule,
    RouterLink,
    ReactiveFormsModule,
    ButtonModule,
    ConfirmDialogModule,
    InputTextModule,
    Message,
  ],
  templateUrl: 'hall-add-edit.html',
  styleUrl: 'hall-add-edit.css',
})
export class HallAddEdit implements HasUnsavedChanges {
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private hallsFacade = inject(HallsFacade);
  private fb = inject(FormBuilder);
  private toastService = inject(ToastService);
  private confirmationService = inject(ConfirmationService);

  hallId = signal<string | null>(null);
  isEdit = computed(() => Boolean(this.hallId()));
  formSubmitted = false;
  seatsError = signal<string | null>(null);

  form = this.fb.group({
    name: ['', Validators.required],
    rowCount: [1, [Validators.required, Validators.min(1)]],
    seatsPerRow: [10, [Validators.required, Validators.min(1)]],
    seats: this.fb.array([]),
  });

  constructor() {
    this.route.paramMap.pipe(takeUntilDestroyed()).subscribe((params) => {
      const id = params.get('id');
      this.hallId.set(id);
      if (id) {
        this.loadHall(id);
      } else {
        this.resetForm();
      }
    });
  }

  get seatsArray(): FormArray {
    return this.form.get('seats') as FormArray;
  }

  hasUnsavedChanges(): boolean {
    return this.form.dirty;
  }

  getErrorMessageByName(controlName: string): string | null {
    return getValidationError(this.form.get(controlName));
  }

  isInvalid(controlName: string): boolean {
    const control = this.form.get(controlName);
    return !!(control?.invalid && (control.touched || this.formSubmitted));
  }

  addSeat(row = 1, number = 1): void {
    this.seatsArray.push(this.createSeatGroup(row, number));
    this.form.markAsDirty();
  }

  removeSeat(index: number): void {
    this.seatsArray.removeAt(index);
    this.form.markAsDirty();
  }

  generateSeats(): void {
    const rowCount = this.toPositiveInteger(this.form.get('rowCount')?.value);
    const seatsPerRow = this.toPositiveInteger(this.form.get('seatsPerRow')?.value);

    if (rowCount < 1 || seatsPerRow < 1) {
      this.form.get('rowCount')?.markAsTouched();
      this.form.get('seatsPerRow')?.markAsTouched();
      return;
    }

    this.seatsArray.clear();
    for (let row = 1; row <= rowCount; row += 1) {
      for (let seatNumber = 1; seatNumber <= seatsPerRow; seatNumber += 1) {
        this.seatsArray.push(this.createSeatGroup(row, seatNumber));
      }
    }

    this.seatsError.set(null);
    this.form.markAsDirty();
  }

  totalSeats(): number {
    return this.seatsArray.length;
  }

  totalRows(): number {
    const rows = new Set(
      this.seatsArray.controls
        .map((control) => this.toPositiveInteger(control.get('row')?.value))
        .filter((row) => row > 0),
    );
    return rows.size;
  }

  saveHall(): void {
    this.formSubmitted = true;
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const seats = this.buildSeatPayload();
    if (!seats) {
      return;
    }

    this.confirmationService.confirm({
      header: 'Confirm Save',
      message: this.isEdit()
        ? 'Are you sure you want to save these changes?'
        : 'Are you sure you want to create this hall?',
      rejectButtonProps: { label: 'Cancel', severity: 'secondary', outlined: true },
      acceptButtonProps: { label: this.isEdit() ? 'Save' : 'Create', severity: 'contrast' },
      accept: () => this.executeSave(seats),
    });
  }

  deleteHall(): void {
    const hallId = this.hallId();
    if (!hallId) return;

    this.confirmationService.confirm({
      header: 'Confirm Delete',
      message: 'Are you sure you want to delete this hall? This action cannot be undone.',
      rejectButtonProps: { label: 'Cancel', severity: 'secondary', outlined: true },
      acceptButtonProps: { label: 'Delete', severity: 'danger' },
      accept: () => this.executeDelete(hallId),
    });
  }

  private async executeSave(seats: HallSeatInput[]): Promise<void> {
    const request: CreateHallRequestInput = {
      name: (this.form.get('name')?.value ?? '').trim(),
      seats,
    };

    if (this.isEdit() && this.hallId()) {
      await this.hallsFacade.updateHall({
        ...request,
        id: this.hallId()!,
      });
      this.toastService.success('Hall updated successfully');
    } else {
      await this.hallsFacade.createHall(request);
      this.toastService.success('Hall created successfully');
    }

    this.form.markAsPristine();
    this.router.navigate(['/halls'], { state: { refresh: true } }).then();
  }

  private async executeDelete(id: string): Promise<void> {
    await this.hallsFacade.deleteHall(id);
    this.toastService.success('Hall deleted successfully');
    this.form.markAsPristine();
    this.router.navigate(['/halls'], { state: { refresh: true } }).then();
  }

  private async loadHall(id: string): Promise<void> {
    const hall = await this.hallsFacade.getHallById(id);

    this.form.patchValue({
      name: hall.name ?? '',
      rowCount: Math.max(1, new Set((hall.seats ?? []).map((seat) => seat.row)).size),
      seatsPerRow: Math.max(
        1,
        ...Object.values(
          (hall.seats ?? []).reduce<Record<number, number>>((acc, seat) => {
            const row = seat.row ?? 0;
            acc[row] = (acc[row] ?? 0) + 1;
            return acc;
          }, {}),
        ),
      ),
    });

    this.seatsArray.clear();
    const sortedSeats = [...(hall.seats ?? [])].sort((a, b) => {
      if (a.row !== b.row) {
        return a.row - b.row;
      }
      return a.number - b.number;
    });

    for (const seat of sortedSeats) {
      this.seatsArray.push(this.createSeatGroup(seat.row, seat.number));
    }

    this.seatsError.set(null);
    this.form.markAsPristine();
  }

  private resetForm(): void {
    this.formSubmitted = false;
    this.seatsError.set(null);
    this.form.reset({
      name: '',
      rowCount: 1,
      seatsPerRow: 10,
    });
    this.seatsArray.clear();
  }

  private createSeatGroup(row: number, number: number): AbstractControl {
    return this.fb.group({
      row: [row, [Validators.required, Validators.min(1)]],
      number: [number, [Validators.required, Validators.min(1)]],
    });
  }

  private buildSeatPayload(): HallSeatInput[] | null {
    const seats = this.seatsArray.controls.map((control) => ({
      row: this.toPositiveInteger(control.get('row')?.value),
      number: this.toPositiveInteger(control.get('number')?.value),
    }));

    if (seats.length === 0) {
      this.seatsError.set('At least one seat is required.');
      return null;
    }

    if (seats.some((seat) => seat.row < 1 || seat.number < 1)) {
      this.seatsError.set('Seat row and number must be greater than 0.');
      return null;
    }

    const uniqueSeats = new Set(seats.map((seat) => `${seat.row}-${seat.number}`));
    if (uniqueSeats.size !== seats.length) {
      this.seatsError.set('Seat row/number combinations must be unique.');
      return null;
    }

    this.seatsError.set(null);
    return seats;
  }

  private toPositiveInteger(value: unknown): number {
    const numericValue = Number(value);
    if (!Number.isFinite(numericValue)) {
      return 0;
    }

    return Math.max(0, Math.floor(numericValue));
  }
}
