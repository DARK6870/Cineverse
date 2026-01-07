import { Component, inject } from '@angular/core';
import { ButtonDirective, ButtonLabel } from 'primeng/button';
import { InputText } from 'primeng/inputtext';
import { Message } from 'primeng/message';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { getValidationError } from '@cineverse/infrastructure-common';
import { AuthApiService } from '../../api/rest/auth.api.service';
import { LoginRequest } from '../../api/rest/auth.api.types';

@Component({
  selector: 'app-login',
  imports: [
    ButtonDirective,
    ButtonLabel,
    InputText,
    Message,
    ReactiveFormsModule,
    RouterLink,
  ],
  standalone: true,
  templateUrl: 'login.html',
  styleUrl: 'login.css'
})

export class Login {
  protected readonly getErrorMessage = getValidationError;
  private formBuilder = inject(FormBuilder);
  private authApiService = inject(AuthApiService);

  loginForm : FormGroup = this.formBuilder.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required]]
  });

  formSubmitted = false;

  getErrorMessageByName(controlName: string): string | null {
    return this.getErrorMessage(this.loginForm.get(controlName));
  }

  isInvalid(controlName: string) {
    const control = this.loginForm.get(controlName);
    return control?.invalid && (control.touched || this.formSubmitted);
  }

  async onSubmit(){
    this.loginForm.markAllAsTouched();
    this.formSubmitted = true;

    if (this.loginForm.valid) {
      const request : LoginRequest = {
        email: this.loginForm.value.email,
        password: this.loginForm.value.password
      }
      // TODO: save token
      await this.authApiService.loginAsync(request);
    }
  }
}
