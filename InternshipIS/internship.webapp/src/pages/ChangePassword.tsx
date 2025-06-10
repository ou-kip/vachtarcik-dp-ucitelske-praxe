import React from 'react';
import ChangePasswordForm from '../Components/Forms/ChangePasswordForm/ChangePasswordForm'

const ChangePassword: React.FC = () => {
    return (
        <div className="d-flex flex-column justify-content-center align-items-center vh-100 vw-100" style={containerStyle}>
            <ChangePasswordForm />
        </div>
    );
};

const containerStyle: React.CSSProperties = {
    backgroundColor: '#f0f2f5',
};

export default ChangePassword;