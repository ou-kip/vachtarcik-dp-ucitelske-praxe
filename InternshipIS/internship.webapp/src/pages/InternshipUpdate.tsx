import { useNavigate } from 'react-router-dom';
import '../css/Dashboard.css'
import { useAuth } from '../Components/Exports/UseAuth'
import MainMenu from '../Components/Menu/MainMenu'
import InternshipCreateUpdateForm from "../Components/Forms/Internship/InternshipCreateUpdateForm";
import InternshipTasksForm from "../Components/Forms/Internship/InternshipTasksForm";
import { useSearchParams } from "react-router-dom";
import SystemMessageForm from '../Components/Forms/SystemMessageForm/SystemMessageForm';


const InternshipUpdate: React.FC = () => {
    const { isAuthenticated } = useAuth();
    const [searchParams] = useSearchParams();
    let id = searchParams.get("id");
    const internshipId = searchParams.get("internshipId");

    const navigate = useNavigate();

    const handleSuccess = (createdId: string | null) => {
        console.log('Navigace volána s ID:', createdId);
        navigate(`/internship/edit?id=${createdId}`, { replace: true });

        id = searchParams.get("id");
    };

    if (!isAuthenticated) {
        return (
            <div className="main-style">
                <SystemMessageForm failMsg="" isFailed={false} successMsg="Nejste pøihlášeni. Znovu se pøihlaste." redirectUrl="/" />
            </div>
        )
    }

    return (
        <div>
            <MainMenu />
            <div className="main-style">
                {id ? (<InternshipTasksForm internshipId={id} isUpdate={true} />) : (<InternshipCreateUpdateForm isUpdate={true} onSuccess={handleSuccess} internshipId={internshipId} />)}
            </div>
        </div>
    );
};

export default InternshipUpdate;