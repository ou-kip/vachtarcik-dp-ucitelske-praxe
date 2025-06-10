import DropdownDbSingleSelect from '../DropdownDbSingleSelect/DropdownDbSingleSelect';
import { Student } from '../../Exports/Student';
import axios from 'axios';

const fetchStudents = async (): Promise<Student[]> => {
    const response = await axios.get('api/v1/person/getstudents', { withCredentials: true });
    return response.data?.data?.students || [];
};

export default function StudentSelect({
    selectedStudent,
    onStudentSelect
}: {
    selectedStudent: Student | null,
    onStudentSelect: (s: Student | null) => void
}) {
    return (
        <DropdownDbSingleSelect
            placeholder="Vybrat studenta"
            selectedItem={selectedStudent}
            fetchData={fetchStudents}
            getLabel={(s) => s.fullName}
            onSelect={onStudentSelect}
        />
    );
}