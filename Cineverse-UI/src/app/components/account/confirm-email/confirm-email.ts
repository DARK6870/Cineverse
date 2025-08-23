import { Component, ChangeDetectorRef } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthenticationGraphQlService } from '../../../services/graphQl/authentication-graphql-service';
import { getErrorMessage } from '../../../common/helpers/validation.helper';
import { MessageService } from 'primeng/api';
import { ButtonDirective, ButtonLabel } from 'primeng/button';
import { Message } from 'primeng/message';
import { InputOtp } from 'primeng/inputotp';

@Component({
  selector: 'app-confirm-email',
  imports: [
    ButtonDirective,
    ButtonLabel,
    FormsModule,
    Message,
    ReactiveFormsModule,
    InputOtp,
  ],
  templateUrl: 'confirm-email.html',
  styleUrl: 'confirm-email.css'
})
export class ConfirmEmail {
  protected confirmEmailForm: FormGroup;
  private formSubmitted = false;
  protected readonly getErrorMessage = getErrorMessage;

  protected canResend = true;
  protected countDown = 0;
  private timer? : ReturnType<typeof setInterval>;

  constructor(
    fb: FormBuilder,
    private router: Router,
    private authenticationService: AuthenticationGraphQlService,
    private messageService: MessageService,
    private cdr: ChangeDetectorRef
  ) {
    this.confirmEmailForm = fb.group({
      verificationCode: ['', [Validators.required, Validators.minLength(5)]],
    })
  }

  onSubmit() {
    this.confirmEmailForm.markAllAsTouched();
    this.formSubmitted = true;

    if (this.confirmEmailForm.valid) {
      const code : number = this.confirmEmailForm.value.verificationCode;
      this.authenticationService.confirmEmail(code).subscribe({
        next: () => {
          this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Successfully logged in' });
          this.router.navigate(['/account']).then();
        }
      })
    }
  }

  resendVerificationCode() {
    if (!this.canResend) return;

    // Resend verification code
    this.authenticationService.resendVerificationCode().subscribe({
      next: () => {
        this.messageService.add({
          severity: 'info',
          summary: 'Verification code resent',
          detail: 'A new verification code has been resent'
        });
      }
    })
    // Block for 2 min
    this.canResend = false;
    this.countDown = 120;

    this.timer = setInterval(() => {
        this.countDown--;
        this.cdr.detectChanges();
        if (this.countDown <= 0) {
          this.canResend = true;
          clearInterval(this.timer);
        }
      }, 1000);
  }

  getErrorMessageByName(controlName: string) : string | null {
    return this.getErrorMessage(this.confirmEmailForm.get(controlName));
  }

  isInvalid(controlName: string) {
    const control = this.confirmEmailForm.get(controlName);
    return control?.invalid && (control.touched || this.formSubmitted);
  }
}
