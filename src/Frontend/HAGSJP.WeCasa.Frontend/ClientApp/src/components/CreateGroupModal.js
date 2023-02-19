import React, { Component } from 'react';
import { Modal, ConfigProvider, Button, Row, Col, Image, Space, Input, Form, Switch } from 'antd';
import * as Styles from '../styles/ConstStyles.js';
import '../styles/System.css';
import '../index.css';
import defaultImage from '../assets/defaultimgs/wecasatemp.jpg';

const CreateGroupModal = (props) => {
    //Development Only:
    const tempFeatureDesc = "DESCRIPTION: Lorem ipsum dolor sit amet consectetur. In non proin et interdum at. Vel mi praesent tincidunt tincidunt odio at mauris nisl cras."

    return (
        <Modal
            open={props.show}
            closable={false}
            centered="true"
            footer={[
                <Button key="cancel" onClick={props.close} type="default" style={Styles.defaultButtonModal}>Exit</Button>,
                <Button key="logout" onClick={props.confirm} type="primary" style={Styles.primaryButtonModal}>Create Group</Button>
            ]}
            maskClosable="false"
            >
            
            <div class="padding">
                <h2 class="padding-bottom"><b>Create group</b></h2>
                <Form id="groupCreationForm">
                    <Row gutter={[24, 24]} align="middle">
                        <Col span={8} class="group-icon-selection">
                            <Image style={Styles.groupIconSelection} src={defaultImage} preview={false} height="120px" width="120px" />
                        </Col>
                    <Col span={16} class="group-name-input">
                            <Form.Item name="groupName">
                                <Input style={Styles.inputFieldStyle} placeholder="Group name" />
                            </Form.Item>
                    </Col>
                </Row>

                <h5 class="padding-top mulish-font">Invite roommates</h5>
                <Row gutter={[24, 24]}>
                    <Col span={18} class="invite-group-members">
                            <Form.Item name="memberUsername">
                                <Input style={Styles.inputFieldStyle} placeholder="Roommate Email/Username" />
                            </Form.Item>
                    </Col>
                    <Col span={6} class="invite-group-members-btn">
                            <Button style={Styles.primaryButtonStyleNoMargins} type="primary" htmlType="submit">Invite</Button>
                    </Col>
                </Row>

                <h5 class="mulish-font">Customize group features</h5>
                    <div class="group-feature-row">
                        <Row gutter={24} style={Styles.groupFeatureContainer}>
                            <Col span={18}>
                                <h6 class="mulish-font">Budget Bar</h6>
                            </Col>
                            <Col span={6}>
                                <Switch defaultChecked="true" checkedChildren="ON" uncheckedChildren="OFF" />
                            </Col>
                        </Row>
                        <p class="group-feature-desc-p">{tempFeatureDesc}</p>
                    </div>

                    <div class="group-feature-row">
                        <Row gutter={24} style={Styles.groupFeatureContainer}>
                            <Col span={18}>
                                <h6 class="mulish-font">Bulletin Board</h6>
                            </Col>
                            <Col span={6}>
                                <Switch defaultChecked="true" checkedChildren="ON" uncheckedChildren="OFF" />
                            </Col>
                        </Row>
                        <p class="group-feature-desc-p">{tempFeatureDesc}</p>
                    </div>

                    <div class="group-feature-row">
                        <Row gutter={24} style={Styles.groupFeatureContainer}>
                            <Col span={18}>
                                <h6 class="mulish-font">Calendar</h6>
                            </Col>
                            <Col span={6}>
                                <Switch defaultChecked="true" checkedChildren="ON" uncheckedChildren="OFF" />
                            </Col>
                        </Row>
                        <p class="group-feature-desc-p">{tempFeatureDesc}</p>
                    </div>

                    <div class="group-feature-row">
                        <Row gutter={24} style={Styles.groupFeatureContainer}>
                            <Col span={18}>
                                <h6 class="mulish-font">Finances</h6>
                            </Col>
                            <Col span={6}>
                                <Switch defaultChecked="true" checkedChildren="ON" uncheckedChildren="OFF" />
                            </Col>
                        </Row>
                        <p class="group-feature-desc-p">{tempFeatureDesc}</p>
                    </div>
                </Form>
            </div>
        </Modal>
    );
}

export default CreateGroupModal;