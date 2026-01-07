import { gql } from 'apollo-angular';

export const generateAccessTokenMutation = (refreshToken: string) => ({
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
