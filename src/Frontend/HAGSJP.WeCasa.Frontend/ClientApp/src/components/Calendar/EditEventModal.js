import React, { Component, useState } from 'react';
import { Modal, DatePicker, TimePicker, Button, Row, Col, Image, Space, Card, Input, Form, Radio, Spin } from 'antd';
import * as Styles from '../../styles/ConstStyles';
import DeletionModal from '../DeletionModal.js';
import config from '../../appsettings.json'

const repeatOptions = ['Monthly', 'Bi-weekly', 'Weekly', 'Daily'];
const eventTypeOptions = ['Private', 'Public'];
const reminderOptions = ['30 minutes', 'A day', 'A week'];


const EditEventModal = (props) => {
    const [eventDate, setEventDate] = useState('');
    const [showModal, setShowModal] = useState(false);
    const [repeat, setRepeat] = useState('');
    const [eventType, setEventType] = useState('');
    const [reminder, setReminder] = useState('');
    const [eventColor, setEventColor] = useState('');
    const [form] = Form.useForm();
    const eventColors = ['#0256D4', '#F4B105', '#FFEE58', '#FF2929', '#10B364'];
    const event = props.event;

    const onDateChange = (date, dateString) => {
        setEventDate(dateString);
    }

    const onRepeatChange = (e) => {
        setRepeat(e.target.value);
    }

    const onTypeChange = (e) => {
        setEventType(e.target.value);
    }

    const onReminderChange = (e) => {
        setReminder(e.target.value);
    }

    const attemptSubmission = () => {
        form.setFieldsValue({
            eventDate: eventDate,
            color: eventColor
        });
        form.validateFields()
            .then((values) => {
                props.confirm(values);
            })
            .catch((errorInfo) => { console.log(errorInfo) });
    }

    const deleteEvent = () => {
        console.log('deleting event...');
        setShowModal(false);
    }

    const displayEventColors = () => {
        return (
            eventColors.map((color, i) =>
                <div key={i}>
                    <Col span={5} style={{ marginLeft: 10 }}>
                        <Card
                            onClick={() => setEventColor(i)}
                            bordered={true}
                            hoverable
                            style={(eventColor == i) ? {
                                border: '5px solid #555', backgroundColor: color, borderRadius: 50, width: 50, height: 50
                            } : { backgroundColor: color, borderRadius: 50, width: 50, height: 50 }}></Card>
                    </Col>
                </div>)
        );
    }

    return (
        <Modal
            open={props.show}
            closable={true}
            onCancel={props.close}
            centered="true"
            footer={null}
            maskClosable="false"
        >
            <div className="padding">
                    <h2 className="mulish-font"><b>Edit Event</b></h2>
                    <h6 className="mulish-font">Name</h6>
                    <Form id="eventCreationForm" onFinish={attemptSubmission} form={form}>
                        <Row gutter={[24, 24]} align="middle">
                            <Col span={16} className="event-name-input">
                                <Form.Item name="eventName">
                                    <Input style={Styles.eventInputFieldStyle} placeholder={event == null ? "" : event.eventName}/>
                                </Form.Item>
                            </Col>
                        </Row>

                        <h6 className="padding-top mulish-font">Description</h6>
                        <Row gutter={[24, 24]}>
                            <Col span={18} className="description-field">
                                <Form.Item name="description">
                                    <Input style={Styles.eventDescTextField} placeholder={event == null ? "" : event.description}/>
                                </Form.Item>
                            </Col>
                        </Row>

                        <h6 className="mulish-font">Date and Time</h6>
                        <div className="datetime-row padding-bottom">
                            <Row gutter={24} style={{ display: 'flex', flexDirection: 'horizontal' }}>
                                <Col span={16}>
                                    <Form.Item name="eventDate">
                                        <DatePicker format="YYYY-MM-DD hh:mm:ss"
                                            showTime={true}
                                            onChange={onDateChange}
                                            placeholder={event == null ? "" : new Date(event.eventDate).toLocaleString()}
                                         />
                                    </Form.Item>
                                </Col>
                            </Row>
                        </div>

                        <h6 className="mulish-font">Repeats</h6>
                        <div className="repeats-row padding-bottom">
                            <Row gutter={24} style={{ display: 'flex', flexDirection: 'horizontal' }} >
                                <Col span={20}>
                                    <Form.Item name="repeat" value={repeat}>
                                        <Radio.Group options={repeatOptions} onChange={onRepeatChange} defaultValue={(event == null || event.repeat == "never") ? null : event.repeat} />
                                    </Form.Item>
                                </Col>
                            </Row>
                        </div>

                        <h6 className="mulish-font">Type</h6>
                        <div className="eventtype-row padding-bottom">
                            <Row gutter={24} style={{display:'flex', flexDirection:'horizontal'}}>
                                <Col span={18}>
                                    <Form.Item name="type" value={eventType}>
                                        <Radio.Group onChange={onTypeChange} defaultValue={event != null ? event.type : null}>
                                            <Radio value={'private'}>Private</Radio>
                                            <Radio value={'public'}>Public</Radio>
                                        </Radio.Group>
                                    </Form.Item>
                                </Col>
                            </Row>
                        </div>

                        <h6 className="mulish-font">Send a reminder before the event</h6>
                        <div className="reminder-row padding-bottom">
                            <Row gutter={24} style={{ display: 'flex', flexDirection: 'horizontal' }}>
                                <Col span={18}>
                                    <Form.Item name="reminder" value={reminder}>
                                        <Radio.Group options={reminderOptions} value={reminder} onChange={onReminderChange} defaultValue={(event == null || event.reminder == "none") ? null : event.reminder} />
                                    </Form.Item>
                                </Col>
                            </Row>
                        </div>

                        <h5 className="mulish-font">Color Tag</h5>
                        <div className="tag-row padding-bottom">
                            <Row gutter={24} style={{ display: 'flex', flexDirection: 'horizontal' }}>
                                {displayEventColors()}
                                <Form.Item name="color"></Form.Item>
                            </Row>
                        </div>
                        <Row gutter={24} style={{alignItems:'center', justifyContent:'center', gap:'30px'}} > 
                            <Button key="delete" onClick={() => setShowModal(true)} type="default" style={Styles.deleteButtonLeft}>Delete Event</Button>
                            <Button key="create" htmlType="submit" type="primary" style={Styles.primaryButtonModal}>Save</Button>
                        </Row>
                    </Form>
                <DeletionModal show={showModal} close={() => setShowModal(false)} message="Are you sure you want to delete the event?" confirm={deleteEvent} />
            </div>
        </Modal>
    );
}

export default EditEventModal;