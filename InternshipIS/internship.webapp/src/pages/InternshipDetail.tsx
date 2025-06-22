import { useAuth } from '../Components/Exports/UseAuth'
import MainMenu from '../Components/Menu/MainMenu'
/*import InternshipDetailForm from "../Components/Forms/Internship/Detail/InternshipDetailForm";*/
import { useSearchParams } from "react-router-dom";
import SystemMessageForm from '../Components/Forms/SystemMessageForm/SystemMessageForm';
import InternshipOverview from '../Components/Forms/Internship/Overview/InternshipOverview';

const InternshipDetail: React.FC = () => {
    const { isAuthenticated } = useAuth();
    const [searchParams] = useSearchParams();
    const id = searchParams.get("internshipId");

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
            <div className="main-dashboard">
                <InternshipOverview internshipId={id} />
            </div>
        </div>
    );
};

export default InternshipDetail;