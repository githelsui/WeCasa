import React, { Component, useState, useEffect } from 'react';
import { Form, Input, Button, notification } from 'antd';
import { Routes, Route, useNavigate, Link, withRouter } from 'react-router-dom';
import axios from 'axios';
import '../styles/System.css';
import '../styles/Registration.css';
import { Login } from "./Login";

//Using javascript instead of css files to edit AntDesign elements because it
//won't let me override the styles in a separate css file :-(, if anyone knows a way around this lmk
const inputFieldStyle = {
    fontFamily: 'Mulish',
    backgroundColor: '#F4F5F4',
    borderRadius: '2%'
}

const registerButtonStyle = {
    boxShadow: '0px 4px 4px rgba(0, 0, 0, 0.25)',
    fontFamily: 'Mulish',
    width: '100%',
    fontWeight: 'bold',
    backgroundColor: '#111827',
    color: '#FFFFFF',
    borderRadius: '1%',
    marginTop: 0 
}

const loginButtonStyle = {
    fontFamily: 'Mulish',
    fontWeight: 'bold',
    backgroundColor: '#FFFFFF',
    borderRadius: '2%',
    width: '100%',
    borderWidth: 'thin',
    borderColor: '#111827',
    textDecoration: 'none'
}

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

    render() {
        return (
            <div>
                <h1>Create an Account</h1>
                {
                    this.state.registrationResults ?
                        (<div><h2>Account {this.state.newAccount} created successfully.</h2></div>)
                        :
                        (<div id="RegistrationForm">
                            <p>a home organized.</p>
                            <Form id="registrationForm" onFinish={(values) => this.submitForm(values)}>

                                <Input.Group compact="true">
                                    <Form.Item name="firstName" id="FirstName" style={{ width: '49%', marginRight: '3%', marginBottom: 10 }}>
                                        <Input style={inputFieldStyle} placeholder="First Name" />
                                    </Form.Item>

                                    <Form.Item name="lastName" style={{ width: '48%', marginBottom: 20 }}>
                                        <Input style={inputFieldStyle} placeholder="Last Name"/>
                                    </Form.Item>
                                </Input.Group>
                                
                                <Form.Item name="email" style={{ marginBottom: 20 }}>
                                    <Input style={inputFieldStyle} placeholder="Email" />
                                </Form.Item>

                                <Form.Item name="password1" style={{ marginBottom: 20 }}>
                                    <Input.Password style={inputFieldStyle} placeholder="Password" />
                                </Form.Item>

                                <Form.Item name="password2">
                                    <Input.Password style={inputFieldStyle} placeholder="Reenter Password" />
                                </Form.Item>

                                <Button type="primary" htmlType="submit" style={registerButtonStyle}>Create Account</Button>
                            </Form>
                        </div>)
                }
                <div id="LoginButton">
                    <Link to="/login"><Button style={loginButtonStyle}>Login</Button></Link>
                </div>
            </div>
        );
    }
}
