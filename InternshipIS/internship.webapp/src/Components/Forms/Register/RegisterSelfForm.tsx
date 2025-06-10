import React, { useState } from 'react';
import axios from 'axios';
import './RegisterSelfForm.css';
import SystemMessageForm from '../SystemMessageForm/SystemMessageForm';

/*const accountTypes = ['Student', 'Učitel', 'Firemní osoba'];*/


const RegisterSelfForm: React.FC = () => {
    /*const [activeType, setActiveType] = useState('Student');*/
    const activeType = 'Student';
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

            switch (activeType) {
                case 'Student': response = await axios.post('/api/v1/auth/register/student', { code: username, name: name, lastName: lastName, email: email, username: email, password: password }, { withCredentials: true }); break;
                //case 'Učitel': response = await axios.post('/api/v1/auth/register/teacher', { name: name, lastName: lastName, email: email, username: email, password: password }, { withCredentials: true }); break;
                //case 'Firemní osoba': response = await axios.post('/api/v1/auth/register/relative', { name: name, lastName: lastName, email: email, username: email, password: password, companyName: company }, { withCredentials: true }); break;
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

    const renderAdditionalField = () => {
        if (activeType === 'Student') {
            return (
                <>
                    <div className="row">
                        <div className="col-md-12">
                            <div className="input-container">
                                <h2 className="input-title">Studentský kód</h2>
                                <input
                                    type="text"
                                    value={username}
                                    onChange={(e) => setUserName(e.target.value)}
                                    className="form-input"
                                    autoComplete="off"
                                />
                            </div>
                        </div>
                    </div>
                </>
            )
        }
        if (activeType === 'Firemní osoba') {
            return (
                <>
                    <div className="row">
                        <div className="col-md-12">
                            <div className="input-container">
                                <h2 className="input-title">Název firmy</h2>
                                <input
                                    type="text"
                                    value={company}
                                    onChange={(e) => setCompany(e.target.value)}
                                    className="form-input"
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
        <div className="register-multi-step-form register-form">
            <h1 className="form-title">Registrace</h1>
            {/*<div className="step-button-header">*/}
            {/*    {accountTypes.map((type) => (*/}
            {/*        <div*/}
            {/*            key={type}*/}
            {/*            className={`step-button ${type === activeType ? 'active' : ''}`}*/}
            {/*            onClick={() => setActiveType(type)}*/}
            {/*        >*/}
            {/*            {type}*/}
            {/*        </div>*/}
            {/*    ))}*/}
            {/*</div>*/}
            <div className="register-content">
                <div className="row">
                    <div className="col-md-6">
                        <div className="input-container">
                            <h2 className="input-title">Jméno</h2>
                            <input
                                type="text"
                                value={name}
                                onChange={(e) => setName(e.target.value)}
                                className="form-input"
                                autoComplete="given-name"
                            />
                        </div>
                    </div>
                    <div className="col-md-6">
                        <div className="input-container">
                            <h2 className="input-title">Příjmení</h2>
                            <input
                                type="text"
                                value={lastName}
                                onChange={(e) => setLastName(e.target.value)}
                                className="form-input"
                                autoComplete="family-name"
                            />
                        </div>
                    </div>
                </div>

                {renderAdditionalField()}

                <div className="row">
                    <div className="col-md-12">
                        <div className="input-container">
                            <h2 className="input-title">Email</h2>
                            <input
                                type="email"
                                value={email}
                                onChange={(e) => setEmail(e.target.value)}
                                className="form-input"
                                autoComplete="email"
                            />
                        </div>
                    </div>
                </div>
                <div className="row">
                    <div className="col-md-12">
                        <div className="input-container">
                            <h2 className="input-title">Heslo</h2>
                            <input
                                type="password"
                                value={password}
                                onChange={(e) => setPassword(e.target.value)}
                                className="form-input"
                                autoComplete="new-password"
                            />
                        </div>
                    </div>
                </div>

                <button type="submit" className="button-finish button-middle" onClick={handleSubmit}>Registrovat</button>
            </div>
        </div>
    ) : (<SystemMessageForm failMsg="Něco se nezdařilo." isFailed={isFailed} successMsg={"Registrace proběhla úspěšně. Byl odeslán potvrzovací email."} redirectUrl={"/"} />);
};

export default RegisterSelfForm;