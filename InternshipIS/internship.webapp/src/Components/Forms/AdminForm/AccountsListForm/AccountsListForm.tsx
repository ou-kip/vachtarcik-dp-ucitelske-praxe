import React, { useContext, useState, useEffect, useCallback } from 'react';
import { Account } from '../../../Exports/Account';
import axios from 'axios';
import PersonTypeSelect from '../../../Inputs/PersonTypeSelect/PersonTypeSelect';
import ThrashIcon from '../../../../assets/trash3-fill.svg';
import { AuthContext } from '../../../../Components/AuthProvider';
import './AccountsListForm.css';
import AuthApi from '../../../Exports/AuthApi';

interface ApiResponse {
    statusCode: number;
    message: string | null;
    data: {
        accounts: Account[];
        statusCode: number;
        message: string | null;
    };
    errors: string[];
    hasErrors: boolean;
}

interface ApiDeleteResponse {
    statusCode: number;
    message: string | null;
    data: {
        deleted: boolean;
        statusCode: number;
        message: string | null;
    };
    errors: string[];
    hasErrors: boolean;
}

const AccountsListForm: React.FC = () => {
    const [accounts, setAccounts] = useState<Array<Account>>([]);
    const [offset, setOffset] = useState<number>(0);
    const [take, setTake] = useState<number>(20);
    const [accType, setAccType] = useState<number>(0);
    const [fullNameSearch, setFullNameSearch] = useState<string | null>(null);
    const authContext = useContext(AuthContext);
    const [userRole, setUserRole] = useState<string | null>(null);

    axios.defaults.baseURL = 'https://praxeosu.cz:5005';

    const loadAccounts = useCallback(async () => {
        try {
            const response = await axios.get<ApiResponse>('/api/v1/person/getall/', {
                withCredentials: true,
                params: {
                    offset,
                    take,
                    personType: accType,
                    search: fullNameSearch
                }
            });

            if (response.data?.data?.accounts) {
                setAccounts(response.data.data.accounts);
            }
        } catch (error) {
            console.error('Chyba při načítání dat:', error);
        }
    }, [accType, take, offset, fullNameSearch]);

    useEffect(() => {
        const loadRole = async () => {
            if (authContext) {
                const role = await authContext.role;
                setUserRole(role);
                console.log('Role: ', role);
            }
        };

        loadRole();
    }, [authContext]);

    useEffect(() => {
        loadAccounts();
    }, [accType, take, offset, fullNameSearch, loadAccounts]);

    const renderActionButton = (personId: string) => {
        if (userRole === 'Teacher') {
            return (<>
                <div className="action-buttons">
                    <button title="Smazat" className="button-red" onClick={(e) => { e.stopPropagation(); deleteAccount(personId); }}>
                        <img src={ThrashIcon} alt="Smazat" />
                    </button>
                </div>
            </>)
        }
        else if (userRole === 'Admin') {
            return (<>
                <div className="action-buttons">
                    <button title="Smazat" className="button-red" onClick={(e) => { e.stopPropagation(); deleteAccount(personId); }}>
                        <img src={ThrashIcon} alt="Smazat" />
                    </button>
                </div>
            </>)
        }
    }

    const deleteAccount = async (email: string | null) => {
        try {
            const response = await AuthApi.delete<ApiDeleteResponse>(`/api/v1/auth`,
                {
                    params: { email: email }
                });
            if (response.data.data.deleted) {
                loadAccounts();
            }
        }
        catch (error) {
            console.error('Chyba při smazání dat:', error);
        }
    }

    return (
        <div className="container content-child">
            <h1 className="title-custom">Seznam účtů</h1>
            <div className="container-fluid">
                <div className="row">
                    <div className="col-md-3">
                        <div className="input-container">
                            <h3 className="input-title">Typ účtu</h3>
                            <PersonTypeSelect onTypeSelect={(type) => setAccType(type as number)} selectedType={accType} />
                        </div>
                    </div>
                    <div className="col-md-3">
                        <div className="input-container">
                            <h3 className="input-title">Začít od</h3>
                            <input type="number" onChange={(e) => setOffset(e.target.valueAsNumber)} defaultValue={0} className="input" />
                        </div>
                    </div>
                    <div className="col-md-3">
                        <div className="input-container">
                            <h3 className="input-title">Počet záznamů</h3>
                            <input type="number" onChange={(e) => setTake(e.target.valueAsNumber)} defaultValue={20} className="input" />
                        </div>
                    </div>
                    <div className="col-md-3">
                        <div className="input-container">
                            <h3 className="input-title">Hledat</h3>
                            <input type="text" onChange={(e) => { if (e.target.value.length > 3 || e.target.value.length === 0) { setFullNameSearch(e.target.value) } }} className="input" />
                        </div>
                    </div>
                </div>
            </div>
            <div className="table scroll-container custom-size-container">
                <table className="table table-hover table-centered">
                    <thead className="table-thead-custom">
                        <tr>
                            <th scope="col" className="blue-th">#</th>
                            <th scope="col" className="blue-th">Jméno</th>
                            <th scope="col" className="blue-th">Příjmení</th>
                            <th scope="col" className="blue-th">Email</th>
                            <th scope="col" className="blue-th">Akce</th>
                        </tr>
                    </thead>
                    <tbody className="scroll-body">
                        {accounts.map((account, index) => (
                            <tr key={account.id}>
                                <td>{index + 1}</td>
                                <td>{account.name || ''}</td>
                                <td>{account.lastName || ''}</td>
                                <td>{account.email}</td>
                                <td>{renderActionButton(account.email)}</td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            </div>
        </div>
    )
}

export default AccountsListForm;