import { Component, inject, computed, signal } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { toSignal, takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { map, switchMap, debounceTime, distinctUntilChanged, filter, catchError, of } from 'rxjs';
import { UserListService } from '../../core/services/user-list.service';
import { GameService } from '../../core/services/game.service';
import { CommonModule } from '@angular/common';
import { GameCardComponent } from "../game-card/game-card";
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { MatAutocompleteModule, MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatDialog } from '@angular/material/dialog';
import { AddGameDialogComponent } from '../add-game-dialog/add-game-dialog';
import { CompleteGameDialogComponent } from '../complete-game-dialog/complete-game-dialog';
import { GameInfo } from '../../core/interfaces/gameInfo.interface';
import { GameStatus } from '../../core/interfaces/game-status.type';

@Component({
  selector: 'app-list-detail',
  imports: [
    CommonModule,
    GameCardComponent,
    ReactiveFormsModule,
    MatAutocompleteModule,
    MatInputModule,
    MatFormFieldModule
  ],
  templateUrl: './list-detail.html',
  styleUrl: './list-detail.scss',
})
export class ListDetailComponent {
  private route = inject(ActivatedRoute);
  private userListSv = inject(UserListService);
  private gameSv = inject(GameService);
  private dialog = inject(MatDialog);

  // Obtiene el ID de la ruta como signal
  private listId = toSignal(
    this.route.paramMap.pipe(
      map(params => Number(params.get('id')))
    )
  );

  // Computed signal que obtiene la lista actual
  currentList = computed(() => {
    const id = this.listId();
    return id ? this.userListSv.getUserListById(id)() : null;
  });

  // Nombre de la lista (para usar fácilmente en el template)
  listName = computed(() => this.currentList()?.name ?? 'Cargando...');

  // Obtiene los juegos de la lista
  games = signal<GameInfo[]>([]);

  constructor() {
    this.route.paramMap.pipe(
      map(params => Number(params.get('id'))),
      switchMap(id => this.userListSv.getGamesByListId(id).pipe(
        catchError(() => of([]))
      )),
      takeUntilDestroyed()
    ).subscribe(games => this.games.set(games));
  }

  // Control del buscador
  searchControl = new FormControl('');

  // Resultados de la búsqueda
  searchResults = toSignal(
    this.searchControl.valueChanges.pipe(
      debounceTime(300),
      distinctUntilChanged(),
      filter(value => (value?.length ?? 0) >= 3),
      switchMap(value => this.gameSv.searchGames(value!))
    ),
    { initialValue: [] }
  );

  // Función para mostrar el nombre del juego en el input cuando se selecciona
  displayGame(game: GameInfo | string): string {
    return typeof game === 'string' ? game : (game?.name ?? '');
  }

  onStatusChange(game: GameInfo, status: GameStatus): void {
    const STATUS_ID_MAP: Record<GameStatus, number> = {
      'pendiente': 1,
      'en-progreso': 2,
      'completado': 3,
    };

    if (status === 'completado') {
      const dialogRef = this.dialog.open(CompleteGameDialogComponent, {
        width: '400px',
        data: { game }
      });

      dialogRef.afterClosed().subscribe(confirmed => {
        if (confirmed) {
          this.callUpdateStatus(game, status, STATUS_ID_MAP[status], new Date().toISOString());
        }
      });
    } else {
      this.callUpdateStatus(game, status, STATUS_ID_MAP[status]);
    }
  }

  private callUpdateStatus(game: GameInfo, status: GameStatus, statusId: number, completedAt?: string): void {
    const listId = this.listId();
    if (!listId) return;
    this.userListSv.updateGameStatus(listId, game.id!, statusId, completedAt).subscribe({
      next: () => this.games.update(list =>
        list.map(g => g.id === game.id ? { ...g, gameStatusId: statusId, gameStatusName: status } : g)
      ),
      error: err => console.error('Error al actualizar estado:', err)
    });
  }

  // Manejar la selección de un juego del autocomplete
  onGameSelected(event: MatAutocompleteSelectedEvent): void {
    const game = event.option.value as GameInfo;

    const dialogRef = this.dialog.open(AddGameDialogComponent, {
      width: '400px',
      data: {
        game: game,
        listName: this.listName()
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        const listId = this.listId();
        if (listId) {
          this.userListSv.addGameToList(listId, game).subscribe({
            next: () => {
              console.log('Juego agregado exitosamente');
              // Recargar la página para ver el juego agregado
              window.location.reload();
            },
            error: (err) => {
              console.error('Error al agregar juego:', err);
            }
          });
        }
      }
      // Limpiar el input
      this.searchControl.setValue('');
    });
  }
}
