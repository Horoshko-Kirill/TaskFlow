import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../environments/environment';
import { Project } from '../models/project.model';
import { PagedResult } from '../models/paged-result.model';
import { CreateProjectRequest } from '../models/create-project-request.model';
import { UpdateProjectRequest } from '../models/update-project-request.model';

@Injectable({
  providedIn: 'root'
})
export class ProjectService {
  private readonly http = inject(HttpClient);

  private readonly apiUrl = `${environment.apiUrl}/projects`;

  getProjects(
    page: number = 1,
    pageSize: number = 20
  ): Observable<PagedResult<Project>> {
    return this.http.get<PagedResult<Project>>(
      `${this.apiUrl}?page=${page}&pageSize=${pageSize}`
    );
  }

  getMyProjects(
    userId: string,
    page: number = 1,
    pageSize: number = 20
  ): Observable<PagedResult<Project>> {
    return this.http.get<PagedResult<Project>>(
      `${this.apiUrl}/my?userId=${userId}&page=${page}&pageSize=${pageSize}`
    );
  }

  getProject(id: string): Observable<Project> {
    return this.http.get<Project>(
      `${this.apiUrl}/${id}`
    );
  }

  createProject(
    userId: string,
    request: CreateProjectRequest
  ): Observable<Project> {
    return this.http.post<Project>(
      `${this.apiUrl}?userId=${userId}`,
      request
    );
  }

  updateProject(
    id: string,
    request: UpdateProjectRequest
  ): Observable<Project> {
    return this.http.put<Project>(
      `${this.apiUrl}/${id}`,
      request
    );
  }

  deleteProject(id: string): Observable<void> {
    return this.http.delete<void>(
      `${this.apiUrl}/${id}`
    );
  }
}