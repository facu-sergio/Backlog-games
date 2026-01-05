import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../enviroments/enviroment';
import { map, Observable } from 'rxjs';
import { UserList } from '../interfaces/userList.interface';
import { UserListResponse } from '../interfaces/response.interface';

@Injectable({
  providedIn: 'root',
})

export class UserListService {
  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getAllUserList(): Observable<UserListResponse> {
    return this.http.get<UserListResponse>(`${this.apiUrl}/UserList`);
  }
}
