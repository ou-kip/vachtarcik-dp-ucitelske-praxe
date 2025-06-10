import React, { useState, useEffect } from 'react';
import './InternshipTaskEvaluationListForm.css'
import axios from 'axios';
import { RichTask } from '../../Exports/RichTask';
import { useNavigate } from 'react-router-dom';
import InternshipIcon from '../../../assets/briefcase-fill.svg';

interface InternshipTaskEvaluationListFormProps {
    createdByMe: boolean,
    filterProperty: string | null,
    filterValue: string | null,
    orderProperty: string | null,
    orderDirection: number,
    userRole: string | null,
}

const ResolveState = (state: number) => {
    switch (state) {
        case 0: return "ToDo";
        case 1: return "Aktivní";
        case 2: return "Dokončeno"
        case 3: return "Odevzdáno"
        case 4: return "Zrušeno";
    }
}

const InternshipTaskEvaluationListForm: React.FC<InternshipTaskEvaluationListFormProps> = ({ createdByMe, filterProperty, filterValue, orderProperty, orderDirection, userRole }) => {
    const [tasks, setTasks] = useState<Array<RichTask>>([]);
    const navigate = useNavigate();

    useEffect(() => {

        axios.defaults.baseURL = 'https://praxeosu.cz:5005';
        axios.get("/api/v1/internship/task/filter/getCollection", { withCredentials: true, params: { createdByMe: createdByMe, filterProperty: filterProperty, orderProperty: orderProperty, orderDirection: orderDirection, filterValue: filterValue } })
            .then(response => {
                if (response.data && response.data.data.tasks) {
                    setTasks(response.data.data.tasks);
                }
            })
            .catch(error => console.error('Chyba při načítání úkolů:', error));
    }, [createdByMe, filterProperty, filterValue, orderProperty, orderDirection, userRole]);

    const handleRowClick = (id: string | null) => {
        navigate(`/task/detail?taskId=${id}`)
    };

    const handleInternshipClick = (id: string | null) => {
        navigate(`/internship/detail?internshipId=${id}`)
    };

    return (
        <div className="content">
            <div className="input-container">
                <div className="container content-child">
                    <h1 className="title-custom">
                        {userRole === "Student" ? "Úkoly k odevzdání" : "Úkoly k ohodnocení"}
                    </h1>
                    <div className="table table-wrapper">
                        <table className="table table-hover table-centered">
                            <thead className="table-thead-custom">
                                <tr>
                                    <th scope="col" className="blue-th">#</th>
                                    <th scope="col" className="blue-th">Praxe</th>
                                    <th scope="col" className="blue-th">Úkol</th>
                                    <th scope="col" className="blue-th">Zadání</th>
                                    <th scope="col" className="blue-th">Termín odevzdání</th>
                                    <th scope="col" className="blue-th">Stav</th>
                                    <th scope="col" className="blue-th">Akce</th>
                                </tr>
                            </thead>
                            <tbody>
                                {tasks.map((task, index) => (
                                    <tr key={task.id} onClick={() => handleRowClick(task.id)}>
                                        <td>{index + 1}</td>
                                        <td>{task.internshipName || ''}</td>
                                        <td>{task.name || ''}</td>
                                        <td>{task.description.length > 20 ? task.description.slice(0, 20) + "..." : task.description || ''}</td>
                                        <td>{task.endsOn ? new Date(task.endsOn).toLocaleDateString() : ''}</td>
                                        <td>{task.state ? ResolveState(task.state) : ''}</td>
                                        <td>
                                            {(userRole) && (
                                                <button title="Přejít na praxi" className="button-blue-round" onClick={(e) => { e.stopPropagation(); handleInternshipClick(task.internshipId); }}>
                                                    <img src={InternshipIcon} alt="Přejít na praxi" />
                                                </button>
                                            )}
                                        </td>
                                    </tr>
                                ))}
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    )
}

export default InternshipTaskEvaluationListForm;