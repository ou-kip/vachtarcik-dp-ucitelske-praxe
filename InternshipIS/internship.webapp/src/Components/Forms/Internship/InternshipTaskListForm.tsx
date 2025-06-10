import React, { useState, useEffect } from 'react';
import './InternshipTaskListForm.css'
import axios from 'axios';
import { Task } from '../../Exports/Task';

interface InternshipTaskListFormProps {
    internshipId: string | null | undefined;
    isUpdate: boolean
    listRefreshKey: number
    onRowEdit?: (id: string | null) => void;
}

const ResolveState = (state: number) => {
    switch (state) {
        case 0: return "ToDo";
        case 1: return "Aktivní";
        case 2: return "Dokončeno"
        case 3: return "Odevzdáno";
        case 4: return "Zrušeno";
    }
}

const InternshipTaskListForm: React.FC<InternshipTaskListFormProps> = ({ internshipId, isUpdate, listRefreshKey, onRowEdit }) => {
    const [tasks, setTasks] = useState<Array<Task>>([]);

    useEffect(() => {
        if (isUpdate && internshipId) {
            axios.defaults.baseURL = 'https://praxeosu.cz:5005';
            axios.get(`/api/v1/internship/task/getCollection?InternshipId=${internshipId}`, { withCredentials: true })
                .then(response => {
                    if (response.data && response.data.data.tasks) {
                        setTasks(response.data.data.tasks);
                    }
                })
                .catch(error => console.error('Chyba při načítání úkolů:', error));
        }
    }, [isUpdate, internshipId, listRefreshKey]);

    const handleRowClick = (id: string | null) => {
        onRowEdit?.(id);
    };

    return (
        <div className="container content-child">
            <h1 className="title-custom">Seznam úkolů</h1>
            <div className="table task-scroll-container">
                <table className="table table-hover table-centered">
                    <thead className="table-thead-custom">
                        <tr>
                            <th scope="col" className="blue-th">#</th>
                            <th scope="col" className="blue-th">Název</th>
                            <th scope="col" className="blue-th">Popis</th>
                            <th scope="col" className="blue-th">Termín odevzdání</th>
                            <th scope="col" className="blue-th">Stav</th>
                        </tr>
                    </thead>
                    <tbody>
                        {tasks.map((task, index) => (
                            <tr key={task.id} onClick={() => handleRowClick(task.id)}>
                                <td>{index + 1}</td>
                                <td>{task.name || ''}</td>
                                <td>{task.description || ''}</td>
                                <td>{task.endsOn ? new Date(task.endsOn).toLocaleDateString() : ''}</td>
                                <td>{task.state ? ResolveState(task.state) : ''}</td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            </div>
        </div>
    )
}

export default InternshipTaskListForm;