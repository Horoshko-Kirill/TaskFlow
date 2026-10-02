import { ChangeDetectorRef, Component, inject } from '@angular/core';
import { DatePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import {
  CdkDrag,
  CdkDragDrop,
  CdkDragPlaceholder,
  CdkDropList,
  CdkDropListGroup
} from '@angular/cdk/drag-drop';

import {
  Task,
  TaskPriority,
  TaskStatus
} from '../../models/task.model';

import { Project } from '../../models/project.model';

import { TaskService } from '../../services/task.service';
import { ProjectService } from '../../services/project.service';
import { CurrentUserService } from '../../services/current-user.service';

@Component({
  selector: 'app-my-tasks',
  imports: [
    FormsModule,
    RouterLink,
    DatePipe,
    CdkDrag,
    CdkDragPlaceholder,
    CdkDropList,
    CdkDropListGroup
  ],
  templateUrl: './my-tasks.html',
  styleUrl: './my-tasks.css'
})
export class MyTasks {
  private readonly taskService =
    inject(TaskService);

  private readonly projectService =
    inject(ProjectService);

  private readonly changeDetectorRef =
    inject(ChangeDetectorRef);

  readonly currentUserService =
    inject(CurrentUserService);

  tasks: Task[] = [];

  projects: Project[] = [];

  selectedPriority: TaskPriority | null = null;

  TaskStatus = TaskStatus;
  TaskPriority = TaskPriority;

  isLoading = false;
  errorMessage = '';

  constructor() {
    this.loadTasks();
    this.loadProjects();
  }

  loadTasks(): void {
    const userId =
      this.currentUserService.userId;

    if (!userId) {
      this.tasks = [];
      return;
    }

    this.isLoading = true;
    this.errorMessage = '';

    this.taskService.getMyTasks(userId, 1, 100).subscribe({
      next: result => {
        this.tasks = result.items;
        this.isLoading = false;
        this.changeDetectorRef.detectChanges();
      },
      error: () => this.failLoadingTasks()
    });
  }

  private failLoadingTasks(): void {
    this.isLoading = false;
    this.errorMessage = 'Failed to load tasks.';
    this.changeDetectorRef.detectChanges();
  }

  loadProjects(): void {
    const userId =
      this.currentUserService.userId;

    if (!userId) {
      this.projects = [];
      return;
    }

    this.projectService
      .getMyProjects(userId, 1, 100)
      .subscribe({
        next: result => {
          this.projects = result.items;
          this.changeDetectorRef.detectChanges();
        },
        error: () => {
          this.errorMessage =
            'Failed to load projects.';
          this.changeDetectorRef.detectChanges();
        }
      });
  }

  get filteredTasks(): Task[] {
    return this.tasks.filter(task =>
      this.selectedPriority === null || task.priority === this.selectedPriority
    );
  }

  getTasksByStatus(status: TaskStatus): Task[] {
    return this.filteredTasks.filter(task => task.status === status);
  }

  changeStatus(task: Task, status: TaskStatus): void {
    if (task.status === status) return;
    const previousStatus = task.status;
    task.status = status;
    this.taskService.updateTask(task.id, {
      assigneeId: task.assigneeId,
      title: task.title,
      description: task.description,
      status,
      priority: task.priority,
      dueDate: task.dueDate
    }).subscribe({
      next: updated => {
        Object.assign(task, updated);
        this.changeDetectorRef.detectChanges();
      },
      error: () => {
        task.status = previousStatus;
        this.errorMessage = 'Failed to change task status.';
        this.changeDetectorRef.detectChanges();
      }
    });
  }

  dropTask(event: CdkDragDrop<TaskStatus>): void {
    const task = event.item.data as Task;
    this.changeStatus(task, event.container.data);
  }

  getProjectName(
    projectId: string
  ): string {
    return this.projects.find(
      project => project.id === projectId
    )?.name ?? 'Unknown project';
  }

  getPriorityName(
    priority: TaskPriority
  ): string {
    switch (priority) {
      case TaskPriority.Low:
        return 'Low';

      case TaskPriority.Medium:
        return 'Medium';

      case TaskPriority.High:
        return 'High';

      case TaskPriority.Critical:
        return 'Critical';
    }
  }

  getStatusName(
    status: TaskStatus
  ): string {
    switch (status) {
      case TaskStatus.Todo:
        return 'To do';

      case TaskStatus.InProgress:
        return 'In progress';

      case TaskStatus.Done:
        return 'Done';
    }
  }
}
