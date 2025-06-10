import React from 'react';
import PasswordResetForm from '../Components/Forms/PasswordReset/PasswordResetForm'

const ResetPassword: React.FC = () => {
    return (
        <div className="d-flex flex-column justify-content-center align-items-center vh-100 vw-100" style={containerStyle}>
            <PasswordResetForm />
        </div>
    );
};

const containerStyle: React.CSSProperties = {
    backgroundColor: '#f0f2f5',
};

export default ResetPassword;