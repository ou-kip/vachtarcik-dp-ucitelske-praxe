import '../Menu/MainMenu.css'
import logo from '../../assets/logo-osu-sm.svg';
import React, { useContext, useEffect, useState } from 'react';
import axios from 'axios';
import { AuthContext } from '../AuthProvider';
import { useNavigate } from "react-router-dom";

const MainMenu: React.FC = () => {
    const navigate = useNavigate();
    const authContext = useContext(AuthContext);
    const [userRole, setUserRole] = useState<string | null>(null);
    const [userFullName, setUserFullName] = useState<string | null>(null);

    useEffect(() => {
        const loadRole = async () => {
            if (authContext) {
                const role = await authContext.role;
                const fullName = await authContext.fullName;

                setUserRole(role);
                setUserFullName(fullName);

                console.log('Role: ', role);
            }
        };

        loadRole();
    }, [authContext]);

    if (authContext?.loading) {
        return <p>Načítám položky...</p>;
    }

    const logout = async () => {

        axios.defaults.baseURL = 'https://praxeosu.cz:5001';
        await axios.post('/api/v1/auth/logout', {}, { withCredentials: true });

        navigate('/');
    }

    const renderMenuItems = () => {
        switch (userRole) {
            case 'Admin': return (
                <>
                    <a className="nav-link nav-link-fixed" href="/internship/create">Vytvořit praxi</a>
                    <a className="nav-link nav-link-fixed" href="/internships">Seznam praxí</a>
                    <a className="nav-link nav-link-fixed" href="/admin">Administrace</a>
                </>)
            case 'Teacher': return (
                <>
                    <a className="nav-link nav-link-fixed" href="/internship/create">Vytvořit praxi</a>
                    <a className="nav-link nav-link-fixed" href="/internships">Seznam praxí</a>
                    <a className="nav-link nav-link-fixed" href="/admin">Administrace</a>
                </>)
            case 'CompanyRelative': return (
                <>
                    <a className="nav-link nav-link-fixed" href="/internship/create">Vytvořit praxi</a>
                    <a className="nav-link nav-link-fixed" href="/internships">Seznam praxí</a>
                </>)
            case 'Student': return (
                <>
                    <a className="nav-link nav-link-fixed" href="/dashboard">Moje praxe</a>
                    <a className="nav-link nav-link-fixed" href="/internships">Seznam praxí</a>
                </>)
        }
    }

    return (
        <header className="container-fluid">
            <div className="row">
                <nav className="navbar navbar-expand-lg shadow-sm" style={menuBg}>
                    <div className="container-fluid">
                        <a className="navbar-brand">
                            <img className="ms-5" src={logo} alt="Logo" style={logoSize} />
                        </a>
                        <h1 className="display-6">Portál pro zadávání praxí</h1>
                        <button className="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNavAltMarkup"
                            aria-controls="navbarNavAltMarkup" aria-expanded="false" aria-label="Toggle navigation">
                            <span className="navbar-toggler-icon"></span>
                        </button>
                        <div className="collapse navbar-collapse" id="navbarNavAltMarkup">
                            <div className="navbar-nav ms-auto  mb-lg-0">
                                <a className="nav-link nav-link-fixed active" aria-current="page" href="/dashboard">Dashboard</a>
                                {renderMenuItems()}

                                <div className="d-flex align-items-start gap-2 mt-2 mt-lg-0 ps-lg-3 border-start border-custom flex-column flex-lg-row">
                                    <span className="nav-link nav-link-fixed text-blue"><b>Přihlášen:</b> {userFullName}</span>
                                    <a className="nav-link nav-link-fixed" href="#" onClick={logout}>Odhlášení</a>
                                </div>
                            </div>
                        </div>
                    </div>
                </nav>
            </div>
        </header>
    );
};

const menuBg: React.CSSProperties = {
    backgroundColor: 'white'
};

const logoSize: React.CSSProperties = {
    //maxWidth: '194,4px',
    //maxHeight: '90px',
    /*marginLeft: '15vw'*/
};

export default MainMenu;