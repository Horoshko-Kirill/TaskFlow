export interface ProjectMember {
  projectId: string;
  userId: string;
  joinedAt: string;
}

export interface ProjectMemberListItem {
  userId: string;
  name: string;
  email: string;
  joinedAt: string;
}