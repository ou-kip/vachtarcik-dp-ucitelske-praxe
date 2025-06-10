import React from "react";
import '../css/Dashboard.css'
import { useAuth } from '../Components/Exports/UseAuth'
import MainMenu from '../Components/Menu/MainMenu'
import SystemMessageForm from "../Components/Forms/SystemMessageForm/SystemMessageForm";

const InternshipOverview: React.FC = () => {
    const { isAuthenticated } = useAuth();

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
        < div style = { containerStyle } >
            <h1>Dashboard </h1>
            < p > Vítejte na stránce Dashboard! </p>
                </div>
                </div>
    );
};

const containerStyle: React.CSSProperties = {
    paddingTop: '10vh',
    display: 'flex',
    flexDirection: 'column',
    alignItems: 'center',
    width: '100vw',
    backgroundColor: '#f0f2f5',
};

export default InternshipOverview;