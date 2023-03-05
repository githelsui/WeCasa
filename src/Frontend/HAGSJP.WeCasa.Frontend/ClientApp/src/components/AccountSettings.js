import React, { Component, useState, useEffect } from 'react';
import { NavLink, NavItem, Nav } from 'reactstrap';
import { Modal, ConfigProvider, Button, Row, Col, Image, Space, Input, Form, Switch, Tabs, Divider } from 'antd';
import * as Styles from '../styles/ConstStyles.js';
import '../styles/System.css';
import '../index.css';
import { useAuth } from './AuthContext';
import image1 from '../assets/profileimgs/1.jpg';
import DeleteAccountModal from './DeleteAccountModal.js';
import * as ValidationFuncs from '../scripts/InputValidation.js';
const TabPane = Tabs.TabPane;

export const AccountSettings = () => {
    const [newIcon, setNewIcon] = useState(null);
    const [showModal, setShowModal] = useState(false);
    const [setting, setCurrentSetting] = useState(null);
    const { currentUser } = useAuth();

    useEffect(() => { setCurrentSetting('Update Email'); }, []);

    const validateEmail = (value, callback) => {
        if (value != undefined && value != null) {
            var ruleResult = ValidationFuncs.validateEmail(value)
            if (ruleResult.isSuccessful) {
                callback();
            } else {
                callback(ruleResult.message);
            }
        } else {
            callback('');
        }
    };

    const validatePassword = (value, callback) => {
        if (value != undefined && value != null && value.length > 0) {
            var ruleResult = ValidationFuncs.validatePassword(value)
            if (ruleResult.isSuccessful) {
                callback();
            } else {
                callback(ruleResult.message);
            }
        } else {
            callback('');
        }
    };

    const submitEmailUpdateForm = () => {

    }

    const submitPwUpdateForm = () => {

    }

    const submitNotifSettingsForm = () => {

    }

    const displaySettings = () => {
        switch (setting) {
            case 'Update Email':
                return (
                    <div id="updating-email">
                        <h2>{setting}</h2>
                        <Form id="emailUpdateForm" onFinish={(values) => submitEmailUpdateForm(values)}>

                            <Form.Item name="email1" style={{ marginBottom: 15 }}>
                                <Input style={Styles.inputFieldStyle} placeholder="Email" />
                            </Form.Item>

                            <Form.Item name="email2">
                                <Input.Password style={Styles.inputFieldStyle} placeholder="Confirm Email" />
                            </Form.Item>

                            <Button style={Styles.primaryButtonStyle} type="primary" htmlType="submit">Submit</Button>
                        </Form>
                    </div>
                );
            case 'Update Password':
                return (
                    <div id="updating-pw">
                        <h2>{setting}</h2>
                        <Form id="pwUpdateForm" onFinish={(values) => submitPwUpdateForm(values)}>

                            <Form.Item name="password1" style={{ marginBottom: 15 }}>
                                <Input style={Styles.inputFieldStyle} placeholder="Password" />
                            </Form.Item>

                            <Form.Item name="password2">
                                <Input.Password style={Styles.inputFieldStyle} placeholder="Confirm Password" />
                            </Form.Item>

                            <Button style={Styles.primaryButtonStyle} type="primary" htmlType="submit">Submit</Button>
                        </Form>
                    </div>
                );
            case 'Notifications':
                return (
                    <div id="notification-settings">
                        <h2>{setting}</h2>
                        <Form id="notifSettingsForm" onFinish={(values) => submitNotifSettingsForm(values)}>
                            <h5>Get notifications to find out what's going on when you're offline.</h5>
                            <Row gutter={24} style={Styles.groupFeatureContainer} align="middle">
                                <Col span={14}>
                                    <h6 className="mulish-font">Email</h6>
                                </Col>
                                <Col span={6}>
                                    <Form.Item name="email" valuePropName="checked" initialValue={(currentUser.notifications.includes("email")) ? true : false}>
                                        <Switch defaultChecked="false" checkedChildren="ON" unCheckedChildren="OFF" />
                                    </Form.Item>
                                </Col>
                            </Row>
                            <Row gutter={24} style={Styles.groupFeatureContainer} align="middle">
                                <Col span={14}>
                                    <h6 className="mulish-font">Email</h6>
                                </Col>
                                <Col span={6}>
                                    <Form.Item name="sms" valuePropName="checked" initialValue={(currentUser.notifications.includes("sms")) ? true : false}>
                                        <Switch defaultChecked="false" checkedChildren="ON" unCheckedChildren="OFF" />
                                    </Form.Item>
                                </Col>
                            </Row>
                        </Form>
                    </div>
                );
            default: return (<div></div>);
        }
    }

    const attemptDeleteAcc = () => {

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
                        <h2 className="padding-top mulish-font"><b>{currentUser["firstName"]} {currentUser["lastName"]}</b></h2>
                        <h5 className="padding-bottom mulish-font" style={{color:"gray"}}><b>{currentUser["username"]}</b></h5>
                    </Col>
                </Row>
                <Row gutter={[24, 24]} align="left">
                    <Col span={6} className="navbar-nav padding-top mulish-font">
                        <Nav vertical>
                            <NavItem>
                                <NavLink onClick={()=>setCurrentSetting('Update Email')}>
                                    Update Email
                                </NavLink>
                            </NavItem>
                            <NavItem>
                                <NavLink onClick={() => setCurrentSetting('Update Password')}>
                                    Update Password
                                </NavLink>
                            </NavItem>
                            <NavItem>
                                <NavLink>
                                    Edit Profile
                                </NavLink>
                            </NavItem>
                            <NavItem>
                                <NavLink onClick={() => setCurrentSetting('Notifications')}>
                                    Notifications
                                </NavLink>
                            </NavItem>
                            <NavItem>
                                <NavLink onClick={() => setCurrentSetting('Data Export')}>
                                    Data Export
                                </NavLink>
                            </NavItem>
                            <Divider plain>
                            </Divider>
                            <NavItem>
                                <NavLink onClick={() => setShowModal(true)}>
                                    Delete Account
                                </NavLink>
                            </NavItem>
                        </Nav>
                    </Col>
                    <Col span={3} className="padding-top">
                        <Divider
                            type="vertical" style={{ height: "100%", color:"black" }}>
                        </Divider>
                    </Col>
                    <Col span={13} className="padding-top mulish-font">
                        {displaySettings()}
                    </Col>
                </Row>
                <DeleteAccountModal show={showModal} close={() => setShowModal(false)} confirm={attemptDeleteAcc} />
            </div>
        </div>
    );
};

export default AccountSettings;
