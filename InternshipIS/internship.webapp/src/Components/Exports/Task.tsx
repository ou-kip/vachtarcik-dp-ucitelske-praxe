import { TaskLink } from './TaskLink'

export interface Task {
    id: string | null;
    name: string;
    description: string;
    endsOn: string;
    summary: string;
    teacherSummary: string;
    isReported: boolean;
    links: TaskLink[];
    state: number;
}