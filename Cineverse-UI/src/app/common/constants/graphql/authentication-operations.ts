import { gql } from 'apollo-angular';

const LOGIN_MUTATION = gql`
mutation login($request: LoginRequestInput!) {
  login(request: $request) {
    accessToken
    refreshToken
    success
    message
  }
}
`

const REGISTER_MUTATION = gql`
mutation register($request: RegisterRequestInput!) {
  register(registerRequest: $request) {
    accessToken
    refreshToken
    success
    message
  }
}
`

const CONFIRM_EMAIL_MUTATION = gql`
mutation confirmEmail($code: Int!){
  confirmEmail(verificationCode: $code)
}
`

const RESEND_VERIFICATION_CODE_MUTATION = gql`
mutation generateVerificationCode{
  generateEmailVerificationCode
}
`

const GENERATE_ACCESS_TOKEN_MUTATION = gql`
mutation generateAccessToken($refreshToken: String!){
  generateAccessToken(refreshToken: $refreshToken){
    success
    message
    accessToken
    refreshToken
  }
}
`

const DELETE_REFRESH_TOKEN_MUTATION = gql`
mutation deleteRefreshToken{
  deleteRefreshToken
}
`

export {
  LOGIN_MUTATION,
  REGISTER_MUTATION,
  CONFIRM_EMAIL_MUTATION,
  RESEND_VERIFICATION_CODE_MUTATION,
  GENERATE_ACCESS_TOKEN_MUTATION,
  DELETE_REFRESH_TOKEN_MUTATION
};
