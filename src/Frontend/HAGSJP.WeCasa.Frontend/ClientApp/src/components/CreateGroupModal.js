import React, { Component, useState } from 'react';
import { Modal, ConfigProvider, Button, Row, Col, Image, Space, Input, Form, Switch, notification } from 'antd';
import * as Styles from '../styles/ConstStyles.js';
import '../styles/System.css';
import '../index.css';
import defaultImage from '../assets/defaultimgs/wecasatemp.jpg';
import * as ValidationFuncs from '../scripts/InputValidation.js';
import axios from 'axios';

const CreateGroupModal = (props) => {
    //Development Only:
    const tempFeatureDesc = "DESCRIPTION: Lorem ipsum dolor sit amet consectetur. In non proin et interdum at. Vel mi praesent tincidunt tincidunt odio at mauris nisl cras."
    const [newIcon, setNewIcon] = useState(null);
    const [roommate, setRoommate] = useState("");
    const [invitedRoommates, setInvitedRoommates] = useState([])
    const [noInvitations, setNoInvitations] = useState(true);

    const inviteRoommate = () => {
        if (roommate == '') {
            notification.open({
                message: "Please enter a username",
                duration: 5,
                placement: "topRight",
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
                    let tempRoommates = getRoommatesCopy()
                    tempRoommates.push(username)
                    if (tempRoommates.length > 0) {
                        setNoInvitations(false)
                    } else {
                        setNoInvitations(true)
                    }
                    setInvitedRoommates(tempRoommates)
                    props.onInvitationListUpdated(tempRoommates)
                } else {
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
                <h2 className="padding-bottom"><b>Create group</b></h2>
                <Form id="groupCreationForm" onFinish={props.confirm}>
                    <Row gutter={[24, 24]} align="middle">
                        <Col span={8} className="group-icon-selection">
                            <Image style={Styles.groupIconSelection} src={defaultImage} preview={false} height="120px" width="120px" />
                        </Col>
                    <Col span={16} className="group-name-input">
                            <Form.Item name="groupName">
                                <Input style={Styles.inputFieldStyle} placeholder="Group name" required />
                            </Form.Item>
                    </Col>
                </Row>


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
                    <Button key="cancel" onClick={props.close} type="default" style={Styles.defaultButtonModal}>Exit</Button>,
                    <Button key="create" type="primary" htmlType="submit" style={Styles.primaryButtonModal}>Create Group</Button>
                </Form>
            </div>
        </Modal>
    );
}

export default CreateGroupModal;