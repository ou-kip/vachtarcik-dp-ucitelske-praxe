import React, { useState } from 'react';
import './TaskStateSelect.css';

interface TaskStateSelectProps {
    selectedState: number | null;
    onStateSelect: (state: number | null) => void;
}

const InternshipStateSelect: React.FC<TaskStateSelectProps> = ({ selectedState, onStateSelect }) => {
    const [isOpen, setIsOpen] = useState(false);
    const [hoveredIndex, setHoveredIndex] = useState<number | null>(null);

    const states: number[] = [0, 1, 2, 3, 4];

    const toggleDropdown = () => {
        setIsOpen((prev) => !prev);
    };

    const handleStateClick = (state: number | null) => {
        onStateSelect(state || 0);
        setIsOpen(false);
    };

    const resolveState = (state: number) => {
        switch (state) {
            case 0: return "ToDo";
            case 1: return "Aktivní";
            case 2: return "Dokončeno";
            case 3: return "Odevzdáno";
            case 4: return "Zrušeno";
            default: return "Neznámý stav";
        }
    };

    return (
        <div style={{ position: 'relative', width: '100%' }}>
            <div
                onClick={toggleDropdown}
                style={{
                    padding: '10px',
                    borderRadius: '10px',
                    backgroundColor: '#f0f2f5',
                    cursor: 'pointer',
                }}
            >
                {selectedState !== null ? resolveState(selectedState) : 'Vyberte stav'}
            </div>

            {isOpen && (
                <div
                    style={{
                        position: 'absolute',
                        top: '100%',
                        left: 0,
                        width: '100%',
                        border: '1px solid #ccc',
                        backgroundColor: '#fff',
                        zIndex: 1000,
                    }}
                >
                    {states.map((state, index) => (
                        <div
                            key={state}
                            onClick={() => handleStateClick(state)}
                            onMouseEnter={() => setHoveredIndex(index)}
                            onMouseLeave={() => setHoveredIndex(null)}
                            style={{
                                padding: '10px',
                                backgroundColor:
                                    hoveredIndex === index ? 'rgb(240, 242, 245)' : '#fff',
                                cursor: 'pointer',
                            }}
                        >
                            {resolveState(state)}
                        </div>
                    ))}
                </div>
            )}
        </div>
    );
};

export default InternshipStateSelect;