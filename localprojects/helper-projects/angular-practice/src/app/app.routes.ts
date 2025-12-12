import { Routes } from '@angular/router';
import { HomeDemo } from './layout/home-demo/home-demo';
import { ListDemo } from './layout/list-demo/list-demo';
import { InputForm } from './layout/form/input-form/input-form';
import { LoginForm } from './layout/form/login-form/login-form';
import { CardsDemo } from './layout/cards-demo/cards-demo';

export const routes: Routes = [
  {
    path: '',
    component: HomeDemo,
  },
  {
    path: 'demo-listing',
    component: ListDemo,
  },
  {
    path: 'input-forms',
    component: InputForm,
  },
  {
    path: 'login-forms',
    component: LoginForm,
  },
  {
    path: 'cards',
    component: CardsDemo,
  },
  {
    path: 'download-large-file',
    loadComponent: () => import('./donwload-button/donwload-button').then((m) => m.DonwloadButton),
  },
  {
    path: 'diorective-button',
    loadComponent: () =>
      import('./directive-button/directive-button').then((m) => m.DirectiveButton),
  },
  {
    path: 'forms',
    loadComponent: () =>
      import('./directive-button/directive-button').then((m) => m.DirectiveButton),
  },
];
