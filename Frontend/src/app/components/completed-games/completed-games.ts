import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { MatDialog } from '@angular/material/dialog';
import { GameCardComponent } from '../game-card/game-card';
import { CompleteGameDialogComponent } from '../complete-game-dialog/complete-game-dialog';
import { UserListService } from '../../core/services/user-list.service';
import { GameInfo } from '../../core/interfaces/gameInfo.interface';
import { GameStatus } from '../../core/interfaces/game-status.type';

const STATUS_ID_MAP: Record<GameStatus, number> = {
  'pendiente': 1,
  'en-progreso': 2,
  'completado': 3,
};

const STATUS_LABEL_MAP: Record<GameStatus, string> = {
  'pendiente': 'Pendiente',
  'en-progreso': 'En Progreso',
  'completado': 'Completado',
};

@Component({
  selector: 'app-completed-games',
  standalone: true,
  imports: [CommonModule, GameCardComponent, MatIconModule],
  templateUrl: './completed-games.html',
  styleUrl: './completed-games.scss',
})
export class CompletedGamesComponent {
  private userListSv = inject(UserListService);
  private dialog = inject(MatDialog);

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

  onStatusChange(game: GameInfo, status: GameStatus): void {
    const dialogRef = this.dialog.open(CompleteGameDialogComponent, {
      width: '400px',
      data: { game, statusLabel: STATUS_LABEL_MAP[status] }
    });

    dialogRef.afterClosed().subscribe(confirmed => {
      if (confirmed) {
        const completedAt = status === 'completado' ? new Date().toISOString() : undefined;
        this.callUpdateStatus(game, status, completedAt);
      }
    });
  }

  private callUpdateStatus(game: GameInfo, status: GameStatus, completedAt?: string): void {
    const listId = game.userListId;
    if (!listId || !game.id) return;
    const statusId = STATUS_ID_MAP[status];
    this.userListSv.updateGameStatus(listId, game.id, statusId, completedAt).subscribe({
      next: () => this.games.update(list => list.filter(g => g.id !== game.id)),
      error: err => console.error('Error al actualizar estado:', err),
    });
  }
}
