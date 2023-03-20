import React, { Component } from 'react';
import { Modal, ConfigProvider, Button, Form, Input, Row, Col, notification } from 'antd';
import * as Styles from '../styles/ConstStyles.js';
import '../styles/System.css';
import '../index.css';
import * as ValidationFuncs from '../scripts/InputValidation.js';
import { useAuth } from './AuthContext.js';
import axios from 'axios';

const InviteRoommateModal = (props) => {
    const [form] = Form.useForm();
    const { currentUser, currentGroup } = useAuth();

    const submitForm = () => {
        form.validateFields()
            .then((values) => {
                console.log(values)
                if (values['roommateEmail'] == undefined) {
                    toast('Enter an email to generate link.')
                } else {
                    addGroupMember(values['roommateEmail'])
                }
            })
            .catch((errorInfo) => { });
    };

    const addGroupMember = (username) => {
        let groupMemberForm = {
            GroupId: currentGroup['groupId'],
            GroupMember: username
        }

        console.log('GROUP ID: ' + currentGroup['groupId'])

        axios.post('group-settings/AddGroupMembers', groupMemberForm)
            .then(res => {
                var isSuccessful = res.data['isSuccessful'];
                if (isSuccessful) {
                    console.log("Successfully invited group member.")
                    props.close()
                    toast(("Successfully invited " + groupMemberForm.GroupMember))
                } else {
                    toast('Try again', res.data['message']);
                }
            })
            .catch((error => { console.error(error) }));
    }

    const validateEmail = (rule, value, callback) => {
        var ruleResult = ValidationFuncs.validateEmail(value)
        if (ruleResult.isSuccessful) {
            callback();
        } else {
            callback(ruleResult.message);
        }
    };

    const toast = (title, desc = '') => {
        notification.open({
            message: title,
            description: desc,
            duration: 5,
            placement: 'bottom',
        });
    }

    return (
        <Modal
            title={"Invite to " + props.group.groupName}
            open={props.show}
            closable={true}
            onCancel={props.close}
            centered="true"
            footer={null}>
            <div className="invite-modal-content">
                <Form id="inviteRoommateForm" form={form}>
                    <Form.Item name="roommateEmail" required>
                        <Input style={Styles.inputFieldStyle} placeholder="Email" required/>
                    </Form.Item>
                </Form>
                    <Row gutter={[8, 8]} align="middle">
                        <Col span={18}>
                            <div className="read-only-text">Temporary Link</div>
                        </Col>
                        <Col span={6}>
                            <Button onClick={submitForm} style={Styles.primaryButtonStyle}>Create Link</Button>
                        </Col>
                    </Row>
              
            </div>
        </Modal>
    );
}

export default InviteRoommateModal;