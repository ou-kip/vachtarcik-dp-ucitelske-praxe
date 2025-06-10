import React, { useEffect, useState, useContext } from "react";
import '../css/Dashboard.css'
import { useAuth } from '../Components/Exports/UseAuth'
import  MainMenu  from '../Components/Menu/MainMenu'
import SystemMessageForm from "../Components/Forms/SystemMessageForm/SystemMessageForm";
import { AuthContext } from '../Components/AuthProvider';
import DashboardForm from "../Components/Forms/DashboardForm/DashboardForm";

const Dashboard: React.FC = () => {
    const { isAuthenticated } = useAuth();
    const [userRole, setUserRole] = useState<string | null>(null);
    const authContext = useContext(AuthContext);

    useEffect(() => {
        const loadRole = async () => {
            if (authContext) {
                const role = await authContext.role;
                setUserRole(role);
                console.log('Role: ', role);
            }
        };

        loadRole();
    }, [authContext]);

    if (!isAuthenticated) {
        return (
            <div className="main-style">
                <SystemMessageForm failMsg="" isFailed={false} successMsg="Nejste přihlášeni. Znovu se přihlaste." redirectUrl="/" />
            </div>
        )
    }

    return (
        <div>
            <MainMenu />
            <div className="main-dashboard">
                <DashboardForm userRole={userRole} />
            </div>
        </div>
    );
};

export default Dashboard;