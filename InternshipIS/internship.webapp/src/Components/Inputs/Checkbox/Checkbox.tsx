import React from 'react';

const CustomCheckbox: React.FC<{ checked: boolean; onChange: (checked: boolean) => void; label: string }> = ({ checked, onChange, label }) => {
    const handleClick = () => {
        onChange(!checked);
    };

    return (
        <div style={checkboxContainerStyle} onClick={handleClick}>
            <div style={{ ...checkboxStyle, backgroundColor: checked ? '#00B7CF' : 'rgb(238, 240, 243)' }}>
                {checked && <span style={checkmarkStyle}>✓</span>}
            </div>
            <span style={checkboxLabelStyle}>{label}</span>
        </div>
    );
};

export default CustomCheckbox;

const checkboxContainerStyle: React.CSSProperties = {
    display: 'flex',
    alignItems: 'center',
    cursor: 'pointer',
    marginBottom: '10px',
};

const checkboxStyle: React.CSSProperties = {
    width: '20px',
    height: '20px',
    border: 'none',
    borderRadius: '4px',
    display: 'flex',
    alignItems: 'center',
    justifyContent: 'center',
    marginRight: '10px',
    transition: 'background-color 0.2s',
};

const checkmarkStyle: React.CSSProperties = {
    color: '#fff',
    fontWeight: 'bold',
    fontSize: '14px',
};

const checkboxLabelStyle: React.CSSProperties = {
    fontSize: '16px',
    color: '#333',
};