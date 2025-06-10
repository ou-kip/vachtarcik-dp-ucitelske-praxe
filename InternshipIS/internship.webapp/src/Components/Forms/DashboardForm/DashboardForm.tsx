import React from 'react';
import './DashboardForm.css'
import axios from 'axios';
import InternshipTaskEvaluationListForm from '../Task/InternshipTaskEvaluationListForm';
import Calendar from './Calendar';

interface DashboardFormProps {
    userRole: string | null
}

const DashboardForm: React.FC<DashboardFormProps> = ({ userRole }) => {

    axios.defaults.baseURL = 'https://praxeosu.cz:5005';

    return (
        <div className="container-fluid">
            <div className="row">
                <div className="col-md-6">
                       <Calendar />
                </div>
                <div className="col-md-6">
                    {(userRole === 'Teacher' || userRole === 'Admin') && (
                        <InternshipTaskEvaluationListForm createdByMe={true} filterProperty="State" filterValue="3" orderDirection={0} orderProperty="EndsOn" userRole={userRole} />
                    )}
                    {(userRole === 'Student') && (
                        <InternshipTaskEvaluationListForm createdByMe={true} filterProperty="State" filterValue="1" orderDirection={0} orderProperty="EndsOn" userRole={userRole} />
                    )}
                </div>
            </div>
        </div>
    )
}

export default DashboardForm