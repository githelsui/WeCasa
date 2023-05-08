import React, { Component, useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { Modal, Avatar, notification, Button, Row, Col, Image, Space, Input, Form, Switch, Tabs, Divider } from 'antd';
import * as Styles from '../../styles/ConstStyles.js';
import '../../styles/System.css';
import '../../index.css';
import axios from 'axios';
import OTPInput from '../OTPInput.js';
import * as ValidationFuncs from '../../scripts/InputValidation.js';
import defaultImage from '../../assets/defaultimgs/wecasatemp.jpg';
const TabPane = Tabs.TabPane;

export const AccountRecovery = () => {
    const [username, setUsername] = useState('');
    const [inputFailures, setInputFailures] = useState(false);
    const [recoveryProcess, setRecoveryProcess] = useState(null);
    const navigate = useNavigate();

    useEffect(() => { setRecoveryProcess(1); }, []);


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

    const arrEquals = (a, b) => {
        if (a === b) return true;
        if (a == null || b == null) return false;
        if (a.length !== b.length) return false;

        for (var i = 0; i < a.length; ++i) {
            if (a[i] !== b[i]) return false;
        }
        return true;
    }

    const submitEmailForm = (values) => {
        setUsername(values.email1);
        let account = {
            Email: username
        }
        axios.post('account-recovery/GetOTPCode', account)
            .then(res => {
                var isSuccessful = res.data['isSuccessful'];
                if (isSuccessful) {
                    setRecoveryProcess(2);
                } else {
                    // username does not exist
                    failureAccountRecoveryView(res.data['message']);
                }
            })
            .catch((error => { console.error(error) }));
    }

    const submitOTPForm = (values) => {
        let account = {
            Email: username,
            OTP: values
        }
        console.log("Verifying OTP...",account);
        axios.post('account-recovery/VerifyOTPCode', account)
            .then(res => {
                var isSuccessful = res.data['isSuccessful'];
                if (isSuccessful) {
                    setRecoveryProcess(3);
                    successAccountRecoveryView(res.data['message']);
                } else {
                    failureAccountRecoveryView(res.data['message']);
                }
            })
            .catch((error => { console.error(error) }));
    }

    const submitPwUpdateForm = (values) => {
        let failureMessage = '';
        if (values.password1 != values.password2) {
            setInputFailures(true);
            failureMessage += 'Passwords must match.'
            failureAccountRecoveryView(failureMessage);
        }

        let account = {
            Email: username,
            NewField: values.password1,
        }

        axios.post('account-recovery/UpdatePasswordSecured', account)
            .then(res => {
                var isSuccessful = res.data['isSuccessful'];
                if (isSuccessful) {
                    successAccountRecoveryView(res.data['message']);
                    navigate('/login'); // returning to login page
                } else {
                    failureAccountRecoveryView(res.data['message']);
                }
            })
            .catch((error => { console.error(error) }));
    }


    const successAccountRecoveryView = (successMessage) => {
        notification.open({
            message: successMessage,
            duration: 10,
            placement: "topLeft",
        });
    }

    const failureAccountRecoveryView = (failureMessage) => {
        // Accounts for user failure cases and system errors
        notification.open({
            message: "An error occurred.",
            description: failureMessage,
            duration: 10,
            placement: "topLeft",
        });
    }

    const accountRecovery = () => {
        switch (recoveryProcess) {
            case 1:
                return (
                    <div id="get-email">
                        <Form id="emailForm" onFinish={(values) => submitEmailForm(values)}>
                            <h6 className="padding-top mulish-font">Type in your email address associated with your account</h6>
                            <Form.Item name="email1" hasFeedback style={{ marginBottom: 15 }} rules={[{ required: true, message: '' }, () => ({
                                validator(rule, value) {
                                    let valid = ValidationFuncs.validateEmail(value);
                                    if (!value || valid.isSuccessful) {
                                        return Promise.resolve();
                                    }
                                    return Promise.reject(valid.message);
                                },
                            })]}>
                                <Input style={Styles.inputFieldStyle} placeholder="Email" />
                            </Form.Item>
                            <Row gutter={24} style={{ alignItems: 'center', justifyContent: 'center', gap: '30px' }} >
                                <Button key="create" htmlType="submit" type="primary" style={Styles.primaryButtonModal}>Send Code</Button>
                            </Row>
                        </Form>
                    </div>
                );
            case 2: 
                return (
                    <div id="verifyOTP">
                        <h6>Provide the verification code from your inbox to reset your password</h6>
                        <OTPInput submit={submitOTPForm} cancel={() => setRecoveryProcess(1)}/>
                    </div>
                );
            case 3: 
                return (
                    <div id="updating-pw">
                        <Form id="pwUpdateForm" onFinish={(values) => submitPwUpdateForm(values)}>
                            <h6 className="padding-top mulish-font">Type in your new password</h6>
                            <Form.Item name="password1" style={{ marginBottom: 15 }} hasFeedback rules={[{ required: true, message: '' }, () => ({
                                validator(rule, value) {
                                    let valid = ValidationFuncs.validatePassword(value);
                                    if (!value || valid.isSuccessful) {
                                        return Promise.resolve();
                                    }
                                    return Promise.reject(valid.message);
                                },
                            })]}>
                                <Input.Password style={Styles.inputFieldStyle} placeholder="Password" />
                            </Form.Item>
                            <h6 className="mulish-font">Re-type password</h6>

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
                    </div>);
            default:
                return (<div></div>);
        }
    }

    return (
        <div id="acc-recovery">
            <Row gutter={[48, 48]} align="middle">
                <Col span={12} style={{ fontFamily: 'Mulish' }}>
                    <div>
                        <h1>Reset Password</h1>
                        {accountRecovery()}
                    </div>
                </Col>
                <Col span={12}>
                    <Image style={Styles.defaultImage} src={defaultImage} preview={false} />
                </Col>
            </Row>
        </div>        
    );
}

export default AccountRecovery;
