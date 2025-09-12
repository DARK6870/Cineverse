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
