import React, { Component, useState, useEffect } from 'react';
import { Image, Row, Col } from 'antd';
import { useAuth } from "./Auth/AuthContext";
import axios from 'axios';
import * as Styles from '../styles/ConstStyles.js';
import image4 from '../assets/profileimgs/4.jpg';

export const NotFound = () => {
    const { setAuth, setCurrentUser, setCurrentGroup } = useAuth();
    setAuth(false);
    setCurrentUser(null);
    setCurrentGroup(null);

    return (
        <div className='padding-bottom'>
            <Row gutter={24}>
                <Col span={8}>
                    <Image src={image4} preview={false} />
                </Col>
            </Row>
            <h2 style={{fontFamily:'Mulish'}}>Oops! The requested page or file does not exist</h2>
        </div>
    );
}; 

export default NotFound;