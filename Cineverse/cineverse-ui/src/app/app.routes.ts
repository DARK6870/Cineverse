import { Routes } from '@angular/router';
import { Home } from './features/home/pages/home/home';
import { About } from './features/about/pages/about/about';
import { Contact } from './features/contact/pages/contact/contact';
import { Login } from './features/auth/pages/login/login';
import { Register } from './features/auth/pages/register/register';
import { Profile } from './features/profile/pages/profile/profile';
import { Movies } from './features/movie/pages/movies/movies';
import { MovieDetails } from './features/movie/pages/movie-details/movie-details';
import { ConfirmEmail } from './features/auth/pages/confirm-email/confirm-email';
import { Logout } from './features/auth/pages/logout/logout';
import { Booking } from './features/booking/pages/booking/booking';
import { CreateBooking } from './features/booking/pages/create-booking/create-booking';
import { ChangePassword } from './features/auth/pages/change-password/change-password';
import { PersonalInformation } from './features/auth/pages/personal-information/personal-information';
import { MyBookings } from './features/booking/pages/my-bookings/my-bookings';
import { RestorePasswordRequest } from './features/auth/pages/restore-password/restore-password-request/restore-password-request';
import { RestorePasswordConfirm } from './features/auth/pages/restore-password/restore-password-confirm/restore-password-confirm';
import { authenticationGuard, notAuthorizedGuard } from '@cineverse/infrastructure-auth';

export const routes: Routes = [
  {
    path: '',
    component: Home,
    title: 'Cineverse - Home'
  },
  {
    path: 'about',
    component: About,
    title: 'About Us'
  },
  {
    path: 'contact',
    component: Contact,
    title: 'Contact'
  },
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
    path: 'movies',
    component: Movies,
    title: 'Movies'
  },
  {
    path: 'movies-details/:movieId',
    component: MovieDetails,
    title: 'Movie Details'
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
    path: 'booking/:bookingId',
    component: Booking,
    title: 'Booking Details',
    canActivate: [authenticationGuard]
  },
  {
    path: 'book/:screeningId',
    component: CreateBooking,
    title: 'Create Booking',
    canActivate: [authenticationGuard]
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
    path: 'my-bookings',
    component: MyBookings,
    title: 'My Bookings',
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
  }
];
