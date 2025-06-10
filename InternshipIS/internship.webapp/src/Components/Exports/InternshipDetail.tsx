import { InternshipLink } from './InternshipLink';
import { Student } from './Student';
import { Teacher } from './Teacher';
import { CompanyRelative } from './CompanyRelative';
import { InternshipCategory } from './InternshipCategory';

export interface InternshipDetail {
    id: string | null;
    name: string;
    description: string;
    companyName: string;
    selectedOn: Date | null;
    createdOn: Date | null
    endsOn: Date | null;
    links: InternshipLink[];
    state: number;
    student: Student | null;
    teachers: Teacher[]
    companyRelatives: CompanyRelative[];
    category: InternshipCategory | null
    createdByName: string;
}