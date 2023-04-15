import React, { Component, useState, useEffect } from 'react';
import { Modal, ConfigProvider, Button, Row, Col, Image, Space, Input, Form, Switch, notification, Card } from 'antd';
import * as Styles from '../../styles/ConstStyles.js';
import GroupDeletionModal from './GroupDeletionModal.js';
import { useAuth } from '../AuthContext.js';
import '../../styles/System.css';
import '../../index.css';
import axios from 'axios';
import defaultImage from '../../assets/defaultimgs/wecasatemp.jpg';
import * as ValidationFuncs from '../../scripts/InputValidation.js';
import IconSelectorModal from "../IconSelectorModal.js";
import { EditOutlined } from '@ant-design/icons';
const maxConfiguredFeatures = 6;

export const GroupSettingsTab = (props) => {

    //Development Only:
    const tempGroup = {
        GroupId: 1,
        GroupName: "Group1",
        Icon: "#0256D4",
        Features: ["Budget Bar"]
    }
    const tempFeatureDesc = "DESCRIPTION: Lorem ipsum dolor sit amet consectetur. In non proin et interdum at. Vel mi praesent tincidunt tincidunt odio at mauris nisl cras."

    const [showModal, setShowModal] = useState(false);
    const [newIcon, setNewIcon] = useState(null);
    const [featureSwitches, setFeatures] = useState([]);
    const [roommate, setRoommate] = useState("");
    const [invitedRoommates, setInvitedRoommates] = useState([])
    const [noInvitations, setNoInvitations] = useState(true);
    const [showIconModal, setShowIconModal] = useState(false);
    const [selectedIcon, setSelectedIcon] = useState('')
    const { currentUser, currentGroup, setCurrentGroup } = useAuth();

    const updateFeatureSection = () => {
        var features = currentGroup['Features'];
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

    const deleteGroup = () => {
        setShowModal(false)

        let groupMemberForm = {
            GroupId: currentGroup['groupId'],
            GroupMember: currentUser['username']
        }

        axios.post('group-settings/DeleteGroup', groupMemberForm)
            .then(res => {
                var isSuccessful = res.data['isSuccessful'];
                if (isSuccessful) {
                    console.log("Successfully deleted group.")
                    toast("Successfully deleted group.")
                } else {
                    toast('Error deleting group.', res.data['message']);
                }
            })
            .catch((error => { console.error(error) }));
    }

    const leaveGroup = () => {
        setShowModal(false)

        let groupMemberForm = {
            GroupId: currentGroup['groupId'],
            GroupMember: currentUser['username']
        }

        axios.post('group-settings/LeaveGroup', groupMemberForm)
            .then(res => {
                var isSuccessful = res.data['isSuccessful'];
                if (isSuccessful) {
                    console.log("Successfully left group.")
                    toast("Successfully left group.")
                } else {
                    toast('Error leaving group.', res.data['message']);
                }
            })
            .catch((error => { console.error(error) }));
    }

    const submitGroupSettings = (values) => {
        // Editing features 
        let newFeatures = [];
        if (values.budgetBar) newFeatures.push("Budget Bar");
        if (values.bulletinBoard) newFeatures.push("Bulletin Board");
        if (values.calendar) newFeatures.push("Calendar");
        if (values.choreList) newFeatures.push("Chore List");
        if (values.groceryList) newFeatures.push("Grocery List");
        if (values.circularProgressBar) newFeatures.push("Circular Progress Bar");
        if (values.length == maxConfiguredFeatures) newFeatures = ['all'];

        let groupForm = {
            GroupId: currentGroup['groupId'],
            Owner: currentGroup['owner'],
            GroupName: currentGroup['groupName'],
            Icon: selectedIcon,
            Features: newFeatures
        }

        //Editing group name
        if (values.groupName != undefined && values.groupName != '') {
            groupForm['groupName'] = values.groupName
        }

        console.log(groupForm)
        axios.post('group-settings/EditGroup', groupForm)
            .then(res => {
                var isSuccessful = res.data['isSuccessful'];
                if (isSuccessful) {
                    console.log("Successfully saved group.")
                    toast("Successfully saved group.")
                    setCurrentGroup(res.data['returnedObject'])
                } else {
                    toast('Try again.', res.data['message']);
                }
            })
            .catch((error => { console.error(error) }));
    }

    const toast = (title, desc = '') => {
        notification.open({
            message: title,
            description: desc,
            duration: 5,
            placement: 'bottom',
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

    useEffect(() => {
        setSelectedIcon(currentGroup["icon"]); //Default
    }, []);
    return (
        <div className="group-settings-tab padding">

            <Form id="groupSettingsForm" onFinish={(values) => submitGroupSettings(values)}>
                <Row gutter={[24, 24]} align="middle">
                    <Col span={8} className="group-icon-selection">
                        <Card onClick={() => setShowIconModal(true)} style={{ backgroundColor: selectedIcon, borderRadius: 5, width: 100, height: 100, cursor: 'pointer' }}>
                            <EditOutlined style={{ color: 'gray', backgroundColor: 'white', marginLeft: 35, marginTop: 35, borderRadius: 50, height: 30, width: 30, opacity: 0.9, justifyContent: 'center' }} />
                        </Card> 
                        <IconSelectorModal show={showIconModal} close={() => setShowIconModal(false)} confirm={setSelectedIcon} iconType='Group' />
                    </Col>
                    <Col span={14} className="group-name-input">
                        <Form.Item name="groupName">
                            <Input style={Styles.inputFieldStyle} placeholder="Group name" />
                        </Form.Item>
                    </Col>
                </Row>

                <h5 className="padding-top mulish-font">Customize group features</h5>
                <div className="group-feature-row">
                    <Row gutter={24} style={Styles.groupFeatureContainer} align="middle">
                        <Col span={18}>
                            <h6 className="mulish-font">Budget Bar</h6>
                        </Col>
                        <Col span={6}>
                            <Form.Item name="budgetBar"
                                valuePropName="checked"
                                initialValue={currentGroup.features.includes("Budget Bar") || currentGroup.features.includes("all") ? true : false}>
                                <Switch defaultChecked="false" checkedChildren="ON" unCheckedChildren="OFF" />
                            </Form.Item>
                        </Col>
                    </Row>
                    <p className="group-feature-desc-p">{tempFeatureDesc}</p>
                    <Row gutter={24} style={Styles.groupFeatureContainer} align="middle">
                        <Col span={18}>
                            <h6 className="mulish-font">Bulletin Board</h6>
                        </Col>
                        <Col span={6}>
                            <Form.Item name="bulletinBoard"
                                valuePropName="checked"
                                initialValue={currentGroup.features.includes("Bulletin Board") || currentGroup.features.includes("all") ? true : false}>
                                <Switch defaultChecked="false" checkedChildren="ON" unCheckedChildren="OFF" />
                            </Form.Item>
                        </Col>
                    </Row>
                    <p className="group-feature-desc-p">{tempFeatureDesc}</p>
                    <Row gutter={24} style={Styles.groupFeatureContainer} align="middle">
                        <Col span={18}>
                            <h6 className="mulish-font">Calendar</h6>
                        </Col>
                        <Col span={6}>
                            <Form.Item name="calendar"
                                valuePropName="checked"
                                initialValue={currentGroup.features.includes("Calendar") || currentGroup.features.includes("all") ? true : false}>
                                <Switch defaultChecked="false" checkedChildren="ON" unCheckedChildren="OFF" />
                            </Form.Item>
                        </Col>
                    </Row>
                    <p className="group-feature-desc-p">{tempFeatureDesc}</p>
                    <Row gutter={24} style={Styles.groupFeatureContainer} align="middle">
                        <Col span={18}>
                            <h6 className="mulish-font">Chore List</h6>
                        </Col>
                        <Col span={6}>
                            <Form.Item name="choreList"
                                valuePropName="checked"
                                initialValue={currentGroup.features.includes("Chore List") || currentGroup.features.includes("all") ? true : false}>
                                <Switch defaultChecked="false" checkedChildren="ON" unCheckedChildren="OFF" />
                            </Form.Item>
                        </Col>
                    </Row>
                    <p className="group-feature-desc-p">{tempFeatureDesc}</p>
                    <Row gutter={24} style={Styles.groupFeatureContainer} align="middle">
                        <Col span={18}>
                            <h6 className="mulish-font">Grocery List</h6>
                        </Col>
                        <Col span={6}>
                            <Form.Item name="groceryList"
                                valuePropName="checked"
                                initialValue={currentGroup.features.includes("Grocery List") || currentGroup.features.includes("all") ? true : false}>
                                <Switch defaultChecked="false" checkedChildren="ON" unCheckedChildren="OFF" />
                            </Form.Item>
                        </Col>
                    </Row>
                    <p className="group-feature-desc-p">{tempFeatureDesc}</p>
                    <Row gutter={24} style={Styles.groupFeatureContainer} align="middle">
                        <Col span={18}>
                            <h6 className="mulish-font">Circular Progress Bar</h6>
                        </Col>
                        <Col span={6}>
                            <Form.Item name="circularProgressBar"
                                valuePropName="checked"
                                initialValue={currentGroup.features.includes("Circular Progress Bar") || currentGroup.features.includes("all") ? true : false}>
                                <Switch defaultChecked="false" checkedChildren="ON" unCheckedChildren="OFF" />
                            </Form.Item>
                        </Col>
                    </Row>
                    <p className="group-feature-desc-p">{tempFeatureDesc}</p>
                </div>
                    <div className="save-group-settings padding-vertical">
                        <Button key="create" type="primary" htmlType="submit" style={Styles.saveButtonStyle}>Save</Button>
                </div>
                <div className="group-deletion-section">
                    {(currentUser["username"] == currentGroup["owner"]) ?
                    (<div><Button type="primary" style={Styles.deleteButtonStyle} onClick={() => setShowModal(true)}>Delete Group</Button>
                        <GroupDeletionModal show={showModal} close={() => setShowModal(false)} confirm={deleteGroup} /></div>
                    ) :
                    (<div><Button type="primary" style={Styles.deleteButtonStyle} onClick={() => setShowModal(true)}>Leave Group</Button>
                        <GroupDeletionModal show={showModal} close={() => setShowModal(false)} confirm={leaveGroup} /></div>
                    )}
                </div>
            </Form>
        </div>
    );
};

export default GroupSettingsTab;