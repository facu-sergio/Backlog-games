import { Component, Input } from '@angular/core';
import { GameInfo } from '../../core/interfaces/gameInfo.interface';

@Component({
  selector: 'app-game-card',
  imports: [],
  templateUrl: './game-card.html',
  styleUrl: './game-card.scss',
})
export class GameCardComponent {
    @Input() game!: GameInfo;

  getFormattedYear(): string | null {
    if (!this.game.firstReleaseDate) return null;
    const date = new Date(this.game.firstReleaseDate * 1000);
    return date.getFullYear().toString();
  }

  getDefaultCover(): string {
    return 'assets/images/no-cover.png'; // o una imagen por defecto
  }

  getRatingColor(): string {
    if (!this.game.rating) return 'transparent';
    if (this.game.rating >= 80) return '#4ade80'; // verde
    if (this.game.rating >= 60) return '#fbbf24'; // amarillo
    return '#f87171'; // rojo
  }
}
