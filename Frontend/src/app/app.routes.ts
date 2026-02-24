import { Routes } from '@angular/router';
import { ListDetailComponent } from './components/list-detail/list-detail';
import { HomeComponent } from './components/home/home';
import { CompletedGamesComponent } from './components/completed-games/completed-games';
import { LoginComponent } from './components/login/login';
import { ChangePasswordComponent } from './components/change-password/change-password';
import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  {
    path: 'login',
    component: LoginComponent,
  },
  {
    path: '',
    component: HomeComponent,
    canActivate: [authGuard],
  },
  {
    path: 'juegos/:id',
    component: ListDetailComponent,
    canActivate: [authGuard],
  },
  {
    path: 'completados',
    component: CompletedGamesComponent,
    canActivate: [authGuard],
  },
  {
    path: 'change-password',
    component: ChangePasswordComponent,
    canActivate: [authGuard],
  },
];
