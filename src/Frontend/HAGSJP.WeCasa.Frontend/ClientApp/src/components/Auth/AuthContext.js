import { createContext, useContext, useEffect, useState } from 'react';
import axios from 'axios'

// Reference: https://stackoverflow.com/questions/71960194/update-navbar-after-success-login-or-logout-redirection

const AuthContext = createContext({
    auth: null,
    setAuth: () => { },
    admin: false,
    setAdmin: () => { },
    currentUser: null,
    setCurrentUser: () => { },
    setCurrentGroup: null,
    setCurrentGroup: () => { }
});

export const useAuth = () => useContext(AuthContext);

const AuthProvider = ({ children }) => {
    const [auth, setAuth] = useState(null);
    const [admin, setAdmin] = useState(true);
    const [currentUser, setCurrentUser] = useState(null);
    const [currentGroup, setCurrentGroup] = useState(null);

    return (
        <AuthContext.Provider value={{ auth, setAuth, admin, setAdmin, currentUser, setCurrentUser, currentGroup, setCurrentGroup }}>
            {children}
        </AuthContext.Provider>
    );
};

export default AuthProvider;