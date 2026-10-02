import {
  ChangeDetectorRef,
  Component,
  inject,
  OnInit
} from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { DatePipe } from '@angular/common';

import { ProjectService } from '../../services/project.service';
import { CurrentUserService } from '../../services/current-user.service';
import { Project } from '../../models/project.model';

@Component({
  selector: 'app-projects',
  imports: [FormsModule, RouterLink, DatePipe],
  templateUrl: './projects.html',
  styleUrl: './projects.css'
})
export class Projects implements OnInit {
  private readonly projectService = inject(ProjectService);
  private readonly changeDetectorRef = inject(ChangeDetectorRef);

  readonly currentUserService = inject(CurrentUserService);

  projects: Project[] = [];

  page = 1;
  pageSize = 10;
  totalPages = 1;
  totalCount = 0;

  showCreateForm = false;

  projectName = '';
  projectDescription = '';

  isLoading = false;
  isCreating = false;

  errorMessage = '';
  successMessage = '';

  ngOnInit(): void {
    this.loadProjects();
  }

  loadProjects(): void {
    const userId = this.currentUserService.userId;

    if (!userId) {
      this.projects = [];
      this.totalPages = 1;
      this.totalCount = 0;
      return;
    }

    this.isLoading = true;
    this.errorMessage = '';

    this.projectService
      .getMyProjects(
        userId,
        this.page,
        this.pageSize
      )
      .subscribe({
        next: result => {
          this.projects = result.items;
          this.totalPages = result.totalPages;
          this.totalCount = result.totalCount;
          this.isLoading = false;

          this.changeDetectorRef.detectChanges();
        },
        error: () => {
          this.isLoading = false;
          this.errorMessage =
            'Failed to load projects.';

          this.changeDetectorRef.detectChanges();
        }
      });
  }

  createProject(): void {
    this.errorMessage = '';
    this.successMessage = '';

    const userId = this.currentUserService.userId;
    const name = this.projectName.trim();

    if (!userId) {
      this.errorMessage =
        'Select a current user before creating a project.';

      return;
    }

    if (!name) {
      this.errorMessage =
        'Enter a project name.';

      return;
    }

    this.isCreating = true;

    this.projectService
      .createProject(userId, {
        name,
        description:
          this.projectDescription.trim() || null
      })
      .subscribe({
        next: project => {
          this.isCreating = false;

          this.projectName = '';
          this.projectDescription = '';
          this.showCreateForm = false;

          this.page = 1;

          this.loadProjects();

          this.successMessage =
            `Project "${project.name}" created successfully.`;

          this.changeDetectorRef.detectChanges();
        },
        error: error => {
          this.isCreating = false;

          if (error.status === 404) {
            this.errorMessage =
              'Current user was not found.';
          } else {
            this.errorMessage =
              'Failed to create project.';
          }

          this.changeDetectorRef.detectChanges();
        }
      });
  }

  deleteProject(project: Project): void {
    const confirmed = confirm(
      `Delete project "${project.name}"?`
    );

    if (!confirmed) {
      return;
    }

    this.errorMessage = '';
    this.successMessage = '';

    this.projectService
      .deleteProject(project.id)
      .subscribe({
        next: () => {
          this.loadProjects();

          this.successMessage =
            `Project "${project.name}" deleted.`;

          this.changeDetectorRef.detectChanges();
        },
        error: () => {
          this.errorMessage =
            'Failed to delete project.';

          this.changeDetectorRef.detectChanges();
        }
      });
  }

  changePage(page: number): void {
    if (page < 1 || page > this.totalPages) {
      return;
    }

    this.page = page;
    this.loadProjects();
  }

  changePageSize(): void {
    this.page = 1;
    this.loadProjects();
  }
}