import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ListCardComponent } from '../list-card/list-card';
import { UserListService } from '../../core/services/user-list.service';
import { GameInfo } from '../../core/interfaces/gameInfo.interface';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, ListCardComponent],
  templateUrl: './home.html',
  styleUrl: './home.scss'
})
export class HomeComponent implements OnInit {
  private userListService = inject(UserListService);

  userLists = this.userListService.userLists;
  gamesPerList = signal<Map<number, GameInfo[]>>(new Map());

  ngOnInit(): void {
    this.loadGamesForLists();
  }

  private loadGamesForLists(): void {
    const lists = this.userLists();
    if (lists.length === 0) {
      // Si las listas aún no cargaron, reintentar después
      setTimeout(() => this.loadGamesForLists(), 500);
      return;
    }

    const requests = lists.map(list =>
      this.userListService.getGamesByListId(list.id)
    );

    forkJoin(requests).subscribe({
      next: (results) => {
        const gamesMap = new Map<number, GameInfo[]>();
        lists.forEach((list, index) => {
          gamesMap.set(list.id, results[index]);
        });
        this.gamesPerList.set(gamesMap);
      },
      error: (error) => {
        console.error('Error loading games for lists:', error);
      }
    });
  }

  getGamesForList(listId: number): GameInfo[] {
    return this.gamesPerList().get(listId) || [];
  }
}
