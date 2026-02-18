import { Routes } from '@angular/router';
import { ListDetailComponent } from './components/list-detail/list-detail';
import { HomeComponent } from './components/home/home';

export const routes: Routes = [
  {
    path: '',
    component: HomeComponent
  },
  {
    path: 'juegos/:id',
    component: ListDetailComponent
  }
];
