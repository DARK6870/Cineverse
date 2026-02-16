import { Routes } from '@angular/router';
import { Dashboard } from './features/dashboard/pages/dashboard/dashboard';
import { Movies } from './features/movie/pages/movies/movies';
import { MovieAddEdit } from './features/movie/pages/movie-add-edit/movie-add-edit';
import { Bookings } from './features/booking/pages/bookings/bookings';
import { Users } from './features/user/pages/users/users';
import { Halls } from './features/hall/pages/halls/halls';
import { Screenings } from './features/screening/pages/screenings/screenings';

export const routes: Routes = [
  {
    path: '',
    component: Dashboard,
    title: 'Admin - Dashboard',
  },
  {
    path: 'movies',
    component: Movies,
    title: 'Admin - Movies',
  },
  {
    path: 'movies/new',
    component: MovieAddEdit,
    title: 'Admin - New Movie',
  },
  {
    path: 'movies/:id/edit',
    component: MovieAddEdit,
    title: 'Admin - Edit Movie',
  },
  {
    path: 'bookings',
    component: Bookings,
    title: 'Admin - Bookings',
  },
  {
    path: 'users',
    component: Users,
    title: 'Admin - Users',
  },
  {
    path: 'halls',
    component: Halls,
    title: 'Admin - Halls',
  },
  {
    path: 'screenings',
    component: Screenings,
    title: 'Admin - Screenings',
  },
  {
    path: '**',
    redirectTo: '',
  }
];
