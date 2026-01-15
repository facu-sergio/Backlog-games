import { HttpClient } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { environment } from '../../../enviroments/enviroment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class GameService {
  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  // TODO: Agregar métodos para obtener juegos
  // Ejemplo: getGamesByListId(listId: number)
  // Ejemplo: searchGames(query: string)
  // Ejemplo: addGameToList(gameId: number, listId: number)
}
