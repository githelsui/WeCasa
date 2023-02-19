import React, { Component, useState } from 'react';
import { Navigate } from 'react-router-dom'
import { useAuth } from './AuthContext';
import { Col, Card, Row, Space, Avatar, Button } from 'antd';
import { PlusCircleOutlined } from '@ant-design/icons'
import axios from 'axios';
import * as Styles from '../styles/ConstStyles.js';
import CreateGroupModal from './CreateGroupModal.js'
const { Meta } = Card;

// FOR TESTING
const data = [
    {
        GroupId: 1,
        GroupName: "Group1",
        Icon: "#0256D4"
    },
    {
        GroupId: 2,
        GroupName: "OtherGroup",
        Icon: "#668D6A"
    }
]

export const Groups = () => {
    const [loading, setLoading] = useState(true);
    const { auth, currentUser } = useAuth();
    const [groups, setGroups] = useState([]);
    const [showModal, setShowModal] = useState(false);

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
                <Col span={10} style={{marginTop:16}}>
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
    }


    const attemptGroupCreation = () => {

    }

    return (
        <div>
            <Space direction="horizonal" size={32}>
                {displayGroupView(data)}
            </Space>
            <Row gutter={24} style={{ display: "flex"}}>
                <Col span={24}>
                    <Card hoverable style={{ marginTop:16, marginLeft:200, marginRight:200, background:"#ececec", fontFamily: 'Mulish'}}>
                        <Meta
                            onClick={() => setShowModal(true)}
                            avatar={<PlusCircleOutlined />}
                            title="Create Group"
                            type="inner"
                            style={{textAlign:"center", display:"flex"}}
                        />
                    </Card>
                </Col>
            </Row>
            <CreateGroupModal show={showModal} close={() => setShowModal(false)} confirm={attemptGroupCreation} />
        </div>
    );
}; 

export default Groups;