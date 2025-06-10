import { InternshipLink } from './InternshipLink';

export interface Internship {
    id: string | null;
    name: string;
    description: string;
    companyName: string;
    startsOn: Date |null;
    endsOn: Date |null;
    state: number;
    studentName: string;
    teacherName: string;
    companyRelativeName: string;
    createdByName: string;
    studentId: string;
    teacherIds: string[];
    companyRelativeIds: string[];
    links: InternshipLink[];
    categoryId: string;
    categoryCodeName: string;
}