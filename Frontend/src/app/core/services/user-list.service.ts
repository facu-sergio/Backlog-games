import { HttpClient } from '@angular/common/http';
import { Injectable, signal, computed } from '@angular/core';
import { environment } from '../../../enviroments/enviroment';
import { map, Observable } from 'rxjs';
import { UserList } from '../interfaces/userList.interface';
import { GamesByListResponse, UserListResponse } from '../interfaces/response.interface';
import { GameInfo } from '../interfaces/gameInfo.interface';
import { GameStatus } from '../interfaces/game-status.type';
import { AddGameToList } from '../interfaces/addGameToList.interface';

@Injectable({
  providedIn: 'root',
})

export class UserListService {
  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) {
    this.loadUserLists();
  }

  // Signal centralizado con todas las listas
  private userListsSignal = signal<UserList[]>([]);

  // Señal pública de solo lectura
  readonly userLists = this.userListsSignal.asReadonly();

  // Método para obtener una lista específica por ID (computed signal)
  getUserListById(id: number) {
    return computed(() =>
      this.userListsSignal().find(list => list.id === id)
    );
  }

  // Carga inicial de las listas
  private loadUserLists(): void {
    this.http.get<UserListResponse>(`${this.apiUrl}/UserList`)
      .subscribe({
        next: (res) => {
          this.userListsSignal.set(res.data);
        },
        error: (error) => {
          console.error('Error al cargar listas:', error);
        }
      });
  }

  // Método legacy para mantener compatibilidad si lo necesitas
  getAllUserList(): Observable<UserListResponse> {
    return this.http.get<UserListResponse>(`${this.apiUrl}/UserList`);
  }

  private readonly STATUS_NORMALIZE: Record<string, GameStatus> = {
    'Pendiente': 'pendiente',
    'En Progreso': 'en-progreso',
    'Completado': 'completado',
  };

  private normalizeGames(games: GameInfo[]): GameInfo[] {
    return games.map(g => {
      const rawStatus = (g.gameStatusName ?? (g as any).statusName) as string;
      return {
        ...g,
        gameStatusName: this.STATUS_NORMALIZE[rawStatus] ?? 'pendiente',
      };
    });
  }

  getGamesByListId(idList: number): Observable<GameInfo[]> {
    return this.http.get<GamesByListResponse>(`${this.apiUrl}/UserList/${idList}/with-games`)
      .pipe(map(response => this.normalizeGames(response.data)));
  }

  addGameToList(listId: number, game: GameInfo, statusId?: number): Observable<any> {
    const payload: AddGameToList = {
      userListId: listId,
      gameInfo: game,
      statusId: statusId
    };
    return this.http.post(`${this.apiUrl}/UserList/add-game-to-list/${listId}`, payload);
  }

  getCompletedGames(year?: number): Observable<GameInfo[]> {
    const params = year ? `?year=${year}` : '';
    return this.http.get<GamesByListResponse>(`${this.apiUrl}/UserList/completed-games${params}`)
      .pipe(map(response => this.normalizeGames(response.data)));
  }

  updateGameStatus(listId: number, gameId: number, statusId: number, completedAt?: string): Observable<any> {
    const body: { statusId: number; completedAt?: string } = { statusId };
    if (completedAt) body.completedAt = completedAt;
    return this.http.patch(`${this.apiUrl}/UserList/${listId}/games/${gameId}/status`, body);
  }
}
