import { Routes } from '@angular/router';
import { Home } from './components/home/home';
import { Screenings } from './components/screenings/screenings';
import { AboutUs } from './components/about-us/about-us';
import { Contact } from './components/contact/contact';

export const routes: Routes = [
  {
    path: '',
    component: Home,
    title: 'Cineverse - Home'
  },
  {
    path: 'screenings',
    component: Screenings,
    title: 'Screenings'
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
];
