import DropdownDbSelect from '../DropdownDbSelect/DropdownDbSelect'
import InternshipApi from '../../Exports/InternshipApi';
import { Teacher } from '../../Exports/Teacher';

const fetchTeachers = async (): Promise<Teacher[]> => {
    const response = await InternshipApi.get('api/v1/person/getteachers', { withCredentials: true });
    return response.data?.data?.teachers || [];
};

export default function CompanyRelativeSelect({ onTeacherAdd }: { onTeacherAdd: (r: Teacher) => void }) {
    return (
        <DropdownDbSelect
            placeholder="Přidat učitele"
            fetchData={fetchTeachers}
            getLabel={(r) => r.fullName}
            onSelect={onTeacherAdd}
        />
    );
}