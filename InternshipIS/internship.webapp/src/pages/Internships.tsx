import React, { useContext, useEffect, useState } from 'react';
import { useAuth } from '../Components/Exports/UseAuth'
import MainMenu from '../Components/Menu/MainMenu'
import '../css/Internships.css'
//import { useSearchParams } from "react-router-dom";
import { AuthContext } from '../Components/AuthProvider';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import CustomCheckbox from '../Components/Inputs/Checkbox/Checkbox';
import { CompanyRelative } from '../Components/Exports/CompanyRelative';
import { InternshipCategory } from '../Components/Exports/InternshipCategory';
import { Teacher } from '../Components/Exports/Teacher';
import { InternshipLink } from '../Components/Exports/InternshipLink';
import { Student } from '../Components/Exports/Student';
import ThrashIcon from '../assets/trash3-fill.svg';
import PickIcon from '../assets/calendar-check-fill.svg';
import EditIcon from '../assets/pencil-square.svg';
import SystemMessageForm from '../Components/Forms/SystemMessageForm/SystemMessageForm';

interface Internship {
    id: string;
    name: string;
    companyName: string;
    description: string;
    endsOn: string;
    companyRelatives: CompanyRelative[];
    student: Student | null;
    teachers: Teacher[];
    category: InternshipCategory | null;
    links: InternshipLink[];
    state: number;
    selectedOn: string | null;
    createdOn: string;
    createdByName: string;
    isCreatedByRequester: boolean
}

interface ApiResponse {
    statusCode: number;
    message: string | null;
    data: {
        items: Internship[];
        statusCode: number;
        message: string | null;
    };
    errors: string[];
    hasErrors: boolean;
}

