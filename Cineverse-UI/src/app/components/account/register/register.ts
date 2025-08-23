import { Component } from '@angular/core';
import { ButtonDirective, ButtonLabel } from 'primeng/button';
import { InputText } from 'primeng/inputtext';
import { Message } from 'primeng/message';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { getErrorMessage } from '../../../common/helpers/validation.helper';
import { AuthenticationGraphQlService } from '../../../services/graphQl/authentication-graphql-service';
import { MessageService } from 'primeng/api';
import {Router, RouterLink} from '@angular/router';

@Component({
  selector: 'app-register',
  imports: [
    ButtonDirective,
    ButtonLabel,
    InputText,
    Message,
    ReactiveFormsModule,
    RouterLink
  ],
  templateUrl: 'register.html',
  styleUrl: 'register.css'
})

export class Register {
  registerForm: FormGroup;
  formSubmitted = false;
  protected readonly getErrorMessage = getErrorMessage;

  constructor(
    private authenticationService: AuthenticationGraphQlService,
    private messageService: MessageService,
    private router: Router,
    private fb: FormBuilder
  ) {
    this.registerForm = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(20)]],
      confirmPassword: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(20)]],
    })
  }

  getErrorMessageByName(controlName: string) : string | null {
    return this.getErrorMessage(this.registerForm.get(controlName));
  }

  isInvalid(controlName: string) {
    const control = this.registerForm.get(controlName);
    return control?.invalid && (control.touched || this.formSubmitted);
  }

  onSubmit(){
    this.registerForm.markAllAsTouched();
    this.formSubmitted = true;

    if (this.registerForm.valid) {

      // Create request
      const request = {
        firstName: this.registerForm.value.firstName,
        lastName: this.registerForm.value.lastName,
        email: this.registerForm.value.email,
        password: this.registerForm.value.password,
        confirmPassword: this.registerForm.value.confirmPassword
      };

      this.authenticationService.register(request).subscribe({
        next: () => {
          this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Account created successfully' });
          this.router.navigate(['/']).then();
        }
      })
    }
  }
}
