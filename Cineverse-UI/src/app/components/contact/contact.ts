import { Component } from '@angular/core';
import { InputTextModule } from 'primeng/inputtext';
import { ButtonModule } from 'primeng/button';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Textarea } from 'primeng/textarea';
import { GraphqlService } from '../../services/graphql-service'
import { MessageService } from 'primeng/api';
import {Message} from 'primeng/message';
import {Select} from 'primeng/select';
import { getErrorMessage } from '../../common/helpers/validation.helper';
import { departmentOptions } from '../../common/mappers/department-options';

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
  templateUrl: 'contact.html',
  styleUrl: 'contact.css'
})
export class Contact {
  supportForm : FormGroup;
  formSubmitted = false;
  protected readonly getErrorMessage = getErrorMessage;

  constructor(private graphqlService: GraphqlService,
              private fb: FormBuilder,
              private messageService: MessageService
  ) {
    this.supportForm  = this.fb.group({
      department: ['', Validators.required],
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      subject: ['', [Validators.required, Validators.minLength(10), Validators.maxLength(50)]],
      description: ['', [Validators.required, Validators.minLength(30), Validators.maxLength(300)]],
    });
  }

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

      this.graphqlService.createSupportTicket(request).subscribe({
        next: () => {
          this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Ticket created successfully' });
          this.supportForm.reset();
          this.formSubmitted = false;
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
