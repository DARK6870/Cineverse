import { Routes } from '@angular/router';
import { Dashboard } from './features/dashboard/pages/dashboard/dashboard';
import { Movies } from './features/movie/pages/movies/movies';
import { MovieAddEdit } from './features/movie/pages/movie-add-edit/movie-add-edit';
import { Bookings } from './features/booking/pages/bookings/bookings';
import { Users } from './features/user/pages/users/users';
import { UserEdit } from './features/user/pages/user-edit/user-edit';
import { Halls } from './features/hall/pages/halls/halls';
import { HallAddEdit } from './features/hall/pages/hall-add-edit/hall-add-edit';
import { Screenings } from './features/screening/pages/screenings/screenings';
import { ScreeningAddEdit } from './features/screening/pages/screening-add-edit/screening-add-edit';
import { unsavedChangesGuard } from './core/guards/unsaved-changes.guard';
import { adminManagerGuard } from './core/guards/admin-manager.guard';

export const routes: Routes = [
  {
    path: '',
    canActivateChild: [adminManagerGuard],
    children: [
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
        canDeactivate: [unsavedChangesGuard],
      },
      {
        path: 'movies/:id/edit',
        component: MovieAddEdit,
        title: 'Admin - Edit Movie',
        canDeactivate: [unsavedChangesGuard],
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
        path: 'users/:id/edit',
        component: UserEdit,
        title: 'Admin - Edit User',
        canDeactivate: [unsavedChangesGuard],
      },
      {
        path: 'halls',
        component: Halls,
        title: 'Admin - Halls',
      },
      {
        path: 'halls/new',
        component: HallAddEdit,
        title: 'Admin - New Hall',
        canDeactivate: [unsavedChangesGuard],
      },
      {
        path: 'halls/:id/edit',
        component: HallAddEdit,
        title: 'Admin - Edit Hall',
        canDeactivate: [unsavedChangesGuard],
      },
      {
        path: 'screenings',
        component: Screenings,
        title: 'Admin - Screenings',
      },
      {
        path: 'screenings/new',
        component: ScreeningAddEdit,
        title: 'Admin - New Screening',
        canDeactivate: [unsavedChangesGuard],
      },
      {
        path: 'screenings/:id/edit',
        component: ScreeningAddEdit,
        title: 'Admin - Edit Screening',
        canDeactivate: [unsavedChangesGuard],
      },
      {
        path: '**',
        redirectTo: '',
      },
    ],
  }
];
