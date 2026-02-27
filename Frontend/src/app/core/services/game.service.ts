import { HttpClient } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { environment } from '../../../enviroments/environment';
import { Observable } from 'rxjs';
import { GameInfo } from '../interfaces/gameInfo.interface';

@Injectable({
  providedIn: 'root',
})
export class GameService {
  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  searchGames(name: string): Observable<GameInfo[]> {
    return this.http.get<GameInfo[]>(`${this.apiUrl}/Games/Search`, {
      params: { name }
    });
  }

  // TODO: Agregar métodos para obtener juegos
  // Ejemplo: getGamesByListId(listId: number)
  // Ejemplo: addGameToList(gameId: number, listId: number)
}
