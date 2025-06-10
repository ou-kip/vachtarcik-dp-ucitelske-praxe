import React from 'react';
import DropdownSelect, { DropdownOption } from '../DropdownSelect/DropdownSelect';

interface PersonTypeSelectProps {
    selectedType: number | null;
    onTypeSelect: (type: number | null) => void;
    allowedStates?: number[];
}

const typeLabels: Record<number, string> = {
    0: "Nevybráno",
    1: "Studenti",
    2: "Firemní osoby",
    3: "Učitelé"
};

const PersonTypeSelect: React.FC<PersonTypeSelectProps> = ({ selectedType, onTypeSelect, allowedStates }) => {
    const fullStateList = Object.entries(typeLabels).map(([value, label]) => ({
        value: parseInt(value),
        label,
    }));

    const options: DropdownOption<number>[] = allowedStates
        ? fullStateList.filter(option => allowedStates.includes(option.value))
        : fullStateList;

    return (
        <DropdownSelect<number>
            selected={selectedType}
            onSelect={onTypeSelect}
            options={options}
            placeholder="Vybrat typ účtu"
        />
    );
};

export default PersonTypeSelect;