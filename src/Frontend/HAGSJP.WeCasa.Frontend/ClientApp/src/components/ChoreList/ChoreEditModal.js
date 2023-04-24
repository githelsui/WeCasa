import React, { Component, useState, useEffect } from 'react';
import { Modal, ConfigProvider, Button, Row, Col, Image, Space, Input, Form, Switch, notification, Spin, Card, Checkbox, Radio } from 'antd';
import * as Styles from '../../styles/ConstStyles.js';
import '../../styles/System.css';
import '../../index.css';
import * as ValidationFuncs from '../../scripts/InputValidation.js';
import image1 from '../../assets/profileimgs/1.jpg';
import image2 from '../../assets/profileimgs/2.jpg';
import image3 from '../../assets/profileimgs/3.jpg';
import image4 from '../../assets/profileimgs/4.jpg';
import image5 from '../../assets/profileimgs/5.jpg';
import image6 from '../../assets/profileimgs/6.jpg';
import axios from 'axios';

const ChoreEditModal = (props) => {
    const profileImages = [image1, image2, image3, image4, image5, image6];
    const [form] = Form.useForm();
    const [fetchedRoommates, setFetchedRoommates] = useState(false);
    const [assignments, setAssignments] = useState([])
    //Chore properties
    const [selectedAssignments, setSelectedAssignments] = useState([])
    const [hasValidData, setValidData] = useState(true)
    const [choreRepeats, setChoreRepeats] = useState('')

    const attemptSubmission = () => {
        form.validateFields()
            .then((values) => {
                console.log(values)
                var name = values['choreName']
                var daysReq = values['choreDaysReq']

                // Frontend input validation
                // -> Chore name (required)
                if (name == undefined) {
                    name = props.chore['name']
                }
                var nameResult = ValidationFuncs.validate60CharLimit(name)
                console.log(nameResult)
                if (!nameResult.isSuccessful) {
                    setValidData(false)
                    toast('Chore name: ' + nameResult.message);
                }

                // -> Chore notes (optional)
                if (values['choreNotes'] == undefined) {
                    values['choreNotes'] = props.chore['notes']
                }

                var notes = values['choreNotes']
                var noteResult = ValidationFuncs.validateCharacterLimit(notes)
                if (!noteResult.isSuccessful) {
                    setValidData(false)
                    toast('Chore notes: ' + noteResult.message);
                }

                // -> Chore days (required)
                if (daysReq == undefined || daysReq.length == 0) {
                    setValidData(false)
                    toast('Select the days the chore is set for.')
                }

                // Send data to parent ChoreList component
                if (hasValidData && name != undefined && (daysReq != undefined || daysReq.length > 0)) {
                    // organize modalConfiguration with chore properties
                    let choreForm = {
                        CurrentUser: props.user['username'],
                        GroupId: props.chore['groupId'],
                        Name: name,
                        Notes: notes,
                        Repeats: choreRepeats,
                        Days: daysReq,
                        AssignedTo: (selectedAssignments.length == 0 ? [props.user['username']] : selectedAssignments),
                        ChoreId: props.chore['choreId']
                    }
                    console.log(choreForm)
                    props.confirm(choreForm)
                    values['choreDaysReq'] = undefined
                    closeForm();
                }
            })
            .catch((errorInfo) => { });
    }

    const getAssignedUsernames = (assignments) => {
        var arr = []
        for (let i = 0; i < assignments.length; i++) {
            var userProfile = assignments[i]
            var username = userProfile['username']
            arr.push(username)
        }
        return arr;
    }

    const closeForm = () => {
        setValidData(true)
        props.close()
    }

    const fetchCurrentRoommates = () => {
        // axios call to fetch current group members UserProfiles
        let groupMemberForm = {
            GroupId: props.chore['groupId']
        }

        axios.post('chorelist/GetCurrentGroupMembers', groupMemberForm)
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

    //Chore Repeats
    const repeatOnChange = (e) => {
        setChoreRepeats(e.target.value);
    };

    //Chore Assignments
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

    const toast = (title, desc = '') => {
        notification.open({
            message: title,
            description: desc,
            duration: 2,
            placement: 'bottom',
        });
    }

    const setDefaultAssignments = () => {
        var arr = getAssignedUsernames(props.chore['assignedTo'])
        setSelectedAssignments(arr)
    }

    const defaultDaysLabel = () => {
        var days = props.chore['days']
        var label = ''
        for (let i = 0; i < days.length; i++) {
            if (i == days.length - 1) {
                label += days[i]
            } else {
                label += days[i] + ', '
            }
        }
        return label
    }

    const defaultRepeatsLabel = () => {
        if (props.chore['repeats'] == '' || props.chore['repeats'] == undefined) {
            return 'No Repeats'
        } else {
            return props.chore['repeats']
        }
       
    }

    useEffect(() => {
        fetchCurrentRoommates()
        setDefaultAssignments()
    }, []);

    return (<Modal
        open={props.show}
        closable={false}
        centered="true"
        footer={null}
        width={600}
        maskClosable="false"
    >
        <div className="padding">
            <h2 className="padding-bottom mulish-font">Edit chore</h2>
            <Form id="choreCreationForm" form={form}>
                <p className="mulish-font" style={{ marginBottom: 2, fontWeight: '100' }}>Name</p>
                <Form.Item name="choreName">
                    <Input style={Styles.inputFieldStyle} defaultValue={props.chore['name']}/>
                </Form.Item>
                <p className="mulish-font" style={{ marginBottom: 2 }}>Notes</p>
                <Form.Item name="choreNotes">
                    <Input style={Styles.largeTextField} defaultValue={props.chore['notes']}/>
                </Form.Item>
                <p className="mulish-font" style={{ marginBottom: 2 }}>Days</p>
                <p className="mulish-font">Originally set for <i>{defaultDaysLabel()}</i></p>
                <Form.Item name="choreDaysReq">
                    <Checkbox.Group>
                        <Checkbox value="MON" style={{ lineHeight: '32px' }}>Mon</Checkbox>
                        <Checkbox value="TUES" style={{ lineHeight: '32px' }}>Tues</Checkbox>
                        <Checkbox value="WED" style={{ lineHeight: '32px' }}>Wed</Checkbox>
                        <Checkbox value="THURS" style={{ lineHeight: '32px' }}>Thurs</Checkbox>
                        <Checkbox value="FRI" style={{ lineHeight: '32px' }}>Fri</Checkbox>
                        <Checkbox value="SAT" style={{ lineHeight: '32px' }}>Sat</Checkbox>
                        <Checkbox value="SUN" style={{ lineHeight: '32px' }}>Sun</Checkbox>
                    </Checkbox.Group>
                </Form.Item>
                <p className="mulish-font" style={{ marginBottom: 2 }}>Repeats</p>
                <p className="mulish-font">Originally set as <i>{defaultRepeatsLabel()}</i></p>
                <Radio.Group onChange={repeatOnChange} value={choreRepeats}>
                    <Radio value="Monthly" style={{ lineHeight: '32px' }}>Monthly</Radio>
                    <Radio value="Bi-weekly" style={{ lineHeight: '32px' }}>Bi-weekly</Radio>
                    <Radio value="Weekly" style={{ lineHeight: '32px' }}>Weekly</Radio>
                </Radio.Group>
                <p className="mulish-font padding-top" style={{ marginBottom: 2 }}>Select members to reassign chore to.</p>
                <p className="mulish-font"><i>If none are selected, chore is assigned to you.</i></p>
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
                                    <p style={{ color: 'gray', fontSize: 10, marginLeft: 10 }}>{member['firstName'] + ' ' + member['lastName']}</p>
                                </Col>)}
                        </Row>) :
                        (<p className='mulish-font'>Chore will be assigned to self</p>)}
                </div>
                <div style={{ marginLeft: 80 }}>
                    <Button key="cancel" onClick={closeForm} type="default" style={Styles.defaultButtonModal}>Exit</Button>
                    <Button key="create" onClick={attemptSubmission} htmlType="submit" type="primary" style={Styles.primaryButtonModal}>Save</Button>
                </div>
                <Button style={{ border: 1, marginLeft: 180, marginTop: 25, color: 'red' }} onClick={() => { props.deleteChore(); closeForm(); }} >Delete Chore</Button>
            </Form>
        </div>
    </Modal>);
}

export default ChoreEditModal;
