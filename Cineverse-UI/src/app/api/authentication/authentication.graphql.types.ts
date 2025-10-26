export interface AuthenticationResponse {
  refreshToken: string,
  accessToken: string,
  success: boolean,
  message?: string
}

export interface LoginRequestInput {
  email: string,
  password: string
}

export interface RegisterRequestInput {
  firstName: string,
  lastName: string,
  email: string,
  password: string,
  confirmPassword: string
}

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
