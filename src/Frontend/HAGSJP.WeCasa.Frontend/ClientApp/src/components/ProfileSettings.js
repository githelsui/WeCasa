import React, { Component, useState, useEffect } from 'react';
import { NavLink, NavItem, Nav } from 'reactstrap';
import { useNavigate } from 'react-router-dom';
import { Modal, Avatar, notification, Button, Row, Col, Image, Space, Input, Form, Switch, Tabs, Divider } from 'antd';
import { UserOutlined } from '@ant-design/icons'
import * as Styles from '../styles/ConstStyles.js';
import '../styles/System.css';
import '../index.css';
import axios from 'axios';
import { useAuth } from './AuthContext';
import image1 from '../assets/profileimgs/1.jpg';
import image2 from '../assets/profileimgs/2.jpg';
import image3 from '../assets/profileimgs/3.jpg';
import image4 from '../assets/profileimgs/4.jpg';
import image5 from '../assets/profileimgs/5.jpg';
import image6 from '../assets/profileimgs/6.jpg';
import DeleteAccountModal from './DeleteAccountModal.js';
import * as ValidationFuncs from '../scripts/InputValidation.js';

export const ProfileSettings = () => {
    const { currentUser, setCurrentUser, setCurrentGroup } = useAuth();
    const images = [image1, image2, image3, image4, image5, image6];

    return (
        <div className="acc-settings-page padding">
            <h1>Edit Profile</h1>
            <Row gutter={[24, 24]} align="middle">
                <Col span={4} className="user-icon">
                    {(currentUser.image == 0 || currentUser.image == null) ?
                        (<Avatar size={64} shape="square" icon={<UserOutlined />}></Avatar>) :
                        (<Image style={{
                            borderRadius: '5%',
                            padding: '5px',
                            border: '1px solid #555',
                            objectFit: 'cover',
                            margin: '5%'
                        }}
                            src={images[currentUser.image - 1]}
                            preview={false} height="150px" width="150px" />)}
                </Col>
                <Col span={20} className="user-info">
                    <div className="padding">
                        <Form>
                            <Form.Item>
                                <Input style={Styles.inputFieldStyle} size="large" placeholder="First name" />
                            </Form.Item>
                            <Form.Item>
                                <Input style={Styles.inputFieldStyle} size="large" placeholder="Last name" />
                            </Form.Item>
                            <Button type="primary" htmlType="submit" style={Styles.primaryButtonStyle}>Save</Button>
                        </Form>
                    </div>
                </Col>
            </Row>
        </div>
    );
};

export default ProfileSettings;
