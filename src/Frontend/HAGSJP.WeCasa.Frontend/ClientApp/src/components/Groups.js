import React, { Component, useState } from 'react';
import { Navigate } from 'react-router-dom'
import { useAuth } from './AuthContext';
import { Col, Card, Row } from 'antd';
import { PlusCircleOutlined } from '@ant-design/icons'
import axios from 'axios';
import * as Styles from '../styles/ConstStyles.js';
const { Meta } = Card;

// FOR TESTING
const data = [
    {
        GroupId: 1,
        GroupName: "Group1",
        Icon: "0256D4"
    },
    {
        GroupId: 2,
        GroupName: "OtherGroup",
        Icon: "668D6A"
    }
]

export const Groups = () => {
    const [loading, setLoading] = useState(true);
    const { auth, currentUser } = useAuth();
    const [groups, setGroups] = useState([]);

    const getGroups = () => {
        axios.get('home/GetGroups', currentUser)
            .then(res => {
                var isSuccessful = res.data['isSuccessful'];
                if (isSuccessful) {
                    // setGroups
                    setGroups(data);
                } else {
                    failureGroupView(res.data['message']);
                }
            })
            .catch((error) => { console.error(error) });
    }

    const displayGroupView = () => {
        return groups.map(group => (
            <Card
                hoverable
                style={{ width: 300 }}
                actions={[

                ]}>
                <Meta
                    avatar={group.icon == null ? "D9D9D9" : group.icon}
                    title={group.GroupName}
                />
            </Card>
        ));
    }

    const failureGroupView = (failureMessage) => {
        // display failure to load message
    }

    return (
        <Row gutter={24}>
            <Col span={18}>
                <div id="groups">
                    <Card hoverable style={{ marginTop:16, background:"#ececec", fontFamily: 'Mulish'}}>
                        <Meta
                            avatar={<PlusCircleOutlined />}
                            title="Create Group"
                            type="inner"
                        />
                    </Card>
                </div>
            </Col>
        </Row>
    );
}; 

export default Groups;