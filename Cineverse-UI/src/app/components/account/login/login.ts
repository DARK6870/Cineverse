import { Component } from '@angular/core';
import { ButtonDirective, ButtonLabel } from 'primeng/button';
import { InputText } from 'primeng/inputtext';
import { Message } from 'primeng/message';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { getErrorMessage } from '../../../common/helpers/validation.helper';
import { AuthenticationGraphQlService } from '../../../services/graphQl/authentication-graphql-service';
import { Router, RouterLink } from '@angular/router';
import { MessageService } from 'primeng/api';

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
export class Login {
  loginForm : FormGroup;
  formSubmitted = false;
  protected readonly getErrorMessage = getErrorMessage;

  constructor(
    private authenticationService: AuthenticationGraphQlService,
    private fb: FormBuilder,
    private router: Router,
    private messageService: MessageService
  ) {
    this.loginForm  = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(20)]],
    });
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

      // Send mutation
      this.authenticationService.login(request).subscribe({
        next: () => {
          this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Successfully logged in' });
          this.router.navigate(['/']).then();
        }
      });

    }
  }
}
