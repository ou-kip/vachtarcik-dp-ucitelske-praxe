import { useState } from 'react';
import DropdownArrow from '../../../assets/arrow-down-square-fill.svg';

export interface DropdownOption<T> {
    value: T;
    label: string;
}

interface DropdownSelectProps<T> {
    selected: T | null;
    onSelect: (value: T | null) => void;
    options: DropdownOption<T>[];
    placeholder?: string;
}

const DropdownSelect = <T,>({
    selected,
    onSelect,
    options,
    placeholder = 'Vybrat hodnotu',
}: DropdownSelectProps<T>) => {
    const [isOpen, setIsOpen] = useState(false);
    const [hoveredIndex, setHoveredIndex] = useState<number | null>(null);

    const toggleDropdown = () => {
        setIsOpen((prev) => !prev);
    };

    const handleSelect = (value: T) => {
        onSelect(value);
        setIsOpen(false);
    };

    const selectedLabel = options.find(o => o.value === selected)?.label;

    return (
        <div style={{ position: 'relative', width: '100%' }}>
            <div
                onClick={toggleDropdown}
                style={{
                    display: 'flex',
                    justifyContent: 'space-between',
                    alignItems: 'center',
                    padding: '12px',
                    borderRadius: '8px',
                    border: '1px solid #ccc',
                    backgroundColor: '#fff',
                    cursor: 'pointer',
                    transition: 'border-color 0.2s ease',
                    fontWeight: 500,
                    boxShadow: isOpen ? '0 0 0 2px #007bff33' : 'none',
                    maxHeight: '44px'
                }}
            >
                <span style={{ color: selected !== null ? '#000' : '#999' }}>
                    {selectedLabel || placeholder}
                </span>
                <span
                    style={{
                        marginLeft: '8px',
                        transform: isOpen ? 'rotate(180deg)' : 'rotate(0deg)',
                        transition: 'transform 0.2s ease',
                    }}
                >
                    <img src={DropdownArrow} style={{ width: '25px', height: '25px' }} alt="Rozbalit" />
                </span>
            </div>

            {isOpen && (
                <div
                    style={{
                        position: 'absolute',
                        top: '100%',
                        left: 0,
                        width: '100%',
                        border: '1px solid #ccc',
                        borderTop: 'none',
                        backgroundColor: '#fff',
                        zIndex: 1000,
                        borderRadius: '0 0 8px 8px',
                        boxShadow: '0 4px 10px rgba(0,0,0,0.05)',
                    }}
                >
                    <div style={{ maxHeight: '320px', overflowY: 'auto' }}>
                        {options.length > 0 ? (
                            options.map((option, index) => (
                                <div
                                    key={String(option.value)}
                                    onClick={() => handleSelect(option.value)}
                                    onMouseEnter={() => setHoveredIndex(index)}
                                    onMouseLeave={() => setHoveredIndex(null)}
                                    style={{
                                        padding: '10px',
                                        backgroundColor:
                                            hoveredIndex === index ? 'rgb(240, 242, 245)' : '#fff',
                                        cursor: 'pointer',
                                    }}
                                >
                                    {option.label}
                                </div>
                            ))
                        ) : (
                            <div style={{ padding: '10px', color: '#999' }}>
                                Žádné možnosti k zobrazení.
                            </div>
                        )}
                    </div>
                </div>
            )}
        </div>
    );
};

export default DropdownSelect;