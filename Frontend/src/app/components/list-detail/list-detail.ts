import { Component, inject, computed } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { toSignal } from '@angular/core/rxjs-interop';
import { map, switchMap } from 'rxjs';
import { UserListService } from '../../core/services/user-list.service';
import { CommonModule } from '@angular/common';
import { GameCardComponent } from "../game-card/game-card";

@Component({
  selector: 'app-list-detail',
  imports: [CommonModule, GameCardComponent],
  templateUrl: './list-detail.html',
  styleUrl: './list-detail.scss',
})
export class ListDetailComponent {
  private route = inject(ActivatedRoute);
  private userListSv = inject(UserListService);

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
}
