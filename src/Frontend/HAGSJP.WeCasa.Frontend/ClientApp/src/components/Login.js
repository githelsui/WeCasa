import React, { Component, useState, useEffect } from 'react';
import { Form, Input, Button, notification } from 'antd';
import { withRouter } from 'react-router-dom';
import axios from 'axios';

export class Login extends Component {
    static displayName = Login.name;

    constructor(props) {
        super(props);
        this.state = {
            loginResults: false,
            inputFailures: false,
            loginProcess: 1,
            loading: true,
            otp: null
        };
    }

    submitForm = (values) => {
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
                    console.log(res.data)
                    var isSuccessful = res.data['isSuccessful'];
                    if (isSuccessful) {
                        this.submitOTP(userAccount['Username']);
                    } else {
                        this.failureLoginView(res.data['message']);
                    }
                })
                .catch((error) => { console.error(error) });
        }
    };

    // Login Process 2: Receive new OTP
    submitOTP = (userAccount) => {
        this.state.loginProcess = 2;
        this.forceUpdate();
    };


    failureLoginView = (failureMessage) => {
        // Accounts for user failure cases and system errors
        this.state.registrationResults = false
        console.log("registration result: " + this.state.loginResults)
        console.log("User Failure Cases")
        notification.open({
            message: "Try again.",
            description: failureMessage,
            duration: 10,
            placement: "topRight",
        });
    }

    successLoginView = (accountName) => {
        this.state.loginResults = true

        // go to logged in home page
        this.props.history.push('/home');

    }

    render() {
        return (
            <div>
                <h1>Login</h1>
                <div>
                    {(this.state.loginProcess == 1) ?
                        (<div id="LoginForm">
                            <Form id="loginForm" onFinish={(values) => this.submitForm(values)}>

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
                            <h3>Account Name</h3>
                            <h5>One-time login code: </h5>
                            <h5>Expires at: </h5>
                            <Form id="loginForm" onFinish={(values) => this.submitForm(values)}>

                                <Form.Item name="otp">
                                    <Input placeholder="Enter one-time login code" />
                                </Form.Item>

                                <Button type="primary" htmlType="submit">Login</Button>
                            </Form>
                        </div>)

                }
                </div>
      
            </div>
        );
    }
}
