import { TaskLink } from './TaskLink'

export interface RichTask {
    id: string | null;
    internshipId: string | null;
    internshipName: string | null;
    name: string;
    description: string;
    endsOn: string;
    summary: string;
    teacherSummary: string;
    isReported: boolean;
    links: TaskLink[];
    state: number;
}