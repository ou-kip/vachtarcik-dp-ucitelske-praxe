import React, { useEffect, useState } from 'react';
import './Calendar.css';
import axios from 'axios';
import { CalendarEvent } from '../../Exports/CalendarEvent';
import ArrowLeft from '../../../assets/arrow-left-square-fill.svg';
import ArrowRight from '../../../assets/arrow-right-square-fill.svg';

const Calendar: React.FC = () => {
    const [events, setEvents] = useState<CalendarEvent[]>([]);
    const [currentDate, setCurrentDate] = useState(new Date());
    const [selectedEvent, setSelectedEvent] = useState<CalendarEvent | null>(null);
    const [mobileDayIndex, setMobileDayIndex] = useState(0);

    const daysOfWeek = ['Po', 'Út', 'St', 'Čt', 'Pá', 'So', 'Ne'];
    const monthNames = [
        'Leden', 'Únor', 'Březen', 'Duben', 'Květen', 'Červen',
        'Červenec', 'Srpen', 'Září', 'Říjen', 'Listopad', 'Prosinec'
    ];

    const year = currentDate.getFullYear();
    const month = currentDate.getMonth();
    const firstDay = new Date(year, month, 1);
    const startDay = (firstDay.getDay() + 6) % 7;
    const daysInMonth = new Date(year, month + 1, 0).getDate();

    const isMobile = window.innerWidth <= 768;

    useEffect(() => {
        axios.defaults.baseURL = 'https://praxeosu.cz:5005';
        axios.get("/api/v1/calendar/get", { withCredentials: true })
            .then(response => {
                if (response.data?.data?.events) {
                    setEvents(response.data.data.events);
                }
            })
            .catch(error => console.error('Chyba při načítání událostí:', error));
    }, []);

    const prevMonth = () => setCurrentDate(new Date(year, month - 1, 1));
    const nextMonth = () => setCurrentDate(new Date(year, month + 1, 1));

    const prevMobileDays = () => {
        if (mobileDayIndex > 0) setMobileDayIndex(mobileDayIndex - 1);
    };

    const nextMobileDays = () => {
        if (mobileDayIndex < daysInMonth - 3) setMobileDayIndex(mobileDayIndex + 1);
    };

    const getDateKey = (date: Date) => date.toISOString().split('T')[0];

    const getEventsForDay = (date: Date) => {
        const key = getDateKey(date);
        return events.filter(e => e.eventTime && getDateKey(new Date(e.eventTime)) === key);
    };

    const renderCalendarCells = () => {
        const cells: JSX.Element[] = [];
        for (let i = 0; i < startDay; i++) {
            cells.push(<div key={`empty-${i}`} className="calendar-cell empty"></div>);
        }

        for (let day = 1; day <= daysInMonth; day++) {
            const date = new Date(year, month, day);
            const dayEvents = getEventsForDay(date);

            cells.push(
                <div key={day} className="calendar-cell">
                    <div className="cell-header">{day}</div>
                    <div className="cell-events">
                        {dayEvents.map(event => (
                            <div
                                key={event.eventId}
                                className="event-chip"
                                onClick={() => setSelectedEvent(event)}
                            >
                                {event.eventName}
                            </div>
                        ))}
                    </div>
                </div>
            );
        }

        return cells;
    };

    const renderMobileCells = () => {
        const cells: JSX.Element[] = [];

        for (let i = 0; i < 3; i++) {
            const dayNumber = mobileDayIndex + i + 1;
            if (dayNumber > daysInMonth) break;
            const day = new Date(year, month, dayNumber);
            const dayEvents = getEventsForDay(day);

            cells.push(
                <div key={i} className="calendar-cell mobile">
                    <div className="cell-header">{daysOfWeek[(day.getDay() + 6) % 7]} {day.getDate()}</div>
                    <div className="cell-events">
                        {dayEvents.map(event => (
                            <div
                                key={event.eventId}
                                className="event-chip"
                                onClick={() => setSelectedEvent(event)}
                            >
                                {event.eventName}
                            </div>
                        ))}
                    </div>
                </div>
            );
        }

        return cells;
    };

    return (
        <div className="content">
            <div className="input-container">
                <div className="container content-child">
                    <div className="calendar-header">
                        <button title="Předchozí" className="button-blue-round" onClick={isMobile ? prevMobileDays : prevMonth}>
                            <img src={ArrowLeft} alt="Předchozí" />
                        </button>
                        <h1 className="title-custom">{monthNames[month]} {year}</h1>
                        <button title="Další" className="button-blue-round" onClick={isMobile ? nextMobileDays : nextMonth}>
                            <img src={ArrowRight} alt="Další" />
                        </button>
                    </div>

                    <div className={`calendar-grid ${isMobile ? 'mobile-grid' : ''}`}>
                        {!isMobile && daysOfWeek.map(day => (
                            <div key={day} className="calendar-cell header">{day}</div>
                        ))}
                        {isMobile ? renderMobileCells() : renderCalendarCells()}
                    </div>
                </div>
            </div>

            {selectedEvent && (
                <div className="event-modal" onClick={() => setSelectedEvent(null)}>
                    <div className="event-modal-content" onClick={e => e.stopPropagation()}>
                        <h2>{selectedEvent.eventName}</h2>
                        {selectedEvent.eventTime && (
                            <p>{new Date(selectedEvent.eventTime).toLocaleString('cs-CZ')}</p>
                        )}
                        <button className="close-button" onClick={() => setSelectedEvent(null)}>Zavřít</button>
                    </div>
                </div>
            )}
        </div>
    );
};

export default Calendar;
