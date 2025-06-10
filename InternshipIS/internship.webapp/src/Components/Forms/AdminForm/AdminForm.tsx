import './AdminForm.css';
import RegisterForm from '../Register/RegisterForm';
import AccountListForm from './AccountsListForm/AccountsListForm';
import { useState } from 'react';

const AdminForm: React.FC = () => {
    const [selectedView, setSelectedView] = useState<string | null>(null);

    const handleViewChange = (view: string) => {
        setSelectedView(prev => (prev === view ? null : view)); // toggle behavior (optional)
    };

    return (
        <div className="admin-form">
            <h2 className="title">Administrace</h2>
            <div className="reg-buttons">
                <button className="button-custom" onClick={() => handleViewChange("registerStudent")}>Registrace studenta</button>
                <button className="button-custom" onClick={() => handleViewChange("registerTeacher")}>Registrace učitele</button>
                <button className="button-custom" onClick={() => handleViewChange("registerPerson")}>Registrace firemní osoby</button>
                <button className="button-custom" onClick={() => handleViewChange("accountList")}>Seznam účtů</button>
            </div>

            {selectedView?.startsWith("register") && (
                <RegisterForm selectedItem={selectedView} />
            )}

            {selectedView === "accountList" && (
                <AccountListForm />
            )}

        </div>
    );
}

export default AdminForm;