import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { GameCardComponent } from '../game-card/game-card';
import { UserListService } from '../../core/services/user-list.service';
import { GameInfo } from '../../core/interfaces/gameInfo.interface';

@Component({
  selector: 'app-completed-games',
  standalone: true,
  imports: [CommonModule, GameCardComponent, MatIconModule],
  templateUrl: './completed-games.html',
  styleUrl: './completed-games.scss',
})
export class CompletedGamesComponent {
  private userListSv = inject(UserListService);

  readonly currentYear = new Date().getFullYear();
  selectedYear = signal<number>(this.currentYear);
  games = signal<GameInfo[]>([]);
  errorMessage = signal<string | null>(null);

  constructor() {
    this.loadGames(this.currentYear);
  }

  loadGames(year: number): void {
    this.selectedYear.set(year);
    this.errorMessage.set(null);
    this.games.set([]);
    this.userListSv.getCompletedGames(year).subscribe({
      next: games => this.games.set(games),
      error: err => this.errorMessage.set(err.error?.message ?? 'Ocurrió un error al cargar los juegos.'),
    });
  }
}
