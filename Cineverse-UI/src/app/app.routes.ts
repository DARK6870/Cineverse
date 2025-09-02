import { Routes } from '@angular/router';
import { Home } from './components/home/home';
import { AboutUs } from './components/about-us/about-us';
import { Contact } from './components/contact/contact';
import { Login } from './components/account/login/login';
import { Register } from './components/account/register/register';
import { Account } from './components/account/account/account';
import { Movies } from './components/movie/movies/movies';
import { Movie } from './components/movie/movie/movie';
import { ConfirmEmail } from './components/account/confirm-email/confirm-email';
import { Logout } from './components/account/logout/logout';

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
    path: 'movie/:movieId',
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
  }
];
