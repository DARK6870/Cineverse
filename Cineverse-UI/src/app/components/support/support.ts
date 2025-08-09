import { Component } from '@angular/core';
import { InputTextModule } from 'primeng/inputtext';
import { ButtonModule } from 'primeng/button';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { Message } from 'primeng/message';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Textarea } from 'primeng/textarea';
import  { GraphqlService } from '../../services/graphql'

@Component({
  selector: 'app-support',
  imports: [
    InputTextModule,
    ButtonModule,
    FormsModule,
    Message,
    ReactiveFormsModule,
    Textarea
  ],
  templateUrl: 'support.html',
  styleUrl: 'support.css'
})
export class Support {
  supportForm : FormGroup;
  formSubmitted = false;

  constructor(private graphqlService: GraphqlService, private fb: FormBuilder) {
    this.supportForm  = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      email: ['', Validators.required],
      subject: ['', Validators.required],
      description: ['', Validators.required]
    });
  }

  onSubmit() {
    this.formSubmitted = true;
    if (this.supportForm.valid) {

      const request = {
        firstName: this.supportForm.value.firstName,
        lastName: this.supportForm.value.lastName,
        email: this.supportForm.value.email,
        subject: this.supportForm.value.subject,
        description: this.supportForm.value.description
      };

      this.graphqlService.createSupportTicket(request).subscribe({
        next: (result) => {
          console.log('Ticket created:', result);
        },
        error: (error) => {
          console.error('Error creating ticket:', error);
        }
      });
      this.supportForm.reset();
      this.formSubmitted = false;
    }
  }

  isInvalid(controlName: string) {
    const control = this.supportForm.get(controlName);
    return control?.invalid && (control.touched || this.formSubmitted);
  }
}
