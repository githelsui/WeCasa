import React, { Component, useState } from 'react';
import { Modal, ConfigProvider, Button, Row, Col, Image, Space, Input, Form, Switch, Tabs} from 'antd';
import * as Styles from '../styles/ConstStyles.js';
import '../styles/System.css';
import '../index.css';
import GroupSettingsTab from './GroupSettingsTab'
import defaultImage from '../assets/defaultimgs/wecasatemp.jpg';
const TabPane = Tabs.TabPane;

export const GroupSettings = (props) => {
    //Development Only:
    const tempFeatureDesc = "DESCRIPTION: Lorem ipsum dolor sit amet consectetur. In non proin et interdum at. Vel mi praesent tincidunt tincidunt odio at mauris nisl cras."
    const [newIcon, setNewIcon] = useState(null);

    const tabItemClick = (key) => {
        console.log('tab click', key);
    };

    return (
        <div className="group-settings-page padding">
            <div className="group-settings-header padding-vertical">
                <Row gutter={[24, 24]} align="middle">
                    <Col span={4} className="group-icon">
                        <Image style={Styles.groupIconSelection} src={defaultImage} preview={false} height="80px" width="80px" />
                    </Col>
                    <Col span={8} className="group-name">
                        <h2 className="padding-bottom mulish-font"><b>Group name</b></h2>
                    </Col>
                </Row>
            </div>

            <Tabs defaultActiveKey="2" onChange={tabItemClick}>
                <TabPane tab="Group Members" key="1">contents</TabPane>
                <TabPane tab="Settings" key="2"><GroupSettingsTab/></TabPane>
            </Tabs>


           
        </div>
        );
};

export default GroupSettings;