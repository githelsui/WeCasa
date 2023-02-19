import React, { Component } from 'react';
import { Navigate, useLocation } from 'react-router-dom'
import { useAuth } from './AuthContext';
import { Modal, notification } from 'antd';
import { Groups } from './Groups.js'
import axios from 'axios';
import * as Styles from '../styles/ConstStyles.js';

export const Home = () => {
    const { auth, currentUser } = useAuth();

    return (
        <div>
            {(!auth) ?
                (<Navigate to='/login'></Navigate>)
                : (
                    <div>
                        <div><h2 align={"center"}>Home</h2></div>
                        <Groups />
                    </div>)}
        </div>
    );
}; 

export default Home;