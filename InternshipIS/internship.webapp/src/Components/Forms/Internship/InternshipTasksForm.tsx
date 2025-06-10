import React, { useState } from 'react';
import './InternshipTasksForm.css'
import InternshipTaskListForm from './InternshipTaskListForm';
import InternshipTaskCreateUpdateForm from './InternshipTaskCreateUpdateForm';

interface InternshipTasksFormProps {
    internshipId: string | null;
    isUpdate: boolean
}

const InternshipTasksForm: React.FC<InternshipTasksFormProps> = ({ internshipId, isUpdate }) => {
    const [showTaskEdit, setShowTaskEdit] = useState(false);
    const [selectedTaskId, setSelectedTaskId] = useState<string | null>(null);
    const [isTaskUpdate, setIsTaskUpdate] = useState<boolean>(false);
    const [listRefreshKey, setListRefreshKey] = useState<number>(0);

    const handleRowEdit = (taskId: string | null) => {
        setSelectedTaskId(taskId);
        setShowTaskEdit(true);
        setIsTaskUpdate(true);
    };

    const handleCreate = () => {
        setShowTaskEdit(true);
        setSelectedTaskId(null);
        setIsTaskUpdate(false);
    };

    const handleCreateCompleted = () => {
        setShowTaskEdit(false);
        setIsTaskUpdate(false);
        setSelectedTaskId(null);
        setListRefreshKey(prev => prev === 0 ? 1 : 0);
    };


    if (showTaskEdit) {
        return (
            <>
                <div className="container content">
                    <h1 className="title-custom">Úkoly</h1>
                    <div className="head-buttons">
                        <button className="button-custom" onClick={(e) => { e.stopPropagation(); handleCreate(); }}>Přidat nový úkol</button>
                    </div>

                    <InternshipTaskCreateUpdateForm internshipId={internshipId} taskId={selectedTaskId} isUpdate={isTaskUpdate} onSubmitted={handleCreateCompleted} />
                </div>
            </>
        );
    }
   
    return (
        <div className="container content">
            <h1 className="title-custom">Úkoly</h1>
            <div className="head-buttons">
                <button className="button-custom" onClick={(e) => { e.stopPropagation(); handleCreate(); }}>Přidat nový úkol</button>
            </div>

            <InternshipTaskListForm internshipId={internshipId} isUpdate={isUpdate} onRowEdit={handleRowEdit} listRefreshKey={listRefreshKey} />
        </div>
    )
}

export default InternshipTasksForm;