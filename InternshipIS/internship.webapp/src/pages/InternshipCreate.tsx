import { useNavigate } from 'react-router-dom';
import { useAuth } from '../Components/Exports/UseAuth'
import MainMenu from '../Components/Menu/MainMenu'
import InternshipCreateUpdateForm from "../Components/Forms/Internship/InternshipCreateUpdateForm";
import InternshipTasksForm from "../Components/Forms/Internship/InternshipTasksForm";
import { useSearchParams } from "react-router-dom";
import SystemMessageForm from '../Components/Forms/SystemMessageForm/SystemMessageForm';


const InternshipCreate: React.FC = () => {
    const { isAuthenticated } = useAuth();
    const [searchParams] = useSearchParams();
    let id = searchParams.get("id");

    const navigate = useNavigate();

    const handleSuccess = (createdId: string | null) => {
        console.log('Navigace volána s ID:', createdId);
        navigate(`/internship/create?id=${createdId}`, { replace: true });

        id = searchParams.get("id");
    };

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
                {id ? (<InternshipTasksForm internshipId={id} isUpdate={false} />) : (<InternshipCreateUpdateForm isUpdate={false} onSuccess={handleSuccess} internshipId = {null} />) }
            </div>
        </div>
    );
};

export default InternshipCreate;