import { useState, useEffect } from 'react';
import './InternshipCreateUpdateForm.css';
import axios from 'axios';
import { CompanyRelative } from '../../Exports/CompanyRelative';
import { InternshipLink } from '../../Exports/InternshipLink';
import StudentSelect from '../../Inputs/StudentSelect/StudentSelect';
import TeacherSelect from '../../Inputs/TeacherSelect/TeacherSelect';
import CompanyRelativeSelect from '../../Inputs/CompanyRelativeSelect/CompanyRelativeSelect';
import InternshipCategorySelect from '../../Inputs/CategorySelect/InternshipCategorySelect';
import InternshipStateSelect from '../../Inputs/InternshipStateSelect/InternshipStateSelect';
import { Student } from '../../Exports/Student';
import { Teacher } from '../../Exports/Teacher';
import { InternshipCategory } from '../../Exports/InternshipCategory';

const steps = [
    'Základní údaje',
    'Studenti',
    'Učitelé',
    'Firemní osoby'
];

const InternshipCreateUpdateForm = ({
    isUpdate,
    onSuccess,
    internshipId,
}: {
    isUpdate: boolean;
    onSuccess?: (id: string | null) => void;
    internshipId: string | null;
}) => {
    const [step, setStep] = useState(0);
    const [formData, setFormData] = useState<{
        name: string;
        companyName: string;
        description: string;
        endsOn: string;
        companyRelatives: CompanyRelative[];
        student: Student | null;
        teachers: Teacher[];
        category: InternshipCategory | null;
        links: InternshipLink[];
        state: number;
    }>({
        name: '',
        companyName: '',
        description: '',
        endsOn: '',
        companyRelatives: [],
        student: null,
        teachers: [],
        category: null,
        links: [],
        state: 0
    });

    axios.defaults.baseURL = 'https://praxeosu.cz:5005';

    useEffect(() => {
        if (isUpdate) {
            axios.get(`/api/v1/internship/get?InternshipId=${internshipId}`, { withCredentials: true })
                .then(response => {
                    if (response.data && response.data.data.internshipDto) {
                        const internshipData = response.data.data.internshipDto;
                        setFormData({
                            name: internshipData.name || '',
                            companyName: internshipData.companyName || '',
                            description: internshipData.description || '',
                            endsOn: internshipData.endsOn ? new Date(internshipData.endsOn).toISOString().split('T')[0] : '',
                            companyRelatives: internshipData.companyRelatives || [],
                            student: internshipData.student || null,
                            teachers: internshipData.teachers || [],
                            category: internshipData.category || null,
                            links: internshipData.links || [],
                            state: internshipData.state
                        });
                    }
                })
                .catch(error => console.error('Chyba při načítání dat:', error));
        }
    }, [isUpdate, internshipId]);

    const handleNext = () => {
        setStep(prev => Math.min(prev + 1, steps.length - 1));
    };

    const handleBack = () => {
        setStep(prev => Math.max(prev - 1, 0));
    };

    type FormDataKeys = keyof typeof formData;

    type FormDataValues = string | string[] | Student | Teacher[] | CompanyRelative[] | InternshipCategory | InternshipLink[] | null | number;

    const handleChange = (field: FormDataKeys, value: FormDataValues) => {
        setFormData(prev => ({ ...prev, [field]: value }));
    };

    const handleRemoveTeacher = (id: string) => {
        setFormData(prev => ({
            ...prev,
            teachers: prev.teachers.filter(teacher => teacher.id !== id)
        }));
    };

    const handleRemoveRelative = (id: string) => {
        setFormData(prev => ({
            ...prev,
            companyRelatives: prev.companyRelatives.filter(relative => relative.id !== id)
        }));
    };

    const handleLinkAdd = () => {
        setFormData(prev => ({
            ...prev,
            links: [...prev.links, { id: null, name: '', url: '' }]
        }));
    };


    const handleLinkChange = (index: number, field: keyof InternshipLink, value: string) => {
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

    const handleSubmit = async () => {
        try {
            if (isUpdate) {
                const payload = {
                    Id: internshipId,
                    Name: formData.name,
                    CompanyName: formData.companyName,
                    Description: formData.description,
                    CategoryId: formData.category?.id ?? null,
                    Students: formData.student ? [formData.student] : [],
                    Teachers: formData.teachers,
                    CompanyRelatives: formData.companyRelatives,
                    Links: formData.links,
                    EndsOn: formData.endsOn ? new Date(formData.endsOn).toISOString() : null,
                    State: formData.state
                };

                await axios.post('/api/v1/internship/update', payload, { withCredentials: true });
                onSuccess?.(internshipId);
            }
            else {
                const payload = {
                    Name: formData.name,
                    CompanyName: formData.companyName,
                    Description: formData.description,
                    CategoryId: formData.category?.id ?? null,
                    StudentId: formData.student?.id ?? null,
                    TeacherIds: formData.teachers.map(t => t.id),
                    CompanyRelativeIds: formData.companyRelatives.map(cr => cr.id),
                    Links: formData.links.map(link => ({ Name: link.name, Url: link.url, Id: link.id })),
                    EndsOn: formData.endsOn ? new Date(formData.endsOn).toISOString() : null,
                    State: formData.state
                };

                const response = await axios.post('/api/v1/internship/create', payload, { withCredentials: true });
                onSuccess?.(response.data.data.id);
            }          
        } catch (error) {
            console.error('Chyba při odesílání:', error);
        }
    };

    return (
        <div className="multi-step-form">
            <div className="step-header">
                {steps.map((s, index) => (
                    <div
                        key={index}
                        className={`step ${index === step ? 'active' : ''}`}
                        onClick={() => setStep(index)}
                        style={{ cursor: 'pointer' }}>
                        {s}
                    </div>
                ))}
            </div>
            <div className="form-content">
                {step === 0 && (
                    <div>
                        <h2 className="title">Základní údaje</h2>
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
                                    <h3 className="input-title">Název firmy</h3>
                                    <input type="text" value={formData.companyName} onChange={(e) => handleChange('companyName', e.target.value)} placeholder="Název firmy" className="input" />
                                </div>
                            </div>
                        </div>

                        <div className="row">
                            <div className="col-md-6">
                                <div className="input-container">
                                    <h3 className="input-title">Kategorie</h3>
                                    <InternshipCategorySelect onCategorySelect={(category) => handleChange('category', category)} selectedCategory={formData.category} />
                                </div>
                            </div>
                            <div className="col-md-6">
                                <div className="input-container">
                                    <h3 className="input-title">Stav</h3>
                                    <InternshipStateSelect onStateSelect={(state) => handleChange('state', state)} selectedState={formData.state} allowedStates={isUpdate ? undefined : [0, 1]} />
                                </div>
                            </div>
                        </div>

                        <div className="row">
                            <div className="col-md-12">
                                <div className="input-container">
                                    <h3 className="input-title">Popis</h3>
                                    <textarea className="description" value={formData.description} onChange={(e) => handleChange('description', e.target.value)} placeholder="Popis"></textarea>
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

                    </div>
                )}
                {step === 1 && (
                    <div>
                        <h2 className="title">Výběr studentů</h2>
                        <StudentSelect onStudentSelect={(student) => handleChange('student', student)} selectedStudent={formData.student} />
                    </div>
                )}
                {step === 2 && (
                    <div>
                        <h2 className="title">Výběr účitelů</h2>
                        <TeacherSelect onTeacherAdd={(teacher) => handleChange('teachers', [...formData.teachers, teacher])} />
                        {formData.teachers.length > 0 && (
                            <table className="styled-table">
                                <thead>
                                    <tr>
                                        <th>Jméno</th>
                                        <th>Akce</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    {formData.teachers.map((teacher) => (
                                        <tr key={teacher.id}>
                                            <td>{teacher.fullName}</td>
                                            <td><button onClick={() => handleRemoveTeacher(teacher.id)} className="remove-button">Odebrat</button></td>
                                        </tr>
                                    ))}
                                </tbody>
                            </table>
                        )}
                    </div>
                )}
                {step === 3 && (
                    <div>
                        <h2 className="title">Výběr zodpovědných osob za firmu</h2>
                        <CompanyRelativeSelect onRelativeAdd={(relative) => handleChange('companyRelatives', [...formData.companyRelatives, relative])} />
                        {formData.companyRelatives.length > 0 && (
                            <table className="styled-table">
                                <thead>
                                    <tr>
                                        <th>Jméno</th>
                                        <th>Akce</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    {formData.companyRelatives.map((relative) => (
                                        <tr key={relative.id}>
                                            <td>{relative.fullName}</td>
                                            <td><button onClick={() => handleRemoveRelative(relative.id)} className="remove-button">Odebrat</button></td>
                                        </tr>
                                    ))}
                                </tbody>
                            </table>
                        )}
                    </div>
                )}
                <div className="buttons">
                    {step > 0 && <button className="button" onClick={handleBack}>Zpět</button>}
                    {step < steps.length - 1 ? <button className="button" onClick={handleNext}>Dále</button> : <button className="button-finish" onClick={handleSubmit}>Dokončit</button>}
                </div>
            </div>
        </div>
    );
};

export default InternshipCreateUpdateForm;
