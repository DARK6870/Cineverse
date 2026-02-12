import {
  ChangeDetectorRef,
  Component,
  DestroyRef,
  inject,
  OnInit,
} from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ButtonDirective, ButtonLabel } from 'primeng/button';
import { Message } from 'primeng/message';
import { InputOtp } from 'primeng/inputotp';
import { AuthGraphqlService } from '../../api/graphql/auth.graphql.service';
import { AuthenticationService, UserStatus } from '@cineverse/infrastructure-auth';
import { getValidationError, ToastService } from '@cineverse/infrastructure-common';

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
  private authGraphqlService = inject(AuthGraphqlService);
  private authenticationService = inject(AuthenticationService);
  private toastService = inject(ToastService);
  private cdr = inject(ChangeDetectorRef);

  private formSubmitted = false;
  protected readonly getErrorMessage = getValidationError;

  protected canResend = true;
  protected countDown = 0;
  private timer? : ReturnType<typeof setInterval>;

  protected confirmEmailForm : FormGroup = this.formBuilder.group({
    verificationCode: ['', [Validators.required, Validators.minLength(5)]],
  });


  async ngOnInit() {
    if ((await this.authenticationService.getUserDataAsync()).userStatus != UserStatus.PendingEmailConfirmation) {
      this.router.navigate(['/profile']).then(() => {
        this.toastService.warning('Email already confirmed');
      });
    }

    const {sent, verificationCode} = this.route.snapshot.params;
    if (sent === 'true') {
      this.startCountDown();
    }

    if (verificationCode) {
      this.confirmEmailForm.patchValue({verificationCode});
      this.onSubmit();
    }
  }

  async onSubmit() {
    this.confirmEmailForm.markAllAsTouched();
    this.formSubmitted = true;

    if (this.confirmEmailForm.valid) {
      const code: number = Number(this.confirmEmailForm.value.verificationCode);

      await this.authGraphqlService.confirmEmailAsync(code);
      await this.authenticationService.generateAccessTokenAsync();

      this.router.navigate(['/']).then(() => {
        this.toastService.success('Email confirmed successfully');
      });
    }
  }

  async resendVerificationCode() {
    if (!this.canResend)
      return;

    // Resend verification code
    await this.authGraphqlService.resendEmailVerificationCodeAsync();
    this.toastService.info('A new verification code has been resent');

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
