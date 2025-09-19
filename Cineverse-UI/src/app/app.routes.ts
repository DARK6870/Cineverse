import { Routes } from '@angular/router';
import { Home } from './pages/home/home';
import { AboutUs } from './pages/about-us/about-us';
import { Contact } from './pages/contact/contact';
import { Login } from './pages/authentication/login/login';
import { Register } from './pages/authentication/register/register';
import { Account } from './pages/account/account';
import { Movies } from './pages/movies/movies';
import { Movie } from './pages/movies/movie-details/movie';
import { ConfirmEmail } from './pages/authentication/confirm-email/confirm-email';
import { Logout } from './pages/authentication/logout/logout';
import { Booking } from './pages/booking/booking';

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
