import { useNavigate } from 'react-router-dom';
import { useAuth } from '../Components/Exports/UseAuth'
import MainMenu from '../Components/Menu/MainMenu'
import InternshipCreateUpdateForm from "../Components/Forms/Internship/InternshipCreateUpdateForm";
import SystemMessageForm from '../Components/Forms/SystemMessageForm/SystemMessageForm';


const InternshipCreate: React.FC = () => {
    const { isAuthenticated } = useAuth();
    const navigate = useNavigate();

    const handleSuccess = (createdId: string | null) => {
        console.log('Navigace volána s ID:', createdId);
        navigate(`/internship/detail?internshipId=${createdId}`, { replace: true });
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
                <InternshipCreateUpdateForm isUpdate={false} onSuccess={handleSuccess} internshipId = {null} />
            </div>
        </div>
    );
};

export default InternshipCreate;