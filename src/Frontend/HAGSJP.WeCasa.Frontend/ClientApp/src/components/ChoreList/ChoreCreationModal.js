import React, { Component, useState, useEffect } from 'react';
import { Modal, ConfigProvider, Button, Row, Col, Image, Space, Input, Form, Switch, notification, Spin, Card, Checkbox } from 'antd';
import * as Styles from '../../styles/ConstStyles.js';
import '../../styles/System.css';
import '../../index.css';
import defaultImage from '../../assets/defaultimgs/wecasatemp.jpg';
import * as ValidationFuncs from '../../scripts/InputValidation.js';
import IconSelectorModal from "../IconSelectorModal.js";
import { EditOutlined } from '@ant-design/icons';
import axios from 'axios';
import image1 from '../../assets/profileimgs/1.jpg';
import image2 from '../../assets/profileimgs/2.jpg';
import image3 from '../../assets/profileimgs/3.jpg';
import image4 from '../../assets/profileimgs/4.jpg';
import image5 from '../../assets/profileimgs/5.jpg';
import image6 from '../../assets/profileimgs/6.jpg';


const ChoreCreationModal = (props) => {
    const profileImages = [image1, image2, image3, image4, image5, image6];
    const [form] = Form.useForm();
    const [asignments, setAssignments] = useState([])
    const [selectedAssignments, setSelectedAssignments] = useState([])

    const selectAssignment = (index) => {
        if (selectedAssignments.includes(index)) {
            //Undo assignment selection
            var copy = selectedAssignments.filter(e => e !== index)
            setSelectedAssignments(copy)
        } else {
            //Make assignment selection
            let copy = []
            for (let i = 0; i < selectedAssignments.length; i++) {
                if (selectedAssignments[i] != null) {
                    copy.push(selectedAssignments[i])
                }
            }
            copy.push(index)
            setSelectedAssignments(copy)
        }
    }

    useEffect(() => {
        if (props.fetchedRoommates) {
            setAssignments(props.currentMembers)
        } else {
            setAssignments([])
        }
    }, []);

    return (<Modal
        open={props.show}
        closable={false}
        centered="true"
        footer={null}
        maskClosable="false"
    >
        <div className="padding">
            <h2 className="padding-bottom mulish-font">Add chore</h2>
            <Form id="groupCreationForm" onFinish={props.confirm} form={form}>
                <p className="mulish-font" style={{ marginBottom: 2 }}>Name</p>
                <Form.Item name="choreName">
                    <Input style={Styles.inputFieldStyle} required placeholder="Chore Name" />
                </Form.Item>
                <p className="mulish-font" style={{ marginBottom: 2 }}>Notes</p>
                <Form.Item name="choreNotes">
                    <Input style={Styles.inputFieldStyle} required placeholder="Notes" />
                </Form.Item>
                <p className="mulish-font" style={{ marginBottom: 2 }}>Days</p>
                <Form.Item name="isRepeated" valuePropName="daysChecked">
                    <Checkbox value="MON" style={{ lineHeight: '32px' }}>Mon</Checkbox>
                    <Checkbox value="TUES" style={{ lineHeight: '32px' }}>Tues</Checkbox>
                    <Checkbox value="WED" style={{ lineHeight: '32px' }}>Wed</Checkbox>
                    <Checkbox value="THURS" style={{ lineHeight: '32px' }}>Thurs</Checkbox>
                    <Checkbox value="FRI" style={{ lineHeight: '32px' }}>Fri</Checkbox>
                    <Checkbox value="SAT" style={{ lineHeight: '32px' }}>Sat</Checkbox>
                    <Checkbox value="SUN" style={{ lineHeight: '32px' }}>Sun</Checkbox>
                </Form.Item>
                <p className="mulish-font" style={{ marginBottom: 2 }}>Repeats</p>
                <Form.Item name="isRepeated" valuePropName="repeatChecked">
                    <Checkbox value="M" style={{ lineHeight: '32px' }}>Monthly</Checkbox>
                    <Checkbox value="B" style={{ lineHeight: '32px' }}>Bi-weekly</Checkbox>
                    <Checkbox value="W" style={{ lineHeight: '32px' }}>Weekly</Checkbox>
                    <Checkbox value="D" style={{ lineHeight: '32px' }}>Daily</Checkbox>
                </Form.Item>
                <p className="mulish-font" style={{ marginBottom: 2 }}>Select members to assign to</p>
                <div className='assignment-selections padding'>
                    <Row gutter={24}>
                        {asignments.map((member, i) =>
                        <Col span={5} style={{ marginLeft: 5 }}>
                            <Card
                                onClick={() => selectAssignment(i)} 
                                cover={<Image style={{
                                    borderRadius: '50%',
                                    border: '0.5px solid #555',
                                }}
                                        src={profileImages[member['image']]} preview={false} />}
                                    style={(selectedAssignments.includes(i)) ? { border: '5px solid #555', borderRadius: '50%', width: 50, height: 50, cursor: 'pointer' } : { borderRadius: '50%', width: 50, height: 50, cursor: 'pointer' }}></Card>
                                <p style={{ color: 'gray', fontSize: 10, marginLeft: 10 }}>{member['firstName'] + member['lastName']}</p>
                        </Col>)}
                    </Row>
                </div>
                <div style={{ marginLeft: 80 }}>
                    <Button key="cancel" onClick={props.close} type="default" style={Styles.defaultButtonModal}>Exit</Button>
                    <Button key="create" htmlType="submit" type="primary" style={Styles.primaryButtonModal}>Save</Button>
                </div>
            </Form>
        </div>
    </Modal>);
}

export default ChoreCreationModal;
