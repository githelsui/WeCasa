import React, { Component, useState } from 'react';
import { Modal, ConfigProvider, Button, Row, Col, Image, Space, Input, Form, Switch, notification } from 'antd';
import * as Styles from '../styles/ConstStyles.js';
import '../styles/System.css';
import '../index.css';
import axios from 'axios';
import defaultImage from '../assets/defaultimgs/wecasatemp.jpg';
import * as ValidationFuncs from '../scripts/InputValidation.js';

export const GroupSettingsTab = (props) => {

    //Development Only:
    const group = {
        GroupId: 1,
        GroupName: "Group1",
        Icon: "#0256D4",
        Features: ["Budget Bar"]
    }
    const tempFeatureDesc = "DESCRIPTION: Lorem ipsum dolor sit amet consectetur. In non proin et interdum at. Vel mi praesent tincidunt tincidunt odio at mauris nisl cras."

    const [newIcon, setNewIcon] = useState(null);
    const [featureSwitches, setFeatures] = useState([]);
    const [roommate, setRoommate] = useState("");
    const [invitedRoommates, setInvitedRoommates] = useState([])
    const [noInvitations, setNoInvitations] = useState(true);

    const updateFeatureSection = () => {
        var features = props.group.Features
        const tempFeatures = [false, false, false, false, false, false]
        for (let i = 0; i < features.length; i++) {
            let feature = features[i];
            if (feature == "all") 
                tempFeatures = [true, true, true, true, true, true]
            if (feature == "Budget Bar")
                tempFeatures[0] = true
            if (feature == "Bulletin Board")
                tempFeatures[1] = true
            if (feature == "Calendar")
                tempFeatures[2] = true
            if (feature == "Chore List")
                tempFeatures[3] = true
            if (feature == "Grocery List")
                tempFeatures[4] = true
            if (feature == "Circular Progress Bar")
                tempFeatures[5] = true
        }
        setFeatures(tempFeatures)
        console.log(features)
    };

    const inviteRoommate = () => {
        if (roommate == '') {
            notification.open({
                message: "Please enter a username",
                duration: 5,
                placement: "topRight",
            });
        } else {
            //call web api method
            addGroupMember(roommate)
        }
    };

    const addGroupMember = (username) => {
        let groupMemberForm = {
            GroupId: group.GroupId,
            GroupMember: username
        }

        axios.post('group-settings/AddGroupMembers', groupMemberForm)
            .then(res => {
                var isSuccessful = res.data['isSuccessful'];
                if (isSuccessful) {
                    console.log("Successfully invited group member.")
                    let tempRoommates = invitedRoommates
                    tempRoommates.push(username)
                    console.log(tempRoommates)
                    if (tempRoommates.length > 0) {
                        setNoInvitations(false)
                        setInvitedRoommates(tempRoommates)
                    } else {
                        setNoInvitations(true)
                    }
                    successToast("Successfully invited group member " + groupMemberForm.GroupMember)
                    this.forceUpdate()
                } else {
                    failureToast(res.data['message']);
                }
            })
            .catch((error => { console.error(error) }));
    }

    const successToast = (successMessage) => {
        notification.open({
            message: successMessage,
            duration: 5,
            placement: "topRight",
        });
    }

    const failureToast = (failureMessage) => {
        // Accounts for user failure cases and system errors
        console.log("User Failure Cases")
        notification.open({
            message: "Try again.",
            description: failureMessage,
            duration: 5,
            placement: "topRight",
        });
    }

    const validateEmail = (rule, value, callback) => {
        var ruleResult = ValidationFuncs.validateEmail(value)
        if (ruleResult.isSuccessful) {
            callback();
        } else {
            callback(ruleResult.message);
        }
    };

    return (
        <div className="group-settings-tab padding">

            <h4 className="padding-bottom mulish-font"><b>Group Settings</b></h4>


            <Form id="groupCreationForm">
                <Row gutter={[24, 24]} align="middle">
                    <Col span={6} className="group-icon-selection">
                        <Image style={Styles.groupIconSelection} src={defaultImage} preview={false} height="120px" width="120px" />
                    </Col>
                    <Col span={16} className="group-name-input">
                        <Form.Item name="groupName">
                            <Input style={Styles.inputFieldStyle} placeholder="Group name" required />
                        </Form.Item>
                    </Col>
                </Row>

                <h5 className="padding-top mulish-font">Invite roommates</h5>
                <Row gutter={[24, 24]}>
                    <Col span={18} className="invite-group-members">
                        <Form.Item name="memberUsername" hasFeedback rules={[{ validator: validateEmail }]}>
                            <Input style={Styles.inputFieldStyle} placeholder="Roommate Email/Username" value={roommate} onChange={(e) => { setRoommate(e.target.value) }} />
                        </Form.Item>
                    </Col>
                    <Col span={6} className="invite-group-members-btn">
                        <Button style={Styles.primaryButtonStyleNoMargins} type="primary" onClick={inviteRoommate}>Invite</Button>
                    </Col>
                </Row>

                <div className="pending-invitations-section padding-vertical">
                    <h5 className="mulish-font">Pending invitations</h5>
                    <div>
                        {
                            (noInvitations) ?
                            (<p>No pending invitations sent.</p>) :
                            (<div>
                                {invitedRoommates.map((member) => <p>{member}</p>)}
                            </div>)
                        }
                    </div>
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

                    <div className="group-deletion-section padding-vertical">
                        <h5 className="mulish-font">Group deletion</h5>
                        <p className="group-feature-desc-p">Are you sure you want to delete this group?</p>
                        <Button key="create" type="primary" htmlType="submit" style={Styles.deleteButtonStyle}>Delete Group</Button>
                    </div>

                    <Button key="create" type="primary" htmlType="submit" style={Styles.primaryButtonStyle}>Save</Button>
                </div>
            </Form>
        </div>
    );
};

export default GroupSettingsTab;