import React, { Component, useState } from 'react';
import { Navigate, useNavigate } from 'react-router-dom'
import { useAuth } from './AuthContext';
import { Col, Card, Row, Space, Avatar, Button, notification } from 'antd';
import { PlusCircleOutlined } from '@ant-design/icons'
import axios from 'axios';
import * as Styles from '../styles/ConstStyles.js';
import CreateGroupModal from './CreateGroupModal.js';
import NavMenu from './NavMenu';
const { Meta } = Card;

// FOR TESTING
const data = [
    {
        GroupId: 1,
        GroupName: "Group1",
        Icon: "#0256D4",
        Features: ["Budget Bar"]
    },
    {
        GroupId: 2,
        GroupName: "OtherGroup",
        Icon: "#668D6A",
        Features: ["all"]
    }
]

const maxConfiguredFeatures = 6;

export const Groups = () => {
    const [loading, setLoading] = useState(true);
    const { auth, currentUser } = useAuth();
    const [currentGroup, setCurrentGroup] = useState(null);
    const [groups, setGroups] = useState([]);
    const [showModal, setShowModal] = useState(false);
    const navigate = useNavigate();

    const getGroups = () => {
        axios.get('home/GetGroups', currentUser)
            .then(res => {
                var isSuccessful = res.data['isSuccessful'];
                if (isSuccessful) {
                    // setGroups(res.data)
                    setGroups(data);
                } else {
                    failureGroupView(res.data['message']);
                }
            })
            .catch((error) => { console.error(error) });
    }

    const displayGroupView = (groupList) => {
        return groupList.map(group => (
            <div key={group.GroupId}>
                <Col span={10} style={{marginTop:16, marginLeft:16}}>
                    <Card
                        hoverable
                        style={{ width: 500 }}
                        actions={[
                            <Button key="view" type="primary" style={Styles.primaryButtonModal}>View</Button>,
                            <Button key="settings" type="default" style={Styles.defaultButtonModal}>Settings</Button>
                        ]}>
                        <Meta
                            avatar={<Avatar style={{ backgroundColor: "var(group.Icon)" }} />}
                            title={group.GroupName}
                            type="inner"
                        />
                    </Card>
                </Col>
            </div>
        ));
    }

    const failureGroupView = (failureMessage) => {
        // display failure to load message
        notification.open({
            message: "Sorry, an error occurred.",
            description: failureMessage,
            duration: 10,
            placement: "topLeft"
        });
    }


    const attemptGroupCreation = (groupConfig) => {
        // Adding features as strings to a list if they are checked in the modal
        let features = [];
        if (groupConfig.budgetBar.checked) features.push("Budget Bar");
        if (groupConfig.bulletinBoard.checked) features.push("Bulletin Board");
        if (groupConfig.calendar.checked) features.push("Calendar");
        if (groupConfig.choreList.checked) features.push("Chore List");
        if (groupConfig.groceryList.checked) features.push("Grocery List");
        if (groupConfig.circularProgressBar.checked) features.push("Circular Progress Bar");
        if (features.length == maxConfiguredFeatures) features = ['all'];

        let group = {
            GroupName: groupConfig.groupName,
            Owner: currentUser,
            Icon: (groupConfig.icon == null) ? "#668D6A" : groupConfig.icon,
            Features: features
        }
        console.log(group);
        axios.post('home/CreateGroup', group)
            .then(res => {
                var isSuccessful = res.data['isSuccessful'];
                if (isSuccessful) {
                    setCurrentGroup(group);
                } else {
                    failureGroupView(res.data['message']);
                }
            })
            .catch((error => { console.error(error) }));
    }

    return (
        <div>
            {(currentGroup == null) ?
                (<div><Space direction="horizonal" size={32}>
                    {displayGroupView(data)}
                </Space>
                    <Row gutter={24} style={{ display: "flex" }}>
                        <Col span={24}>
                            <Card hoverable style={{ marginTop: 16, marginLeft: 150, marginRight: 150, background: "#ececec", fontFamily: 'Mulish' }}>
                                <Meta
                                    onClick={() => setShowModal(true)}
                                    avatar={<PlusCircleOutlined />}
                                    title="Create Group"
                                    type="inner"
                                    style={{ textAlign: "center", display: "flex" }}
                                />
                            </Card>
                        </Col>
                    </Row>
                    <CreateGroupModal show={showModal} close={() => setShowModal(false)} confirm={attemptGroupCreation} reject={failureGroupView} user={currentUser} />
                </div>) : (
                <div>
                    <NavMenu />
                </div>)}
        </div>
    );
}; 

export default Groups;