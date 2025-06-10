import DropdownDbSelect from '../DropdownDbSelect/DropdownDbSelect'
import axios from 'axios';
import { Teacher } from '../../Exports/Teacher';

const fetchTeachers = async (): Promise<Teacher[]> => {
    const response = await axios.get('api/v1/person/getteachers', { withCredentials: true });
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