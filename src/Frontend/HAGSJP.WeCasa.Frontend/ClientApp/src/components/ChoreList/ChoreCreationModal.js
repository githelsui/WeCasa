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

const ChoreCreationModal = (props) => {
    const profileImages = [image1, image2, image3, image4, image5, image6];
    const [form] = Form.useForm();
    const [assignments, setAssignments] = useState([])
    //Chore properties
    const [selectedAssignments, setSelectedAssignments] = useState([])
    const [hasValidData, setValidData] = useState(true)
    const [choreNotes, setChoreNotes] = useState('')
    const [choreRepeats, setChoreRepeats] = useState('')

    const attemptSubmission = () => {
        form.validateFields()
            .then((values) => {
                console.log(values)
                var name = values['choreName']
                var daysReq = values['choreDaysReq']

                // Frontend input validation
                // -> Chore name (required)
                var nameResult = ValidationFuncs.validate60CharLimit(name)
                console.log(nameResult)
                if (!nameResult.isSuccessful) {
                    setValidData(false)
                    toast('Chore name: ' + nameResult.message);
                }

                // -> Chore notes
                var noteResult = ValidationFuncs.validateCharacterLimit(choreNotes)
                if (!noteResult.isSuccessful) {
                    setValidData(false)
                    toast('Chore notes: ' + noteResult.message);
                }

                // -> Chore days (required)
                if (daysReq == undefined || daysReq.length == 0) {
                    toast('Select the days the chore is set for.')
                }

                // Send data to parent ChoreList component
                if (hasValidData && name != undefined && (daysReq != undefined || daysReq.length > 0)) {
                    // organize modalConfiguration with chore properties
                    var modalConfig = {
                        ChoreName: name,
                        ChoreNotes: choreNotes,
                        ChoreDays: daysReq,
                        ChoreRepeats: choreRepeats,
                        ChoreAssignments: selectedAssignments
                    }
                    props.confirm(modalConfig)
                    props.close()
                }
            })
            .catch((errorInfo) => { });
    }

    //Chore Repeats
    const repeatOnChange = (e) => {
        setChoreRepeats(e.target.value);
    };

    //Chore Assignments
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

    const toast = (title, desc = '') => {
        notification.open({
            message: title,
            description: desc,
            duration: 2,
            placement: 'bottom',
        });
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
        width={600}
        maskClosable="false"
    >
        <div className="padding">
            <h2 className="padding-bottom mulish-font">Add chore</h2>
            <Form id="choreCreationForm" form={form}>
                <p className="mulish-font" style={{ marginBottom: 2, fontWeight: '100' }}>Name</p>
                <Form.Item name="choreName">
                    <Input style={Styles.inputFieldStyle} required placeholder="Chore Name" />
                </Form.Item>
                <p className="mulish-font" style={{ marginBottom: 2 }}>Notes</p>
                <Form.Item name="choreNotes">
                    <Input style={Styles.largeTextField} placeholder="Optional notes..." />
                </Form.Item>
                <p className="mulish-font" style={{ marginBottom: 2 }}>Days</p>
                <Form.Item name="choreDaysReq">
                <Checkbox.Group required>
                    <Checkbox value="MON" style={{ lineHeight: '32px' }}>Mon</Checkbox>
                    <Checkbox value="TUES" style={{ lineHeight: '32px' }}>Tues</Checkbox>
                    <Checkbox value="WED" style={{ lineHeight: '32px' }}>Wed</Checkbox>
                    <Checkbox value="THURS" style={{ lineHeight: '32px' }}>Thurs</Checkbox>
                    <Checkbox value="FRI" style={{ lineHeight: '32px' }}>Fri</Checkbox>
                    <Checkbox value="SAT" style={{ lineHeight: '32px' }}>Sat</Checkbox>
                    <Checkbox value="SUN" style={{ lineHeight: '32px' }}>Sun</Checkbox>
                    </Checkbox.Group>
                </Form.Item>
                <p className="mulish-font padding-top" style={{ marginBottom: 2 }}>Repeats</p>
                <Radio.Group onChange={repeatOnChange} value={choreRepeats}>
                    <Radio value="Monthly" style={{ lineHeight: '32px' }}>Monthly</Radio>
                    <Radio value="Bi-weekly" style={{ lineHeight: '32px' }}>Bi-weekly</Radio>
                    <Radio value="Weekly" style={{ lineHeight: '32px' }}>Weekly</Radio>
                    <Radio value="Daily" style={{ lineHeight: '32px' }}>Daily</Radio>
                </Radio.Group>
                <p className="mulish-font padding-top" style={{ marginBottom: 2 }}>Select members to assign chore to</p>
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
                                    style={(selectedAssignments.includes(i)) ? { border: '5px solid #555', borderRadius: '50%', width: 50, height: 50, cursor: 'pointer' } : { borderRadius: '50%', width: 50, height: 50, cursor: 'pointer' }}></Card>
                                <p style={{ color: 'gray', fontSize: 10, marginLeft: 10 }}>{member['firstName'] + member['lastName']}</p>
                            </Col>)}
                        </Row>) :
                        (<p className='mulish-font'>Chore will be assigned to self</p>)}
                   
                </div>
                <div style={{ marginLeft: 80 }}>
                    <Button key="cancel" onClick={props.close} type="default" style={Styles.defaultButtonModal}>Exit</Button>
                    <Button key="create" onClick={attemptSubmission} htmlType="submit" type="primary" style={Styles.primaryButtonModal}>Save</Button>
                </div>
            </Form>
        </div>
    </Modal>);
}

export default ChoreCreationModal;
