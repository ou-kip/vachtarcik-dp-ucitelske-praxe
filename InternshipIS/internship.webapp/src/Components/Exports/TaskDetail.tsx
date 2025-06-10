import { TaskLink } from './TaskLink'
import { UploadedFile } from './UploadedFile'

export interface TaskDetail {
    id: string | null;
    internshipId: string | null;
    name: string;
    description: string;
    endsOn: string;
    summary: string;
    teacherSummary: string;
    isReported: boolean;
    links: TaskLink[];
    state: number;
    files: UploadedFile[];
}