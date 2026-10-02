import { ChangeDetectorRef, Component, DestroyRef, inject } from '@angular/core';
import { DatePipe } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';

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
import { User } from '../../models/user.model';
import {
  ProjectMemberListItem
} from '../../models/project-member.model';
import { Comment } from '../../models/comment.model';

import { ProjectService } from '../../services/project.service';
import { TaskService } from '../../services/task.service';
import { UserService } from '../../services/user.service';
import { ProjectMemberService } from '../../services/project-member.service';
import { CommentService } from '../../services/comment.service';
import { CurrentUserService } from '../../services/current-user.service';

interface KanbanColumn {
  status: TaskStatus;
  title: string;
}

@Component({
  selector: 'app-project-details',
  imports: [
    FormsModule,
    RouterLink,
    DatePipe,
    CdkDrag,
    CdkDragPlaceholder,
    CdkDropList,
    CdkDropListGroup
  ],
  templateUrl: './project-details.html',
  styleUrl: './project-details.css'
})
export class ProjectDetails {
  private readonly route = inject(ActivatedRoute);
  private readonly changeDetectorRef = inject(ChangeDetectorRef);
  private readonly destroyRef = inject(DestroyRef);
  private readonly projectService = inject(ProjectService);
  private readonly taskService = inject(TaskService);
  private readonly userService = inject(UserService);
  private readonly projectMemberService =
    inject(ProjectMemberService);
  private readonly commentService =
    inject(CommentService);

  readonly currentUserService =
    inject(CurrentUserService);

  project: Project | null = null;

  tasks: Task[] = [];

  users: User[] = [];

  members: ProjectMemberListItem[] = [];

  showCreateForm = false;
  showMembers = true;

  title = '';
  description = '';
  assigneeId: string | null = null;
  priority = TaskPriority.Medium;
  dueDate = '';

  selectedMemberId = '';

  onlyMyTasks = false;

  private currentErrorMessage = '';
  private currentSuccessMessage = '';
  private errorMessageTimer: ReturnType<typeof setTimeout> | null = null;
  private successMessageTimer: ReturnType<typeof setTimeout> | null = null;

  get errorMessage(): string {
    return this.currentErrorMessage;
  }

  set errorMessage(message: string) {
    this.currentErrorMessage = message;
    if (this.errorMessageTimer) clearTimeout(this.errorMessageTimer);
    this.errorMessageTimer = message
      ? setTimeout(() => {
          this.currentErrorMessage = '';
          this.errorMessageTimer = null;
          this.changeDetectorRef.detectChanges();
        }, 5000)
      : null;
  }

  get successMessage(): string {
    return this.currentSuccessMessage;
  }

  set successMessage(message: string) {
    this.currentSuccessMessage = message;
    if (this.successMessageTimer) clearTimeout(this.successMessageTimer);
    this.successMessageTimer = message
      ? setTimeout(() => {
          this.currentSuccessMessage = '';
          this.successMessageTimer = null;
          this.changeDetectorRef.detectChanges();
        }, 5000)
      : null;
  }

  isCreatingTask = false;
  isAddingMember = false;

  TaskStatus = TaskStatus;
  TaskPriority = TaskPriority;

  columns: KanbanColumn[] = [
    {
      status: TaskStatus.Todo,
      title: 'To do'
    },
    {
      status: TaskStatus.InProgress,
      title: 'In progress'
    },
    {
      status: TaskStatus.Done,
      title: 'Done'
    }
  ];

  commentsByTask: Record<string, Comment[]> = {};
  commentTexts: Record<string, string> = {};
  commentsExpanded: Record<string, boolean> = {};
  commentsLoaded: Record<string, boolean> = {};
  commentsLoading: Record<string, boolean> = {};

  constructor() {
    this.destroyRef.onDestroy(() => {
      if (this.errorMessageTimer) clearTimeout(this.errorMessageTimer);
      if (this.successMessageTimer) clearTimeout(this.successMessageTimer);
    });

    const projectId =
      this.route.snapshot.paramMap.get('id');

    if (!projectId) {
      return;
    }

    this.loadProject(projectId);
    this.loadTasks(projectId);
    this.loadMembers(projectId);
    this.loadUsers();
  }

  loadProject(id: string): void {
    this.projectService
      .getProject(id)
      .subscribe({
        next: project => {
          this.project = project;
          this.changeDetectorRef.detectChanges();
        },
        error: () => {
          this.errorMessage =
            'Failed to load project.';
          this.changeDetectorRef.detectChanges();
        }
      });
  }

  loadTasks(projectId: string): void {
    this.taskService
      .getByProject(projectId, 1, 100)
      .subscribe({
        next: result => {
          this.tasks = result.items;
          this.changeDetectorRef.detectChanges();
        },
        error: () => {
          this.errorMessage =
            'Failed to load tasks.';
          this.changeDetectorRef.detectChanges();
        }
      });
  }

