import DropdownDbSelect from '../DropdownDbSelect/DropdownDbSelect'
import { CompanyRelative } from '../../Exports/CompanyRelative';
import axios from 'axios';

const fetchRelatives = async (): Promise<CompanyRelative[]> => {
    const response = await axios.get('api/v1/person/getrelatives', { withCredentials: true });
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