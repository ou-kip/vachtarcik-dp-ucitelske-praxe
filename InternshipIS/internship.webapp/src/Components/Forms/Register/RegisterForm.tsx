import React, { useState } from 'react';
import axios from 'axios';
import SystemMessageForm from '../SystemMessageForm/SystemMessageForm';

const formStyle: React.CSSProperties = {
    fontFamily: 'Roboto, Arial, sans-serif',
    display: 'flex',
    flexDirection: 'column',
    padding: '20px',
    backgroundColor: '#fff',
    width: '100%',
    maxWidth: '760px',
    boxSizing: 'border-box',
    marginTop: '7vh'
};

const inputContainerStyle: React.CSSProperties = {
    marginBottom: '15px',
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

interface RegisterFormProps {
    selectedItem: string | null;
}

const RegisterForm: React.FC<RegisterFormProps> = ({ selectedItem }) => {
    const [email, setEmail] = useState('');
    const [name, setName] = useState('');
    const [lastName, setLastName] = useState('');
    const [username, setUserName] = useState('');
    const [password, setPassword] = useState('');
    const [company, setCompany] = useState('');
    const [completed, setCompleted] = useState(false);
    const [isFailed, setIsFailed] = useState(false);

    const handleSubmit = async (event: React.FormEvent) => {
        event.preventDefault();

        try {

            axios.defaults.baseURL = 'https://praxeosu.cz:5001';
            let response;
            let displayResponse = '';

            switch (selectedItem) {
                case 'registerStudent': response = await axios.post('/api/v1/auth/register/student', { code: username, name: name, lastName: lastName, email: email, username: email, password: password }, { withCredentials: true }); break;
                case 'registerTeacher': response = await axios.post('/api/v1/auth/register/teacher', { name: name, lastName: lastName, email: email, username: email, password: password }, { withCredentials: true }); break;
                case 'registerPerson': response = await axios.post('/api/v1/auth/register/relative', { name: name, lastName: lastName, email: email, username: email, password: password, companyName: company }, { withCredentials: true }); break;
            }

            if (response?.data.statusCode === 201) {
                displayResponse = 'Registrace úspěšná';
                setCompleted(true);
                setIsFailed(false);
            }

            console.log('Úspěšná registrace:', displayResponse);

        } catch (error) {
            console.error('Chyba při registraci:', error);
            setCompleted(true);
            setIsFailed(true);
        }
    };

    const renderTitle = () => {
        switch (selectedItem) {
            case 'registerStudent': return ('Registrace studenta')
            case 'registerTeacher': return ('Registrace učitele')
            case 'registerPerson': return ('Registrace pověřené osoby')
        }
    }

    const renderAdditionalField = () => {
        if (selectedItem === 'registerStudent') {
            return (
                <>
                    <div className="row">
                        <div className="col-md-12">
                            <div style={inputContainerStyle}>
                                <h2 style={inputTitleStyle}>Studentský kód</h2>
                                <input
                                    type="text"
                                    value={username}
                                    onChange={(e) => setUserName(e.target.value)}
                                    style={inputStyle}
                                    autoComplete="off"
                                />
                            </div>
                        </div>
                    </div>
                </>
            )
        }
        if (selectedItem === 'registerPerson') {
            return (
                <>
                    <div className="row">
                        <div className="col-md-12">
                            <div style={inputContainerStyle}>
                                <h2 style={inputTitleStyle}>Název firmy</h2>
                                <input
                                    type="text"
                                    value={company}
                                    onChange={(e) => setCompany(e.target.value)}
                                    style={inputStyle}
                                    autoComplete="organization"
                                />
                            </div>
                        </div>
                    </div>
                </>
            )
        }
    }

    return !completed ? (
        <form onSubmit={handleSubmit} style={formStyle}>
            <h1 style={titleStyle}>
                {renderTitle()}
            </h1>
            <div className="row">
                <div className="col-md-6">
                    <div style={inputContainerStyle}>
                        <h2 style={inputTitleStyle}>Jméno</h2>
                        <input
                            type="text"
                            value={name}
                            onChange={(e) => setName(e.target.value)}
                            style={inputStyle}
                            autoComplete="given-name"
                        />
                    </div>
                </div>
                <div className="col-md-6">
                    <div style={inputContainerStyle}>
                        <h2 style={inputTitleStyle}>Příjmení</h2>
                        <input
                            type="text"
                            value={lastName}
                            onChange={(e) => setLastName(e.target.value)}
                            style={inputStyle}
                            autoComplete="family-name"
                        />
                    </div>
                </div>
            </div>
            {renderAdditionalField()}
            <div className="row">
                <div className="col-md-12">
                    <div style={inputContainerStyle}>
                        <h2 style={inputTitleStyle}>Email</h2>
                        <input
                            type="email"
                            value={email}
                            onChange={(e) => setEmail(e.target.value)}
                            style={inputStyle}
                            autoComplete="email"
                        />
                    </div>
                </div>
            </div>
            <div className="row">
                <div className="col-md-12">
                    <div style={inputContainerStyle}>
                        <h2 style={inputTitleStyle}>Heslo</h2>
                        <input
                            type="password"
                            value={password}
                            onChange={(e) => setPassword(e.target.value)}
                            style={inputStyle}
                            autoComplete="new-password"
                        />
                    </div>
                </div>
            </div>

            <button type="submit" className="button-finish button-end">Registrovat</button>
        </form>
    ) : (
            <SystemMessageForm failMsg="Něco se nezdařilo." isFailed={isFailed} successMsg={"Registrace proběhla úspěšně"} />
    );

}
export default RegisterForm;
