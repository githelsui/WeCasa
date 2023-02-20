import React, { Component, useState, useEffect } from 'react';
import { List, Avatar, ConfigProvider, Button, Row, Col, Image, Space, Input, Form, Skeleton, notification } from 'antd';
import * as Styles from '../styles/ConstStyles.js';
import '../styles/System.css';
import '../index.css';
import axios from 'axios';
import defaultImage from '../assets/defaultimgs/wecasatemp.jpg';

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

    const getFullName = (first, last) => {
        return first + ' ' + last
    }

    const fetchMemberList = () => {
        let groupMemberForm = {
            GroupId: groupData.GroupId,
            GroupMember: ''
        }

        axios.post('group-settings/GetGroupMembers', groupMemberForm)
            .then(res => {
                console.log(res.data)
                var memberArrRes = res.data['returnedObject']
                console.log(memberArrRes)
                setMembersList(memberArrRes)
            })
            .catch((error => { console.error(error) }));
    }

    const removeRoommate = (username) => {
        console.log('remove ' + username)
        let groupMemberForm = {
            GroupId: groupData.GroupId,
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
        <div className="group-members-tab padding">
            <List
                className="group-members-list"
                itemLayout="vertical"
                dataSource={membersList}
                renderItem={(item) => (
                    <List.Item className="padding-vertical">
                        <Skeleton avatar title={false} loading={false} >
                            <List.Item.Meta 
                                avatar={<Avatar src={defaultImage} />}
                                title={getFullName(item.firstName, item.lastName)}
                                description={item.username}
                            />
                            <Button onClick={(e) => {
                                removeRoommate(item.username)
                            }} type="default" style={Styles.removeGroupMemberButton}>X  Remove Member</Button>
                        </Skeleton>
                    </List.Item>
                )}
            />
        </div>
    );
};

export default GroupMembersTab;