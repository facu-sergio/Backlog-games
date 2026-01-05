import { Component, inject } from '@angular/core';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { CommonModule } from '@angular/common';
import { UserList } from '../../core/interfaces/userList.interface';
import { UserListService } from '../../core/services/user-list.service';
import { RouterLink } from '@angular/router';
import { toSignal } from '@angular/core/rxjs-interop';
import { map } from 'rxjs';

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

  userlist = toSignal(
    this.userListSv.getAllUserList().pipe(
      map(res => res.data)
    ),
    { initialValue: [] as UserList[] }
  );

  agregarLista() {
    console.log('Agregar nueva lista');
  }
}
