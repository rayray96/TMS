export interface TaskModel {
    id: number;
    name: string;
    description: string;
    author: string;
    assignee: string;
    priority: string;
    status: string;
    progress: number;
    startDate: Date;
    finishDate: Date;
    deadline: Date;
}