import { gql } from 'apollo-angular';

const LOGIN_MUTATION = gql`
mutation login($request: LoginRequestInput!) {
  login(request: $request) {
    accessToken
    refreshToken
  }
}
`

const REGISTER_MUTATION = gql`
mutation register($request: RegisterRequestInput!) {
  register(registerRequest: $request) {
    accessToken
    refreshToken
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

export {
  LOGIN_MUTATION,
  REGISTER_MUTATION,
  CONFIRM_EMAIL_MUTATION,
  RESEND_VERIFICATION_CODE_MUTATION
};
