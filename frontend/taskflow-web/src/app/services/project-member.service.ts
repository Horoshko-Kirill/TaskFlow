import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../environments/environment';
import {
  ProjectMember,
  ProjectMemberListItem
} from '../models/project-member.model';
import { PagedResult } from '../models/paged-result.model';

@Injectable({
  providedIn: 'root'
})
export class ProjectMemberService {
  private readonly http = inject(HttpClient);

  private readonly apiUrl = `${environment.apiUrl}/projects`;

  getByProject(
    projectId: string,
    page: number = 1,
    pageSize: number = 100
  ): Observable<PagedResult<ProjectMemberListItem>> {
    return this.http.get<PagedResult<ProjectMemberListItem>>(
      `${this.apiUrl}/${projectId}/members?page=${page}&pageSize=${pageSize}`
    );
  }

  addMember(
    projectId: string,
    userId: string
  ): Observable<ProjectMember> {
    return this.http.post<ProjectMember>(
      `${this.apiUrl}/${projectId}/members`,
      { userId }
    );
  }

  removeMember(
    projectId: string,
    userId: string
  ): Observable<void> {
    return this.http.delete<void>(
      `${this.apiUrl}/${projectId}/members/${userId}`
    );
  }
}