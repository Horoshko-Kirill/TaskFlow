import { Routes } from '@angular/router';

import { Projects } from './pages/projects/projects';
import { ProjectDetails } from './pages/project-details/project-details';
import { MyTasks } from './pages/my-tasks/my-tasks';
import { Users } from './pages/users/users';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'projects',
    pathMatch: 'full'
  },
  {
    path: 'projects',
    component: Projects
  },
  {
    path: 'projects/:id',
    component: ProjectDetails
  },
  {
    path: 'my-tasks',
    component: MyTasks
  },
  {
    path: 'users',
    component: Users
  },
  {
    path: '**',
    redirectTo: 'projects'
  }
];