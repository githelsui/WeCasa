import React, { Component, useState, useEffect } from 'react';
import { Form, Input, Button, notification, ConfigProvider, Row, Gutter, Col, Image} from 'antd';
import { withRouter, useNavigate, Navigate } from 'react-router-dom';
import axios from 'axios';
import * as Styles from '../styles/ConstStyles.js';
import defaultImage from '../assets/defaultimgs/wecasatemp.jpg';

export const resetPassBtnStyle = {
    fontFamily: 'Mulish',
    width: '100%',
    fontWeight: 'thin',
    borderWidth: 'thin'
}

export class Login extends Component {
    static displayName = Login.name;

    constructor(props) {
        super(props);
        
        this.state = {
            loginResults: false,
            inputFailures: false,
            loginProcess: 1,
            loading: true,
            otpCode: '',
            otpExpiration: '',
            account: '',
        };
    }

    submitLoginForm = (values) => {
        this.state.inputFailures = false
        var failureMessage = '';

        let userAccount = {
            Username: values.email,
            Password: values.password
        };

        // -- Client-side Input Validation

        // Blank User Inputs
        for (let key in userAccount) {
            if (userAccount[key] == null) {
                this.state.inputFailures = true;
                failureMessage = 'Empty fields are not accepted.'
                break;
            }
        }

        if (!this.state.inputFailures) {
            this.attemptInitialLogin(userAccount)
        } else {
            this.failureLoginView(failureMessage);
        }
    };

    // Login Process 1: Prior to receiving an OTP
    attemptInitialLogin = (userAccount) => {
        if (!this.state.inputFailures) {

            this.state.loginProcess = 1;

            axios.post('login/AttemptLogin', userAccount)
                .then(res => {
                    var isSuccessful = res.data['isSuccessful'];
                    if (isSuccessful) {
                        this.state.loginProcess = 2;
                        this.getNewOTP(userAccount);
                    } else {
                        this.failureLoginView(res.data['message']);
                    }
                })
                .catch((error) => { console.error(error) });
        }
    };

    // Login Process 2: Receive new OTP and submit OTP
    getNewOTP = (userAccount) => {
        axios.post('login/GetOTP', userAccount)
            .then(res => {
                var otpRes = res.data
                this.state.account = otpRes['username']
                this.state.otpCode = otpRes['code']
                this.state.otpExpiration = otpRes['expirationTime']
                this.forceUpdate();
            })
            .catch((error) => { console.error(error) });

    };

    submitOTPForm = (values) => {
        let otpData = {
            Username: this.state.account,
            Password: values.otp
        }
        if (values.otp != null) {
            axios.post('login/LoginWithOTP', otpData)
                .then(res => {
                    var isSuccessful = res.data['isSuccessful'];
                    if (isSuccessful) {
                        console.log(res.data)
                        this.successLoginView();
                    } else {
                        this.failureLoginView(res.data['message']);
                    }
                })
                .catch((error) => { console.error(error) });
        } else {
            this.failureLoginView('Empty fields not accepted.')
        }
    }

    failureLoginView = (failureMessage) => {
        // Accounts for user failure cases and system errors
        this.state.registrationResults = false
        notification.open({
            message: "Try again.",
            description: failureMessage,
            duration: 10,
            placement: "topLeft",
        });
    }

    successLoginView = () => {
        console.log("Successful login");
        this.state.loginResults = true;
        this.forceUpdate();
    }

    render() {
        return (
            <div id="LoginPage"> 
                <Row gutter={[48, 48]} align="middle">
                    <Col span={12} style={{ fontFamily: 'Mulish' }}>
                        <div>
                            <h1>Welcome Back</h1>
                            {(this.state.loginResults == true) ?
                                (<Navigate to="/home" replace={true} />) :
                                (<div>
                                    {(this.state.loginProcess == 1) ?
                                        (<div id="LoginForm">
                                            <p>a home organized.</p>
                                            <Form id="loginForm" onFinish={(values) => this.submitLoginForm(values)}>

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
                                            <h6>Account Name: {this.state.account}</h6>
                                            <h6>One-time login code: {this.state.otpCode}</h6>
                                            <h6>Expires at: {this.state.otpExpiration}</h6>
                                            <Form id="loginForm" onFinish={(values) => this.submitOTPForm(values)}>

                                                <Form.Item name="otp">
                                                    <Input style={Styles.inputFieldStyle} placeholder="Enter one-time login code" />
                                                </Form.Item>

                                                <Button style={Styles.primaryButtonStyle} type="primary" htmlType="submit">Login</Button>
                                            </Form>
                                        </div>)

                                    }
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
}
