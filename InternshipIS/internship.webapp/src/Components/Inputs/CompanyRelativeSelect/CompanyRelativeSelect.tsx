import DropdownDbSelect from '../DropdownDbSelect/DropdownDbSelect'
import { CompanyRelative } from '../../Exports/CompanyRelative';
import InternshipApi from '../../Exports/InternshipApi';

const fetchRelatives = async (): Promise<CompanyRelative[]> => {
    const response = await InternshipApi.get('api/v1/person/getrelatives', { withCredentials: true });
    return response.data?.data?.companyRelatives || [];
};

export default function CompanyRelativeSelect({ onRelativeAdd }: { onRelativeAdd: (r: CompanyRelative) => void }) {
    return (
        <DropdownDbSelect
            placeholder="Přidat firemní osobu"
            fetchData={fetchRelatives}
            getLabel={(r) => r.fullName}
            onSelect={onRelativeAdd}
        />
    );
}