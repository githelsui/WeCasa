import React, { Component, useState, useEffect } from 'react';
import { Form, Input, Button, notification } from 'antd';
import axios from 'axios';

export class Login extends Component {
    static displayName = Login.name;

    constructor(props) {
        super(props);
        this.state = {
            loginResults: false,
            inputFailures: false,
            loading: true
        };
    }

    submitForm = (values) => {
        var failureMessage = '';

        let userAccount = {
            Username: values.email,
            Password: values.password1
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
            this.loginAccount(userAccount)
        } else {
            this.failureLoginView(failureMessage);
        }
    };

    loginAccount = (userAccount) => {
        if (!this.state.inputFailures) {
            axios.post('registration', userAccount)
                .then(res => {
                    console.log(res.data)
                    var isSuccessful = res.data['isSuccessful'];
                    if (isSuccessful) {
                        this.successLoginView(userAccount['Username']);
                    } else {
                        this.failureLoginView(res.data['message']);
                    }
                })
                .catch((error) => { console.error(error) });
        }
    };

    failureLoginView = (failureMessage) => {
        // Accounts for user failure cases and system errors
        this.state.registrationResults = false
        console.log("registration result: " + this.state.registrationResults)
        console.log("User Failure Cases")
        notification.open({
            message: "Try again.",
            description: failureMessage,
            duration: 10,
            placement: "topRight",
        });
    }

    successLoginView = (accountName) => {
        this.state.registrationResults = true
        this.state.newAccount = accountName
        this.forceUpdate();

        // go to logged in home page
    }

    render() {
        return (
            <div>
                <h1>Login</h1>
                <div id="LoginForm">
                    <Form id="registrationForm" onFinish={(values) => this.submitForm(values)}>

                        <Form.Item name="email">
                            <Input placeholder="Email" />
                        </Form.Item>

                        <Form.Item name="password">
                            <Input.Password placeholder="Password" />
                        </Form.Item>
                        <Button type="primary" htmlType="submit">Login</Button>
                    </Form>
                </div>
            </div>
        );
    }
}
