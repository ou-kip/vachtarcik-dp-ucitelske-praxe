﻿import React, { useState } from 'react';
import axios from 'axios';
import { useSearchParams } from "react-router-dom";
import { useNavigate } from "react-router-dom";

const ChangePasswordForm: React.FC = () => {
    const navigate = useNavigate();
    const [isHovered, setIsHovered] = useState(false);
    const [password, setPassword] = useState('');
    const [confirmPassword, setConfirmPassword] = useState('');
    const [message, setMessage] = useState("");
    const [success, setSuccess] = useState(false);

    const [searchParams] = useSearchParams();
    const token = searchParams.get("token");

    const handleSubmit = async (event: React.FormEvent) => {
        event.preventDefault();

        try {

            if (password !== confirmPassword) {
                setMessage("Hesla se neshodují.");
                return;
            }

            axios.defaults.baseURL = 'https://praxeosu.cz:5001';
            const response = await axios.post('/api/v1/auth/password/update', { token: token, password: password }, { withCredentials: true });

            if (response.status === 200) {
                setMessage('Heslo úspěšně změněno')
                setSuccess(true);
            }
            else {
                setMessage('Něco se nezdařilo, zkuste akci opakovat.')
            }


        } catch (error) {
            console.error('Chyba', error);
            setMessage('Něco se nezdařilo, zkuste akci opakovat.')
        }
    };

    const navigateBack = () => {
        navigate("/");
    }

    return (
        <form onSubmit={handleSubmit} style={formStyle}>
            {
                !success ?
                    (
                        <>
                            <h1 style={titleStyle}>Změna Hesla</h1>
                            <div>
                                <h2 style={inputTitleStyle}>Nové heslo:</h2>
                                <input
                                    type="password"
                                    value={password}
                                    onChange={(e) => setPassword(e.target.value)}
                                    style={inputStyle}
                                    autoComplete="off"
                                />
                            </div>
                            <div>
                                <h2 style={inputTitleStyle}>Potvrdit heslo:</h2>
                                <input
                                    type="password"
                                    value={confirmPassword}
                                    onChange={(e) => setConfirmPassword(e.target.value)}
                                    style={inputStyle}
                                    autoComplete="off"
                                />
                            </div>
                            <button type="submit" style={isHovered ? buttonHoverStyle : buttonStyle}
                                onMouseEnter={() => setIsHovered(true)}
                                onMouseLeave={() => setIsHovered(false)}>Odeslat</button>
                            <h3 style={ messageErrorStyle }>{message}</h3>
                        </>
                    ) :
                    (
                        <>
                            <h3 style={messageStyle}>{message}</h3>
                            <button type="button" onClick={navigateBack} style={isHovered ? buttonHoverStyle : buttonStyle}
                                onMouseEnter={() => setIsHovered(true)}
                                onMouseLeave={() => setIsHovered(false)}>
                                Zavřít
                            </button>
                        </>
                    )

            }
        </form>
    );
}

export default ChangePasswordForm;

const buttonStyle: React.CSSProperties = {
    padding: '10px 20px',
    borderRadius: '4px',
    border: 'none',
    backgroundColor: '#00b7cf',
    color: 'white',
    fontWeight: 'bold',
    cursor: 'pointer',
    alignSelf: 'end',
    marginTop: '1.64vh'
};

const buttonHoverStyle: React.CSSProperties = {
    ...buttonStyle,
    backgroundColor: '#0090a6',
};

const formStyle: React.CSSProperties = {
    fontFamily: 'Roboto, Arial, sans-serif',
    display: 'flex',
    flexDirection: 'column',
    padding: '20px',
    boxShadow: '0 0 10px rgba(0, 0, 0, 0.1)',
    borderRadius: '8px',
    backgroundColor: '#fff',
    width: '100%',
    maxWidth: '600px',
    boxSizing: 'border-box',
    marginTop: '1vh'
};

const inputStyle: React.CSSProperties = {
    padding: '10px',
    width: '100%',
    borderRadius: '10px',
    border: 'none',
    backgroundColor: '#f0f2f5',
    boxSizing: 'border-box',
    color: '#515151'
};

const titleStyle: React.CSSProperties = {
    fontSize: '24px',
    fontWeight: 'bold',
    marginBottom: '10px',
    color: '#4e5e6d',
    textAlign: 'center',
};

const inputTitleStyle: React.CSSProperties = {
    color: '#717171',
    textAlign: 'left',
    margin: 0,
    fontSize: '16px'
};

const messageStyle: React.CSSProperties = {
    fontSize: 'larger',
    alignSelf: 'center',
    color: '#717171'
}

const messageErrorStyle: React.CSSProperties = {
    fontSize: '16px',
}