import { Component } from '@angular/core';
import {ButtonDirective, ButtonLabel} from "primeng/button";
import {FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators} from "@angular/forms";
import {InputText} from "primeng/inputtext";
import {Message} from "primeng/message";
import { getErrorMessage } from '../../../utils/helpers/validation.helper';
import {passwordMatchValidator} from '../../../utils/validators/password-match.validator';
import {ToastService} from '../../../services/toast/toast.service';
import {AuthenticationService} from '../../../services/authentication/authentication.service';
import {Router} from '@angular/router';
import {
  ChangePasswordRequestInput,
  RestorePasswordRequestInput
} from '../../../api/authentication/authentication.graphql.types';


@Component({
  selector: 'app-change-password',
    imports: [
        ButtonDirective,
        ButtonLabel,
        FormsModule,
        InputText,
        Message,
        ReactiveFormsModule
    ],
  templateUrl: 'change-password.html',
  styleUrl: 'change-password.css'
})
export class ChangePassword {
  changePasswordForm: FormGroup;
  formSubmitted = false;
  protected readonly getErrorMessage = getErrorMessage;

  constructor(
    private fb: FormBuilder,
    private toastService: ToastService,
    private authenticationService: AuthenticationService,
    private router: Router
  ) {
    this.changePasswordForm = fb.group({
      currentPassword: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(20)]],
      password: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(20)]],
      confirmPassword: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(20)]],
    }, {validators: passwordMatchValidator}
    );
  }

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
}
