import {
  ChangeDetectorRef,
  Component,
  inject
} from '@angular/core';
import { FormsModule } from '@angular/forms';

import { UserService } from '../../services/user.service';
import { CurrentUserService } from '../../services/current-user.service';
import { User } from '../../models/user.model';

@Component({
  selector: 'app-users',
  imports: [FormsModule],
  templateUrl: './users.html',
  styleUrl: './users.css'
})
export class Users {
  private readonly userService = inject(UserService);
  private readonly currentUserService = inject(CurrentUserService);
  private readonly changeDetectorRef = inject(ChangeDetectorRef);

  users: User[] = [];

  name = '';
  email = '';

  currentUserId: string | null = null;

  isCreating = false;
  errorMessage = '';
  successMessage = '';

  constructor() {
    this.currentUserId = this.currentUserService.userId;
    this.loadUsers();
  }

  loadUsers(): void {
    this.errorMessage = '';

    this.userService.getUsers(1, 100).subscribe({
      next: result => {
        this.users = result.items;

        this.changeDetectorRef.detectChanges();
      },
      error: () => {
        this.errorMessage = 'Failed to load users.';

        this.changeDetectorRef.detectChanges();
      }
    });
  }

  createUser(): void {
    this.errorMessage = '';
    this.successMessage = '';

    const name = this.name.trim();
    const email = this.email.trim();

    if (!name) {
      this.errorMessage = 'Enter a user name.';
      return;
    }

    if (!email) {
      this.errorMessage = 'Enter an email address.';
      return;
    }

    this.isCreating = true;

    this.userService.createUser({
      name,
      email
    }).subscribe({
      next: user => {
        this.users = [user, ...this.users];

        this.name = '';
        this.email = '';

        this.currentUserService.setUser(user.id);
        this.currentUserId = user.id;

        this.isCreating = false;
        this.successMessage =
          `User "${user.name}" created successfully.`;

        this.changeDetectorRef.detectChanges();
      },
      error: error => {
        this.isCreating = false;

        if (error.status === 409) {
          this.errorMessage =
            'A user with this email already exists.';
        } else {
          this.errorMessage =
            'Failed to create user.';
        }

        this.changeDetectorRef.detectChanges();
      }
    });
  }

  selectUser(user: User): void {
    this.currentUserService.setUser(user.id);
    this.currentUserId = user.id;

    this.successMessage =
      `Current user: ${user.name}.`;

    this.errorMessage = '';

    this.changeDetectorRef.detectChanges();
  }

  isCurrentUser(user: User): boolean {
    return user.id === this.currentUserId;
  }
}