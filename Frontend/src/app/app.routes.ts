import { Routes } from '@angular/router';
import { ListDetailComponent } from './components/list-detail/list-detail';
import { HomeComponent } from './components/home/home';
import { CompletedGamesComponent } from './components/completed-games/completed-games';

export const routes: Routes = [
  {
    path: '',
    component: HomeComponent
  },
  {
    path: 'juegos/:id',
    component: ListDetailComponent
  },
  {
    path: 'completados',
    component: CompletedGamesComponent
  }
];