  loadUsers(): void {
    this.userService
      .getUsers(1, 100)
      .subscribe({
        next: result => {
          this.users = result.items;
          this.changeDetectorRef.detectChanges();
        },
        error: () => {
          this.errorMessage =
            'Failed to load users.';
          this.changeDetectorRef.detectChanges();
        }
      });
  }

  loadMembers(projectId: string): void {
    this.projectMemberService
      .getByProject(projectId, 1, 100)
      .subscribe({
        next: result => {
          this.members = result.items;
          this.changeDetectorRef.detectChanges();
        },
        error: () => {
          this.errorMessage =
            'Failed to load project members.';
          this.changeDetectorRef.detectChanges();
        }
      });
  }

  get availableUsers(): User[] {
    const memberIds =
      new Set(this.members.map(x => x.userId));

    return this.users.filter(
      user => !memberIds.has(user.id)
    );
  }

  get minDueDate(): string {
    const today = new Date();
    const year = today.getFullYear();
    const month = String(today.getMonth() + 1).padStart(2, '0');
    const day = String(today.getDate()).padStart(2, '0');
    return `${year}-${month}-${day}`;
  }

  get visibleTasks(): Task[] {
    const userId =
      this.currentUserService.userId;

    if (!this.onlyMyTasks || !userId) {
      return this.tasks;
    }

    return this.tasks.filter(
      task => task.assigneeId === userId
    );
  }

  getTasksByStatus(status: TaskStatus): Task[] {
    return this.visibleTasks.filter(
      task => task.status === status
    );
  }

  getUserName(userId: string | null): string {
    if (!userId) {
      return 'Unassigned';
    }

    const user = this.users.find(
      x => x.id === userId
    );

    return user?.name ?? 'Unknown user';
  }

