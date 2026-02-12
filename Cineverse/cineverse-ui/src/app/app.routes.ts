import { Routes } from '@angular/router';
import { Home } from './features/home/pages/home/home';
import { About } from './features/about/pages/about/about';
import { Contact } from './features/contact/pages/contact/contact';
import { Movies } from './features/movie/pages/movies/movies';
import { MovieDetails } from './features/movie/pages/movie-details/movie-details';
import { Booking } from './features/booking/pages/booking/booking';
import { CreateBooking } from './features/booking/pages/create-booking/create-booking';
import { MyBookings } from './features/booking/pages/my-bookings/my-bookings';
import {
  authenticationGuard,
  UserStatus,
  userStatusGuard,
} from '@cineverse/infrastructure-auth';

export const routes: Routes = [
  {
    path: '',
    component: Home,
    title: 'Cineverse - Home',
  },
  {
    path: 'about',
    component: About,
    title: 'About Us',
  },
  {
    path: 'contact',
    component: Contact,
    title: 'Contact',
  },
  {
    path: 'movies',
    component: Movies,
    title: 'Movies',
  },
  {
    path: 'movies-details/:movieId',
    component: MovieDetails,
    title: 'Movie Details',
  },
  {
    path: 'booking/:bookingId',
    component: Booking,
    title: 'Booking Details',
    canActivate: [authenticationGuard],
  },
  {
    path: 'book/:screeningId',
    component: CreateBooking,
    title: 'Create Booking',
    canActivate: [
      authenticationGuard,
      userStatusGuard(UserStatus.Normal, 'Please confirm your email to proceed')
    ],
  },
  {
    path: 'my-bookings',
    component: MyBookings,
    title: 'My Bookings',
    canActivate: [authenticationGuard],
  },
  {
    path: '**',
    redirectTo: '',
  },
];
