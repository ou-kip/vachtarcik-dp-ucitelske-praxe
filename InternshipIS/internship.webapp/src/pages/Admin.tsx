import React from "react";
import '../css/Admin.css'
import { useAuth } from '../Components/Exports/UseAuth'
import MainMenu from '../Components/Menu/MainMenu'
import AdminForm from '../Components/Forms/AdminForm/AdminForm'
import SystemMessageForm from "../Components/Forms/SystemMessageForm/SystemMessageForm";

const Admin: React.FC = () => {
    const { isAuthenticated, loading } = useAuth();

    if (!isAuthenticated) {
        return (
            <div className="main-style">
                <SystemMessageForm failMsg="" isFailed={false} successMsg="Nejste přihlášeni. Znovu se přihlaste." redirectUrl="/" />
            </div>
        )
    }

    if (loading) {
        return <div>Načítání</div>;
    }

    return (
        <div>
            <MainMenu />
            <div className="main-style">
                <AdminForm />
            </div>
        </div>
    );
};

export default Admin;