import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { UserList } from '../../core/interfaces/userList.interface';
import { GameInfo } from '../../core/interfaces/gameInfo.interface';

@Component({
  selector: 'app-list-card',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './list-card.html',
  styleUrl: './list-card.scss'
})
export class ListCardComponent {
  @Input({ required: true }) list!: UserList;
  @Input() games: GameInfo[] = [];

  readonly totalCards = 5;

  get visibleGames(): (GameInfo | null)[] {
    const games = this.games.slice(0, this.totalCards);
    const slots: (GameInfo | null)[] = [...games];
    // Rellenar con nulls para completar 5 cartas
    while (slots.length < this.totalCards) {
      slots.push(null);
    }
    return slots;
  }

  get gamesCount(): number {
    return this.list.gamesCount ?? this.games.length;
  }

  getDefaultCover(): string {
    return 'assets/images/default-cover.png';
  }
}
