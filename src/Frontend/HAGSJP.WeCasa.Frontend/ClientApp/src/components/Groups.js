import React, { Component, useState, useEffect } from 'react';
import { Navigate, useNavigate } from 'react-router-dom'
import { useAuth } from './AuthContext';
import { Col, Card, Row, Space, Avatar, Button, notification } from 'antd';
import { PlusCircleOutlined } from '@ant-design/icons'
import axios from 'axios';
import * as Styles from '../styles/ConstStyles.js';
import CreateGroupModal from './CreateGroupModal.js';
import NavMenu from './NavMenu';
const { Meta } = Card;

const maxConfiguredFeatures = 6;

export const Groups = () => {
    const [loading, setLoading] = useState(true);
    const { currentUser } = useAuth();
    const [currentGroup, setCurrentGroup] = useState(null);
    const [invitedRoommates, setInvitedRoommates] = useState([])
    const [groups, setGroups] = useState([]);
    const [showModal, setShowModal] = useState(false);
    const navigate = useNavigate();

    useEffect(() => { getGroups(); }, []);

    const getGroups = () => {
        let account = {
            Username: currentUser
        }
        axios.post('home/GetGroups', account)
            .then(res => {
                setGroups(res.data['returnedObject']);
            })
            .catch((error) => { console.error(error) });
        }

    const attemptGroupCreation = (groupConfig) => {
        // Adding features as strings to a list if they are checked in the modal
        let features = [];
        if (groupConfig.budgetBar) features.push("Budget Bar");
        if (groupConfig.bulletinBoard) features.push("Bulletin Board");
        if (groupConfig.calendar) features.push("Calendar");
        if (groupConfig.choreList) features.push("Chore List");
        if (groupConfig.groceryList) features.push("Grocery List");
        if (groupConfig.circularProgressBar) features.push("Circular Progress Bar");
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
                var createdGroup = res.data['returnedObject']
                var isSuccessful = res.data['isSuccessful'];
                var groupId = createdGroup['groupId']
                if (isSuccessful) {
                    inviteGroupMembers(createdGroup);
                } else {
                    failureGroupView(res.data['message']);
                }
            })
            .catch((error => { console.error(error) }));
    }

    const viewGroup = (group) => {
        setCurrentGroup(group);
    }

    const editGroup = (group) => {
        setCurrentGroup(group);
        navigate('/group-settings', { state: group });
    }

    const handleGroupMembersChange = (membersList) => {
        let memberCopy = getRoommatesCopy(membersList)
        setInvitedRoommates(memberCopy)
    }

    const inviteGroupMembers = (group) => {
        console.log(invitedRoommates)

        let groupMemberForm = {
            GroupId: group['groupId'],
            GroupMembers: invitedRoommates
        }

        axios.post('home/NewGroupAddMembers', groupMemberForm)
            .then(res => {
                var isSuccessful = res.data['isSuccessful'];
                if (isSuccessful) {
                    setCurrentGroup(group);
                    navigate('/group-settings', { state: group });
                } else {
                    failureGroupView(res.data['message']);
                }
            })
            .catch((error => { console.error(error) }));
    }

    const getRoommatesCopy = (original) => {
        let copy = []
        for (let i = 0; i < original.length; i++) {
            copy.push(original[i])
        }
        return copy
    }

    const displayGroupView = () => {
        return groups.map(group => (
            <div key={group.groupId}>
                <Col span={10} style={{marginTop:16, marginLeft:16}}>
                    <Card
                        hoverable
                        style={{ width: 500 }}
                        actions={[
                            <Button key="view" type="primary" onClick={() => viewGroup(group)} style={Styles.primaryButtonModal}>View</Button>,
                            <Button key="settings" type="default" onClick={() => editGroup(group)} style={Styles.defaultButtonModal}>Settings</Button>
                        ]}>
                        <Meta
                            avatar={<Avatar style={{ "backgroundColor": group.icon }} />}
                            title={group.groupName}
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

    return (
        <div>
            {(currentGroup == null) ?
                (<div><Space direction="horizonal" size={32}>
                    {displayGroupView()}
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
                    <CreateGroupModal show={showModal} close={() => setShowModal(false)} confirm={attemptGroupCreation} reject={failureGroupView} user={currentUser} onInvitationListUpdated={handleGroupMembersChange}/>
                </div>) : (
                <div>
                    <NavMenu />
                </div>)}
        </div>
    );
}; 

export default Groups;