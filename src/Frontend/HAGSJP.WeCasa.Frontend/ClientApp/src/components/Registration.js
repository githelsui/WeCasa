import React, { Component, useState, useEffect } from 'react';
import { Form, Input, Button, notification } from 'antd';
import { Routes, Route, useNavigate, Link, withRouter } from 'react-router-dom';
import axios from 'axios';
import '../styles/Registration.css';
import { Login } from "./Login";

export class Registration extends Component {
    static displayName = Registration.name;
    //static navigate = this.useNavigate();

    constructor(props) {
        super(props);
        this.state = {
            registrationResults: false,
            inputFailures: false,
            loading: true,
            newAccount: null
        };
    }

    submitForm = (values) => {
        var failureMessage = '';

        let userAccount = {
            FirstName: values.firstName,
            LastName: values.lastName,
            Username: values.email,
            Password: values.password1
        };

        // -- Client-side Input Validation

        // Blank User Inputs
        for (let key in userAccount) {
            if (userAccount[key] == null) {
                this.state.inputFailures = true;
                failureMessage ='Empty fields are not accepted.'
                break;
            }
        }

        // Password Matching Validation
        if (values.password1 != values.password2) {
            this.state.inputFailures = true;
            failureMessage += 'Passwords must match.'
        }

        if (!this.state.inputFailures) {
            this.registerAccount(userAccount)
        } else {
            this.failureRegistrationView(failureMessage);
        }
    };

    registerAccount = (userAccount) => {
        if (!this.state.inputFailures) {
            axios.post('registration', userAccount)
                .then(res => {
                    console.log(res.data)
                    var isSuccessful = res.data['isSuccessful'];
                    if (isSuccessful) {
                        this.successRegistrationView(userAccount['Username']);
                    } else {
                        this.failureRegistrationView(res.data['message']);
                    }
                })
                .catch((error) => { console.error(error) });
        }
    };

    failureRegistrationView = (failureMessage) => {
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

    successRegistrationView = (accountName) => {
        this.state.registrationResults = true
        this.state.newAccount = accountName
        this.forceUpdate();
    }

    navigateLogin = () => {
        //console.log("clicked")
        //alert("hello world")
        //this.useNavigate.navigate('/login');
    }

    render() {
        return (
            <div>
                <h1>Register Account</h1>
                {
                    this.state.registrationResults ?
                        (<div><h2>Account {this.state.newAccount} created successfully.</h2></div>)
                        :
                        (<div id="RegistrationForm">
                            <Form id="registrationForm" onFinish={(values) => this.submitForm(values)}>
                                <Form.Item name="firstName">
                                    <Input placeholder="First Name" />
                                </Form.Item>

                                <Form.Item name="lastName">
                                    <Input placeholder="Last Name" />
                                </Form.Item>
                                <Form.Item name="email">
                                    <Input placeholder="Email" />
                                </Form.Item>

                                <Form.Item name="password1">
                                    <Input.Password placeholder="Password" />
                                </Form.Item>

                                <Form.Item name="password2">
                                    <Input.Password placeholder="Reenter Password" />
                                </Form.Item>
                                <Button type="primary" htmlType="submit">Register</Button>
                            </Form>
                        </div>)
                }
                <div id="LoginButton">
                    <Link style={{ textDecoration: 'none' }} to="/login"><Button>Login</Button></Link>
                </div>
            </div>
        );
    }
}
