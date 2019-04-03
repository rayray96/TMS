export interface CreateTaskModel {
    name: string;
    description: string;
    assignee: string;
    priorityId: number;
    deadline: Date;
}