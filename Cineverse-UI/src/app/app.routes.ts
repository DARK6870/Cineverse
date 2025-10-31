import { Routes } from '@angular/router';
import { Home } from './features/home/pages/home/home';
import { About } from './features/about/pages/about/about';
import { Contact } from './features/contact/pages/contact/contact';
import { Login } from './features/auth/pages/login/login';
import { Register } from './features/auth/pages/register/register';
import { Account } from './features/profile/pages/profile/account';
import { Movies } from './features/movie/pages/movies/movies';
import { MovieDetails } from './features/movie/pages/movie-details/movie-details';
import { ConfirmEmail } from './features/auth/pages/confirm-email/confirm-email';
import { Logout } from './features/auth/pages/logout/logout';
import { Booking } from './features/booking/pages/booking/booking';
import { CreateBooking } from './features/booking/pages/create-booking/create-booking';
import { ChangePassword } from './features/auth/pages/change-password/change-password';
import { PersonalInformation } from './features/auth/pages/personal-information/personal-information';
import { MyBookings } from './features/booking/pages/my-bookings/my-bookings';
import { AuthenticationGuard } from './core/guards/authentication.guard';
import { NotAuthorizedGuard } from './core/guards/not-authorized.guard';
import { RestorePasswordRequest } from './features/auth/pages/restore-password/restore-password-request/restore-password-request';
import { RestorePasswordConfirm } from './features/auth/pages/restore-password/restore-password-confirm/restore-password-confirm';

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
    canActivate: [NotAuthorizedGuard]
  },
  {
    path: 'register',
    component: Register,
    title: 'Create an profile',
    canActivate: [NotAuthorizedGuard]
  },
  {
    path: 'profile',
    component: Account,
    title: 'Account'
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
    canActivate: [AuthenticationGuard]
  },
  {
    path: 'booking/:bookingId',
    component: Booking,
    title: 'Booking Details',
    canActivate: [AuthenticationGuard]
  },
  {
    path: 'book/:screeningId',
    component: CreateBooking,
    title: 'Create Booking',
    canActivate: [AuthenticationGuard]
  },
  {
    path: 'change-password',
    component: ChangePassword,
    title: 'Change Password',
    canActivate: [AuthenticationGuard]
  },
  {
    path: 'personal-information',
    component: PersonalInformation,
    title: 'Personal Information',
    canActivate: [AuthenticationGuard]
  },
  {
    path: 'my-bookings',
    component: MyBookings,
    title: 'My Bookings',
    canActivate: [AuthenticationGuard]
  },
  {
    path: 'restore-password',
    component: RestorePasswordRequest,
    title: 'Restore Password',
    canActivate: [NotAuthorizedGuard],
    pathMatch: 'full'
  },
  {
    path: 'restore-password/:email/:code',
    component: RestorePasswordConfirm,
    title: 'Restore Password',
    canActivate: [NotAuthorizedGuard]
  }
];
