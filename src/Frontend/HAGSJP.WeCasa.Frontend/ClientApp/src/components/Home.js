import React, { Component, useState, useEffect } from 'react';
import { Navigate, useLocation, useNavigate } from 'react-router-dom'
import { useAuth } from './Auth/AuthContext';
import { Modal, notification } from 'antd';
import { Groups } from './Group/Groups.js';
import { NavMenu } from './NavMenu.js';
import { AdminNavMenu } from './AdminNavMenu.js';
import axios from 'axios';
import * as Styles from '../styles/ConstStyles.js';

export const Home = () => {
    const { auth, currentUser, currentGroup, setCurrentGroup } = useAuth();
    const navigate = useNavigate();
    const [isAdmin, setIsAdmin] = useState(false);

    const updateGroup = (newGroup) => {
        setCurrentGroup(newGroup);
    }

    const adminPrivileges = () => {

    }

    useEffect(() => {
        //DEVELOPMENT ONLY
        setIsAdmin(true)

        adminPrivileges();
    }, [])

    return (
        <div>
            <div>
                {isAdmin ? (<div><AdminNavMenu/></div>) : (<div></div>)}
            </div>
            <div> {(auth && !currentGroup == null) ?
                (<div>
                    <NavMenu />
                </div>) : (<Groups user={currentUser} selected={updateGroup} />)}
            </div>
        </div>
    );
}; 

export default Home;