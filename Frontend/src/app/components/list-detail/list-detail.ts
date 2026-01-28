import { Component, inject, computed, signal } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { toSignal } from '@angular/core/rxjs-interop';
import { map, switchMap, debounceTime, distinctUntilChanged, filter } from 'rxjs';
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
import { GameInfo } from '../../core/interfaces/gameInfo.interface';

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
  games = toSignal(
    this.route.paramMap.pipe(
      map(params => Number(params.get('id'))),
      switchMap(id => this.userListSv.getGamesByListId(id))
    ),
    { initialValue: [] }
  );

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
  displayGame(game: any): string {
    return game && game.name ? game.name : '';
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
