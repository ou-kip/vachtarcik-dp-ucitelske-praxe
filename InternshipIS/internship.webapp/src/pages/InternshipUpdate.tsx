import { useNavigate } from 'react-router-dom';
import '../css/Dashboard.css'
import { useAuth } from '../Components/Exports/UseAuth'
import MainMenu from '../Components/Menu/MainMenu'
import InternshipCreateUpdateForm from "../Components/Forms/Internship/InternshipCreateUpdateForm";
import { useSearchParams } from "react-router-dom";
import SystemMessageForm from '../Components/Forms/SystemMessageForm/SystemMessageForm';


const InternshipUpdate: React.FC = () => {
    const { isAuthenticated } = useAuth();
    const [searchParams] = useSearchParams();
    const internshipId = searchParams.get("internshipId");
    const navigate = useNavigate();

    const handleSuccess = (createdId: string | null) => {
        console.log('Navigace volána s ID:', createdId);
        navigate(`/internship/detail?internshipId=${createdId}`, { replace: true });
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
                <InternshipCreateUpdateForm isUpdate={true} onSuccess={handleSuccess} internshipId={internshipId} />
            </div>
        </div>
    );
};

export default InternshipUpdate;