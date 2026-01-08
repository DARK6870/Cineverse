import { Component, DestroyRef, inject, OnInit } from '@angular/core';
import { ButtonDirective, ButtonLabel } from "primeng/button";
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from "@angular/forms";
import { InputText } from "primeng/inputtext";
import { Message } from "primeng/message";
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { ActivatedRoute, Router } from '@angular/router';
import { ConfirmationService } from 'primeng/api';
import { ConfirmDialog } from 'primeng/confirmdialog';
import {getValidationError, ToastService} from '@cineverse/infrastructure-common';
import {AuthGraphqlService} from '../../../api/graphql/auth.graphql.service';
import {passwordMatchValidator} from '../../../../../shared/validators/password-match.validator';
import {RestorePasswordRequestInput} from '../../../api/graphql/auth.graphql.types';


@Component({
  selector: 'app-restore-password-confirm',
    imports: [
        ButtonDirective,
        ButtonLabel,
        FormsModule,
        InputText,
        Message,
        ReactiveFormsModule,
        ConfirmDialog
    ],
  standalone: true,
  templateUrl: 'restore-password-confirm.html',
  styleUrl: 'restore-password-confirm.css'
})
export class RestorePasswordConfirm implements OnInit {
  private formBuilder = inject(FormBuilder);
  private route = inject(ActivatedRoute);
  private destroyRef = inject(DestroyRef);
  private toastService = inject(ToastService);
  private router = inject(Router);
  private authGraphqlService = inject(AuthGraphqlService);
  private confirmationService = inject(ConfirmationService);

  email: string | null = null;
  code: string | null = null;

  restorePasswordForm: FormGroup = this.formBuilder.group({
      password: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(20)]],
      confirmPassword: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(20)]],
    }, {validators: passwordMatchValidator}
  );

  formSubmitted = false;
  protected readonly getErrorMessage = getValidationError;

  ngOnInit(): void {
    this.route.paramMap
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe(async paramMap => {
        this.email = paramMap.get('email');
        this.code = paramMap.get('code');

        if (!this.email || !this.code) {
          this.router.navigate(['/']).then(() => {
            this.toastService.error('Invalid restore password link')
          })
        }
      });
    }

  getErrorMessageByName(controlName: string) : string | null {
    return this.getErrorMessage(this.restorePasswordForm.get(controlName));
  }

  isInvalid(controlName: string) {
    const control = this.restorePasswordForm.get(controlName);
    return control?.invalid && (control.touched || this.formSubmitted);
  }

  protected async onSubmit() {
    this.restorePasswordForm.markAllAsTouched();
    this.formSubmitted = true;

    if (this.restorePasswordForm.valid) {
      this.confirmationService.confirm({
        message: 'Are you sure that you want to proceed?',
        header: 'Confirmation',
        icon: 'pi pi-exclamation-triangle',
        acceptLabel: 'Save Changes',
        rejectLabel: 'Cancel',
        acceptButtonStyleClass: 'p-button-primary',
        rejectButtonStyleClass: 'p-button-secondary',
        accept: () => this.submitForm(),
      });
    }
  }

  private async submitForm(){
    const request : RestorePasswordRequestInput = {
      email: this.email!,
      code: this.code!,
      password: this.restorePasswordForm.value.password,
      confirmPassword: this.restorePasswordForm.value.confirmPassword
    };

    await this.authGraphqlService.restorePasswordAsync(request);

    this.router.navigate(['/login']).then(() => {
      this.toastService.success('Password reset successfully');
    });
  }
}
