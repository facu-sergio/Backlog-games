import { Component, inject, signal } from '@angular/core';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatDividerModule } from '@angular/material/divider';
import { MatInputModule } from '@angular/material/input';
import { CommonModule } from '@angular/common';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
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
    MatDividerModule,
    MatInputModule,
    ReactiveFormsModule,
    RouterLink
  ],
  templateUrl: './sidenav.html',
  styleUrl: './sidenav.scss',
})
export class Sidenav {
  private userListSv = inject(UserListService);

  userlist = this.userListSv.userLists;
  showInput = signal(false);
  newListName = new FormControl('');

  toggleInput(): void {
    this.showInput.update(v => !v);
    this.newListName.setValue('');
  }

  confirmCreate(): void {
    const name = this.newListName.value?.trim();
    if (!name) return;
    this.userListSv.createList(name).subscribe({
      next: () => {
        this.showInput.set(false);
        this.newListName.setValue('');
      },
      error: err => console.error('Error al crear lista:', err),
    });
  }
}
