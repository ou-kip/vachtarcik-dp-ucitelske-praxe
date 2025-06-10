import React, { createContext, useEffect, useState, ReactNode } from 'react';
import { useLocation } from 'react-router-dom';
import AuthApi from './Exports/AuthApi';

interface AuthContextType {
    isAuthenticated: boolean;
    loading: boolean;
    role: string | null;
    fullName: string | null;
}

interface AuthProviderProps {
    children: ReactNode;
}

export const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider: React.FC<AuthProviderProps> = ({ children }) => {

    const location = useLocation();
    const [isAuthenticated, setIsAuthenticated] = useState(false);
    const [loading, setLoading] = useState(true);
    const [role, setRole] = useState<string | null>(null);
    const [fullName, setFullName] = useState<string | null>(null);

    const publicRoutes = ['/', '/index'];

    useEffect(() => {
        const checkAuth = async () => {
            try {
                const response = await AuthApi.get('/api/v1/auth/role/getuser');
                const userRole = response.data.role;
                const userFullName = response.data.fullName;

                if (userRole) {
                    setRole(userRole);
                    setIsAuthenticated(true);
                } else {
                    setIsAuthenticated(false);
                }

                if (userFullName) {
                    setFullName(userFullName);
                    setIsAuthenticated(true);
                } else {
                    setIsAuthenticated(false);
                }

            } catch {
                setIsAuthenticated(false);
            } finally {
                setLoading(false);
            }
        };

        checkAuth();
    }, [location, publicRoutes]);

    return (
        <AuthContext.Provider value={{ isAuthenticated, loading, role, fullName }}>
            {children}
        </AuthContext.Provider>
    );
};