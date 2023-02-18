import React, { Component } from 'react';
import { Modal, ConfigProvider, Button, Row, Col, Image, Space, Input, Form } from 'antd';
import * as Styles from '../styles/ConstStyles.js';
import '../styles/System.css';
import '../index.css';
import defaultImage from '../assets/defaultimgs/wecasatemp.jpg';

const CreateGroupModal = (props) => {
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
            
            <div class="create-group">
                <h2><b>Create group</b></h2>
                <Form id="groupCreationForm">
                <Row gutter={[24, 24]}>
                    <Col span={8} class="group-icon-selection">
                        <Space size={8}>
                            <Image style={Styles.groupIconSelection} src={defaultImage} preview={false} height="100px" width="100px" />
                        </Space>
                    </Col>
                    <Col span={16} class="group-name-input">
                            <Form.Item name="groupName">
                                <Input style={Styles.inputFieldStyle} placeholder="Group name" />
                            </Form.Item>
                    </Col>
                </Row>

                <h6>Invite roommates</h6>
                <Row gutter={[24, 24]}>
                    <Col span={12} class="invite-group-members">

                    </Col>
                    <Col span={12} class="invite-group-members-btn">

                    </Col>
                </Row>

                <h6>Customize group features</h6>
                <Row gutter={24}>
                    <Col span={18} class="invite-group-members">
                        <div class="group-feature-container">

                        </div>
                    </Col>
                    </Row>
                  </Form>
            </div>
        </Modal>
    );
}

export default CreateGroupModal;