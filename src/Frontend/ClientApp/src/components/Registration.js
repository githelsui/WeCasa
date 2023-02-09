
import React, { Component } from 'react';
import { Form, Input, Button } from 'antd';

export class Registration extends Component {
    static displayName = Registration.name;

    render() {
        return (
            <div>
                <h1>Register Account</h1>
                <Form id="fullNameForm">
                    <Form.Item name="firstName">
                        <Input placeholder="First Name"/>
                    </Form.Item>

                    <Form.Item name="lastName">
                        <Input placeholder="Last Name" />
                    </Form.Item>
                </Form>
                <Form id="contactInfoForm">
                    <Form.Item name="email">
                        <Input placeholder="Email" />
                    </Form.Item>

                    <Form.Item name="password1">
                        <Input.Password placeholder="Password" />
                    </Form.Item>

                    <Form.Item name="password2">
                        <Input.Password placeholder="Reenter Password" />
                    </Form.Item>
                </Form>
                <Button>Register</Button>
            </div>
        );
    }
}
