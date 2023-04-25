import React, { Component, useState, useEffect } from 'react';
import { Modal, ConfigProvider, Button, Row, Col, Image, Space, Input, Form, Switch, notification, Spin, Card, Checkbox } from 'antd';
import * as Styles from '../../styles/ConstStyles.js';
import '../../styles/System.css';
import '../../index.css';
import * as ValidationFuncs from '../../scripts/InputValidation.js';
import axios from 'axios';
import image1 from '../../assets/profileimgs/1.jpg';
import image2 from '../../assets/profileimgs/2.jpg';
import image3 from '../../assets/profileimgs/3.jpg';
import image4 from '../../assets/profileimgs/4.jpg';
import image5 from '../../assets/profileimgs/5.jpg';
import image6 from '../../assets/profileimgs/6.jpg';

export const AddGroceryModal = (props) => {
    const [form] = Form.useForm();
    const profileImages = [image1, image2, image3, image4, image5, image6];
    const [assignments, setAssignments] = useState([])
    const [fetchedRoommates, setFetchedRoommates] = useState(false);
    const [hasValidData, setValidData] = useState(true)
    // Grocery item properties
    const [selectedAssignments, setSelectedAssignments] = useState([])

    const attemptSubmission = () => {
        form.validateFields()
            .then((values) => {
                console.log(values)
                var name = values['groceryItemName']
                // Frontend input validation
                // -> Grocery name (required)
                var nameResult = ValidationFuncs.validate60CharLimit(name)
                if (!nameResult.isSuccessful) {
                    setValidData(false)
                    toast('Grocery name: ' + nameResult.message);
                }

                // -> Grocery notes (optional)
                if (values['groceryItemNotes'] == undefined) {
                    values['groceryItemNotes'] = ''
                }

                var notes = values['groceryItemNotes']
                var noteResult = ValidationFuncs.validateCharacterLimit(notes)
                if (!noteResult.isSuccessful) { 
                    setValidData(false)
                    toast('Grocery notes: ' + noteResult.message);
                }

                // Send data to parent GroceryList component
                if (hasValidData) {
                    // organize modalConfiguration with grocery item properties
                    var modalConfig = {
                        Name: name,
                        Notes: values['groceryItemNotes'],
                        Assignments: selectedAssignments
                    }
                    console.log(modalConfig)
                    props.confirm(modalConfig)
                    closeForm();
                }
            })
            .catch((errorInfo) => { });
    }

    //Grocery Assignment
    const selectAssignment = (index) => {
        var user = assignments[index]['username']
        if (selectedAssignments.includes(user)) {
            //Undo assignment selection
            var copy = selectedAssignments.filter(e => e !== user)
            setSelectedAssignments(copy)
        } else {
            //Make assignment selection
            let copy = []
            for (let i = 0; i < selectedAssignments.length; i++) {
                if (selectedAssignments[i] != null) {
                    copy.push(selectedAssignments[i])
                }
            }
            copy.push(user)
            setSelectedAssignments(copy)
        }
    }

    const fetchCurrentRoommates = () => {
        // axios call to fetch current group members UserProfiles
        let groupMemberForm = {
            GroupId: props.group['groupId']
        }

        console.log(groupMemberForm)
        axios.post('grocerylist/GetCurrentGroupMembers', groupMemberForm)
            .then(res => {
                var isSuccessful = res.data['isSuccessful'];
                if (isSuccessful) {
                    var memberArrRes = res.data['returnedObject']
                    var copyArr = cleanArrayCopy(memberArrRes)
                    setAssignments(copyArr)
                    if (assignments.length > 0) {
                        setFetchedRoommates(true)
                    }
                }
            })
            .catch((error => { console.error(error) }));
    }

    const cleanArrayCopy = (array) => {
        let copy = []
        for (let i = 0; i < array.length; i++) {
            var member = array[i]
            if (member != null && member['username'] != props.user['username']) {
                copy.push(array[i])
            }
        }
        return copy
    }

    const closeForm = () => {
        setSelectedAssignments([])
        setValidData(true)
        props.close()
    }

    const toast = (title, desc = '') => {
        notification.open({
            message: title,
            description: desc,
            duration: 2,
            placement: 'bottom',
        });
    }

    useEffect(() => {
        fetchCurrentRoommates()
    }, []);

    return (
        <Modal
            open={props.show}
            closable={false}
            centered="true"
            footer={null}
            maskClosable="false"
        >
            <div className="padding">
                <h2 className="padding-bottom mulish-font">Add Item</h2>
                <Form id="addGroceryForm" form={form}>
                    <p className="mulish-font" style={{ marginBottom: 2, fontWeight: '100' }}>Name</p>
                    <Form.Item name="groceryItemName">
                        <Input style={Styles.inputFieldStyle} required placeholder="Grocery Item" />
                    </Form.Item>
                    <p className="mulish-font" style={{ marginBottom: 2 }}>Description</p>
                    <Form.Item name="groceryItemNotes">
                        <Input style={Styles.largeTextField} placeholder="Optional notes..." />
                    </Form.Item>
                    <p className="mulish-font" style={{ marginBottom: 2 }}>Select members to assign grocery item to</p>
                    <div className='assignment-selections padding'>
                        {(assignments.length > 0) ?
                            (<Row gutter={24}>
                                {assignments.map((member, i) =>
                                    <Col span={5} style={{ marginLeft: 5 }}>
                                        <Card
                                            onClick={() => selectAssignment(i)}
                                            cover={<Image style={{
                                                borderRadius: '50%',
                                                border: '0.5px solid #555',
                                            }}
                                                src={profileImages[member['image']]} preview={false} />}
                                            style={(selectedAssignments.includes(member['username'])) ? { border: '5px solid #555', borderRadius: '50%', width: 50, height: 50, cursor: 'pointer' } : { borderRadius: '50%', width: 50, height: 50, cursor: 'pointer' }}></Card>
                                        <p style={{ color: 'gray', fontSize: 10, marginLeft: 10 }}>{member['firstName'] + member['lastName']}</p>
                                    </Col>)}
                            </Row>) :
                            (<p className='mulish-font'>Grocery item will be assigned to self</p>)}
                    </div>

                    <div style={{ marginLeft: 80 }}>
                        <Button key="cancel" onClick={closeForm} type="default" style={Styles.defaultButtonModal}>Exit</Button>
                        <Button key="create" onClick={attemptSubmission} htmlType="submit" type="primary" style={Styles.primaryButtonModal}>Save</Button>
                    </div>
                </Form>
            </div>
        </Modal>
    );
};

export default AddGroceryModal;