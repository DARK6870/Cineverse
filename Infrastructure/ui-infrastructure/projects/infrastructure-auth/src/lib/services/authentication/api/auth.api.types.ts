export interface AuthenticationResponse {
  refreshToken: string,
  accessToken: string,
  success: boolean,
  message?: string
}

export interface GenerateAccessTokenRequest {
  refreshToken: string
}
