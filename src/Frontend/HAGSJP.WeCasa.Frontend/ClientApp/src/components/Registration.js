import React, { Component } from 'react';
import { Form, Input, Button } from 'antd';
import axios from 'axios';

//const registerAccount = ({ values }) => {
//    console.log("run")
//}

const registerAccount = (values) => {
    console.log("run")
    console.log(values)

    const headers = {
        "Content-Type": "application/json",
    };

    let userAccount = {
        FirstName: values.firstName,
        LastName: values.lastName,
        Username: values.email,
        Password: values.password1
    };


    const registrationData = new FormData();
    registrationData.append("FirstName", values.firstName);
    registrationData.append("LastName", values.lastName);
    registrationData.append("Email", values.email);
    registrationData.append("Password", values.password1);
  
    //fetch('weatherforecast')
    // fetch('weatherforecast', {
    //    method: 'POST',
    //    headers: { 'Content-type': 'application/json' },
    //    body: JSON.stringify(userAccount),
    //}).then(r => r.json()).then(res => {
    //    if (res) {
    //        console.log(res.data);
    //        this.setState({ message: 'New Account is Created Successfully' });
    //    }
    //});

    //fetch('registration', {
    //    method: 'POST',
    //    headers: { 'Content-type': 'application/json' },
    //    mode:
    //    body: JSON.stringify(registerInfo),
    //});

    //fetch('registration', {
    //    method: 'POST',
    //    headers: { 'Content-type': 'application/json' },
    //    body: JSON.stringify(userAccount),
    //}).then(r => r.json()).then(res => {
    //    if (res) {
    //        console.log(res.data);
    //        this.setState({ message: 'New Account is Created Successfully' });
    //    }
    //});

    //fetch('registration');

    var text = 'hmmmm'

    // -- GET REQUEST -- 
    axios.get('registration')
        .then(res => console.log(res.data))
        .catch((error) => { console.error(error) });

    // -- POST REQUEST --
    axios.post('registration', {
            FirstName: values.firstName,
            LastName: values.lastName,
            Username: values.email,
            Password: values.password1
        })
        .then(res => console.log(res.data))
        .catch((error) => { console.error(error) });
};

export class Registration extends Component {
    static displayName = Registration.name;

    constructor(props) {
        super(props);
        this.state = { registrationResult: [], loading: true };
    }

    //static registerAccount = () => {

    //}

    //static registerAccount = ({ values }) => {
    //    console.log("run")
    //    console.log(values)
    //    let registerInfo = {
    //        'firstname': values.firstName,
    //        lastName: values.lastName,
    //        email: values.email,
    //        password1: values.password1,
    //        password2: values.password2
    //    };

    //    fetch('registration', {
    //        method: 'POST',
    //        headers: { 'Content-type': 'application/json' },
    //        body: registerInfo
    //    }).then(r => r.json()).then(res => {
    //        if (res) {
    //            this.setState({ message: 'New Account is Created Successfully' });
    //        }
    //    });
    //};

    render() {
        return (
            <div>
                <h1>Register Account</h1>
                <Form id="registrationForm" onFinish={(values) => registerAccount(values)}>
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
                    <Button htmlType="submit">Register</Button>
                </Form>

            </div>
        );
    }
}
