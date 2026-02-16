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
import { CheckboxModule } from 'primeng/checkbox';
import { ToastService } from '@cineverse/infrastructure-common';
import { CreateMovieRequestInput, Movie } from '../../api/movie.graphql.types';
import { MoviesFacade } from '../../store/movies.facade';
import { MoviesStore } from '../../store/movies.store';

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
    CheckboxModule,
  ],
  templateUrl: 'movie-add-edit.html',
  styleUrl: 'movie-add-edit.css',
  providers: [MoviesStore, MoviesFacade],
})
export class MovieAddEdit {
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private moviesFacade = inject(MoviesFacade);
  private fb = inject(FormBuilder);
  private toastService = inject(ToastService);

  movieId = signal<string | null>(null);
  isEdit = computed(() => Boolean(this.movieId()));

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

  async saveMovie(): Promise<void> {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
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
        id: this.movieId(),
      });
      this.toastService.success('Movie updated');
    } else {
      await this.moviesFacade.createMovie(request);
      this.toastService.success('Movie created');
    }

    this.router.navigate(['/movies'], {
      state: { refresh: true }
    }).then();
  }

  async deleteMovie(): Promise<void> {
    const movieId = this.movieId();
    if (!movieId) {
      return;
    }
    await this.moviesFacade.deleteMovie(movieId);
    this.toastService.success('Movie deleted');
    this.router.navigate(['/movies'], {
      state: { refresh: true }
    }).then();
  }

  private async loadMovie(id: string): Promise<void> {
    const movie: Movie = await this.moviesFacade.getMovieById(id);
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
  }

  private resetForm(): void {
    this.form.reset({
      title: '',
      description: '',
      posterUrl: '',
      trailerUrl: '',
      duration: 0,
      releaseDate: null,
      genre: '',
      isAvailable: true,
    });
  }

  private formatDate(date: Date | null): string {
    if (!date) {
      return '';
    }
    return date.toISOString().slice(0, 10);
  }
}
