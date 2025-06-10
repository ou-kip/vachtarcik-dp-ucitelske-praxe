import React from 'react';
import RegisterSelfForm from '../Components/Forms/Register/RegisterSelfForm';
import { useSearchParams } from "react-router-dom";
import axios from 'axios';
import SystemMessageForm from '../Components/Forms/SystemMessageForm/SystemMessageForm';

const Register: React.FC = () => {
    const [searchParams] = useSearchParams();
    const token = searchParams.get('token');

    const handleSuccess = async () => {
        axios.defaults.baseURL = 'https://praxeosu.cz:5001';
        await axios.post('/api/v1/auth/register/confirmn', { token: token }, {withCredentials: false});
    };

    return (
        <div className="d-flex flex-column justify-content-center align-items-center vh-100 vw-100" style={containerStyle}>
            {token ? (<SystemMessageForm failMsg="" isFailed={false} successMsg="Potvrďte svůj účet." action={handleSuccess} />) : ( <RegisterSelfForm /> ) }
        </div>
    );
};

const containerStyle: React.CSSProperties = {
    backgroundColor: '#f0f2f5',
};

export default Register;