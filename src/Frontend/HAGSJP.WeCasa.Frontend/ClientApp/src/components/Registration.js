import React, { Component } from 'react';
import { Form, Input, Button } from 'antd';
import axios from 'axios';

const submitForm = (values) => {
    var proceedRegistration = true;

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
            proceedRegistration = false;
            break;
        }
    }

    // Password Matching Validation
    proceedRegistration = values.password1 == values.password2

    if (proceedRegistration) {
        registerAccount(userAccount)
    } else {
        userFailureRegistrationView();
    }
};

const registerAccount = (userAccount) => {
    axios.post('registration', userAccount)
        .then(res => {
            console.log(res.data)
            var isSuccessful = res.data['isSuccessful'];
            var errorStatus = res.data['errorStatus'];
            if (isSuccessful) {
                successRegistrationView();
            } else if (errorStatus != 0) {
                errorRegistrationView();
            } else {
                userFailureRegistrationView();
            }
        })
        .catch((error) => { console.error(error) });
};

const userFailureRegistrationView = () => {
    // Display all input validation messages
    console.log("User Failure Cases")
}

const successRegistrationView = () => {
    // Transition to next view AccountCreationSuccess
    console.log("Success")
}

const errorRegistrationView = () => {
    // Display system error messages
    console.log("System Error")
}

export class Registration extends Component {
    static displayName = Registration.name;

    constructor(props) {
        super(props);
        this.state = { registrationResult: [], loading: true };
    }

    render() {
        return (
            <div>
                <h1>Register Account</h1>
                <Form id="registrationForm" onFinish={(values) => submitForm(values)}>
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
