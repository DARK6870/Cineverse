import { MutationOptions } from '@apollo/client';
import { gql } from 'apollo-angular';
import {LoginRequestInput, RegisterRequestInput} from './authentication.graphql.types';

export const loginUserMutation = (request: LoginRequestInput) : MutationOptions => ({
  mutation: gql`
  mutation login($request: LoginRequestInput!) {
  login(request: $request) {
    accessToken
    refreshToken
    success
    message
  }
}
`,
  variables: {
    request: request
  },
  context: {
    allowAnonymous: true
  }
});

export const registerUserMutation = (request: RegisterRequestInput) : MutationOptions => ({
  mutation: gql`
  mutation register($request: RegisterRequestInput!) {
  register(registerRequest: $request) {
    accessToken
    refreshToken
    success
    message
  }
}
`,
  variables: {
    request: request
  },
  context: {
    allowAnonymous: true
  }
})

export const confirmEmailMutation = (verificationCode: number) : MutationOptions => ({
  mutation: gql`
  mutation confirmEmail($code: Int!){
  confirmEmail(verificationCode: $code)
}
`,
  variables: {
    code: verificationCode
  }
});

export const resendEmailVerificationCodeMutation = ({
  mutation: gql`
  mutation generateVerificationCode{
  generateEmailVerificationCode
}
`,
});

export const generateAccessTokenMutation = (refreshToken: string) : MutationOptions => ({
  mutation: gql`
  mutation generateAccessToken($refreshToken: String!){
  generateAccessToken(refreshToken: $refreshToken){
    success
    message
    accessToken
    refreshToken
  }
}
`,
  variables: {
    refreshToken: refreshToken
  },
  context:{
    allowAnonymous: true
  }
});

export const deleteRefreshTokenMutation = ({
  mutation: gql`
  mutation deleteRefreshToken{
  deleteRefreshToken
}
`
});
