import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../environments/environment';
import { User } from '../models/user.model';
import { PagedResult } from '../models/paged-result.model';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private readonly http = inject(HttpClient);

  private readonly apiUrl = `${environment.apiUrl}/users`;

  getUsers(
    page: number = 1,
    pageSize: number = 20
  ): Observable<PagedResult<User>> {
    return this.http.get<PagedResult<User>>(
      `${this.apiUrl}?page=${page}&pageSize=${pageSize}`
    );
  }

  createUser(request: {
    name: string;
    email: string;
  }): Observable<User> {
    return this.http.post<User>(this.apiUrl, request);
  }

  deleteUser(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}