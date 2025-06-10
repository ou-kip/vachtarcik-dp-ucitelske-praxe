import React, { useState } from 'react';
import axios from 'axios';
import './TaskSubmissionForm.css'
import { UploadedFile } from '../../../Exports/UploadedFile';

interface TaskSubmissionFormProps {
    taskId: string | null;
    onSubmitted: () => void;
}

const TaskSubmissionForm: React.FC<TaskSubmissionFormProps> = ({ taskId, onSubmitted}) => {
    const [formData, setFormData] = useState<{
        solution: string | null;
        taskId: string | null;
        files: UploadedFile[];
    }>({
        solution: '',
        taskId: taskId,
        files: []
    });

    const [newFiles, setNewFiles] = useState<File[]>([]);

    axios.defaults.baseURL = 'https://praxeosu.cz:5005';

    type FormDataKeys = keyof typeof formData;
    type FormDataValues = string;

    const handleChange = (field: FormDataKeys, value: FormDataValues) => {
        setFormData(prev => ({ ...prev, [field]: value }));
    };

    const handleFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        if (e.target.files) {
            setNewFiles(prev => [...prev, ...Array.from(e.target.files!)]);
        }
    };

    const handleSubmit = async () => {
        try {
            const payload = {
                TaskId: formData.taskId,
                Solution: formData.solution,
            };

            const response = await axios.post('/api/v1/internship/task/solution/create', payload, { withCredentials: true });
            const solutionId = response.data.data.solutionId;

            if (newFiles.length && taskId) {
                await Promise.all(newFiles.map(file => {
                    const formData = new FormData();
                    formData.append('SolutionId', solutionId as string);
                    formData.append('File', file);
                    return axios.post('/api/v1/file/solution/upload', formData, {
                        withCredentials: true,
                        headers: { 'Content-Type': 'multipart/form-data' }
                    });
                }));
            }

            alert('Řešení úkolu úspešně odevzdáno');
            onSubmitted();

        } catch (error) {
            console.error('Chyba při odesílání:', error);
        }
    };


    return (
        <div className="content">
            <div className="input-container">
                <div className="container content-child">
                    <h1 className="title-custom">Odevzdat řešení k úkolu</h1>
                    <div className="row">
                        <div className="col-md-12">
                            <div className="input-container">
                                <h3 className="input-title">Řešení</h3>
                                <textarea className="description-child" value={formData.solution as string} onChange={(e) => handleChange('solution', e.target.value)} placeholder=""></textarea>
                            </div>
                        </div>
                    </div>
                    <div className="row">
                        <h3 className="subtitle">Soubory</h3>
                        <div className="col-md-12">
                            <h4 className="input-title">Přidat nové soubory:</h4>
                            <input type="file" multiple onChange={handleFileChange} className="mb-2vh" />
                            {newFiles.map((file, index) => (
                                <div key={index} className="link-container">
                                    <span>{file.name}</span>
                                </div>
                            ))}

                            <h4 className="input-title">Existující soubory:</h4>
                            {formData.files.map((file, index) => (
                                <div key={index} className="link-container">
                                    <input type="text" value={file.fileName} className="input" disabled />
                                </div>
                            ))}
                        </div>
                    </div>
                    <button type="submit" className="button-finish button-end" onClick={handleSubmit}>Odevzdat</button>
                </div>
            </div>
        </div>
    );
}

export default TaskSubmissionForm;