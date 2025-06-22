import React, { useState, useEffect } from 'react';
import axios from 'axios';
import InternshipDetailForm from '../Detail/InternshipDetailForm';
import InternshipTaskListForm from '../../../Forms/Internship/InternshipTaskListForm';
import { useNavigate } from 'react-router-dom';

interface InternshipOverviewProps {
    internshipId: string | null
}

const InternshipOverview: React.FC<InternshipOverviewProps> = ({ internshipId }) => {
    const [selectedTaskId, setSelectedTaskId] = useState<string | null>(null);
    const navigate = useNavigate();
    const taskListRefreshKey = 0;

    axios.defaults.baseURL = 'https://praxeosu.cz:5005';

    const navigateToTaskDetail = (taskId: string | null) => {
        setSelectedTaskId(taskId);
    }

    useEffect(() => {
        if (selectedTaskId) {
            navigate(`/task/detail?taskId=${selectedTaskId}`);
        }

    }, [selectedTaskId, navigate])

    return (
        <div className="container-fluid">
            <div className="row">
                <div className="col-md-6">
                    <InternshipDetailForm internshipId = {internshipId } />
                </div>
                <div className="col-md-6">
                    <InternshipTaskListForm internshipId={internshipId} isUpdate={true} listRefreshKey={taskListRefreshKey} onRowEdit={navigateToTaskDetail} />
                </div>
            </div>
        </div>
    )
}

export default InternshipOverview;