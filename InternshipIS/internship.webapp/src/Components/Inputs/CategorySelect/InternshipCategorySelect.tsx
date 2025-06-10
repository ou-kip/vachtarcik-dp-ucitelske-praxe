import DropdownDbSingleSelect from '../DropdownDbSingleSelect/DropdownDbSingleSelect';
import { InternshipCategory } from '../../Exports/InternshipCategory';
import axios from 'axios';

const fetchCategories = async (): Promise<InternshipCategory[]> => {
    axios.defaults.baseURL = 'https://praxeosu.cz:5005';
    const response = await axios.get('api/v1/internship/get/categories', { withCredentials: true });
    return response.data?.data?.categories || [];
};

export default function InternshipCategorySelect({
    selectedCategory,
    onCategorySelect
}: {
        selectedCategory: InternshipCategory | null,
        onCategorySelect: (s: InternshipCategory | null) => void
}) {
    return (
        <DropdownDbSingleSelect
            placeholder="Vybrat kategorii"
            selectedItem={selectedCategory}
            fetchData={fetchCategories}
            getLabel={(s) => s.codeName}
            onSelect={onCategorySelect}
        />
    );
}