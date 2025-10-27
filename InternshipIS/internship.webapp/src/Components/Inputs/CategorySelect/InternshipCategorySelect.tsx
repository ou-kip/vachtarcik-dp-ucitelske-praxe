import DropdownDbSingleSelect from '../DropdownDbSingleSelect/DropdownDbSingleSelect';
import { InternshipCategory } from '../../Exports/InternshipCategory';
import InternshipApi from '../../Exports/InternshipApi';

const fetchCategories = async (): Promise<InternshipCategory[]> => {
    const response = await InternshipApi.get('api/v1/internship/get/categories', { withCredentials: true });
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