export interface CreateTaskModel {
    name: string;
    description: string;
    assignee: string;
    assigneeId: number;
    priority: string;
    deadline: Date;
}