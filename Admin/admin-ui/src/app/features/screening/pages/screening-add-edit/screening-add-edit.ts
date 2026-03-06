import { CommonModule } from '@angular/common';
import { Component, HostListener, computed, inject, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { InputNumberModule } from 'primeng/inputnumber';
import { InputTextModule } from 'primeng/inputtext';
import { Message } from 'primeng/message';
import { ConfirmationService } from 'primeng/api';
import { ToastService, getValidationError } from '@cineverse/infrastructure-common';
import { HasUnsavedChanges } from '../../../../core/guards/unsaved-changes.guard';
import { CreateScreeningRequestInput } from '../../api/screening.graphql.types';
import { ScreeningsFacade } from '../../store/screenings.facade';

@Component({
  selector: 'app-screening-add-edit',
  imports: [
    CommonModule,
    RouterLink,
    ReactiveFormsModule,
    ButtonModule,
    ConfirmDialogModule,
    InputTextModule,
    InputNumberModule,
    Message,
  ],
  templateUrl: './screening-add-edit.html',
  styleUrl: './screening-add-edit.css',
})
export class ScreeningAddEdit implements HasUnsavedChanges {
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private screeningsFacade = inject(ScreeningsFacade);
  private fb = inject(FormBuilder);
  private toastService = inject(ToastService);
  private confirmationService = inject(ConfirmationService);

  screeningId = signal<string | null>(null);
  isEdit = computed(() => Boolean(this.screeningId()));
  formSubmitted = false;
  timeRangeError = signal<string | null>(null);
  protected movieSearchTerm = '';
  protected isMovieSelectOpen = false;

  form = this.fb.group({
    movieId: ['', Validators.required],
    hallId: ['', Validators.required],
    date: ['', Validators.required],
    startTime: ['', Validators.required],
    endTime: ['', Validators.required],
    ticketPrice: [1, [Validators.required, Validators.min(1)]],
  });

  constructor() {
    this.screeningsFacade.ensureReferenceOptionsLoaded();

    this.route.paramMap.pipe(takeUntilDestroyed()).subscribe((params) => {
      const id = params.get('id');
      this.screeningId.set(id);
      this.movieSearchTerm = '';
      this.isMovieSelectOpen = false;
      if (id) {
        this.loadScreening(id);
      } else {
        this.resetForm();
      }
    });
  }

  protected movieOptions = this.screeningsFacade.movieFilterOptions;
  protected hallOptions = this.screeningsFacade.hallFilterOptions;

  protected filteredMovieOptions() {
    const normalizedSearch = this.movieSearchTerm.trim().toLowerCase();

    if (!normalizedSearch) {
      return this.movieOptions();
    }

    return this.movieOptions().filter((option) => option.label.toLowerCase().includes(normalizedSearch));
  }

  protected toggleMovieSelectMenu(): void {
    const willOpen = !this.isMovieSelectOpen;
    this.isMovieSelectOpen = willOpen;
    if (willOpen) {
      this.screeningsFacade.ensureReferenceOptionsLoaded();
    }
  }

  protected selectedMovieLabel(): string {
    const selectedMovieId = (this.form.get('movieId')?.value ?? '').trim();
    if (!selectedMovieId) {
      return 'Select movie';
    }

    return this.movieOptions().find((option) => option.id === selectedMovieId)?.label ?? 'Select movie';
  }

  protected isMovieSelected(movieId: string): boolean {
    return (this.form.get('movieId')?.value ?? '').trim() === movieId;
  }

  protected selectMovie(movieId: string): void {
    const control = this.form.get('movieId');
    control?.setValue(movieId);
    control?.markAsTouched();
    control?.markAsDirty();
    this.isMovieSelectOpen = false;
  }

  protected clearMovieSelection(): void {
    const control = this.form.get('movieId');
    control?.setValue('');
    control?.markAsTouched();
    control?.markAsDirty();
    this.isMovieSelectOpen = false;
  }

  @HostListener('document:click', ['$event'])
  protected onDocumentClick(event: MouseEvent): void {
    const target = event.target as HTMLElement | null;
    if (target?.closest('.movie-select-menu')) {
      return;
    }

    this.isMovieSelectOpen = false;
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

  saveScreening(): void {
    this.formSubmitted = true;
    this.timeRangeError.set(null);

    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const startTime = this.form.get('startTime')?.value ?? '';
    const endTime = this.form.get('endTime')?.value ?? '';
    if (startTime && endTime && endTime <= startTime) {
      this.timeRangeError.set('End time must be greater than start time.');
      return;
    }

    this.confirmationService.confirm({
      header: 'Confirm Save',
      message: this.isEdit()
        ? 'Are you sure you want to save these changes?'
        : 'Are you sure you want to create this screening?',
      rejectButtonProps: { label: 'Cancel', severity: 'secondary', outlined: true },
      acceptButtonProps: { label: this.isEdit() ? 'Save' : 'Create', severity: 'contrast' },
      accept: () => this.executeSave(),
    });
  }

  deleteScreening(): void {
    const id = this.screeningId();
    if (!id) {
      return;
    }

    this.confirmationService.confirm({
      header: 'Confirm Delete',
      message: 'Are you sure you want to delete this screening? This action cannot be undone.',
      rejectButtonProps: { label: 'Cancel', severity: 'secondary', outlined: true },
      acceptButtonProps: { label: 'Delete', severity: 'danger' },
      accept: () => this.executeDelete(id),
    });
  }

  private async executeSave(): Promise<void> {
    const formValue = this.form.getRawValue();

    const request: CreateScreeningRequestInput = {
      movieId: (formValue.movieId ?? '').trim(),
      hallId: (formValue.hallId ?? '').trim(),
      date: (formValue.date ?? '').trim(),
      startTime: this.normalizeTimeForRequest(formValue.startTime),
      endTime: this.normalizeTimeForRequest(formValue.endTime),
      ticketPrice: formValue.ticketPrice ?? 0,
    };

    if (this.isEdit() && this.screeningId()) {
      await this.screeningsFacade.updateScreening({
        ...request,
        id: this.screeningId()!,
      });
      this.toastService.success('Screening updated successfully');
    } else {
      await this.screeningsFacade.createScreening(request);
      this.toastService.success('Screening created successfully');
    }

    this.form.markAsPristine();
    this.router.navigate(['/screenings'], { state: { refresh: true } }).then();
  }

  private async executeDelete(id: string): Promise<void> {
    await this.screeningsFacade.deleteScreening(id);
    this.toastService.success('Screening deleted successfully');
    this.form.markAsPristine();
    this.router.navigate(['/screenings'], { state: { refresh: true } }).then();
  }

  private async loadScreening(id: string): Promise<void> {
    const screening = await this.screeningsFacade.getScreeningById(id);

    this.form.patchValue({
      movieId: screening.movieId ?? '',
      hallId: screening.hallId ?? '',
      date: screening.date ?? '',
      startTime: this.normalizeTimeForInput(screening.startTime),
      endTime: this.normalizeTimeForInput(screening.endTime),
      ticketPrice: screening.ticketPrice ?? 1,
    });

    this.timeRangeError.set(null);
    this.form.markAsPristine();
  }

  private resetForm(): void {
    this.formSubmitted = false;
    this.timeRangeError.set(null);
    this.movieSearchTerm = '';
    this.isMovieSelectOpen = false;
    this.form.reset({
      movieId: '',
      hallId: '',
      date: '',
      startTime: '',
      endTime: '',
      ticketPrice: 1,
    });
  }

  private normalizeTimeForRequest(time: string | null | undefined): string {
    const normalized = (time ?? '').trim();
    if (!normalized) {
      return '';
    }

    return normalized.length === 5 ? `${normalized}:00` : normalized;
  }

  private normalizeTimeForInput(time: string | null | undefined): string {
    const normalized = (time ?? '').trim();
    if (!normalized) {
      return '';
    }

    return normalized.split(':').slice(0, 2).join(':');
  }
}
