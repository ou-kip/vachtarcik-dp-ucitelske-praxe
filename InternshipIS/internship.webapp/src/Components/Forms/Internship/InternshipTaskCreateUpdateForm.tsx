import React, { useState, useEffect } from 'react';
import axios from 'axios';
import './InternshipTaskCreateUpdateForm.css'
import { TaskLink } from '../../Exports/TaskLink';
import { UploadedFile } from '../../Exports/UploadedFile';
import CustomCheckbox from '../../Inputs/Checkbox/Checkbox';
import TaskStateSelect from '../../Inputs/TaskStateSelect/TaskStateSelect';

interface InternshipTaskListFormProps {
    internshipId: string | null;
    isUpdate: boolean;
    taskId: string | null;
    onSubmitted: () => void;
}
const InternshipTaskCreateUpdateForm: React.FC<InternshipTaskListFormProps> = ({ internshipId, isUpdate, taskId, onSubmitted }) => {
    const [formData, setFormData] = useState<{
        id: string | null;
        name: string;
        description: string;
        endsOn: string;
        summary: string;
        teacherSummary: string
        links: TaskLink[];
        files: UploadedFile[];
        state: number,
        isReported: boolean

    }>({
        id: null,
        name: '',
        description: '',
        endsOn: '',
        summary: '',
        teacherSummary: '',
        links: [],
        files: [],
        state: 0,
        isReported: false
    });

    const [newFiles, setNewFiles] = useState<File[]>([]);

    axios.defaults.baseURL = 'https://praxeosu.cz:5005';

    useEffect(() => {
        if (taskId != null) {
            axios.get(`/api/v1/internship/task/get?Id=${taskId}`, { withCredentials: true })
                .then(response => {
                    if (response.data && response.data.data.task) {
                        const taskData = response.data.data.task;
                        setFormData({
                            id: taskData.id,
                            name: taskData.name || '',
                            description: taskData.description || '',
                            endsOn: taskData.endsOn ? new Date(taskData.endsOn).toISOString().split('T')[0] : '',
                            isReported: taskData.isReported || false,
                            summary: taskData.summary || '',
                            teacherSummary: taskData.teacherSummary || '',
                            links: taskData.links || [],
                            state: taskData.state || 0,
                            files: taskData.files || []
                        });
                    }
                })
                .catch(error => console.error('Chyba při načítání dat:', error));
        }
    }, [internshipId, isUpdate, taskId]);

    type FormDataKeys = keyof typeof formData;
    type FormDataValues = string | number | boolean | TaskLink[] | string | number | null;

    const handleChange = (field: FormDataKeys, value: FormDataValues) => {
        setFormData(prev => ({ ...prev, [field]: value }));
    };

    const handleLinkAdd = () => {
        setFormData(prev => ({
            ...prev,
            links: [...prev.links, { id: null, name: '', url: '' }]
        }));
    };


    const handleLinkChange = (index: number, field: keyof TaskLink, value: string) => {
        setFormData(prev => ({
            ...prev,
            links: prev.links.map((link, i) => i === index ? { ...link, [field]: value } : link)
        }));
    };

    const handleLinkRemove = (index: number) => {
        setFormData(prev => ({
            ...prev,
            links: prev.links.filter((_, i) => i !== index)
        }));
    };

    const handleFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        if (e.target.files) {
            setNewFiles(prev => [...prev, ...Array.from(e.target.files!)]);
        }
    };

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

    const handleSubmit = async () => {
        try {
            if (isUpdate) {
                const payload = {
                    Id: taskId,
                    Name: formData.name,
                    Description: formData.description,
                    Summary: formData.summary ?? '',
                    TeacherSummary: formData.teacherSummary ?? '',
                    EndsOn: new Date(formData.endsOn).toISOString(),
                    IsReported: formData.isReported,
                    Links: formData.links,
                    State: formData.state ?? 0
                };

                await axios.post('/api/v1/internship/task/update', payload, { withCredentials: true });
            }
            else {
                const payload = {
                    Name: formData.name,
                    Description: formData.description,
                    Summary: formData.summary ?? '',
                    TeacherSummary: formData.teacherSummary ?? '',
                    EndsOn: new Date(formData.endsOn).toISOString(),
                    IsReported: formData.isReported,
                    Links: formData.links.map(link => ({ Name: link.name, Url: link.url, Id: link.id })),
                    State: formData.state ?? 0,
                    InternshipId: internshipId
                };

                const response = await axios.post('/api/v1/internship/task/create', payload, { withCredentials: true });
                taskId = response.data.data.id;
            }

            if (newFiles.length && taskId) {
                await Promise.all(newFiles.map(file => {
                    const formData = new FormData();
                    formData.append('TaskId', taskId as string);
                    formData.append('File', file);
                    return axios.post('/api/v1/file/task/upload', formData, {
                        withCredentials: true,
                        headers: { 'Content-Type': 'multipart/form-data' }
                    });
                }));
            }

            alert('Úkol úspěšně uložen!');
            onSubmitted();

        } catch (error) {
            console.error('Chyba při odesílání:', error);
        }
    };

    return (
        <div className="form-container-child">
            <div className="form-content-child">
                <h2 className="title">{isUpdate ? "Aktualizace úkolu" : "Vytvoření úkolu"}</h2>
                <div className="row">
                    <div className="col-md-12">
                        <div className="input-container">
                            <h3 className="input-title">Název</h3>
                            <input type="text" value={formData.name} onChange={(e) => handleChange('name', e.target.value)} placeholder="Název" className="input" />
                        </div>
                    </div>
                </div>
                <div className="row">
                    <div className="col-md-12">
                        <div className="input-container">
                            <h3 className="input-title">Popis</h3>
                            <textarea className="description-child" value={formData.description} onChange={(e) => handleChange('description', e.target.value)} placeholder="" />
                        </div>
                    </div>
                </div>
                <div className="row">
                    <div className="col-md-12">
                        <div className="input-container">
                            <h3 className="input-title">Shrnutí</h3>
                            <textarea className="description-child" value={formData.summary} onChange={(e) => handleChange('summary', e.target.value)} placeholder=""></textarea>
                        </div>
                    </div>
                </div>
                <div className="row">
                    <div className="col-md-12">
                        <div className="input-container">
                            <h3 className="input-title">Shrnutí učitele</h3>
                            <textarea className="description-child" value={formData.teacherSummary} onChange={(e) => handleChange('teacherSummary', e.target.value)} placeholder=""></textarea>
                        </div>
                    </div>
                </div>
                <div className="row">
                    <div className="col-md-6">
                        <div className="input-container">
                            <h3 className="input-title">Termín odevzdání</h3>
                            <input type="date" value={formData.endsOn} onChange={(e) => handleChange('endsOn', e.target.value)} className="input" />
                        </div>
                    </div>
                    <div className="col-md-6">
                        <div className="input-container">
                            <h3 className="input-title">Stav</h3>
                            <TaskStateSelect onStateSelect={(state) => handleChange('state', state)} selectedState={formData.state} />
                        </div>
                    </div>
                    <div className="col-md-6">
                        <div className="input-container">
                            <CustomCheckbox
                                checked={formData.isReported}
                                onChange={(checked) => handleChange("isReported", checked)}
                                label="Reportovat"
                            />
                        </div>
                    </div>
                </div>
                <div className="row">
                    <h3 className="subtitle">Odkazy</h3>
                    <div className="col-md-12">
                        <button type="button" className="linkButton" onClick={handleLinkAdd}>
                            Přidat odkaz
                        </button>
                        {formData.links.map((link, index) => (
                            <div key={index} className="link-container">
                                <input
                                    type="text"
                                    placeholder="Název odkazu"
                                    value={link.name}
                                    onChange={(e) => handleLinkChange(index, 'name', e.target.value)}
                                    className="input"
                                />
                                <input
                                    type="url"
                                    placeholder="URL odkazu"
                                    value={link.url}
                                    onChange={(e) => handleLinkChange(index, 'url', e.target.value)}
                                    className="input"
                                />
                                <button
                                    type="button"
                                    className="remove-button"
                                    onClick={() => handleLinkRemove(index)}
                                >
                                    Odebrat
                                </button>
                            </div>
                        ))}
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
                                <button type="button" className="linkButton-sm" onClick={() => handleDownload(file.fileName)}>Stáhnout</button>
                            </div>
                        ))}
                    </div>
                </div>

                <button type="submit" className="button-finish button-end" onClick={handleSubmit}>{isUpdate ? "Aktualizovat" : "Vytvořit"}</button>
            </div>
        </div >
    );
}

export default InternshipTaskCreateUpdateForm;