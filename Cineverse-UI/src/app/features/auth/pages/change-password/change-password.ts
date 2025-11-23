import { Component, inject } from '@angular/core';
import { ButtonDirective, ButtonLabel } from "primeng/button";
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from "@angular/forms";
import { InputText } from "primeng/inputtext";
import { Message } from "primeng/message";
import { getErrorMessage } from '../../../../shared/helpers/validation.helper';
import { passwordMatchValidator } from '../../../../shared/validators/password-match.validator';
import { ToastService } from '../../../../core/services/toast.service';
import { AuthenticationService } from '../../../../core/services/authentication.service';
import { Router } from '@angular/router';
import {
  ChangePasswordRequestInput,
} from '../../api/auth.graphql.types';
import { ConfirmDialog } from 'primeng/confirmdialog';
import { ConfirmationService } from 'primeng/api';

@Component({
  selector: 'app-change-password',
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
  templateUrl: 'change-password.html',
  styleUrl: 'change-password.css'
})
export class ChangePassword {
  private formBuilder= inject(FormBuilder);
  private toastService = inject(ToastService)
  private authenticationService = inject(AuthenticationService)
  private router = inject(Router);
  private confirmationService = inject(ConfirmationService);

  changePasswordForm: FormGroup = this.formBuilder.group({
      currentPassword: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(20)]],
      password: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(20)]],
      confirmPassword: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(20)]],
    }, {validators: passwordMatchValidator}
  );

  formSubmitted = false;
  protected readonly getErrorMessage = getErrorMessage;

  getErrorMessageByName(controlName: string) : string | null {
    return this.getErrorMessage(this.changePasswordForm.get(controlName));
  }

  isInvalid(controlName: string) {
    const control = this.changePasswordForm.get(controlName);
    return control?.invalid && (control.touched || this.formSubmitted);
  }

  protected async onSubmit() {
    this.changePasswordForm.markAllAsTouched();
    this.formSubmitted = true;

    if (this.changePasswordForm.valid) {
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
    const request : ChangePasswordRequestInput = {
      password: this.changePasswordForm.value.currentPassword,
      newPassword: this.changePasswordForm.value.password,
      confirmNewPassword: this.changePasswordForm.value.confirmPassword
    };

    await this.authenticationService.changePasswordAsync(request);

    this.router.navigate(['/login']).then(() => {
      this.toastService.success('Password updated successfully');
    });
  }
}
