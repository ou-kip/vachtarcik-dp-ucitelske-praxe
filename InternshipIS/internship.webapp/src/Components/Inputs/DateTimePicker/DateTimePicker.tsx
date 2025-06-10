import React, { useState } from 'react';
import DatePicker from 'react-datepicker';
import 'react-datepicker/dist/react-datepicker.css';
import './DateTimePicker.css';

interface DatePickerProps {
    value: string;
    onChange: (value: string) => void;
}

const DatePickerComponent: React.FC<DatePickerProps> = ({ value, onChange }) => {
    const [selectedDate, setSelectedDate] = useState<Date | null>(value ? new Date(value) : null);

    const handleDateChange = (date: Date | null, event?: React.SyntheticEvent) => {
        event?.preventDefault();
        setSelectedDate(date);
        if (date) {
            onChange(date.toISOString());
        }
    };

    return (
        <div style={{ position: 'relative', display: 'inline-block', width: '100%' }}>
            <DatePicker
                selected={selectedDate}
                onChange={handleDateChange}
                dateFormat="dd. MM. yyyy"
                placeholderText="Vyberte datum"
                customInput={<CustomInput />}
                popperClassName="custom-datepicker-popper"
                calendarClassName="custom-datepicker-calendar"
            />
        </div>
    );
};

const CustomInput = React.forwardRef<HTMLButtonElement, { value?: string; onClick?: () => void }>(
    ({ value, onClick }, ref) => (
        <button
            ref={ref}
            onMouseDown={(e) => {
                e.preventDefault();
                if (onClick) onClick();
            }}
            style={{
                padding: '12px',
                borderRadius: '10px',
                backgroundColor: '#ffffff',
                border: '1px solid #ccc',
                color: '#333',
                width: '100%',
                textAlign: 'left',
                fontSize: '16px',
                boxShadow: '0 2px 4px rgba(0, 0, 0, 0.1)',
                cursor: 'pointer',
                transition: 'all 0.3s ease',
            }}
        >
            {value || 'Vyberte datum'}
        </button>
    )
);

CustomInput.displayName = 'CustomInput';

export default DatePickerComponent;