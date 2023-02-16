import React, { Component } from 'react';
import { Navigate, useLocation } from 'react-router-dom'
import { useAuth } from './AuthContext';
import { Modal, notification } from 'antd';
import axios from 'axios';
import * as Styles from '../styles/ConstStyles.js';

export const Home = () => {
    const { auth, currentUser } = useAuth();

    return (
        <div>
            {(!auth) ?
                (<Navigate to='/login'></Navigate>)
                : (
                    <div><h1>Hello, welcome to WeCasa</h1></div>)}
        </div>
    );
}; 

export default Home;