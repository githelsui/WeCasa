import React, { Component, useState, useEffect } from 'react';
import { Navigate, useLocation, useNavigate } from 'react-router-dom'
import { useAuth } from './AuthContext';
import { Modal, notification } from 'antd';
import { Groups } from './Groups.js';
import { NavMenu } from './NavMenu.js';
import axios from 'axios';
import * as Styles from '../styles/ConstStyles.js';

export const Home = () => {
    const { auth, currentUser, currentGroup, setCurrentGroup } = useAuth();
    const navigate = useNavigate();

    useEffect(() => {
        if (!auth) navigate('/login');
        }, []);


    const updateGroup = (newGroup) => {
        setCurrentGroup(newGroup);
    }

    return (
        <div>
            {(auth && !currentGroup==null) ?
                (<div>
                    <NavMenu />
                </div>) : (<Groups user={currentUser} selected={updateGroup} />)}
        </div>
    );
}; 

export default Home;