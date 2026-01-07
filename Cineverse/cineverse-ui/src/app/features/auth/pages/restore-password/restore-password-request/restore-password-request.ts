import { Component, inject, signal } from '@angular/core';
import { ButtonDirective, ButtonLabel } from 'primeng/button';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { InputText } from 'primeng/inputtext';
import { Message } from 'primeng/message';
import { getErrorMessage } from '../../../../../shared/helpers/validation.helper';
import { AuthGraphqlService } from '../../../api/auth.graphql.service';
import { firstValueFrom } from 'rxjs';
import { ToastService } from 'infrastructure-common';

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
  standalone: true,
  templateUrl: 'restore-password-request.html',
  styleUrl: 'restore-password-request.css'
})
export class RestorePasswordRequest {
  private formBuilder = inject(FormBuilder);
  private toastService = inject(ToastService);
  private authenticationGraphqlService = inject(AuthGraphqlService);

  restorePasswordForm: FormGroup = this.formBuilder.group({
    email: ['', [Validators.required, Validators.email]]
  });

  formSubmitted = false;
  sent = signal<boolean>(false);
  protected readonly getErrorMessage = getErrorMessage;


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
