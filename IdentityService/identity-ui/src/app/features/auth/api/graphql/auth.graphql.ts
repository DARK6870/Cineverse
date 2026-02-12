import { gql } from 'apollo-angular';
import {
  ChangePasswordRequestInput,
  RestorePasswordRequestInput
} from './auth.graphql.types';

export const confirmEmailMutation = (verificationCode: number) => ({
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

export const changePasswordMutation = (request: ChangePasswordRequestInput) => ({
  mutation: gql`
  mutation changePassword($request: ChangePasswordRequestInput!){
    changePassword(request: $request)
}
`,
  variables: {
    request: request
  }
});

export const sendRestorePasswordEmailMutation = (email: string) => ({
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

export const restorePasswordMutation = (request: RestorePasswordRequestInput) => ({
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
