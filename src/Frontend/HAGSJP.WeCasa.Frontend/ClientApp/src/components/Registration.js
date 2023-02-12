import React, { Component, useState, useEffect } from 'react';
import { Form, Input, Button, notification, ConfigProvider, Row, Col, Image} from 'antd';
import { Routes, Route, useNavigate, Link, withRouter } from 'react-router-dom';
import axios from 'axios';
import '../styles/System.css';
import * as Styles from '../styles/ConstStyles.js';
import '../styles/Registration.css';
import { Login } from "./Login";
import defaultImage from '../assets/defaultimgs/wecasatemp.jpg';

export class Registration extends Component {
    static displayName = Registration.name;

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
        this.state.inputFailures = false
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
                        console.log(res.data)
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
            <div id="RegistrationPage">
                <Row gutter={[48, 48]} align="middle">
                    <Col span={12} style={{ fontFamily: 'Mulish' }}>
                        <div>
                            {
                                this.state.registrationResults ?
                                    (<div><h4>Account {this.state.newAccount} created successfully.</h4></div>)
                                    :
                                    (<div id="RegistrationForm">
                                        <h2>Create an Account</h2>
                                        <p>a home organized.</p>
                                        <Form id="registrationForm" onFinish={(values) => this.submitForm(values)}>

                                            <Input.Group compact="true">
                                                <Form.Item name="firstName" id="FirstName" style={{ width: '49%', marginRight: '3%', marginBottom: 10 }}>
                                                    <Input style={Styles.inputFieldStyle} placeholder="First Name" />
                                                </Form.Item>

                                                <Form.Item name="lastName" style={{ width: '48%', marginBottom: 20 }}>
                                                    <Input style={Styles.inputFieldStyle} placeholder="Last Name" />
                                                </Form.Item>
                                            </Input.Group>

                                            <Form.Item name="email" style={{ marginBottom: 20 }}>
                                                <Input style={Styles.inputFieldStyle} placeholder="Email" />
                                            </Form.Item>

                                            <Form.Item name="password1" style={{ marginBottom: 20 }}>
                                                <Input.Password style={Styles.inputFieldStyle} placeholder="Password" />
                                            </Form.Item>

                                            <Form.Item name="password2">
                                                <Input.Password style={Styles.inputFieldStyle} placeholder="Reenter Password" />
                                            </Form.Item>

                                            <Button type="primary" htmlType="submit" style={Styles.primaryButtonStyle}>Create Account</Button>
                                        </Form>
                                    </div>)
                            }
                            <div id="LoginButton">
                                <ConfigProvider theme={Styles.buttonHover}>
                                    <Link to="/login"><Button type="default" style={Styles.defaultButtonStyle}>Login</Button></Link>
                                </ConfigProvider>

                            </div>
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
