import {
  ChangeDetectorRef,
  Component,
  DestroyRef,
  inject,
  OnInit,
} from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthGraphqlService } from '../../api/auth.graphql.service';
import { getErrorMessage } from '../../../../shared/helpers/validation.helper';
import { ButtonDirective, ButtonLabel } from 'primeng/button';
import { Message } from 'primeng/message';
import { InputOtp } from 'primeng/inputotp';
import { AuthenticationService } from '../../../../core/services/authentication.service';
import { JwtClaimsService } from '../../../../core/services/jwt-claims.service';
import { UserStatus } from '../../../../shared/models/jwt-payload.model';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { ToastService } from 'infrastructure-common';

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
  standalone: true,
  templateUrl: 'confirm-email.html',
  styleUrl: 'confirm-email.css'
})

export class ConfirmEmail implements OnInit {
  private formBuilder = inject(FormBuilder);
  private router = inject(Router);
  private route = inject(ActivatedRoute);
  private authenticationService = inject(AuthGraphqlService);
  private authService = inject(AuthenticationService);
  private toastService = inject(ToastService);
  private cdr = inject(ChangeDetectorRef);
  private jwtClaimsService = inject(JwtClaimsService);
  private destroyRef = inject(DestroyRef);

  private formSubmitted = false;
  protected readonly getErrorMessage = getErrorMessage;

  protected canResend = true;
  protected countDown = 0;
  private timer? : ReturnType<typeof setInterval>;

  protected confirmEmailForm : FormGroup = this.formBuilder.group({
    verificationCode: ['', [Validators.required, Validators.minLength(5)]],
  });


  async ngOnInit() {
    if ((await this.jwtClaimsService.decodeTokenAsync()).userStatus != UserStatus.PendingEmailConfirmation) {
      this.router.navigate(['/profile']).then(() => {
        this.toastService.warning('Email already confirmed');
      });
    }

    this.route.queryParamMap
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe(params => {
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
  }

  async onSubmit() {
    this.confirmEmailForm.markAllAsTouched();
    this.formSubmitted = true;

    if (this.confirmEmailForm.valid) {
      const code: number = Number(this.confirmEmailForm.value.verificationCode);

      this.authenticationService.confirmEmail(code).subscribe({
        next: () => {
          this.authService.generateAccessTokenAsync();

          this.router.navigate(['/']).then(() => {
            this.toastService.success('Email confirmed successfully');
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
        this.toastService.info('A new verification code has been resent');
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
