import { Routes } from '@angular/router';
import { Login } from './features/auth/pages/login/login';
import {
  RestorePasswordRequest
} from './features/auth/pages/restore-password/restore-password-request/restore-password-request';
import {authenticationGuard, notAuthorizedGuard} from '@cineverse/infrastructure-auth';
import {
  RestorePasswordConfirm
} from './features/auth/pages/restore-password/restore-password-confirm/restore-password-confirm';
import { Register } from './features/auth/pages/register/register';
import { Profile } from './features/profile/pages/profile/profile';
import { Logout } from './features/auth/pages/logout/logout';
import { ConfirmEmail } from './features/auth/pages/confirm-email/confirm-email';
import { ChangePassword } from './features/auth/pages/change-password/change-password';
import { PersonalInformation } from './features/profile/pages/personal-information/personal-information';

export const routes: Routes = [
  {
    path: 'login',
    component: Login,
    title: 'Login',
    canActivate: [notAuthorizedGuard]
  },
  {
    path: 'register',
    component: Register,
    title: 'Create an profile',
    canActivate: [notAuthorizedGuard]
  },
  {
    path: 'profile',
    component: Profile,
    title: 'Profile'
  },
  {
    path: 'confirm-email/:sent',
    component: ConfirmEmail,
    title: 'Confirm Email'
  },
  {
    path: 'logout',
    component: Logout,
    title: 'Logout',
    canActivate: [authenticationGuard]
  },
  {
    path: 'restore-password',
    component: RestorePasswordRequest,
    title: 'Restore Password',
    canActivate: [notAuthorizedGuard],
    pathMatch: 'full'
  },
  {
    path: 'restore-password/:email/:code',
    component: RestorePasswordConfirm,
    title: 'Restore Password',
    canActivate: [notAuthorizedGuard]
  },
  {
    path: 'change-password',
    component: ChangePassword,
    title: 'Change Password',
    canActivate: [authenticationGuard]
  },
  {
    path: 'personal-information',
    component: PersonalInformation,
    title: 'Personal Information',
    canActivate: [authenticationGuard]
  },
  {
    path: '**',
    redirectTo: 'profile'
  }
];
