import { Component, ChangeDetectorRef, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthenticationGraphQlService } from '../../../services/graphQl/authentication-graphql-service';
import { getErrorMessage } from '../../../common/helpers/validation.helper';
import { MessageService } from 'primeng/api';
import { ButtonDirective, ButtonLabel } from 'primeng/button';
import { Message } from 'primeng/message';
import { InputOtp } from 'primeng/inputotp';
import { AuthenticationService } from '../../../services/authentication/authentication-service';

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

// TODO: check somehow if the confirmation code already has been sent
// TODO: auto-submit form when the code is present
// TODO: confirm-by-link
export class ConfirmEmail implements OnInit {
  protected confirmEmailForm: FormGroup;
  private formSubmitted = false;
  protected readonly getErrorMessage = getErrorMessage;

  protected canResend = true;
  protected countDown = 0;
  private timer? : ReturnType<typeof setInterval>;

  constructor(
    fb: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private authenticationService: AuthenticationGraphQlService,
    private authService: AuthenticationService,
    private messageService: MessageService,
    private cdr: ChangeDetectorRef
  ) {
    this.confirmEmailForm = fb.group({
      verificationCode: ['', [Validators.required, Validators.minLength(5)]],
    })
  }

  async ngOnInit() {
    this.route.paramMap.subscribe(params => {
      if (params.get('sent') === 'true')
        this.startCountDown();
    })
    // TODO: get verification code countdown from backend
    this.authService.requireRefreshToken();
    // TODO: if user status != PendingEmailConfirmation -> redirect
  }

  async onSubmit() {
    this.confirmEmailForm.markAllAsTouched();
    this.formSubmitted = true;
    const accessToken = await this.authService.getAccessTokenAsync();

    if (this.confirmEmailForm.valid) {
      const code: number = Number(this.confirmEmailForm.value.verificationCode);
      this.authenticationService.confirmEmail(code, accessToken).subscribe({
        next: () => {
          // TODO: regenerate access token to update user status in token
          this.router.navigate(['/']).then(() => {
            this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Email confirmed successfully' });
          });
        }
      })
    }
  }

  async resendVerificationCode() {
    if (!this.canResend)
      return;

    const accessToken = await this.authService.getAccessTokenAsync();

    // Resend verification code
    this.authenticationService.resendVerificationCode(accessToken).subscribe({
      next: () => {
        this.messageService.add({severity: 'info', summary: 'Verification code resent', detail: 'A new verification code has been resent'});
      }
    })

    this.startCountDown();
  }

  startCountDown() {
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
