import { Component, inject } from '@angular/core';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { CommonModule } from '@angular/common';
import { UserListService } from '../../core/services/user-list.service';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-sidenav',
  imports: [
    CommonModule,
    MatSidenavModule,
    MatListModule,
    MatIconModule,
    MatButtonModule,
    RouterLink
  ],
  templateUrl: './sidenav.html',
  styleUrl: './sidenav.scss',
})
export class Sidenav {
  private userListSv = inject(UserListService);

  // Usa el signal centralizado del servicio
  userlist = this.userListSv.userLists;

  agregarLista() {
    console.log('Agregar nueva lista');
  }
}
