import { CommonModule } from '@angular/common';
import { Component, computed, inject, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { InputTextModule } from 'primeng/inputtext';
import { Message } from 'primeng/message';
import { ConfirmationService } from 'primeng/api';
import { ToastService, getValidationError } from '@cineverse/infrastructure-common';
import { HasUnsavedChanges } from '../../../../core/guards/unsaved-changes.guard';
import { User, UserRole, UserStatus } from '../../api/user.graphql.types';
import { UsersFacade } from '../../store/users.facade';

@Component({
  selector: 'app-user-edit',
  imports: [
    CommonModule,
    RouterLink,
    ReactiveFormsModule,
    ButtonModule,
    ConfirmDialogModule,
    InputTextModule,
    Message,
  ],
  templateUrl: 'user-edit.html',
  styleUrl: 'user-edit.css',
})
export class UserEdit implements HasUnsavedChanges {
  private static readonly ROLE_OPTIONS: UserRole[] = ['Admin', 'Manager', 'User'];
  private static readonly STATUS_OPTIONS: UserStatus[] = ['PendingEmailConfirmation', 'Normal', 'Blocked', 'Disabled'];

  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private usersFacade = inject(UsersFacade);
  private fb = inject(FormBuilder);
  private toastService = inject(ToastService);
  private confirmationService = inject(ConfirmationService);

  userId = signal<string | null>(null);
  isEdit = computed(() => Boolean(this.userId()));
  formSubmitted = false;

  form = this.fb.group({
    firstName: [{ value: '', disabled: true }],
    lastName: [{ value: '', disabled: true }],
    email: [{ value: '', disabled: true }],
    provider: [{ value: '', disabled: true }],
    role: ['', Validators.required],
    status: ['', Validators.required],
  });

  constructor() {
    this.route.paramMap.pipe(takeUntilDestroyed()).subscribe((params) => {
      const id = params.get('id');
      this.userId.set(id);
      if (id) {
        this.loadUser(id);
      }
    });
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

  roleOptions(): UserRole[] {
    return UserEdit.ROLE_OPTIONS;
  }

  statusOptions(): UserStatus[] {
    return UserEdit.STATUS_OPTIONS;
  }

  saveUser(): void {
    this.formSubmitted = true;
    if (this.form.invalid || !this.userId()) {
      this.form.markAllAsTouched();
      return;
    }

    this.confirmationService.confirm({
      header: 'Confirm Save',
      message: 'Are you sure you want to save these changes?',
      rejectButtonProps: { label: 'Cancel', severity: 'secondary', outlined: true },
      acceptButtonProps: { label: 'Save', severity: 'contrast' },
      accept: () => this.executeSave(),
    });
  }

  private async executeSave(): Promise<void> {
    const id = this.userId();
    if (!id) {
      return;
    }

    const formValue = this.form.getRawValue();
    await this.usersFacade.updateUser({
      id,
      role: (formValue.role ?? '').trim() as UserRole,
      status: (formValue.status ?? '').trim() as UserStatus,
    });

    this.toastService.success('User updated successfully');
    this.form.markAsPristine();
    this.router.navigate(['/users'], { state: { refresh: true } }).then();
  }

  private async loadUser(id: string): Promise<void> {
    const user = await this.usersFacade.getUserById(id);
    this.patchForm(user);
    this.form.markAsPristine();
  }

  private patchForm(user: User): void {
    this.form.patchValue({
      firstName: user.firstName ?? '',
      lastName: user.lastName ?? '',
      email: user.email ?? '',
      provider: user.provider ?? '',
      role: user.role ?? '',
      status: user.status ?? '',
    });
  }
}
