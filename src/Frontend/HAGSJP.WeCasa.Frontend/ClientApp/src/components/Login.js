import React, { Component, useState, useEffect } from 'react';
import { Form, Input, Button, notification, ConfigProvider, Row, Gutter, Col, Image} from 'antd';
import { withRouter, useNavigate, Navigate, Link } from 'react-router-dom';
import { useAuth } from './AuthContext';
import axios from 'axios';
import * as Styles from '../styles/ConstStyles.js';
import defaultImage from '../assets/defaultimgs/wecasatemp.jpg';

export const resetPassBtnStyle = {
    fontFamily: 'Mulish',
    width: '100%',
    fontWeight: 'thin',
    borderWidth: 'thin'
}

export const Login = () => {
    const { setAuth, setCurrentUser } = useAuth();
    const navigate = useNavigate();

    const [loginResults, setLoginResults] = useState(false);
    const [inputFailures, setInputFailures] = useState(false);
    const [loginProcess, setLoginProcess] = useState(1);
    const [loading, setLoading] = useState(true);
    const [otpCode, setOtpCode] = useState('');
    const [otpExpiration, setOtpExp] = useState('');
    const [account, setAccount] = useState('');

    const submitLoginForm = (values) => {
        setInputFailures(false);
        var failureMessage = '';

        let userAccount = {
            Username: values.email,
            Password: values.password
        };

        // -- Client-side Input Validation

        // Blank User Inputs
        for (let key in userAccount) {
            if (userAccount[key] == null) {
                setInputFailures(true);
                failureMessage = 'Empty fields are not accepted.'
                break;
            }
        }

        if (!inputFailures) {
            attemptInitialLogin(userAccount)
        } else {
            failureLoginView(failureMessage);
        }
    };

    // Login Process 1: Prior to receiving an OTP
    const attemptInitialLogin = (userAccount) => {
        if (!inputFailures) {

            setLoginProcess(1);

            axios.post('login/AttemptLogin', userAccount)
                .then(res => {
                    var isSuccessful = res.data['isSuccessful'];
                    if (isSuccessful) {
                        setLoginProcess(2);
                        getNewOTP(userAccount);
                    } else {
                        failureLoginView(res.data['message']);
                    }
                })
                .catch((error) => { console.error(error) });
        }
    };

    // Login Process 2: Receive new OTP and submit OTP
    const getNewOTP = (userAccount) => {
        axios.post('login/GetOTP', userAccount,
            { withCredentials: true })
            .then(res => {
                var otpRes = res.data;
                setAccount(otpRes['username']);
                setOtpCode(otpRes['code']);
                setOtpExp(otpRes['expirationTime']);
                //this.forceUpdate();
            })
            .catch((error) => { console.error(error) });

    };

    const submitOTPForm = (values) => {
        let otpData = {
            Username: account,
            Password: values.otp
        };
        if (values.otp != null) {
            axios.post('login/LoginWithOTP', otpData)
                .then(res => {
                    var isSuccessful = res.data['isSuccessful'];
                    if (isSuccessful) {
                        console.log(res.data)
                        setLoginResults(true);
                        setAuth(true);
                        setCurrentUser(res.data.returnedObject);
                        successLoginView();
                    } else {
                        failureLoginView(res.data['message']);
                    }
                })
                .catch((error) => { console.error(error) });
        } else {
            failureLoginView('Empty fields not accepted.')
        }
    }

    const failureLoginView = (failureMessage) => {
        // Accounts for user failure cases and system errors
        setLoginResults(false)
        notification.open({
            message: "Try again.",
            description: failureMessage,
            duration: 10,
            placement: "topLeft",
        });
    }

    const successLoginView = () => {
        console.log("Successful login");
        navigate('/home', { replace:true});
        //this.forceUpdate();
    }

    // DELETE ME
    // for dev purposes lol
   /* setAuth(true);
    const tempUser = {
        FirstName: "allison",
        LastName: "test",
        Username: "test123@gmail.com",
        Image: 1
    }
    setCurrentUser(tempUser);
    successLoginView();*/


    return (
        <div id="LoginPage"> 
            <Row gutter={[48, 48]} align="middle">
                <Col span={12} style={{ fontFamily: 'Mulish' }}>
                    <div>
                        <h1>Welcome Back</h1>
                            {(loginProcess == 1) ?
                                (<div id="LoginForm">
                                    <p>a home organized.</p>
                                    <Form id="loginForm" onFinish={(values) => submitLoginForm(values)}>

                                        <Form.Item name="email" style={{ marginBottom: 15 }}>
                                            <Input style={Styles.inputFieldStyle} placeholder="Email" />
                                        </Form.Item>

                                        <Form.Item name="password">
                                            <Input.Password style={Styles.inputFieldStyle} placeholder="Password" />
                                        </Form.Item>

                                        <Button style={Styles.primaryButtonStyle} type="primary" htmlType="submit">Login</Button>
                                    </Form>

                                    <ConfigProvider theme={Styles.buttonHover}>
                                        <div id="ForgotPassBtn" style={{ marginTop: 15 }}>
                                            <Button style={resetPassBtnStyle}>Forgot Password? Reset Here</Button>
                                        </div>
                                    </ConfigProvider>
                                </div>) :
                                (<div id="OTPSection">
                                    <h6>Account Name: {account}</h6>
                                    <h6>One-time login code: {otpCode}</h6>
                                    <h6>Expires at: {otpExpiration}</h6>
                                    <Form id="loginForm" onFinish={(values) => submitOTPForm(values)}>

                                        <Form.Item name="otp">
                                            <Input style={Styles.inputFieldStyle} placeholder="Enter one-time login code" />
                                        </Form.Item>

                                        <Button style={Styles.primaryButtonStyle} type="primary" htmlType="submit">Login</Button>
                                    </Form>
                            </div>)

                                }
                            </div>
                </Col>
                <Col span={12}>
                    <Image style={Styles.defaultImage} src={defaultImage} preview={false} />
                </Col>
                </Row>
        </div>
    );
}

export default Login;