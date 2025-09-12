import { Routes } from '@angular/router';
import { Home } from './components/pages/home/home';
import { AboutUs } from './components/pages/about-us/about-us';
import { Contact } from './components/pages/contact/contact';
import { Login } from './components/pages/auth/login/login';
import { Register } from './components/pages/auth/register/register';
import { Account } from './components/pages/account/account';
import { Movies } from './components/pages/movies/movies';
import { Movie } from './components/pages/movies/movie-details/movie';
import { ConfirmEmail } from './components/pages/auth/confirm-email/confirm-email';
import { Logout } from './components/pages/auth/logout/logout';
import { Booking } from './components/pages/booking/booking';

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
    title: 'Login'
  },
  {
    path: 'register',
    component: Register,
    title: 'Create an account'
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
    component: Movie,
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
    title: 'Logout'
  },
  {
    path: 'booking/:bookingId',
    component: Booking,
    title: 'Booking Details'
  }
];
