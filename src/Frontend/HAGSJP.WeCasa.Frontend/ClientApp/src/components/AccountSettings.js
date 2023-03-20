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
const TabPane = Tabs.TabPane;

export const AccountSettings = () => {
    const [newIcon, setNewIcon] = useState(null);
    const [showModal, setShowModal] = useState(false);
    const [setting, setCurrentSetting] = useState(null);
    const [inputFailures, setInputFailures] = useState(false);
    const { currentUser, setCurrentUser, setCurrentGroup } = useAuth();
    const navigate = useNavigate();
    const images = [image1, image2, image3, image4, image5, image6];

    useEffect(() => { setCurrentSetting('Update Email'); setCurrentGroup(null); }, []);

    const validateEmail = (value, callback) => {
        if (value != undefined && value != null) {
            var ruleResult = ValidationFuncs.validateEmail(String(value))
            if (ruleResult.isSuccessful) {
                return Promise.resolve()
            } else {
                return Promise.reject(ruleResult.message);
            }
        } else {
            return Promise.resolve('');
        }
    };

    const validatePassword = (value, callback) => {
        if (value != undefined && value != null && value.length > 0) {
            var ruleResult = ValidationFuncs.validatePassword(value)
            if (ruleResult.isSuccessful) {
                return Promise.resolve()
            } else {
                return Promise.reject(ruleResult.message);
            }
        } else {
            return Promise.resolve('');
        }
    };

    // https://stackoverflow.com/questions/3115982/how-to-check-if-two-arrays-are-equal-with-javascript
    const arrEquals = (a, b) => {
        if (a === b) return true;
        if (a == null || b == null) return false;
        if (a.length !== b.length) return false;

        for (var i = 0; i < a.length; ++i) {
            if (a[i] !== b[i]) return false;
        }
        return true;
    }

    const submitEmailUpdateForm = (values) => {
        let updatedUser = {
            Email: currentUser['username'],
            NewField: values.email2
        }

        axios.post('account-settings/UpdateEmail', updatedUser)
            .then(res => {
                var isSuccessful = res.data['isSuccessful'];
                if (isSuccessful) {
                    successAccountSettingsView(res.data['message']);
                } else {
                    failureAccountSettingsView(res.data['message']);
                }
            })
            .catch((error => { console.error(error) }));
    }

    const submitPwUpdateForm = (values) => {
        let failureMessage = '';
        if (values.password1 != values.password2) {
            setInputFailures(true);
            failureMessage += 'Passwords must match.'
            failureAccountSettingsView(failureMessage);
        }


        let updatedUser = {
            Email: currentUser['username'],
            Password: values.password1,
            NewField: values.password2
        }

        // TODO: check that user entered in password correctly

        axios.post('account-settings/UpdatePassword', updatedUser)
            .then(res => {
                var isSuccessful = res.data['isSuccessful'];
                if (isSuccessful) {
                    successAccountSettingsView(res.data['message']);
                } else {
                    failureAccountSettingsView(res.data['message']);
                }
            })
            .catch((error => { console.error(error) }));
    }


    const submitNotifSettingsForm = (values) => {
        let newNotifs = [];
        if (values["email"]) newNotifs.push("email");
        if (values["sms"]) newNotifs.push("sms");

        let updatedUser = {
            Email: currentUser["username"],
            Notifications: newNotifs
        }

        if (arrEquals(newNotifs, currentUser.notifications)) return;

        axios.post('account-settings/UpdateNotifications', updatedUser)
            .then(res => {
                var isSuccessful = res.data['isSuccessful'];
                if (isSuccessful) {
                    //setCurrentUser(updatedUser);
                    successAccountSettingsView(res.data['message']);
                } else {
                    failureAccountSettingsView(res.data['message']);
                }
            })
            .catch((error => { console.error(error) }));
    }

    const attemptDeleteAcc = () => {
        axios.post('account-settings/DeleteAccount', currentUser)
            .then(res => {
                var isSuccessful = res.data['isSuccessful'];
                if (isSuccessful) {
                    // Resetting auth context
                    setCurrentUser(null);
                    // Navigating back to registration
                    successAccountSettingsView(res.data['message']);
                    navigate('/');
                } else {
                    failureAccountSettingsView(res.data['message']);
                }
            })
            .catch((error => { console.error(error) }));
    }

    const successAccountSettingsView = (successMessage) => {
        notification.open({
            message: "Update successful.",
            description: successMessage,
            duration: 10,
            placement: "topLeft",
        });
    }

    const failureAccountSettingsView = (failureMessage) => {
        // Accounts for user failure cases and system errors
        notification.open({
            message: "An error occurred.",
            description: failureMessage,
            duration: 10,
            placement: "topLeft",
        });
    }

    const displaySettings = () => {
        switch (setting) {
            case 'Update Email':
                return (
                    <div id="updating-email">
                        <h2>{setting}</h2>
                        <Form id="emailUpdateForm" onFinish={(values) => submitEmailUpdateForm(values)}>

                            <Form.Item name="email1" hasFeedback style={{ marginBottom: 15 }} rules={[{ validator: validateEmail }]}>
                                <Input style={Styles.inputFieldStyle} placeholder="Email" />
                            </Form.Item>

                            <Form.Item name="email2" hasFeedback rules={[{ required: true, message: '' }, ({ getFieldValue }) => ({
                                validator(rule, value) {
                                    if (!value || getFieldValue('email1') === value) {
                                        return Promise.resolve();
                                    }
                                    return Promise.reject('Emails must match');
                                },
                            })]}>
                                <Input style={Styles.inputFieldStyle} placeholder="Confirm Email" />
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

                            <Form.Item name="password1" style={{ marginBottom: 15 }} hasFeedback rules={[{ validator: validatePassword }]}>
                                <Input.Password style={Styles.inputFieldStyle} placeholder="Password" />
                            </Form.Item>

                            <Form.Item name="password2" hasFeedback rules={[{ required: true, message: '' }, ({ getFieldValue }) => ({
                                validator(rule, value) {
                                    if (!value || getFieldValue('password1') === value) {
                                        return Promise.resolve();
                                    }
                                    return Promise.reject('Passwords must match');
                                },
                            })]}>
                                <Input.Password style={Styles.inputFieldStyle} placeholder="Confirm Password" />
                            </Form.Item>

                            <Button style={Styles.primaryButtonStyle} type="primary" htmlType="submit">Submit</Button>
                        </Form>
                    </div>
                );
            case 'Notifications':
                return (
                    <div id="notification-settings" className="mulish-font">
                        <h2>{setting}</h2>
                        <Form id="notifSettingsForm" onFinish={(values) => submitNotifSettingsForm(values)}>
                            <h6 className="mulish-font" style={{color: "gray"}}>Get notifications to find out what's going on when you're offline.</h6>
                            <Row gutter={24} align="middle">
                                <Col span={6} className="padding-top">
                                    <Form.Item name="email" valuePropName="checked" initialValue={(currentUser.notifications.includes("email")) ? true : false}>
                                        <Switch defaultChecked="false" checkedChildren="ON" unCheckedChildren="OFF" />
                                    </Form.Item>
                                </Col>
                                <Col span={14} className="align-left padding-top">
                                    <h5 className="mulish-font">Email</h5>
                                    <h6 className="padding-bottom mulish-font" style={{ color: "gray" }}><b>Receive updates through email</b></h6>
                                </Col>
                            </Row>
                            <Row gutter={24} align="middle">
                                <Col span={6} className="padding-top">
                                    <Form.Item name="sms" valuePropName="checked" initialValue={(currentUser.notifications.includes("sms")) ? true : false}>
                                        <Switch defaultChecked="false" checkedChildren="ON" unCheckedChildren="OFF" />
                                    </Form.Item>
                                </Col>
                                <Col span={14} className="align-left padding-top">
                                    <h5 className="mulish-font">SMS</h5>
                                    <h6 className="padding-bottom mulish-font" style={{ color: "gray" }}><b>Receive updates through your phone</b></h6>
                                </Col>
                            </Row>
                            <div className="save-acc-settings padding-top">
                                <Button key="create" type="primary" htmlType="submit" style={Styles.saveButtonStyle}>Save</Button>
                            </div>
                        </Form>
                    </div>
                );
            default: return (<div></div>);
        }
    }

    return (
        <div className="acc-settings-page padding">
            <div className="acc-settings-header padding-vertical">
                <h1>Account Settings</h1>
                <Row gutter={[24, 24]} align="middle">
                    <Col span={4} className="user-icon">
                        {(currentUser.image == 0) ?
                            (<Avatar size={64} icon={<UserOutlined />}></Avatar>) :
                            (<Image style={{
                                borderRadius: '5%',
                                objectFit: 'cover',
                                margin: '5%'
                            }}
                                src={images[currentUser.image - 1]}
                                preview={false} height="80px" width="80px" />)}
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
                                <NavLink onClick={() => navigate('/edit-profile')}>
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
                                <NavLink style={{ color:"#ff2929" }} onClick={() => setShowModal(true)}>
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
