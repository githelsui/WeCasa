import { createContext, useContext, useEffect, useState } from 'react';
import axios from 'axios'

const AuthContext = createContext({
    auth: null,
    setAuth: () => {},
    currentUser: null,
});

export const useAuth = () => useContext(AuthContext);

const AuthProvider = ({ children }) => {
    const [auth, setAuth] = useState(null);
    const [currentUser, setCurrentUser] = useState(null);

    /*useEffect(() => {
        const isAuth = async () => {
            try {
                const res = axios.get(
                    'logged-user',
                    { withCredentials: true }
                );
                setCurrentUser(res.data);
            } catch (error) {
                setCurrentUser(null);
            };
        };

        isAuth();
    }, [auth]);*/

    return (
        <AuthContext.Provider value={{ auth, setAuth, currentUser }}>
            {children}
        </AuthContext.Provider>
    );
};

export default AuthProvider;