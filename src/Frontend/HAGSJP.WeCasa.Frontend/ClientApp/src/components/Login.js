import React, { Component, useState, useEffect } from 'react';
import { Form, Input, Button, notification } from 'antd';
import { withRouter, useNavigate, Navigate } from 'react-router-dom';
import axios from 'axios';

export class Login extends Component {
    static displayName = Login.name;
    //navigate = useNavigate()

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
            placement: "topRight",
        });
    }

    successLoginView = () => {
        console.log("Successful login");
        this.state.loginResults = true;
        this.forceUpdate();
    }

    render() {
        return (
            <div>
                <h1>Login</h1>
                { (this.state.loginResults == true) ?
                    (<Navigate to="/home" replace={true} />) :
                    (<div>
                        {(this.state.loginProcess == 1) ?
                            (<div id="LoginForm">
                                <Form id="loginForm" onFinish={(values) => this.submitLoginForm(values)}>

                                    <Form.Item name="email">
                                        <Input placeholder="Email" />
                                    </Form.Item>

                                    <Form.Item name="password">
                                        <Input.Password placeholder="Password" />
                                    </Form.Item>

                                    <Button type="primary" htmlType="submit">Login</Button>
                                </Form>
                            </div>) :
                            (<div id="OTPSection">
                                <h3>Account Name: {this.state.account}</h3>
                                <h5>One-time login code: {this.state.otpCode}</h5>
                                <h5>Expires at: {this.state.otpExpiration}</h5>
                                <Form id="loginForm" onFinish={(values) => this.submitOTPForm(values)}>

                                    <Form.Item name="otp">
                                        <Input placeholder="Enter one-time login code" />
                                    </Form.Item>

                                    <Button type="primary" htmlType="submit">Login</Button>
                                </Form>
                            </div>)

                        }
                    </div>)
                }
      
            </div>
        );
    }
}
