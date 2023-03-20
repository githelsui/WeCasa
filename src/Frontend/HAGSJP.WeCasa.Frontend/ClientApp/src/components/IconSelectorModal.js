import React, { Component } from 'react';
import { Modal, ConfigProvider, Button, Form, Input, Row, Col, notification, Card, Image, Space} from 'antd';
import * as Styles from '../styles/ConstStyles.js';
import '../styles/System.css';
import '../index.css';
import * as ValidationFuncs from '../scripts/InputValidation.js';
import { useAuth } from './AuthContext.js';
import axios from 'axios';
import image1 from '../assets/profileimgs/1.jpg';
import image2 from '../assets/profileimgs/2.jpg';
import image3 from '../assets/profileimgs/3.jpg';
import image4 from '../assets/profileimgs/4.jpg';
import image5 from '../assets/profileimgs/5.jpg';
import image6 from '../assets/profileimgs/6.jpg';
const { Meta } = Card;

export const IconSelectorModal = (props) => {
    const profileImages = [image1, image2, image3, image4, image5, image6];

    return (
        <Modal
            title="Select an icon"
            open={true}
            closable={true}
            onCancel={props.close}
            centered="true"
            width={800}
            footer={null}>
            <div>
                <Space direction="horizonal" size="large" wrap align="center">
                {profileImages.map((image, i) =>
                    <Col span={5} style={{marginLeft: 2}}>
                        <Card
                        bordered={true}
                        hoverable
                        cover={<Image style={{
                            borderRadius: '5%',
                            border: '1px solid #555',
                            objectFit: 'cover',
                            margin: '5%'
                        }}
                            src={profileImages[i]} preview={false} />}
                            style={{ width: 100, height: 100 }}></Card>
                    </Col>)}
                </Space>
                <div style={{ paddingTop: 15, marginLeft: 100 }}>
                    <Button type="default" style={Styles.defaultButtonModal}>Not yet</Button>
                    <Button type="primary" style={Styles.primaryButtonModal}>Let's do it</Button>
                </div>
            </div>
        </Modal>
    );
};

export default IconSelectorModal;