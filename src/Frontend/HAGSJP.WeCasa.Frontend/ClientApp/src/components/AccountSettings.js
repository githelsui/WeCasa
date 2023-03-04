import React, { Component, useState, useEffect } from 'react';
import { useLocation } from 'react-router-dom';
import { Modal, ConfigProvider, Button, Row, Col, Image, Space, Input, Form, Switch, Tabs } from 'antd';
import * as Styles from '../styles/ConstStyles.js';
import '../styles/System.css';
import '../index.css';
import { useAuth } from './AuthContext';
import image1 from '../assets/profileimgs/1.jpg';
const TabPane = Tabs.TabPane;

export const AccountSettings = () => {
    const [newIcon, setNewIcon] = useState(null);
    const { currentUser } = useAuth();

    useEffect(() => { getUserInfo(); }, []);

    const getUserInfo = () => {
        
    }

    return (
        <div className="acc-settings-page padding">
            <div className="acc-settings-header padding-vertical">
                <h1>Account Settings</h1>
                <Row gutter={[24, 24]} align="middle">
                    <Col span={4} className="user-icon">
                        <Image style={{
                            borderRadius: '5%',
                            objectFit: 'cover',
                            margin: '5%'
                        }}
                        src = { image1 } preview={false} height="80px" width="80px" />
                    </Col>
                    <Col span={8} className="user-info">
                        <h2 className="padding-bottom mulish-font"><b>{currentUser["firstName"]} {currentUser["lastName"]}</b></h2>
                    </Col>
                </Row>
            </div>
        </div>
    );
};

export default AccountSettings;
