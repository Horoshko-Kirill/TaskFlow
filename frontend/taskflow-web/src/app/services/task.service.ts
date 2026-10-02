import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../environments/environment';
import { Task } from '../models/task.model';
import { PagedResult } from '../models/paged-result.model';
import { CreateTaskRequest } from '../models/create-task-request.model';
import { UpdateTaskRequest } from '../models/update-task-request.model';

@Injectable({
  providedIn: 'root'
})
export class TaskService {
  private readonly http = inject(HttpClient);

  private readonly apiUrl = `${environment.apiUrl}/tasks`;

  getByProject(
    projectId: string,
    page: number = 1,
    pageSize: number = 100
  ): Observable<PagedResult<Task>> {
    return this.http.get<PagedResult<Task>>(
      `${this.apiUrl}/project/${projectId}?page=${page}&pageSize=${pageSize}`
    );
  }

  getMyTasks(
    userId: string,
    page: number = 1,
    pageSize: number = 20
  ): Observable<PagedResult<Task>> {
    return this.http.get<PagedResult<Task>>(
      `${this.apiUrl}/my?userId=${userId}&page=${page}&pageSize=${pageSize}`
    );
  }

  createTask(request: CreateTaskRequest): Observable<Task> {
    return this.http.post<Task>(this.apiUrl, request);
  }

  updateTask(
    id: string,
    request: UpdateTaskRequest
  ): Observable<Task> {
    return this.http.put<Task>(
      `${this.apiUrl}/${id}`,
      request
    );
  }

  deleteTask(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}