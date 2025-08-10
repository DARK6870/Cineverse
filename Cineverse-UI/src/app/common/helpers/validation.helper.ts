import {AbstractControl} from '@angular/forms';

export function getErrorMessage(control: AbstractControl | null): string | null {
  if (!control || !control.errors) return null;

  if (!(control.touched || control.dirty)) return null;

  const errorMessages: Record<string, (error: any) => string> = {
    required: () => `This field is required`,
    email: () => `Please enter a valid email`,
    minlength: (err) => `Minimum length is ${err.requiredLength} characters`,
    maxlength: (err) => `Maximum length is ${err.requiredLength} characters`,
    pattern: () => `Invalid format`,
  };

  for (const errorName in control.errors) {
    if (errorMessages[errorName]) {
      return errorMessages[errorName](control.errors[errorName]);
    }
  }

  return null;
}
