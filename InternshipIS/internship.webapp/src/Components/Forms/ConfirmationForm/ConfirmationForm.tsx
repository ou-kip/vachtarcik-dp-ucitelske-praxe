import './ConfirmationForm.css'
import React, { useState, useEffect } from 'react';
import axios from 'axios';

interface ConfirmationFormProps {
    msg: string
    returnUrl: string,
    executeUrl: string
}

const ConfirmationForm: React.FC<ConfirmationFormProps> = ({ msg, returnUrl, executeUrl }) => {
    const [message, setMessage] = useState('');
    //const [redirectBack, setRedirectBack] = useState('');
    //const [execute, setExecute] = useState('');

    axios.defaults.baseURL = 'https://praxeosu.cz:5005';

    useEffect(() => {
        setMessage(msg);
        //setRedirectBack(returnUrl || "/dashboard");
        //setExecute(executeUrl)
    }, [msg, returnUrl, executeUrl]);

    return (
        <div className="form-custom">
            <h3 className="message-custom">{message}</h3>
            <button type="button" className="button-custom" >Potvrdit</button>
            <button type="button" className="button-custom" >
                Zrušit
            </button>
        </div>
    );
}

export default ConfirmationForm;