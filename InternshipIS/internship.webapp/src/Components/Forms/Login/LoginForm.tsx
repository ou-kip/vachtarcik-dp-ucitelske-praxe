import React, { useState } from 'react';
import axios from 'axios';
import { useNavigate } from "react-router-dom";
import './LoginForm.css';

const LoginForm: React.FC = () => {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const navigate = useNavigate();
    const [message, setMessage] = useState("");

    const handleSubmit = async (event: React.FormEvent) => {
        event.preventDefault();

        if (!isEmailValid(email)) {
            setMessage("Zadejte platný email.");
            return;
        }

        try {
            axios.defaults.baseURL = 'https://praxeosu.cz:5001';
            const response = await axios.post('/api/v1/auth/login',
                { email, password },
                { withCredentials: true }
            );

            if (response.status === 200) {
                navigate("/dashboard");
            } else {
                setMessage("Nepodařilo se přihlásit. Zkuste to znovu.");
            }

        } catch (error) {
            console.error('Chyba při přihlašování:', error);
            setMessage("Nepodařilo se přihlásit. Zkuste to znovu.");
        }
    };

    const handleLinkClick = (e: React.MouseEvent<HTMLAnchorElement, MouseEvent>, path: string) => {
        e.preventDefault();
        navigate(path);
    };

    const isEmailValid = (value: string) => {
        if (value === "admin") {
            return true; 
        }

        const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        return emailPattern.test(value) ? true : false;
    };

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const value = e.target.value;
        setEmail(value);
    };

    return (
        <form onSubmit={handleSubmit} className="login-form">
            <h1 className="form-title">Portál pro zadávání praxí</h1>
            <div className="input-container">
                <label className="username-container">
                    <input
                        type="text"
                        value={email}
                        onChange={handleChange}
                        className="input-labeled"
                        autoComplete="email"
                        placeholder="Email"
                    />
                </label>
            </div>
            <div>
                <input type="password" value={password} onChange={(e) => setPassword(e.target.value)} className="input" autoComplete="current-password" placeholder="Heslo"/>
            </div>
            <a href="/reset-password" className= "forgotten-password" onClick={(e) => handleLinkClick(e, "/reset-password")}>
                Zapomenuté heslo
            </a>
            <a href="/register" className= "register" onClick={(e) => handleLinkClick(e, "/register")}>
                Jste tady poprvé? Zaregistrujte se.
            </a>
            <button type="submit" className= "login-button">
                Přihlášení
            </button>
            <h3 className="error-message">{message}</h3>
        </form>
    );
};

export default LoginForm;