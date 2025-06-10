import React, { useState, useEffect, useCallback } from 'react';
import './TaskDetailForm.css'
import { TaskDetail } from '../../../Exports/TaskDetail';
import axios from 'axios';
import TaskSubmissionForm from '../Submission/TaskSubmissionForm';
import TaskSubmissionDetailForm from '../Submission/Detail/TaskSubmissionDetailForm';
import { AllowedActions } from '../../../Exports/AllowedActions'; 

interface TaskDetailFormProps {
    taskId: string | null;
}

interface ApiResponse {
    statusCode: number;
    message: string | null;
    data: {
        task: TaskDetail;
        statusCode: number;
        message: string | null;
    };
    errors: string[];
    hasErrors: boolean;
}

interface AllowedApiResponse {
    statusCode: number;
    message: string | null;
    data: {
        allowedActions: AllowedActions;
        statusCode: number;
        message: string | null;
    };
    errors: string[];
    hasErrors: boolean;
}

const TaskDetailForm: React.FC<TaskDetailFormProps> = ({ taskId }) => {
    const [detail, setDetail] = useState<TaskDetail>();
    const [showSubmissionForm, setShowSubmissionForm] = useState(false);
    const [showSubmissionDetailForm, setShowSubmissionDetailForm] = useState(false);
    const [allowedActions, setAllowedActions] = useState<AllowedActions>();

    axios.defaults.baseURL = 'https://praxeosu.cz:5005';

    const fetchData = useCallback(async () => {
        try {
            const response = await axios.get<ApiResponse>(`/api/v1/internship/task/get?Id=${taskId}`, { withCredentials: true });
            setDetail(response.data.data.task);

        } catch (error) {
            alert('Chyba při načítání dat:' + error);
        }
    }, [taskId]);

    useEffect(() => {
        fetchData();
    }, [fetchData]);

    useEffect(() => {
        const fetchAllowedActions = async () => {
            if (!detail?.internshipId) return;

            try {
                const allowedResponse = await axios.get<AllowedApiResponse>(
                    '/api/v1/internship/get/allowedactions',
                    {
                        params: { internshipId: detail.internshipId },
                        withCredentials: true
                    }
                );
                setAllowedActions(allowedResponse.data.data.allowedActions);
            } catch (error) {
                console.error('Chyba při získávání oprávnění:', error);
            }
        };

        fetchAllowedActions();
    }, [detail?.internshipId]);

    const ResolveState = (state: number | undefined) => {
        switch (state) {
            case 0: return "ToDo";
            case 1: return "Aktivní";
            case 2: return "Dokončeno"
            case 3: return "Odevzdáno"
            case 4: return "Zrušeno";
        }
    }

    const handleDownload = async (fileName: string) => {
        if (!taskId) return;

        try {
            const response = await axios.post(
                '/api/v1/file/download',
                { parentId: taskId, fileName: fileName },
                {
                    withCredentials: true,
                    responseType: 'blob'
                }
            );

            const blob = new Blob([response.data]);
            const url = window.URL.createObjectURL(blob);
            const link = document.createElement('a');
            link.href = url;
            link.setAttribute('download', fileName);
            document.body.appendChild(link);
            link.click();
            link.remove();
            window.URL.revokeObjectURL(url);
        } catch (error) {
            console.error('Chyba při stahování souboru:', error);
        }
    };

    const handleSubmisionCreate = () => {
        setShowSubmissionForm(true);
    };

    const handleSubmissionCompleted = async () => {
        setShowSubmissionForm(false);
        await fetchData();
    };

    const handleSubmisionDetail = () => {
        setShowSubmissionDetailForm(true);
    };

    const handleSubmissionDetailCompleted = async () => {
        setShowSubmissionDetailForm(false);
    };

    if (showSubmissionForm) {
        return <TaskSubmissionForm taskId={taskId} onSubmitted={handleSubmissionCompleted} />;
    }

    if (showSubmissionDetailForm) {
        return <TaskSubmissionDetailForm taskId={taskId} onSubmitted={handleSubmissionDetailCompleted} />;
    }

    return (
        <div className="content">
            <h1 className="title-custom">{detail?.name}</h1>

            {allowedActions?.allowed && allowedActions.isStudent && detail?.state !== undefined && detail.state < 2 && (
                <div className="flex-end">
                    <button className="button" onClick={(e) => { e.stopPropagation(); handleSubmisionCreate(); }}>
                        Odevzdat
                    </button>
                </div>
            )}
            {allowedActions?.allowed && detail?.state !== undefined && detail.state === 3 && (
                <div className="flex-end">
                    <button className="button" onClick={(e) => { e.stopPropagation(); handleSubmisionDetail(); }}>
                        Zobrazit řešení
                    </button>
                </div>
            )}

            <div className="row">
                <div className="col-md-12">
                    <div className="input-container">
                        <h3 className="input-title">Zadání</h3>
                        <input type="text" defaultValue={detail?.description} placeholder="Popis" className="input" disabled={true} />
                    </div>
                </div>
            </div>
            <div className="row">
                <div className="col-md-6">
                    <div className="input-container">
                        <h3 className="input-title">Stav</h3>
                        <input type="text" defaultValue={ResolveState(detail?.state)} placeholder="Kategorie" className="input" disabled={true} />
                    </div>
                </div>
                <div className="col-md-6">
                    <div className="input-container">
                        <h3 className="input-title">Termín odevzdání</h3>
                        <input type="text" defaultValue={detail?.endsOn} placeholder="Termín odevzdání" className="input" disabled={true} />
                    </div>
                </div>
                <div className="row">
                    <div className="col-md-12">
                        <div className="input-container">
                            <h3 className="input-title">Popis</h3>
                            <textarea className="description" defaultValue={detail?.description} placeholder="Popis" disabled={true}></textarea>
                        </div>
                    </div>
                </div>
                <div className="row">
                    <div className="col-md-12">
                        <div className="input-container">
                            <h3 className="input-title">Shrnutí</h3>
                            <textarea className="description" defaultValue={detail?.summary} placeholder="Popis" disabled={true}></textarea>
                        </div>
                    </div>
                </div>
                <div className="row">
                    <div className="col-md-12">
                        <div className="input-container">
                            <h3 className="input-title">Shrnutí učitele</h3>
                            <textarea className="description" defaultValue={detail?.teacherSummary} placeholder="Popis" disabled={true}></textarea>
                        </div>
                    </div>
                </div>
                <div className="row">
                    <div className="col-md-6">
                        <h3 className="subtitle">Odkazy</h3>
                        <div className="scrollable-container-links">
                            {detail?.links.map((link, index) => (
                                <div key={index} className="link-container">
                                    <a href={link.url} target="_blank" rel="noopener noreferrer">{link.name}</a>
                                </div>
                            ))}
                        </div>
                    </div>
                    <div className="col-md-6">
                        <h3 className="subtitle">Soubory</h3>
                        <div className="scrollable-container-links">
                            {detail?.files.map((file, index) => (
                                <div key={index} className="link-container">
                                    <a href="#" target="_blank" rel="noopener noreferrer" onClick={(e) => { e.preventDefault(); handleDownload(file.fileName) }}>{file.fileName}</a>
                                </div>
                            ))}
                        </div>
                    </div>
                </div>
            </div>
        </div>
    )
}

export default TaskDetailForm