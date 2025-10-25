import { Routes } from '@angular/router';
import { Home } from './pages/home/home';
import { AboutUs } from './pages/about-us/about-us';
import { Contact } from './pages/contact/contact';
import { Login } from './pages/authentication/login/login';
import { Register } from './pages/authentication/register/register';
import { Account } from './pages/account/account';
import { Movies } from './pages/movies/movies';
import { MovieDetails } from './pages/movies/movie-details/movie-details';
import { ConfirmEmail } from './pages/authentication/confirm-email/confirm-email';
import { Logout } from './pages/authentication/logout/logout';
import { Booking } from './pages/booking/booking';
import { CreateBooking } from './pages/booking/create-booking/create-booking';
import { ChangePassword } from './pages/authentication/change-password/change-password';
import { PersonalInformation } from './pages/authentication/personal-information/personal-information';
import { MyBookings } from './pages/booking/my-bookings/my-bookings';
import { AuthenticationGuard } from './utils/guards/authentication.guard';
import { NotAuthorizedGuard } from './utils/guards/not-authorized.guard';

export const routes: Routes = [
  {
    path: '',
    component: Home,
    title: 'Cineverse - Home'
  },
  {
    path: 'about-us',
    component: AboutUs,
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
    title: 'Create an account',
    canActivate: [NotAuthorizedGuard]
  },
  {
    path: 'account',
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
  }
];