  getPriorityName(priority: TaskPriority): string {
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

  createTask(): void {
    if (!this.project) {
      return;
    }

    const taskTitle = this.title.trim();

    if (!taskTitle) {
      this.errorMessage =
        'Enter a task title.';
      return;
    }

    if (this.dueDate && this.dueDate < this.minDueDate) {
      this.errorMessage =
        'Due date cannot be earlier than today.';
      return;
    }

    this.errorMessage = '';
    this.successMessage = '';
    this.isCreatingTask = true;

    this.taskService
      .createTask({
        projectId: this.project.id,
        assigneeId: this.assigneeId || null,
        title: taskTitle,
        description:
          this.description.trim() || null,
        status: TaskStatus.Todo,
        priority: this.priority,
        dueDate: this.dueDate || null
      })
      .subscribe({
        next: task => {
          this.tasks = [
            ...this.tasks,
            task
          ];

          this.resetForm();
          this.isCreatingTask = false;

          this.successMessage =
            'Task created successfully.';
          this.changeDetectorRef.detectChanges();
        },
        error: () => {
          this.isCreatingTask = false;
          this.errorMessage =
            'Failed to create task.';
          this.changeDetectorRef.detectChanges();
        }
      });
  }

  changeStatus(
    task: Task,
    status: TaskStatus
  ): void {
    if (task.status === status) {
      return;
    }

    const previousStatus = task.status;

    task.status = status;

    this.taskService
      .updateTask(task.id, {
        assigneeId: task.assigneeId,
        title: task.title,
        description: task.description,
        status,
        priority: task.priority,
        dueDate: task.dueDate
      })
      .subscribe({
        next: updatedTask => {
          Object.assign(task, updatedTask);
          this.changeDetectorRef.detectChanges();
        },
        error: () => {
          task.status = previousStatus;
          this.errorMessage =
            'Failed to change task status.';
          this.changeDetectorRef.detectChanges();
        }
      });
  }

  dropTask(
    event: CdkDragDrop<TaskStatus>
  ): void {
    const task =
      event.item.data as Task;

    const status =
      event.container.data;

    if (!task || task.status === status) {
      return;
    }

    this.changeStatus(task, status);
  }

  deleteTask(task: Task): void {
    const confirmed = confirm(
      `Delete task "${task.title}"?`
    );

    if (!confirmed) {
      return;
    }

    this.taskService
      .deleteTask(task.id)
      .subscribe({
        next: () => {
          this.tasks =
            this.tasks.filter(
              x => x.id !== task.id
            );

          delete this.commentsByTask[task.id];
          delete this.commentTexts[task.id];
          delete this.commentsExpanded[task.id];
          delete this.commentsLoaded[task.id];
          this.changeDetectorRef.detectChanges();
        },
        error: () => {
          this.errorMessage =
            'Failed to delete task.';
          this.changeDetectorRef.detectChanges();
        }
      });
  }

  addMember(): void {
    if (!this.project || !this.selectedMemberId) {
      return;
    }

    this.errorMessage = '';
    this.successMessage = '';
    this.isAddingMember = true;

    this.projectMemberService
      .addMember(
        this.project.id,
        this.selectedMemberId
      )
      .subscribe({
        next: () => {
          this.selectedMemberId = '';
          this.isAddingMember = false;

          this.loadMembers(this.project!.id);

          this.successMessage =
            'Member added successfully.';
          this.changeDetectorRef.detectChanges();
        },
        error: error => {
          this.isAddingMember = false;

          if (error.status === 409) {
            this.errorMessage =
              'This user is already a project member.';
            this.changeDetectorRef.detectChanges();
            return;
          }

          this.errorMessage =
            'Failed to add member.';
          this.changeDetectorRef.detectChanges();
        }
      });
  }

  removeMember(
    member: ProjectMemberListItem
  ): void {
    if (!this.project) {
      return;
    }

    const confirmed = confirm(
      `Remove "${member.name}" from the project?`
    );

    if (!confirmed) {
      return;
    }

    this.projectMemberService
      .removeMember(
        this.project.id,
        member.userId
      )
      .subscribe({
        next: () => {
          this.members =
            this.members.filter(
              x => x.userId !== member.userId
            );

          this.tasks = this.tasks.map(task => {
            if (task.assigneeId === member.userId) {
              return {
                ...task,
                assigneeId: null
              };
            }

            return task;
          });

          this.successMessage =
            'Member removed successfully.';
          this.changeDetectorRef.detectChanges();
        },
        error: () => {
          this.errorMessage =
            'Failed to remove member.';
          this.changeDetectorRef.detectChanges();
        }
      });
  }

  toggleComments(taskId: string): void {
    const expanded =
      !this.commentsExpanded[taskId];

    this.commentsExpanded[taskId] =
      expanded;

    if (
      expanded &&
      !this.commentsLoaded[taskId]
    ) {
      this.loadComments(taskId);
    }
  }

  loadComments(taskId: string): void {
    this.commentsLoading[taskId] = true;

    this.commentService
      .getByTask(taskId, 1, 100)
      .subscribe({
        next: result => {
          this.commentsByTask[taskId] =
            result.items;

          this.commentsLoaded[taskId] =
            true;

          this.commentsLoading[taskId] =
            false;
          this.changeDetectorRef.detectChanges();
        },
        error: () => {
          this.commentsLoading[taskId] =
            false;

          this.errorMessage =
            'Failed to load comments.';
          this.changeDetectorRef.detectChanges();
        }
      });
  }

  addComment(taskId: string): void {
    const authorId =
      this.currentUserService.userId;

    const text =
      (this.commentTexts[taskId] ?? '').trim();

    if (!authorId || !text) {
      return;
    }

    this.commentService
      .create(
        taskId,
        authorId,
        text
      )
      .subscribe({
        next: comment => {
          const comments =
            this.commentsByTask[taskId] ?? [];

          this.commentsByTask[taskId] = [
            ...comments,
            comment
          ];

          this.commentTexts[taskId] = '';

          this.commentsExpanded[taskId] =
            true;

          this.commentsLoaded[taskId] =
            true;
          this.changeDetectorRef.detectChanges();
        },
        error: () => {
          this.errorMessage =
            'Failed to add comment.';
          this.changeDetectorRef.detectChanges();
        }
      });
  }

  editComment(comment: Comment): void {
    const text = prompt(
      'Edit comment:',
      comment.text
    );

    if (text === null) {
      return;
    }

    const newText = text.trim();

    if (!newText) {
      return;
    }

    this.commentService
      .update(
        comment.id,
        newText
      )
      .subscribe({
        next: updatedComment => {
          const comments =
            this.commentsByTask[
              updatedComment.taskId
            ] ?? [];

          this.commentsByTask[
            updatedComment.taskId
          ] = comments.map(x =>
            x.id === updatedComment.id
              ? updatedComment
              : x
          );
          this.changeDetectorRef.detectChanges();
        },
        error: () => {
          this.errorMessage =
            'Failed to edit comment.';
          this.changeDetectorRef.detectChanges();
        }
      });
  }

  deleteComment(comment: Comment): void {
    const confirmed = confirm(
      'Delete this comment?'
    );

    if (!confirmed) {
      return;
    }

    this.commentService
      .delete(comment.id)
      .subscribe({
        next: () => {
          const comments =
            this.commentsByTask[
              comment.taskId
            ] ?? [];

          this.commentsByTask[
            comment.taskId
          ] = comments.filter(
            x => x.id !== comment.id
          );
          this.changeDetectorRef.detectChanges();
        },
        error: () => {
          this.errorMessage =
            'Failed to delete comment.';
          this.changeDetectorRef.detectChanges();
        }
      });
  }

  resetForm(): void {
    this.title = '';
    this.description = '';
    this.assigneeId = null;
    this.priority = TaskPriority.Medium;
    this.dueDate = '';
    this.showCreateForm = false;
  }
}