const Internships: React.FC = () => {
    const { isAuthenticated } = useAuth();
    const [internships, setInternships] = useState<Internship[]>([]);
    const navigate = useNavigate();
    const [sortColumn, setSortColumn] = useState<string | null>(null);
    const [sortDirection, setSortDirection] = useState<0 | 1>(0);
    const authContext = useContext(AuthContext);
    const [userRole, setUserRole] = useState<string | null>(null);
    const [createdByMe, setCreatedByMe] = useState(false);
    //const [showDeletion, setShowDeletion] = useState(false);
    //const [showAssignation, setShowAssignation] = useState(false);
    
    axios.defaults.baseURL = 'https://praxeosu.cz:5005';

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
        fetchData(sortColumn, sortDirection, createdByMe);
    }, [sortColumn, sortDirection, createdByMe]);

    const fetchData = async (column?: string | null, direction?: number, isCreatedByMe?: boolean) => {
        try {
            const response = await axios.get<ApiResponse>(`/api/v1/internship/getcollection?OrderProperty=${column}&OrderDirection=${direction}&CreatedByMe=${isCreatedByMe}`,
                {
                    withCredentials: true,
                });

            setInternships(response.data.data.items || []);
        } catch (error) {
            console.error('Chyba při načítání dat:', error);
        }
    };

    const deleteInternship = async (internshipId: string | null) => {
        try {
            await axios.delete(`/api/v1/internship/delete?Id=${internshipId}`,
                {
                    withCredentials: true,
                });
            fetchData(sortColumn, sortDirection, createdByMe);
        }
        catch (error) {
            console.error('Chyba při smazání dat:', error);
        }
    }

    const editInternship = async (internshipId: string | null) => {
        try {
            if (internshipId) {
                navigate(`/internship/edit?internshipId=${internshipId}`);
            }
        }
        catch (error) {
            console.error('Chyba při smazání dat:', error);
        }
    }

    const assignToMe = async (internshipId: string | null) => {
        try {
            await axios.post(`/api/v1/internship/assigntome?InternshipId=${internshipId}`, {},
                {
                    withCredentials: true,
                });
            fetchData(sortColumn, sortDirection, createdByMe);
        }
        catch (error) {
            console.error('Chyba', error);
        }
    }

    const handleRowClick = (id: string | null) => {
        if (id) {
            navigate(`/internship/detail?internshipId=${id}`);
        }
    };

    const handleSort = (column: string) => {
        const newDirection = sortColumn === column && sortDirection === 0 ? 1 : 0;
        setSortColumn(column);
        setSortDirection(newDirection);
    };

    const RenderTeacherFilters = () => {
        switch (userRole) {
            case 'Teacher': return (
                <>
                    <div>
                        <CustomCheckbox
                            checked={createdByMe}
                            onChange={setCreatedByMe}
                            label="Moje vytvořené praxe"
                        />
                    </div>
                </>
            )
        }
    }

    const ResolveState = (state: number) => {
        switch (state) {
            case 0: return "Vytvořeno";
            case 1: return "Publikováno";
            case 2: return "Vybráno"
            case 3: return "Uzavřeno";
            case 4: return "Zrušeno"
        }
    }

    const RenderActionButton = (internship: Internship, student: Student | null) => {
        if (userRole === 'Teacher' && internship.isCreatedByRequester && internship.state != 3) {
            return (
                <div className="action-buttons">
                    <button title="Upravit" className="button-blue-round" onClick={(e) => { e.stopPropagation(); editInternship(internship.id); }}>
                        <img src={EditIcon} alt="Upravit" />
                    </button>
                    {student == null && (
                        <button title="Smazat" className="button-red" onClick={(e) => { e.stopPropagation(); deleteInternship(internship.id); }}>
                            <img src={ThrashIcon} alt="Smazat" />
                        </button>
                    )}
                </div>
            );
        }
        else if (userRole === 'Student' && internship.state < 2) {
            return (
                <div className="action-buttons">
                    <button title="Vybrat" className="button-blue-round" onClick={(e) => { e.stopPropagation(); assignToMe(internship.id); }}>
                        <img src={PickIcon} alt="Vybrat" />
                    </button>
                </div>
            );
        }
        if (userRole === 'Admin') {
            return (
                <div className="action-buttons">
                    <button title="Upravit" className="button-blue-round" onClick={(e) => { e.stopPropagation(); editInternship(internship.id); }}>
                        <img src={EditIcon} alt="Upravit" />
                    </button>
                    <button title="Smazat" className="button-red" onClick={(e) => { e.stopPropagation(); deleteInternship(internship.id); }}>
                        <img src={ThrashIcon} alt="Smazat" />
                    </button>
                </div>
            );
        }
    }

    if (!isAuthenticated) {
        return (
            <div className="main-style">
                <SystemMessageForm failMsg="" isFailed={false} successMsg="Nejste přihlášeni. Znovu se přihlaste." redirectUrl="/" />
            </div>
        )
    }

    return (
        <div>
            <MainMenu />
            <div className="main-style">
                <div className="container contentStyle">
                    <h1 className="titleStyle">Seznam praxí</h1>
                    <div className="container-fluid">
                        {RenderTeacherFilters()}
                    </div>
                    <div className="tableContainerStyle scroll-container">
                        <table className="table table-hover table-centered tableStyle">
                            <thead className="tableHeadStyle">
                                <tr>
                                    <th scope="col" className="blue-th">#</th>
                                    <th scope="col" className="blue-th" onClick={() => handleSort('name')} >Název</th>
                                    <th scope="col" className="blue-th" onClick={() => handleSort('companyName')} >Firma</th>
                                    <th scope="col" className="blue-th" onClick={() => handleSort('description')}>Popis</th>
                                    <th scope="col" className="blue-th" onClick={() => handleSort('endsOn')} >Datum ukončení</th>
                                    <th scope="col" className="blue-th" onClick={() => handleSort('state')} >Stav</th>
                                    <th scope="col" className="blue-th" onClick={() => handleSort('createdByName')}>Vytvořil</th>
                                    <th scope="col" className="blue-th">Akce</th>
                                </tr>
                            </thead>
                            <tbody>
                                {internships.map((internship, index) => (
                                    <tr key={internship.id} onClick={() => handleRowClick(internship.id)}>
                                        <td>{index + 1}</td>
                                        <td>{internship.name || ''}</td>
                                        <td>{internship.companyName || ''}</td>
                                        <td>{internship.description.length > 20 ? internship.description.slice(0, 20) + "..." : internship.description || ''}</td>
                                        <td>{internship.endsOn ? new Date(internship.endsOn).toLocaleDateString() : ''}</td>
                                        <td>{ResolveState(internship.state)}</td>
                                        <td>{internship.createdByName || ''}</td>
                                        <td>
                                            {RenderActionButton(internship, internship.student)}
                                        </td>
                                    </tr>
                                ))}
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default Internships;