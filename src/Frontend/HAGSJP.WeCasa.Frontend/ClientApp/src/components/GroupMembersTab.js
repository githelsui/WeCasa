import React, { Component, useState, useEffect } from 'react';
import { List, Avatar, ConfigProvider, Button, Row, Col, Image, Space, Input, Form, Divider, Skeleton, notification } from 'antd';
import * as Styles from '../styles/ConstStyles.js';
import '../styles/System.css';
import '../index.css';
import axios from 'axios';
import defaultImage from '../assets/defaultimgs/wecasatemp.jpg';
import { useAuth } from './AuthContext';
import { UserOutlined } from '@ant-design/icons'

// FOR TESTING
const data = [
    {
        Username: 'githelsuico@gmail.com',
        FirstName: "githel",
        LastName: "suico"
    },
    {
        Username: 'apple@gmail.com',
        FirstName: "adam",
        LastName: "smith"
    },
    {
        Username: 'apple@gmail.com',
        FirstName: "adam",
        LastName: "smith"
    },
    {
        Username: 'apple@gmail.com',
        FirstName: "adam",
        LastName: "smith"
    }
]

const groupData = {
    GroupId: 1,
    Owner: 'test@gmail.com'
}

export const GroupMembersTab = (props) => {
    const [membersList, setMembersList] = useState([]);
    const { currentUser, currentGroup } = useAuth();

    const getFullName = (first, last) => {
        return first + ' ' + last
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
    }, [])

    return (
        <div className="group-members-tab">
            {currentUser["username"] == currentGroup["owner"] ? (
                <Row gutter={24} style={{ display: "flex", alignItems: "right", justifyContent: "right" }}>
                    <Col span={8}>
                        <Button type="primary" style={Styles.primaryButtonStyle}>Invite roommates</Button>
                    </Col>
                </Row>)
                : (<div></div>)}
            <h6 className="padding-top">Group Owner</h6>
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