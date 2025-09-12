import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthenticationGraphqlService } from '../../../../api/authentication/authentication.graphql.service';
import { getErrorMessage } from '../../../../utils/helpers/validation.helper';
import { MessageService } from 'primeng/api';
import { ButtonDirective, ButtonLabel } from 'primeng/button';
import { Message } from 'primeng/message';
import { InputOtp } from 'primeng/inputotp';
import { AuthenticationService } from '../../../../services/authentication/authentication.service';
import { JwtClaimsService } from '../../../../services/jwt/jwt-claims.service';
import { UserStatus } from '../../../../utils/types/jwt-payload';

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
    private authenticationService: AuthenticationGraphqlService,
    private authService: AuthenticationService,
    private messageService: MessageService,
    private cdr: ChangeDetectorRef,
    private jwtClaimsService: JwtClaimsService,
  ) {
    this.confirmEmailForm = fb.group({
      verificationCode: ['', [Validators.required, Validators.minLength(5)]],
    })
  }

  async ngOnInit() {
    if ((await this.jwtClaimsService.decodeTokenAsync()).userStatus != UserStatus.PendingEmailConfirmation) {
      this.router.navigate(['/account']).then(() => {
        this.messageService.add({severity: 'warn', summary: 'Already Confirmed', detail: 'Email already confirmed'});
      });
    }

    this.route.queryParamMap.subscribe(params => {
      const sent = params.get('sent');
      const verificationCode = params.get('verificationCode');

      if (sent === 'true') {
        this.startCountDown();
      }

      if (verificationCode) {
        this.confirmEmailForm.patchValue({ verificationCode });
        this.onSubmit();
      }
    });

    this.authService.requireRefreshToken();
  }

  async onSubmit() {
    this.confirmEmailForm.markAllAsTouched();
    this.formSubmitted = true;

    if (this.confirmEmailForm.valid) {
      const code: number = Number(this.confirmEmailForm.value.verificationCode);

      this.authenticationService.confirmEmail(code).subscribe({
        next: () => {
          this.authService.regenerateAccessTokenAsync();

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

    // Resend verification code
    this.authenticationService.resendEmailVerificationCode().subscribe({
      next: () => {
        this.messageService.add({severity: 'info', summary: 'Verification code resent', detail: 'A new verification code has been resent'});
      }
    })

    this.startCountDown();
  }

  startCountDown() {
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
