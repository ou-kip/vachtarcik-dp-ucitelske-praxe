import React from 'react';
import LoginForm from '../Components/Forms/Login/LoginForm';
import logo from '../assets/osulogo-hor.png';

const Index: React.FC = () => {
    return (
        <div className="d-flex flex-column justify-content-center align-items-center vh-100 vw-100" style={containerStyle}>
            <img src={logo} alt="Logo" style={imageStyle} />
            
            <LoginForm />
        </div>
    );
};

const containerStyle: React.CSSProperties = {
    backgroundColor: '#f0f2f5',
};

const imageStyle: React.CSSProperties = {
    marginBottom: '20px',
};

export default Index;