import { Component, Input, Output, EventEmitter } from '@angular/core';
import { MatMenuModule } from '@angular/material/menu';
import { MatButtonModule } from '@angular/material/button';
import { GameInfo } from '../../core/interfaces/gameInfo.interface';
import { GameStatus } from '../../core/interfaces/game-status.type';
import { SecondsToHoursPipe } from '../../core/pipes/seconds-to-hours.pipe';


@Component({
  selector: 'app-game-card',
  imports: [MatMenuModule, MatButtonModule, SecondsToHoursPipe],
  templateUrl: './game-card.html',
  styleUrl: './game-card.scss',
})
export class GameCardComponent {
  @Input() game!: GameInfo;
  @Input() status: GameStatus = 'pendiente';
  @Output() statusChange = new EventEmitter<GameStatus>();

  readonly statusOptions: { value: GameStatus; label: string }[] = [
    { value: 'pendiente', label: 'Pendiente' },
    { value: 'en-progreso', label: 'En Progreso' },
    { value: 'completado', label: 'Completado' },
  ];

  getStatusLabel(): string {
    return this.statusOptions.find(o => o.value === this.status)?.label ?? 'Pendiente';
  }

  onStatusChange(newStatus: GameStatus): void {
    this.statusChange.emit(newStatus);
  }

  getFormattedYear(): string | null {
    if (!this.game.firstReleaseDate) return null;
    const date = new Date(this.game.firstReleaseDate * 1000);
    return date.getFullYear().toString();
  }

  getDefaultCover(): string {
    return 'assets/images/no-cover.png';
  }

  get hasTimeToBeatData(): boolean {
    return (
      this.game.hastilySeconds != null ||
      this.game.normallySeconds != null ||
      this.game.completelySeconds != null
    );
  }

  getRatingColor(): string {
    if (!this.game.rating) return 'transparent';
    if (this.game.rating >= 80) return '#4ade80';
    if (this.game.rating >= 60) return '#fbbf24';
    return '#f87171';
  }
}
