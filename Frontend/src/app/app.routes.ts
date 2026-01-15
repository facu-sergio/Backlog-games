import { Routes } from '@angular/router';
import { ListDetailComponent } from './components/list-detail/list-detail';

export const routes: Routes = [
  {
    path: 'juegos/:id',
    component: ListDetailComponent
  }
];
