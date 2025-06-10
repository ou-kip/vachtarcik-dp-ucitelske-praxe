import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import './App.css'
import Dashboard from "./pages/Dashboard";
import Admin from "./pages/Admin";
import Index from "./pages/Index";
import { AuthProvider } from './Components/AuthProvider';
import ResetPassword from './pages/ResetPassword';
import ChangePassword from './pages/ChangePassword';
import InternshipCreate from './pages/InternshipCreate';
import Internships from './pages/Internships';
import InternshipUpdate from './pages/InternshipUpdate';
import InternshipDetail from './pages/InternshipDetail';
import TaskDetail from './pages/TaskDetail';
import Register from './pages/Register';

function App() {

    return (
        <Router>
            <>
                <AuthProvider>
                    <Routes>
                        <Route path="/" element={<Index />} />
                        <Route path="/dashboard" element={<Dashboard />} />
                        <Route path="/internship/create" element={<InternshipCreate />} />
                        <Route path="/internship/edit" element={<InternshipUpdate />} />
                        <Route path="/admin" element={<Admin />} />
                        <Route path="/reset-password" element={<ResetPassword />} />
                        <Route path="/change-password" element={<ChangePassword />} />
                        <Route path="/internships" element={<Internships />} />
                        <Route path="/internship/detail" element={<InternshipDetail />} />
                        <Route path="/task/detail" element={<TaskDetail />} />
                        <Route path="/register" element={<Register />} />
                        <Route path="/register-confirm" element={<Register />} />
                    </Routes>
                </AuthProvider>
            </>
        </Router>
    );
}

export default App
