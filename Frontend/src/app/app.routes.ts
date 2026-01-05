import { Routes } from '@angular/router';
import { GamesComponent } from './components/games-component/games-component';

export const routes: Routes = [
  {
    path: 'juegos/:id',
    component: GamesComponent
  }
];
