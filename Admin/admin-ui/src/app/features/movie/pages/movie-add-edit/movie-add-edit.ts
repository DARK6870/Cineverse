import { CommonModule } from '@angular/common';
import { Component, computed, inject, signal } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { DatePickerModule } from 'primeng/datepicker';
import { InputNumberModule } from 'primeng/inputnumber';
import { InputTextModule } from 'primeng/inputtext';
import { TextareaModule } from 'primeng/textarea';
import { SelectButtonModule } from 'primeng/selectbutton';
import { Message } from 'primeng/message';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { ConfirmationService } from 'primeng/api';
import { ToastService, getValidationError } from '@cineverse/infrastructure-common';
import { CreateMovieRequestInput } from '../../api/movie.graphql.types';
import { MoviesFacade } from '../../store/movies.facade';
import { HasUnsavedChanges } from '../../../../core/guards/unsaved-changes.guard';
import { marked } from 'marked';

@Component({
  selector: 'app-movie-add-edit',
  imports: [
    CommonModule,
    RouterLink,
    ReactiveFormsModule,
    ButtonModule,
    DatePickerModule,
    InputNumberModule,
    InputTextModule,
    TextareaModule,
    SelectButtonModule,
    Message,
    ConfirmDialogModule,
  ],
  templateUrl: 'movie-add-edit.html',
  styleUrl: 'movie-add-edit.css',
})
export class MovieAddEdit implements HasUnsavedChanges {
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private moviesFacade = inject(MoviesFacade);
  private fb = inject(FormBuilder);
  private toastService = inject(ToastService);
  private confirmationService = inject(ConfirmationService);

  movieId = signal<string | null>(null);
  isEdit = computed(() => Boolean(this.movieId()));
  formSubmitted = false;
  private navigatingAway = false;

  previewMarkdown = signal(false);

  availableOptions = [
    { label: 'Yes', value: true },
    { label: 'No', value: false },
  ];

  form = this.fb.group({
    title: ['', Validators.required],
    description: ['', Validators.required],
    posterUrl: ['', Validators.required],
    trailerUrl: ['', Validators.required],
    duration: [0, [Validators.required, Validators.min(60)]],
    releaseDate: [null as Date | null, Validators.required],
    genre: ['', Validators.required],
    isAvailable: [true],
  });

  constructor() {
    this.route.paramMap.pipe(takeUntilDestroyed()).subscribe((params) => {
      const id = params.get('id');
      this.movieId.set(id);
      if (id) {
        this.loadMovie(id);
      } else {
        this.resetForm();
      }
    });
  }

  // ── Guard contract ──

  hasUnsavedChanges(): boolean {
    return this.form.dirty && !this.navigatingAway;
  }

  get parsedDescription(): string | Promise<string> {
    const raw = this.form.get('description')?.value || '';
    return marked.parse(raw);
  }

  getErrorMessageByName(controlName: string): string | null {
    return getValidationError(this.form.get(controlName));
  }

  isInvalid(controlName: string): boolean {
    const control = this.form.get(controlName);
    return !!(control?.invalid && (control.touched || this.formSubmitted));
  }

  saveMovie() {
    this.formSubmitted = true;
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.confirmationService.confirm({
      header: 'Confirm Save',
      message: this.isEdit()
        ? 'Are you sure you want to save these changes?'
        : 'Are you sure you want to create this movie?',
      rejectButtonProps: { label: 'Cancel', severity: 'secondary', outlined: true },
      acceptButtonProps: { label: this.isEdit() ? 'Save' : 'Create', severity: 'contrast' },
      accept: () => this.executeSave(),
    });
  }

  deleteMovie() {
    const movieId = this.movieId();
    if (!movieId) return;

    this.confirmationService.confirm({
      header: 'Confirm Delete',
      message: 'Are you sure you want to delete this movie? This action cannot be undone.',
      rejectButtonProps: { label: 'Cancel', severity: 'secondary', outlined: true },
      acceptButtonProps: { label: 'Delete', severity: 'contrast' },
      accept: () => this.executeDelete(movieId),
    });
  }

  private async executeSave() {
    const formValue = this.form.getRawValue();
    const request: CreateMovieRequestInput = {
      title: (formValue.title ?? '').trim(),
      description: (formValue.description ?? '').trim(),
      posterUrl: (formValue.posterUrl ?? '').trim(),
      trailerUrl: (formValue.trailerUrl ?? '').trim(),
      duration: formValue.duration ?? 0,
      releaseDate: this.formatDate(formValue.releaseDate),
      genre: (formValue.genre ?? '').trim(),
      isAvailable: Boolean(formValue.isAvailable),
    };

    if (this.isEdit() && this.movieId()) {
      await this.moviesFacade.updateMovie({
        ...request,
        id: this.movieId()!,
      });
      this.toastService.success('Movie updated successfully');
    } else {
      await this.moviesFacade.createMovie(request);
      this.toastService.success('Movie created successfully');
    }

    this.navigatingAway = true;
    this.router.navigate(['/movies'], { state: { refresh: true } }).then();
  }

  private async executeDelete(movieId: string) {
    await this.moviesFacade.deleteMovie(movieId);
    this.toastService.success('Movie deleted successfully');
    this.navigatingAway = true;
    this.router.navigate(['/movies'], { state: { refresh: true } }).then();
  }

  private async loadMovie(id: string) {
    const movie = await this.moviesFacade.getMovieById(id);
    this.form.patchValue({
      title: movie.title ?? '',
      description: movie.description ?? '',
      posterUrl: movie.posterUrl ?? '',
      trailerUrl: movie.trailerUrl ?? '',
      duration: movie.duration ? Number(movie.duration) : 0,
      releaseDate: movie.releaseDate ? new Date(movie.releaseDate) : null,
      genre: movie.genre ?? '',
      isAvailable: movie.isAvailable ?? true,
    });
    this.form.markAsPristine();
  }

  private resetForm(): void {
    this.formSubmitted = false;
    this.navigatingAway = false;
    this.form.reset();
  }

  private formatDate(date: Date | null): string {
    if (!date) return '';
    return date.toISOString().slice(0, 10);
  }
}
