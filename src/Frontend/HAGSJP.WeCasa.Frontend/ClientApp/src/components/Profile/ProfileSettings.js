import React, { Component, useState, useEffect } from 'react';
import { NavLink, NavItem, Nav } from 'reactstrap';
import { useNavigate } from 'react-router-dom';
import { Modal, Avatar, notification, Button, Card, Row, Col, Image, Space, Input, Form, Switch, Tabs, Divider } from 'antd';
import { UserOutlined } from '@ant-design/icons'
import * as Styles from '../../styles/ConstStyles.js';
import '../../styles/System.css';
import '../../index.css';
import axios from 'axios';
import { useAuth } from '../Auth/AuthContext';
import image1 from '../../assets/profileimgs/1.jpg';
import image2 from '../../assets/profileimgs/2.jpg';
import image3 from '../../assets/profileimgs/3.jpg';
import image4 from '../../assets/profileimgs/4.jpg';
import image5 from '../../assets/profileimgs/5.jpg';
import image6 from '../../assets/profileimgs/6.jpg';
import DeleteAccountModal from '../Account/DeleteAccountModal.js';
import IconSelectorModal from "../IconSelectorModal.js";
import { EditOutlined } from '@ant-design/icons';
import * as ValidationFuncs from '../../scripts/InputValidation.js';

export const ProfileSettings = () => {
    const { currentUser, setCurrentUser, setCurrentGroup } = useAuth();
    const images = [image1, image2, image3, image4, image5, image6];
    const [showIconModal, setShowIconModal] = useState(false);
    const [selectedIcon, setSelectedIcon] = useState('')
    const [firstName, setFirstName] = useState("");
    const [lastName, setLastName] = useState("");

    const attemptSave = () => {
        let profileForm = {
            Email: currentUser['username']
        }

        //Edit full name
        if (firstName != '' || lastName != '') {
            profileForm['FirstName'] = (firstName == '') ? ' ' : firstName
            profileForm['LastName'] = (lastName == '') ? ' ' : lastName
            axios.post('edit-profile/UpdateFullName', profileForm)
                .then(res => {
                    var isSuccessful = res.data['isSuccessful'];
                    if (isSuccessful) {
                        updateCurrentUser(profileForm);
                        successProfileSettingsView(res.data['message']);
                    } else {
                        failureProfileSettingsView(res.data['message']);
                    }
                })
                .catch((error => { console.error(error) }));
        }

        let profileFormIcon = {
            Email: currentUser['username']
        }
        //Edit icon
        if (selectedIcon != currentUser["image"]) {
            profileFormIcon['Image'] = selectedIcon
            console.log(profileFormIcon)
            axios.post('edit-profile/UpdateProfileIcon', profileFormIcon)
                .then(res => {
                    var isSuccessful = res.data['isSuccessful'];
                    if (isSuccessful) {
                        updateCurrentUser(profileFormIcon);
                        successProfileSettingsView(res.data['message']);
                    } else {
                        failureProfileSettingsView(res.data['message']);
                    }
                })
                .catch((error => { console.error(error) }));
        }
    }

    const successProfileSettingsView = (successMessage) => {
        notification.open({
            message: "Update successful.",
            description: successMessage,
            duration: 10,
            placement: "topLeft",
        });
    }

    const failureProfileSettingsView = (failureMessage) => {
        notification.open({
            message: "An error occurred.",
            description: failureMessage,
            duration: 10,
            placement: "topLeft",
        });
    }

    const updateCurrentUser = (profileForm) => {
        //After updating name or icon
        axios.post('edit-profile/GetUserProfile', profileForm)
            .then(res => {
                var isSuccessful = res.data['isSuccessful'];
                if (isSuccessful) {
                    var userProfile = res.data['returnedObject']
                    setCurrentUser(userProfile);
                } else {
                    failureProfileSettingsView(res.data['message']);
                }
            })
            .catch((error => { console.error(error) }));
        
    }

    useEffect(() => {
        setSelectedIcon(currentUser["image"]); 
    }, []);

    return (
        <div className="acc-settings-page padding">
            <h1>Edit Profile</h1>
            <Row gutter={[24, 24]} align="middle">
                <Col span={4} className="user-icon">
                    {(currentUser.image == null) ?
                        (<div>
                            <Card
                                cover={<Image style={{
                                    height: '150px',
                                    width: '150px',
                                    borderRadius: '5%',
                                    position: 'absolute',
                                    cursor: 'pointer',
                                    padding: '5px',
                                    border: '1px solid #555',
                                    objectFit: 'cover',
                                    margin: '5%',
                                    zIndex: '0'
                                }} preview={false} src={images[0]}></Image>}
                                onClick={() => setShowIconModal(true)}
                            >
                                <EditOutlined style={{ color: 'white', backgroundColor: 'black', marginLeft: 90, marginTop: 90, zIndex: 9, borderRadius: 50, height: 30, width: 30, opacity: 0.9, justifyContent: 'center' }} />
                            </Card>
                        </div>) :
                        (<div>
                            <Card
                                cover={<Image style={{
                                    height: '150px',
                                    width: '150px',
                                    borderRadius: '5%',
                                    position: 'absolute',
                                    cursor: 'pointer',
                                    padding: '5px',
                                    border: '1px solid #555',
                                    objectFit: 'cover',
                                    margin: '5%',
                                    zIndex: '0'
                                }} preview={false} src={images[selectedIcon - 1]}></Image>}
                             onClick={() => setShowIconModal(true)}
                             >
                                <EditOutlined style={{ color: 'white', backgroundColor: 'black', marginLeft: 90, marginTop: 90, zIndex: 9, borderRadius: 50, height: 30, width: 30, opacity: 0.9, justifyContent: 'center' }} />
                            </Card>
                        </div>)}
                    <IconSelectorModal show={showIconModal} close={() => setShowIconModal(false)} confirm={setSelectedIcon} iconType='Profile' />
                </Col>
                <Col span={20} className="user-info">
                    <div className="padding">
                        <Form>
                            <Form.Item>
                                <Input style={Styles.inputFieldStyle} size="large" placeholder="First name" value={firstName} onChange={(e) => { setFirstName(e.target.value) }} />
                            </Form.Item>
                            <Form.Item>
                                <Input style={Styles.inputFieldStyle} size="large" placeholder="Last name" value={lastName} onChange={(e) => { setLastName(e.target.value) }} />
                            </Form.Item>
                            <Button type="primary" htmlType="submit" onClick={attemptSave} style={Styles.primaryButtonStyle}>Save</Button>
                        </Form>
                    </div>
                </Col>
            </Row>
        </div>
    );
};

export default ProfileSettings;
