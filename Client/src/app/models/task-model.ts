export interface TaskModel {
    id: number;
    name: string;
    description: string;
    author: string;
    assignee: string;
    assigneeId: number;
    priority: string;
    status: string;
    progress: number;
    startDate: Date;
    finishDate: Date;
    deadline: Date;
}