import { Component, inject } from '@angular/core';
import { InputTextModule } from 'primeng/inputtext';
import { ButtonModule } from 'primeng/button';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Textarea } from 'primeng/textarea';
import { ContactGraphqlService } from '../../api/contact.graphql.service'
import { Message } from 'primeng/message';
import { Select } from 'primeng/select';
import { departmentOptions } from '../../../../shared/constants/department-options';
import { Router } from '@angular/router';
import { getValidationError, ToastService } from '@cineverse/infrastructure-common';

@Component({
  selector: 'app-contact',
  imports: [
    InputTextModule,
    ButtonModule,
    FormsModule,
    ReactiveFormsModule,
    Textarea,
    Message,
    Select,
  ],
  standalone: true,
  templateUrl: 'contact.html',
  styleUrl: 'contact.css'
})
export class Contact {
  private contactService = inject(ContactGraphqlService);
  private formBuilder = inject(FormBuilder);
  private toastService = inject(ToastService);
  private router = inject(Router);

  supportForm : FormGroup = this.formBuilder.group({
    department: ['', Validators.required],
    firstName: ['', Validators.required],
    lastName: ['', Validators.required],
    email: ['', [Validators.required, Validators.email]],
    subject: ['', [Validators.required, Validators.minLength(10), Validators.maxLength(50)]],
    description: ['', [Validators.required, Validators.minLength(30), Validators.maxLength(300)]],
  });

  formSubmitted = false;
  protected readonly getErrorMessage = getValidationError;

  getErrorMessageByName(controlName: string): string | null {
    return this.getErrorMessage(this.supportForm.get(controlName));
  }

  onSubmit() {
    this.supportForm.markAllAsTouched();
    this.formSubmitted = true;
    if (this.supportForm.valid) {

      const request = {
        department: this.supportForm.value.department.value,
        firstName: this.supportForm.value.firstName,
        lastName: this.supportForm.value.lastName,
        email: this.supportForm.value.email,
        subject: this.supportForm.value.subject,
        description: this.supportForm.value.description
      };

      this.contactService.createContactRequest(request).subscribe({
        next: () => {
          this.toastService.success('Ticket created successfully');
          this.router.navigate(['/']).then();
        }
      });
    }
  }

  isInvalid(controlName: string) {
    const control = this.supportForm.get(controlName);
    return control?.invalid && (control.touched || this.formSubmitted);
  }

  protected readonly departmentOptions = departmentOptions;
}
