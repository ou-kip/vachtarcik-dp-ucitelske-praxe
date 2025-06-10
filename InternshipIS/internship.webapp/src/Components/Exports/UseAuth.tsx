import { useContext } from 'react';
import { AuthContext } from '../AuthProvider';

export const useAuth = () => {
    const context = useContext(AuthContext);
    if (!context) {
        throw new Error('useAuth mus� b�t pou�ito!');
    }

    const isLoggedIn = context.isAuthenticated && context.role !== null;

    return {
        isAuthenticated: isLoggedIn,
        role: context.role,
        loading: context.loading,
        fullName: context.fullName
    };
};