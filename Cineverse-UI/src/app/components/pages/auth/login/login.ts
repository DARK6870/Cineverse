import { Component, OnInit } from '@angular/core';
import { ButtonDirective, ButtonLabel } from 'primeng/button';
import { InputText } from 'primeng/inputtext';
import { Message } from 'primeng/message';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { getErrorMessage } from '../../../../utils/helpers/validation.helper';
import { RouterLink } from '@angular/router';
import { AuthenticationService } from '../../../../services/authentication/authentication.service';

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
  templateUrl: 'login.html',
  styleUrl: 'login.css'
})

export class Login implements OnInit {
  loginForm : FormGroup;
  formSubmitted = false;
  protected readonly getErrorMessage = getErrorMessage;

  constructor(
    private authenticationService: AuthenticationService,
    private fb: FormBuilder
  ) {
    this.loginForm  = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required]]
    });
  }

  ngOnInit() {
    this.authenticationService.ensureUserNotAuthorized();
  }

  getErrorMessageByName(controlName: string): string | null {
    return this.getErrorMessage(this.loginForm.get(controlName));
  }

  isInvalid(controlName: string) {
    const control = this.loginForm.get(controlName);
    return control?.invalid && (control.touched || this.formSubmitted);
  }

  onSubmit(){
    this.loginForm.markAllAsTouched();
    this.formSubmitted = true;

    if (this.loginForm.valid) {
      // Create request
      const request = {
        email: this.loginForm.value.email,
        password: this.loginForm.value.password
      }

      // Login user
      this.authenticationService.loginUser(request);
    }
  }
}
