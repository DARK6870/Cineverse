import { Component, OnInit } from '@angular/core';
import { ButtonDirective, ButtonLabel } from 'primeng/button';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { InputText } from 'primeng/inputtext';
import { Message } from 'primeng/message';
import { getErrorMessage } from '../../../../shared/helpers/validation.helper';
import { ToastService } from '../../../../core/services/toast.service';
import { Router } from '@angular/router';
import { firstValueFrom } from 'rxjs';
import { UserGraphqlService } from '../../../profile/api/user.graphql.service';
import { UpdatePersonalInformationRequestInput } from '../../../profile/api/user.graphql.types';
import { AuthenticationService } from '../../../../core/services/authentication.service';
import { JwtClaimsService } from '../../../../core/services/jwt-claims.service';
import { ConfirmationService } from 'primeng/api';
import { ConfirmDialog } from 'primeng/confirmdialog';

@Component({
  selector: 'app-personal-information',
  imports: [
    ButtonDirective,
    ButtonLabel,
    FormsModule,
    InputText,
    Message,
    ReactiveFormsModule,
    ConfirmDialog,
  ],
  standalone: true,
  templateUrl: 'personal-information.html',
  styleUrl: 'personal-information.css'
})
export class PersonalInformation implements OnInit {
  personalInfoForm: FormGroup;
  formSubmitted = false;
  protected readonly getErrorMessage = getErrorMessage;

  constructor(
    private fb: FormBuilder,
    private toastService: ToastService,
    private router: Router,
    private userGraphqlService: UserGraphqlService,
    private authenticationService: AuthenticationService,
    private jwtClaimsService: JwtClaimsService,
    private confirmationService: ConfirmationService
    ) {
    this.personalInfoForm = fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
    })
  }

  async ngOnInit() {
    const decodedToken = await this.jwtClaimsService.decodeTokenAsync();
    const [firstName, lastName] = decodedToken.fullName.split(' ');

    this.personalInfoForm.patchValue({
      firstName: firstName,
      lastName: lastName
      })
  }

  getErrorMessageByName(controlName: string) : string | null {
    return this.getErrorMessage(this.personalInfoForm.get(controlName));
  }

  isInvalid(controlName: string) {
    const control = this.personalInfoForm.get(controlName);
    return control?.invalid && (control.touched || this.formSubmitted);
  }

  protected async onSubmit() {
    this.personalInfoForm.markAllAsTouched();

    if (this.personalInfoForm.valid) {
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

  private async submitForm()
  {
    const request : UpdatePersonalInformationRequestInput = {
      firstName: this.personalInfoForm.value.firstName,
      lastName: this.personalInfoForm.value.lastName
    };

    await firstValueFrom(
      this.userGraphqlService.updatePersonalInformation(request)
    );

    await this.authenticationService.generateAccessTokenAsync(); // Generate a new token
    this.router.navigate(['/profile']).then(() => {
      this.toastService.success('Information updated successfully');
    });
  }
}
