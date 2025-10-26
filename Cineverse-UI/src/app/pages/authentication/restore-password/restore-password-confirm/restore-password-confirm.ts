import { Component, DestroyRef, OnInit } from '@angular/core';
import { ButtonDirective, ButtonLabel } from "primeng/button";
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from "@angular/forms";
import { InputText } from "primeng/inputtext";
import { Message } from "primeng/message";
import { passwordMatchValidator } from '../../../../utils/validators/password-match.validator';
import { getErrorMessage } from '../../../../utils/helpers/validation.helper';
import { RestorePasswordRequestInput } from '../../../../api/authentication/authentication.graphql.types';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastService } from '../../../../services/toast/toast.service';
import { AuthenticationService } from '../../../../services/authentication/authentication.service';


@Component({
  selector: 'app-restore-password-confirm',
    imports: [
        ButtonDirective,
        ButtonLabel,
        FormsModule,
        InputText,
        Message,
        ReactiveFormsModule
    ],
  templateUrl: 'restore-password-confirm.html',
  styleUrl: 'restore-password-confirm.css'
})
export class RestorePasswordConfirm implements OnInit {
  email: string | null = null;
  code: string | null = null;

  restorePasswordForm: FormGroup;
  formSubmitted = false;
  protected readonly getErrorMessage = getErrorMessage;

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private destroyRef: DestroyRef,
    private toastService: ToastService,
    private router: Router,
    private authenticationService: AuthenticationService
  ) {
    this.restorePasswordForm = this.fb.group({
      password: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(20)]],
      confirmPassword: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(20)]],
    }, {validators: passwordMatchValidator}
    );
  }

  ngOnInit(): void {
    this.route.paramMap
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe(async paramMap => {
        this.email = paramMap.get('email');
        this.code = paramMap.get('code');

        if (!this.email || !this.code) {
          this.router.navigate(['/']).then(() => {
            this.toastService.error('Invalid restore password link')
          })
        }
      });
    }

  getErrorMessageByName(controlName: string) : string | null {
    return this.getErrorMessage(this.restorePasswordForm.get(controlName));
  }

  isInvalid(controlName: string) {
    const control = this.restorePasswordForm.get(controlName);
    return control?.invalid && (control.touched || this.formSubmitted);
  }

  protected async onSubmit() {
    this.restorePasswordForm.markAllAsTouched();
    this.formSubmitted = true;

    if (this.restorePasswordForm.valid) {

      const request : RestorePasswordRequestInput = {
        email: this.email!,
        code: this.code!,
        password: this.restorePasswordForm.value.password,
        confirmPassword: this.restorePasswordForm.value.confirmPassword
      };

      await this.authenticationService.restorePasswordAsync(request);

      this.router.navigate(['/login']).then(() => {
        this.toastService.success('Password reset successfully');
      });
    }
  }
}
