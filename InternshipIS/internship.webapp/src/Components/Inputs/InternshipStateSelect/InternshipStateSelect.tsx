import React from 'react';
import DropdownSelect, { DropdownOption } from '../DropdownSelect/DropdownSelect';

interface StateSelectProps {
    selectedState: number | null;
    onStateSelect: (state: number | null) => void;
    allowedStates?: number[];
}

const stateLabels: Record<number, string> = {
    0: "Vytvořeno",
    1: "Publikováno",
    2: "Vybráno",
    3: "Uzavřeno",
    4: "Zrušeno",
};

const InternshipStateSelect: React.FC<StateSelectProps> = ({ selectedState, onStateSelect, allowedStates }) => {
    const fullStateList = Object.entries(stateLabels).map(([value, label]) => ({
        value: parseInt(value),
        label,
    }));

    const options: DropdownOption<number>[] = allowedStates
        ? fullStateList.filter(option => allowedStates.includes(option.value))
        : fullStateList;

    return (
        <DropdownSelect<number>
            selected={selectedState}
            onSelect={onStateSelect}
            options={options}
            placeholder="Vybrat stav"
        />
    );
};

export default InternshipStateSelect;