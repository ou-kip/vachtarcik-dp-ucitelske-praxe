import { useAuth } from '../Components/Exports/UseAuth'
import MainMenu from '../Components/Menu/MainMenu'
import TaskDetailForm from "../Components/Forms/Task/Detail/TaskDetailForm";
import { useSearchParams } from "react-router-dom";
import SystemMessageForm from '../Components/Forms/SystemMessageForm/SystemMessageForm';

const TaskDetail: React.FC = () => {
    const { isAuthenticated, loading } = useAuth();
    const [searchParams] = useSearchParams();
    const id = searchParams.get("taskId");

    if (!isAuthenticated) {
        return (
            <div className="main-style">
                <SystemMessageForm failMsg="" isFailed={false} successMsg="Nejste p�ihl�eni. Znovu se p�ihlaste." redirectUrl="/" />
            </div>
        )
    }

    if (loading) {
        return <div>Na��t�n�</div>;
    }

    return (
        <div>
            <MainMenu />
            <div className="main-style">
                <TaskDetailForm taskId={id} />
            </div>
        </div>
    );
};

export default TaskDetail;