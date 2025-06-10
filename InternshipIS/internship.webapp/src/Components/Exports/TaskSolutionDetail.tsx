import { UploadedFile } from './UploadedFile'

export interface TaskSolutionDetail {
    id: string | null;
    solution: string | null;
    author: string;
    submittedDate: string;
    files: UploadedFile[];
}