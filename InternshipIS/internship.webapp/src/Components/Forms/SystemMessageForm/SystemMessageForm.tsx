import './SystemMessageForm.css'
import React, { useState, useEffect } from 'react';
import { useNavigate } from "react-router-dom";

interface SystemMessageFormProps {
    successMsg: string
    failMsg: string,
    isFailed: boolean
    redirectUrl?: string | null
    action?: () => void | Promise<void>;
}

const SystemMessageForm: React.FC<SystemMessageFormProps> = ({ successMsg, failMsg, isFailed, redirectUrl, action }) => {
    const navigate = useNavigate();
    const [isHovered, setIsHovered] = useState(false);
    const [message, setMessage] = useState('');
    const [redirect, setRedirect] = useState('');

    useEffect(() => {
        setMessage(isFailed ? failMsg : successMsg);
        setRedirect(redirectUrl || "/dashboard");
    }, [isFailed, successMsg, failMsg, redirectUrl]);

    const handleSubmitBtn = async (event: React.FormEvent) => {
        event.preventDefault();

        try {
            if (action) {
                await action();
            }
        } catch (error) {
            console.error("Chyba při provádění akce:", error);
        }

        navigate(redirect);
    };

    return (
        <div style={formStyle}>
            <h3 style={messageStyle}>{message}</h3>
            <button
                type="button"
                style={isHovered ? buttonHoverStyle : buttonStyle}
                onMouseEnter={() => setIsHovered(true)}
                onMouseLeave={() => setIsHovered(false)}
                onClick={handleSubmitBtn}
            >
                Zavřít
            </button>
        </div>
    );
}

export default SystemMessageForm;

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

const messageStyle: React.CSSProperties = {
    fontSize: 'larger',
    alignSelf: 'center',
    color: '#717171'
}