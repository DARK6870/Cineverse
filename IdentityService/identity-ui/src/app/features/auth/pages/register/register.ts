import { Component, inject } from '@angular/core';
import { ButtonDirective, ButtonLabel } from 'primeng/button';
import { InputText } from 'primeng/inputtext';
import { Message } from 'primeng/message';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { passwordMatchValidator } from '../../../../shared/validators/password-match.validator';
import { AuthApiService } from '../../api/rest/auth.api.service';
import { RegisterRequest } from '../../api/rest/auth.api.types';
import { getValidationError, ToastService } from '@cineverse/infrastructure-common';
import { TokenStorageService } from '@cineverse/infrastructure-auth';

@Component({
  selector: 'app-register',
  imports: [
    ButtonDirective,
    ButtonLabel,
    InputText,
    Message,
    ReactiveFormsModule,
    RouterLink,
  ],
  standalone: true,
  templateUrl: 'register.html',
  styleUrl: 'register.css'
})

export class Register {
  private authApiService = inject(AuthApiService);
  private formBuilder = inject(FormBuilder);
  private tokenStorageService = inject(TokenStorageService);
  private router = inject(Router);
  private toastService = inject(ToastService);

  registerForm: FormGroup = this.formBuilder.group({
    firstName: ['', Validators.required],
    lastName: ['', Validators.required],
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(20)]],
    confirmPassword: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(20)]],
  }, {validators: passwordMatchValidator});

  formSubmitted = false;
  protected readonly getErrorMessage = getValidationError;

  getErrorMessageByName(controlName: string) : string | null {
    return this.getErrorMessage(this.registerForm.get(controlName));
  }

  isInvalid(controlName: string) {
    const control = this.registerForm.get(controlName);
    return control?.invalid && (control.touched || this.formSubmitted);
  }

  protected async onSubmit(){
    this.registerForm.markAllAsTouched();
    this.formSubmitted = true;

    if (this.registerForm.valid) {
      const request : RegisterRequest = {
        firstName: this.registerForm.value.firstName,
        lastName: this.registerForm.value.lastName,
        email: this.registerForm.value.email,
        password: this.registerForm.value.password,
        confirmPassword: this.registerForm.value.confirmPassword
      };

      await this.registerAsync(request);
    }
  }

  private async registerAsync(request: RegisterRequest) {
    const registerResponse = await this.authApiService.registerAsync(request);

    this.tokenStorageService.saveAccessToken(registerResponse.accessToken);
    this.tokenStorageService.saveRefreshToken(registerResponse.refreshToken);

    this.router.navigate(['/confirm-email/true']).then(() => {
      this.toastService.success('Profile created successfully');
    });
  }
}
