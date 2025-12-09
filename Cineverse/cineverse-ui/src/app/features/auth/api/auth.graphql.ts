import { MutationOptions } from '@apollo/client';
import { gql } from 'apollo-angular';
import {
  ChangePasswordRequestInput,
  LoginRequestInput,
  RegisterRequestInput,
  RestorePasswordRequestInput
} from './auth.graphql.types';

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

export const changePasswordMutation = (request: ChangePasswordRequestInput) : MutationOptions => ({
  mutation: gql`
  mutation changePassword($request: ChangePasswordRequestInput!){
    changePassword(request: $request)
}
`,
  variables: {
    request: request
  }
});

export const sendRestorePasswordEmailMutation = (email: string) : MutationOptions => ({
  mutation: gql`
  mutation sendRestorePasswordEmail($email: String!){
  sendRestorePasswordEmail(email: $email)
}
`,
  variables: {
    email: email
  },
  context: {
    allowAnonymous: true
  }
});

export const restorePasswordMutation = (request: RestorePasswordRequestInput) : MutationOptions => ({
  mutation: gql`
  mutation restorePassword($request: RestorePasswordRequestInput!){
  restorePassword(request: $request)
}
`,
  variables: {
    request: request
  },
  context: {
    allowAnonymous: true
  }
});
