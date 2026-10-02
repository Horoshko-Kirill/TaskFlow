import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class CurrentUserService {
  private readonly storageKey = 'taskflow_current_user_id';

  get userId(): string | null {
    return localStorage.getItem(this.storageKey);
  }

  setUser(userId: string): void {
    localStorage.setItem(this.storageKey, userId);
  }

  clear(): void {
    localStorage.removeItem(this.storageKey);
  }

  hasUser(): boolean {
    return this.userId !== null;
  }
}