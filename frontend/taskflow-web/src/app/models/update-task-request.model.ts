import { TaskPriority, TaskStatus } from './task.model';

export interface UpdateTaskRequest {
  assigneeId: string | null;
  title: string;
  description: string | null;
  status: TaskStatus;
  priority: TaskPriority;
  dueDate: string | null;
}