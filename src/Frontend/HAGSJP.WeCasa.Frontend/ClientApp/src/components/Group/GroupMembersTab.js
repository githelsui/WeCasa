import React, { Component, useState, useEffect } from 'react';
import { List, Avatar, ConfigProvider, Button, Row, Col, Image, Space, Input, Form, Divider, Skeleton, notification } from 'antd';
import * as Styles from '../../styles/ConstStyles.js';
import '../../styles/System.css';
import '../../index.css';
import axios from 'axios';
import defaultImage from '../../assets/defaultimgs/wecasatemp.jpg';
import InviteRoommateModal from './InviteRoommateModal.js';
import CircularProgressBar from '../CircularProgressBar.js';
import { useAuth } from '../AuthContext';
import { UserOutlined } from '@ant-design/icons';

export const GroupMembersTab = (props) => {
    const [membersList, setMembersList] = useState([]);
    const { currentUser, currentGroup } = useAuth();
    const [showInviteModal, setShowInviteModal] = useState(false);
    const [daysUntilRefresh, setDaysUntilRefresh] = useState(0);

    const getFullName = (first, last) => {
        return first + ' ' + last
    }

    const getDaysUntilRefresh = () => {
        const currentDate = new Date();
        const refreshDate = new Date(currentDate.getFullYear(), currentDate.getMonth() + 1, 1);
        return Math.ceil(Math.abs(refreshDate.getTime() - currentDate.getTime()) / (1000 * 60 * 60 * 24));
    }

    const fetchMemberList = () => {
        let groupMemberForm = {
            GroupId: currentGroup['groupId'],
            GroupMember: ''
        }

        axios.post('group-settings/GetGroupMembers', groupMemberForm)
            .then(res => {
                var memberArrRes = res.data['returnedObject']
                var copyArr = cleanArrayCopy(memberArrRes)
                setMembersList(copyArr)
            })
            .catch((error => { console.error(error) }));
    }

    const cleanArrayCopy = (array) => {
        let copy = []
        for (let i = 0; i < array.length; i++) {
            if (array[i] != null) {
                copy.push(array[i])
            }
        }
        return copy
    }

    const removeRoommate = (username) => {
        console.log('remove ' + username)
        let groupMemberForm = {
            GroupId: currentGroup['groupId'],
            GroupMember: username
        }

        axios.post('group-settings/RemoveGroupMembers', groupMemberForm)
            .then(res => {
                console.log(res.data)
                var isSuccessful = res.data['isSuccessful'];
                if (isSuccessful) {
                    fetchMemberList();
                    toast(res.data['message'])
                } else {
                    toast('Try again', res.data['message'])
                }
            })
            .catch((error => { console.error(error) }));
    }

    const toast = (title, desc = '') => {
        notification.open({
            message: title,
            description: desc,
            duration: 5,
            placement: "topRight",
        });
    }

    useEffect(() => {
        fetchMemberList()
        setDaysUntilRefresh(getDaysUntilRefresh());
    }, [])

    return (
        <div className="group-members-tab">
            {currentUser["username"] == currentGroup["owner"] ? (
                <Row gutter={24} style={{ display: "flex", alignItems: "right", justifyContent: "right" }}>
                    <Col span={8}>
                        <Button type="primary" style={Styles.primaryButtonStyle} onClick={() => setShowInviteModal(true)}>Invite roommates</Button>
                        <InviteRoommateModal show={showInviteModal} close={() => setShowInviteModal(false)} group={currentGroup}/>
                    </Col>
                </Row>)
                : (<div></div>)}
            <Row gutter={[24, 24]} align="middle">
                <Col span={10}>
                    <h6 className="padding-top">Group Owner</h6>
                </Col>
                <Col span={4} offset={6}>
                    <h6 className="text-end padding-top">Chore progress</h6>
                </Col>
                <Col span={4}>
                    <h6 className="text-end padding-top">Resets in {daysUntilRefresh} days</h6>
                </Col>
            </Row>
            <Divider plain>
            </Divider>
            <List
                className="group-members-list"
                itemLayout="vertical"
                dataSource={membersList}
                renderItem={(item) => (
                    <List.Item className="padding-vertical">
                        <Skeleton avatar title={false} loading={false} >
                            <List.Item.Meta
                                avatar={<Avatar icon={<UserOutlined />} />}
                                title={getFullName(item.firstName, item.lastName)}
                                description={item.username}
                            />
                            <CircularProgressBar percentage={item.progress}></CircularProgressBar>
                            {currentUser["username"] == currentGroup["owner"] && currentGroup["owner"] != item.username ? (
                                <Button onClick={(e) => {
                                    removeRoommate(item.username)
                                }} type="default" style={Styles.removeGroupMemberButton}>X  Remove Member</Button>)
                                : (<div></div>)}
                        </Skeleton>
                    </List.Item>
                )}
            />
        </div>
    );
};

export default GroupMembersTab;