import { Component, signal } from '@angular/core';
import { ButtonDirective, ButtonLabel } from 'primeng/button';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { InputText } from 'primeng/inputtext';
import { Message } from 'primeng/message';
import { getErrorMessage } from '../../../../../shared/helpers/validation.helper';
import { ToastService } from '../../../../../core/services/toast.service';
import { AuthGraphqlService } from '../../../api/auth.graphql.service';
import { firstValueFrom } from 'rxjs';

@Component({
  selector: 'app-restore-password-request',
  imports: [
    ButtonDirective,
    ButtonLabel,
    FormsModule,
    InputText,
    Message,
    ReactiveFormsModule
  ],
  templateUrl: 'restore-password-request.html',
  styleUrl: 'restore-password-request.css'
})
export class RestorePasswordRequest {
  restorePasswordForm: FormGroup;
  formSubmitted = false;
  sent = signal<boolean>(false);
  protected readonly getErrorMessage = getErrorMessage;

  constructor(
    private fb: FormBuilder,
    private toastService: ToastService,
    private authenticationGraphqlService: AuthGraphqlService
  ) {
    this.restorePasswordForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]]
    })
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

    if (this.restorePasswordForm.valid) {
      const email = this.restorePasswordForm.value.email;

      await firstValueFrom(
        this.authenticationGraphqlService.sendRestorePasswordEmail(email)
      );

      this.sent.set(true);
      this.restorePasswordForm.reset();
      this.toastService.success('Restore email message has been sent to your email');
    }
  }
}
