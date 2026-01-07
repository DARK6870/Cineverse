export interface AuthenticationResponse {
  refreshToken: string,
  accessToken: string,
  success: boolean,
  message?: string
}

export interface LoginRequest {
  email: string,
  password: string
}

export interface RegisterRequest {
  firstName: string,
  lastName: string,
  email: string,
  password: string,
  confirmPassword: string
}
