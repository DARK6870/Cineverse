export interface ChangePasswordRequestInput {
  password: string,
  newPassword: string,
  confirmNewPassword: string
}

export interface RestorePasswordRequestInput {
  email: string,
  code: string,
  password: string,
  confirmPassword: string
}
