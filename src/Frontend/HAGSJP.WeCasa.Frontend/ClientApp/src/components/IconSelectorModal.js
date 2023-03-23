import React, { Component, useState, useEffect } from 'react';
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
    const groupColors = ['#668D6A', '#1d3557', '#a2d2ff', '#d4a373', '#ffb703', '#A64253'];
    const [iconType, setIconType] = useState('');
    const [selectedIcon, setSelectedIcon] = useState('')

    const setModalIcons = (type) => {
        // pass in type through props.iconType
        setIconType(type);
    }

    const displayIcons = () => {
        switch (iconType) {
            case 'Profile':
                return (
                    profileImages.map((image, i) =>
                        <Col span={5} style={{ marginLeft: 10 }}>
                            <Card
                                onClick={() => setSelectedIcon(i)} 
                                bordered={true}
                                hoverable
                                cover={<Image style={{
                                    borderRadius: '5%',
                                    border: '0.5px solid #555',
                                    //objectFit: 'cover',
                                }}
                                    src={profileImages[i]} preview={false} />}
                                style={(selectedIcon == i) ? { border: '5px solid #555', borderRadius: 5, width: 100, height: 100 } : { borderRadius: 5, width: 100, height: 100 }}></Card>
                        </Col>)
                );
            case 'Group':
                return (
                    groupColors.map((color, i) =>
                        <Col span={5} style={{ marginLeft: 10 }}>
                            <Card
                                onClick={() => setSelectedIcon(i)} 
                                bordered={true}
                                hoverable
                                style={(selectedIcon == i) ? { border: '5px solid #555', backgroundColor: color, borderRadius: 5, width: 100, height: 100 } : { backgroundColor: color, borderRadius: 5, width: 100, height: 100 }}></Card>
                        </Col>)
                );
            default: return (<div></div>);
        }
    }

    const confirmSelection = () => {
        console.log(selectedIcon)
        if (iconType == 'Profile') {
            //props.setSelectedIcon(selectedIcon)
            props.confirm(selectedIcon)
        } else if (iconType == 'Group') {
            //props.setSelectedIcon(groupColors[selectedIcon])
            props.confirm(groupColors[selectedIcon])
        }
        props.close();
    }

    useEffect(() => {
        setSelectedIcon(0); //Default
        setModalIcons(props.iconType);
    }, []);

    return (
        <Modal
            title="Select an icon"
            open={props.show}
            closable={false}
            maskClosable={true}
            centered="true"
            width={1000}
            footer={null}>
            <div>
                <Space direction="horizonal" size="large" wrap align="center">
                    {displayIcons()}
                </Space>
                <div style={{ paddingTop: 15, marginLeft: 200 }}>
                    <Button type="default" style={Styles.defaultButtonModal} onClick={props.close}>Not yet</Button>
                    <Button type="primary" style={Styles.primaryButtonModal} onClick={confirmSelection}>Let's do it</Button>
                </div>
            </div>
        </Modal>
    );
};

export default IconSelectorModal;