import { Component } from '@angular/core';
import { ButtonDirective, ButtonLabel } from 'primeng/button';
import { InputText } from 'primeng/inputtext';
import { Message } from 'primeng/message';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { getErrorMessage } from '../../../utils/helpers/validation.helper';
import { AuthenticationService } from '../../../services/authentication/authentication.service';
import { RouterLink } from '@angular/router';
import { passwordMatchValidator } from '../../../utils/validators/password-match.validator';
import { RegisterRequestInput } from '../../../api/authentication/authentication.graphql.types';

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
  templateUrl: 'register.html',
  styleUrl: 'register.css'
})

export class Register {
  registerForm: FormGroup;
  formSubmitted = false;
  protected readonly getErrorMessage = getErrorMessage;

  constructor(
    private authenticationService: AuthenticationService,
    private fb: FormBuilder
  ) {
    this.registerForm = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(20)]],
      confirmPassword: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(20)]],
    }, {validators: passwordMatchValidator});
  }

  getErrorMessageByName(controlName: string) : string | null {
    return this.getErrorMessage(this.registerForm.get(controlName));
  }

  isInvalid(controlName: string) {
    const control = this.registerForm.get(controlName);
    return control?.invalid && (control.touched || this.formSubmitted);
  }

  protected onSubmit(){
    this.registerForm.markAllAsTouched();
    this.formSubmitted = true;

    if (this.registerForm.valid) {
      const request : RegisterRequestInput = {
        firstName: this.registerForm.value.firstName,
        lastName: this.registerForm.value.lastName,
        email: this.registerForm.value.email,
        password: this.registerForm.value.password,
        confirmPassword: this.registerForm.value.confirmPassword
      };

      this.authenticationService.registerUser(request);
    }
  }
}
