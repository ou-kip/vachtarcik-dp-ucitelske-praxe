import React, { useState, useEffect, useCallback } from 'react';
import axios from 'axios';
import './TaskSubmissionDetailForm.css'
import { TaskSolutionDetail } from '../../../../Exports/TaskSolutionDetail';

interface SubmissionDetailFormProps {
    taskId: string | null;
    onSubmitted: () => void;
}

interface ApiResponse {
    statusCode: number;
    message: string | null;
    data: {
        solution: TaskSolutionDetail;
        statusCode: number;
        message: string | null;
    };
    errors: string[];
    hasErrors: boolean;
}

const TaskSubmissionDetailForm: React.FC<SubmissionDetailFormProps> = ({ taskId, onSubmitted }) => {
    const [detail, setDetail] = useState<TaskSolutionDetail>();
    const [submittedDateLocale, setSubmittedDateLocale] = useState('');

    axios.defaults.baseURL = 'https://praxeosu.cz:5005';

    const fetchData = useCallback(async () => {
        try {
            const response = await axios.get<ApiResponse>(`/api/v1/internship/task/solution/get?taskId=${taskId}`, { withCredentials: true });
            setDetail(response.data.data.solution);

            if (detail?.submittedDate) {
                const formatted = new Date(detail?.submittedDate).toLocaleString('cs-CZ', {
                    day: '2-digit',
                    month: '2-digit',
                    year: 'numeric',
                    hour: '2-digit',
                    minute: '2-digit',
                });
                setSubmittedDateLocale(formatted);
            }

        } catch (error) {
            alert('Chyba při načítání dat:' + error);
        }
    }, [taskId, detail?.submittedDate]);

    useEffect(() => {
        fetchData();
    }, [fetchData]);

    const handleDownload = async (fileName: string) => {
        if (!taskId) return;

        try {
            const response = await axios.post(
                '/api/v1/file/download',
                { parentId: detail?.id, fileName: fileName },
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

    return (
        <div className="content">
            <div className="input-container">
                <div className="container content-child">
                    <div className="flex-start">
                        <button className="button" onClick={(e) => { e.stopPropagation(); onSubmitted(); }}>Detail úkolu</button>
                    </div>
                    <h1 className="title-custom">Řešení k úkolu</h1>
                    <div className="row">
                        <div className="col-md-6">
                            <div className="input-container">
                                <h3 className="input-title">Odevzdal</h3>
                                <input type="text" defaultValue={detail?.author} placeholder="Odevzdal" className="input" disabled={true} />
                            </div>
                        </div>
                        <div className="col-md-6">
                            <div className="input-container">
                                <h3 className="input-title">Termín odevzdání</h3>
                                <input type="text" defaultValue={submittedDateLocale} placeholder="Termín odevzdání" className="input" disabled={true} />
                            </div>
                        </div>
                    </div>
                    <div className="row">
                        <div className="col-md-12">
                            <div className="input-container">
                                <h3 className="input-title">Řešení</h3>
                                <textarea className="description" defaultValue={detail?.solution ?? ''} placeholder="Odevzdal" disabled={true} />
                            </div>
                        </div>
                    </div>
                    <div className="row">
                        <div className="col-md-12">
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
        </div>
    );
}

export default TaskSubmissionDetailForm;