import { Component, OnInit, signal } from '@angular/core';
import { ButtonDirective, ButtonLabel } from 'primeng/button';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { InputText } from 'primeng/inputtext';
import { Message } from 'primeng/message';
import { getErrorMessage } from '../../../utils/helpers/validation.helper';
import { ToastService } from '../../../services/toast/toast.service';
import { Router } from '@angular/router';
import { firstValueFrom } from 'rxjs';
import { UserGraphqlService } from '../../../api/user/user.graphql.service';
import { UpdatePersonalInformationRequestInput } from '../../../api/user/user.graphql.types';
import { AuthenticationService } from '../../../services/authentication/authentication.service';
import { JwtClaimsService } from '../../../services/jwt-claims/jwt-claims.service';
import { JwtPayload} from '../../../utils/models/jwt-payload.model';

@Component({
  selector: 'app-personal-information',
  imports: [
    ButtonDirective,
    ButtonLabel,
    FormsModule,
    InputText,
    Message,
    ReactiveFormsModule
  ],
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
    private jwtClaimsService: JwtClaimsService
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
      const request : UpdatePersonalInformationRequestInput = {
        firstName: this.personalInfoForm.value.firstName,
        lastName: this.personalInfoForm.value.lastName
      };

      await firstValueFrom(
        this.userGraphqlService.updatePersonalInformation(request)
      );

      await this.authenticationService.generateAccessTokenAsync(); // Generate a new token
      this.router.navigate(['/account']).then(() => {
        this.toastService.success('Information updated successfully');
      });
    }
  }
}
// TODO: Add confirmation for update actions
