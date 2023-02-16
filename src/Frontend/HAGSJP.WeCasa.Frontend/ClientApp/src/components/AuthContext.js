import { createContext, useContext, useEffect, useState } from 'react';
import axios from 'axios'

const AuthContext = createContext({
    auth: null,
    setAuth: () => {},
    currentUser: null,
    setCurrentUser: () => {}
});

export const useAuth = () => useContext(AuthContext);

const AuthProvider = ({ children }) => {
    const [auth, setAuth] = useState(null);
    const [currentUser, setCurrentUser] = useState(null);

    return (
        <AuthContext.Provider value={{ auth, setAuth, currentUser, setCurrentUser }}>
            {children}
        </AuthContext.Provider>
    );
};

export default AuthProvider;