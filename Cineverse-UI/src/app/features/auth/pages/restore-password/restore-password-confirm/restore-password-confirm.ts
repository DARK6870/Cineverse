import { Component, DestroyRef, OnInit } from '@angular/core';
import { ButtonDirective, ButtonLabel } from "primeng/button";
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from "@angular/forms";
import { InputText } from "primeng/inputtext";
import { Message } from "primeng/message";
import { passwordMatchValidator } from '../../../../../shared/validators/password-match.validator';
import { getErrorMessage } from '../../../../../shared/helpers/validation.helper';
import { RestorePasswordRequestInput } from '../../../api/auth.graphql.types';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastService } from '../../../../../core/services/toast.service';
import { AuthenticationService } from '../../../../../core/services/authentication.service';
import { ConfirmationService } from 'primeng/api';
import { ConfirmDialog } from 'primeng/confirmdialog';


@Component({
  selector: 'app-restore-password-confirm',
    imports: [
        ButtonDirective,
        ButtonLabel,
        FormsModule,
        InputText,
        Message,
        ReactiveFormsModule,
        ConfirmDialog
    ],
  standalone: true,
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
    private authenticationService: AuthenticationService,
    private confirmationService: ConfirmationService
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
      this.confirmationService.confirm({
        message: 'Are you sure that you want to proceed?',
        header: 'Confirmation',
        icon: 'pi pi-exclamation-triangle',
        acceptLabel: 'Save Changes',
        rejectLabel: 'Cancel',
        acceptButtonStyleClass: 'p-button-primary',
        rejectButtonStyleClass: 'p-button-secondary',
        accept: () => this.submitForm(),
      });
    }
  }

  private async submitForm(){
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
