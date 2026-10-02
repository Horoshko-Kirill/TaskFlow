import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../environments/environment';
import { Comment } from '../models/comment.model';
import { PagedResult } from '../models/paged-result.model';

@Injectable({
  providedIn: 'root'
})
export class CommentService {
  private readonly http = inject(HttpClient);

  private readonly apiUrl = `${environment.apiUrl}`;

  getByTask(
    taskId: string,
    page: number = 1,
    pageSize: number = 100
  ): Observable<PagedResult<Comment>> {
    return this.http.get<PagedResult<Comment>>(
      `${this.apiUrl}/tasks/${taskId}/comments?page=${page}&pageSize=${pageSize}`
    );
  }

  create(
    taskId: string,
    authorId: string,
    text: string
  ): Observable<Comment> {
    return this.http.post<Comment>(
      `${this.apiUrl}/comments`,
      {
        taskId,
        authorId,
        text
      }
    );
  }

  update(
    id: string,
    text: string
  ): Observable<Comment> {
    return this.http.put<Comment>(
      `${this.apiUrl}/comments/${id}`,
      { text }
    );
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(
      `${this.apiUrl}/comments/${id}`
    );
  }
}