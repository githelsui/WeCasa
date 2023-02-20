import React, { Component, useState, useEffect } from 'react';
import { Modal, ConfigProvider, Button, Row, Col, Image, Space, Input, Form, Switch, Tabs} from 'antd';
import * as Styles from '../styles/ConstStyles.js';
import '../styles/System.css';
import '../index.css';
import GroupSettingsTab from './GroupSettingsTab'
import GroupMembersTab from './GroupMembersTab'
import defaultImage from '../assets/defaultimgs/wecasatemp.jpg';
const TabPane = Tabs.TabPane;

export const GroupSettings = (props) => {
    const [newIcon, setNewIcon] = useState(null);
    const [currentGroup, setCurrentGroup] = useState(null);
    const [refreshGroupMembers, setRefreshGroupMembers] = useState(false)
    const [refreshSettings, setRefreshSettings] = useState(true)

    const tabItemClick = (key) => {
        console.log('tab click', key);
        if (key == 1) {
            setRefreshGroupMembers(true)
            setRefreshSettings(false)
        } else {
            setRefreshGroupMembers(false)
            setRefreshSettings(true)
        }
    };

    //Development Only:
    var tempGroup = {
        GroupId: 1,
        GroupName: "Group1",
        Icon: "#0256D4",
        Features: ["Budget Bar"]
    }

    return (
        <div className="group-settings-page padding">
            <div className="group-settings-header padding-vertical">
                <Row gutter={[24, 24]} align="middle">
                    <Col span={4} className="group-icon">
                        <Image style={Styles.groupIconSelection} src={defaultImage} preview={false} height="80px" width="80px" />
                    </Col>
                    <Col span={8} className="group-name">
                        <h2 className="padding-bottom mulish-font"><b>{tempGroup.GroupName}</b></h2>
                    </Col>
                </Row>
            </div>

            <Tabs defaultActiveKey="2" onChange={tabItemClick} destroyInactiveTabPane>
                <TabPane tab="Group Members" key="1"><GroupMembersTab group={tempGroup} refresh={refreshGroupMembers} /></TabPane>
                <TabPane tab="Settings" key="2"><GroupSettingsTab group={tempGroup} /></TabPane>
            </Tabs>


           
        </div>
        );
};

export default GroupSettings;