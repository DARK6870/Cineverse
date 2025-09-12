import { gql } from 'apollo-angular';

export const LOGIN_MUTATION = gql`
mutation login($request: LoginRequestInput!) {
  login(request: $request) {
    accessToken
    refreshToken
    success
    message
  }
}
`

export const REGISTER_MUTATION = gql`
mutation register($request: RegisterRequestInput!) {
  register(registerRequest: $request) {
    accessToken
    refreshToken
    success
    message
  }
}
`

export const CONFIRM_EMAIL_MUTATION = gql`
mutation confirmEmail($code: Int!){
  confirmEmail(verificationCode: $code)
}
`

export const RESEND_VERIFICATION_CODE_MUTATION = gql`
mutation generateVerificationCode{
  generateEmailVerificationCode
}
`

export const GENERATE_ACCESS_TOKEN_MUTATION = gql`
mutation generateAccessToken($refreshToken: String!){
  generateAccessToken(refreshToken: $refreshToken){
    success
    message
    accessToken
    refreshToken
  }
}
`

export const DELETE_REFRESH_TOKEN_MUTATION = gql`
mutation deleteRefreshToken{
  deleteRefreshToken
}
`
