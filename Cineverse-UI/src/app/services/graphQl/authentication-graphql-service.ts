import { Injectable } from '@angular/core';
import { Apollo } from 'apollo-angular';
import { Observable } from 'rxjs';
import {
  LOGIN_MUTATION,
  REGISTER_MUTATION,
  CONFIRM_EMAIL_MUTATION,
  RESEND_VERIFICATION_CODE_MUTATION
} from '../../common/constants/graphql/authentication-operations';

@Injectable({
  providedIn: 'root'
})

export class AuthenticationGraphQlService {
  constructor(private apollo: Apollo) {}

  login(request: {
    email: string;
    password: string;
  }): Observable<any> {
    return this.apollo.mutate({
      mutation: LOGIN_MUTATION,
      variables: { request }
    });
  }

  register(request: {
    firstName: string;
    lastName: string;
    email: string;
    password: string;
    confirmPassword: string;
  }) : Observable<any> {
    return this.apollo.mutate({
      mutation: REGISTER_MUTATION,
      variables: { request }
    })
  }

  confirmEmail(code: number) : Observable<any> {
    return this.apollo.mutate({
      mutation: CONFIRM_EMAIL_MUTATION,
      variables: { code }
    })
  }

  resendVerificationCode() : Observable<any> {
    return this.apollo.mutate({
      mutation: RESEND_VERIFICATION_CODE_MUTATION
    })
  }
}
