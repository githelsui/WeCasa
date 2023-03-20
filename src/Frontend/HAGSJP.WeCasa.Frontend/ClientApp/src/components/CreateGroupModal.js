import React, { Component, useState } from 'react';
import { Modal, ConfigProvider, Button, Row, Col, Image, Space, Input, Form, Switch, notification, Spin } from 'antd';
import * as Styles from '../styles/ConstStyles.js';
import '../styles/System.css';
import '../index.css';
import defaultImage from '../assets/defaultimgs/wecasatemp.jpg';
import * as ValidationFuncs from '../scripts/InputValidation.js';
import axios from 'axios';

const CreateGroupModal = (props) => {
    //Development Only:
    const tempFeatureDesc = "DESCRIPTION: Lorem ipsum dolor sit amet consectetur. In non proin et interdum at. Vel mi praesent tincidunt tincidunt odio at mauris nisl cras."
    const [loading, setLoading] = useState(false);
    const [newIcon, setNewIcon] = useState(null);
    const [roommate, setRoommate] = useState("");
    const [invitedRoommates, setInvitedRoommates] = useState([])
    const [noInvitations, setNoInvitations] = useState(true);
    const [form] = Form.useForm();

    const inviteRoommate = () => {
        if (roommate == '') {
            notification.open({
                message: "Please enter a username.",
                duration: 5,
                placement: "bottom",
            });
        } else {
            addToInviteList(roommate)
        }
    };

    const addToInviteList = (username) => {
        let form = {
            Username: username
        }
        axios.post('home/ValidateUser', form)
            .then(res => {
                var isSuccessful = res.data['isSuccessful'];
                if (isSuccessful) {
                    if (!invitedRoommates.includes(username)) {
                        let tempRoommates = getRoommatesCopy()
                        tempRoommates.push(username)
                        if (tempRoommates.length > 0) {
                            // Enables UI list of invited users
                            setNoInvitations(false)
                        } else {
                            setNoInvitations(true)
                        }
                        setInvitedRoommates(tempRoommates)
                        props.onInvitationListUpdated(tempRoommates)
                    } else {
                        // User already invited
                        toast('Cannot invite a user that has already been invited to group.');
                    }
                } else {
                    // Invalid user in system
                    toast(res.data['message']);
                }
            })
            .catch((error => { console.error(error) }));
    };

    const getRoommatesCopy = () => {
        let copy = []
        for (let i = 0; i < invitedRoommates.length; i++) {
            copy.push(invitedRoommates[i])
        }
        return copy
    }

    const validateEmail = (rule, value, callback) => {
        var ruleResult = ValidationFuncs.validateEmail(value)
        if (ruleResult.isSuccessful) {
            callback();
        } else {
            callback(ruleResult.message);
        }
    };

    const attemptSubmission = () => {
        form.validateFields()
            .then((values) => {
                props.confirm(values)
                setLoading(true)
                
            })
            .catch((errorInfo) => { });
    }

    const resetForm = () => {
        setNoInvitations(true)
        setInvitedRoommates([])
        props.onInvitationListUpdated([])
        props.close();
    }

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
            open={props.show}
            closable={false}
            centered="true"
            footer={null}
            maskClosable="false"
            >
            
            <div className="padding">
                <Spin spinning={loading}>
                    <h2 className="padding-bottom"><b>Create group</b></h2>
                    <Form id="groupCreationForm" onFinish={attemptSubmission} form={form}>
                    <Row gutter={[24, 24]} align="middle">
                        <Col span={8} className="group-icon-selection">
                            <Image style={Styles.groupIconSelection} src={defaultImage} preview={false} height="120px" width="120px" />
                        </Col>
                    <Col span={16} className="group-name-input">
                            <Form.Item name="groupName">
                                <Input style={Styles.inputFieldStyle} required placeholder="Group name" />
                            </Form.Item>
                    </Col>
                    </Row>

                <h5 className="padding-top mulish-font">Invite roommates</h5>
                <Row gutter={[24, 24]}>
                    <Col span={18} className="invite-group-members">
                        <Form.Item name="memberUsername">
                            <Input style={Styles.inputFieldStyle} placeholder="Roommate Email/Username" value={roommate} onChange={(e) => { setRoommate(e.target.value) }} />
                        </Form.Item>
                    </Col>
                    <Col span={6} className="invite-group-members-btn">
                        <Button style={Styles.primaryButtonStyleNoMargins} type="primary" onClick={inviteRoommate}>Invite</Button>
                    </Col>
                </Row>

                <div>
                    {
                        (noInvitations) ? (<div></div>) :
                            (<div className="pending-invitations-section padding-vertical">
                                <h5 className="mulish-font">Users to invite upon creation</h5>
                                <div>{invitedRoommates.map((member) => <p>{member}</p>)}</div>
                            </div>)
                    }
                </div>

                <h5 className="mulish-font">Customize group features</h5>
                    <div className="group-feature-row">
                        <Row gutter={24} style={Styles.groupFeatureContainer} >
                            <Col span={18}>
                                <h6 className="mulish-font">Budget Bar</h6>
                            </Col>
                            <Col span={6}>
                                <Form.Item name="budgetBar" valuePropName="checked" initialValue={true}>
                                    <Switch defaultChecked="true" checkedChildren="ON" unCheckedChildren="OFF" />
                                </Form.Item>
                            </Col>
                        </Row>
                        <p className="group-feature-desc-p">{tempFeatureDesc}</p>
                        <Row gutter={24} style={Styles.groupFeatureContainer} >
                            <Col span={18}>
                                <h6 className="mulish-font">Bulletin Board</h6>
                            </Col>
                            <Col span={6}>
                                <Form.Item name="bulletinBoard" valuePropName="checked" initialValue={true}>
                                    <Switch defaultChecked="true" checkedChildren="ON" unCheckedChildren="OFF" />
                                </Form.Item>
                            </Col>
                        </Row>
                        <p className="group-feature-desc-p">{tempFeatureDesc}</p>
                        <Row gutter={24} style={Styles.groupFeatureContainer} >
                            <Col span={18}>
                                <h6 className="mulish-font">Calendar</h6>
                            </Col>
                            <Col span={6}>
                                <Form.Item name="calendar" valuePropName="checked" initialValue={true}>
                                    <Switch defaultChecked="true" checkedChildren="ON" unCheckedChildren="OFF" />
                                </Form.Item>
                            </Col>
                        </Row>
                        <p className="group-feature-desc-p">{tempFeatureDesc}</p>
                        <Row gutter={24} style={Styles.groupFeatureContainer} >
                            <Col span={18}>
                                <h6 className="mulish-font">Chore List</h6>
                            </Col>
                            <Col span={6}>
                                <Form.Item name="choreList" valuePropName="checked" initialValue={true}>
                                    <Switch defaultChecked="true" checkedChildren="ON" unCheckedChildren="OFF" />
                                </Form.Item>
                            </Col>
                        </Row>
                        <p className="group-feature-desc-p">{tempFeatureDesc}</p>
                        <Row gutter={24} style={Styles.groupFeatureContainer} >
                            <Col span={18}>
                                <h6 className="mulish-font">Grocery List</h6>
                            </Col>
                            <Col span={6}>
                                <Form.Item name="groceryList" valuePropName="checked" initialValue={true}>
                                    <Switch defaultChecked="true" checkedChildren="ON" unCheckedChildren="OFF" />
                                </Form.Item>
                            </Col>
                        </Row>
                        <p className="group-feature-desc-p">{tempFeatureDesc}</p>
                        <Row gutter={24} style={Styles.groupFeatureContainer} >
                            <Col span={18}>
                                <h6 className="mulish-font">Circular Progress Bar</h6>
                            </Col>
                            <Col span={6}>
                                <Form.Item name="circularProgressBar" valuePropName="checked" initialValue={true}>
                                    <Switch defaultChecked="true" checkedChildren="ON" unCheckedChildren="OFF" />
                                </Form.Item>
                            </Col>
                        </Row>
                        <p className="group-feature-desc-p">{tempFeatureDesc}</p>
                    </div>
                    <Button key="cancel" onClick={resetForm} type="default" style={Styles.defaultButtonModal}>Exit</Button>
                    <Button key="create" htmlType="submit" type="primary" style={Styles.primaryButtonModal}>Create Group</Button>
                    </Form>
                </Spin>
            </div>
        </Modal>
    );
}

export default CreateGroupModal;