import React, { Component, useState, useEffect } from 'react';
import { useAuth } from '../Auth/AuthContext';
import { Calendar, Col, Row, Select, Button, Table, Radio, Badge, notification } from 'antd';
import { PlusCircleOutlined } from '@ant-design/icons'
import dayjs from 'dayjs';
import axios from 'axios';
//import { google } from 'google-auth-library';
import dayLocaleData from 'dayjs/plugin/localeData';
import 'dayjs/locale/en';
import '../../styles/System.css';
import '../../styles/Calendar.css';
import * as Styles from '../../styles/ConstStyles.js';
import AddEventModal from './AddEventModal.js';
import EditEventModal from './EditEventModal.js';
// References:
// http://github.com/react-component/calendar/issues/221
// http://ant.design/components/calendar


export const CalendarView = () => {
    const { currentUser, currentGroup } = useAuth();
    const [value, setValue] = useState(new Date());
    const [events, setEvents] = useState([]);
    const [mode, setMode] = useState('month');
    const [selectedDate, setSelectedDate] = useState(value);
    const [selectedEvent, setSelectedEvent] = useState('');
    const [showAddModal, setShowAddModal] = useState(false);
    const [showEditModal, setShowEditModal] = useState(false);
    dayjs.extend(dayLocaleData);

    useEffect(() => { refreshCalendar();}, []);

    const getEvents = () => {
        let groupForm = {
            GroupId: currentGroup['groupId']
        }
        axios.post('calendar/GetGroupEvents', groupForm)
            .then(res => {
                var isSuccessful = res.data['isSuccessful']
                if (isSuccessful) {
                    let events = res.data['returnedObject']
                    setEvents(events);
                }
                else {
                    failureCalendarView(res.data['message']);
                }
            })
    }

    const refreshCalendar = () => {
        getEvents();
    }

    const addCalendarEvent = (eventConfig) => {
        setShowAddModal(false)
        let newEvent = {
            EventName: eventConfig.eventName,
            Description: (eventConfig.description == null) ? "" : eventConfig.description,
            GroupId: currentGroup.groupId,
            EventDate: eventConfig.eventDate.replace('T', ' '),
            Repeats: (eventConfig.repeats == null) ? "never" : eventConfig.repeats,
            Type: (eventConfig.type == null) ? "public" : eventConfig.type,
            Reminder: (eventConfig.reminder == null) ? "none" : eventConfig.reminder,
            Color: (eventConfig.color == null) ? "#0256D4" : eventConfig.color,
            CreatedBy: currentUser.username
        }
        console.log("adding event...", newEvent);
        axios.post('calendar/AddGroupEvent', newEvent)
            .then(res => {
                var isSuccessful = res.data['isSuccessful']
                if (isSuccessful) {
                    successCalendarView(res.data['message']);
                    refreshCalendar();
                }
                else {
                    failureCalendarView(res.data['message']);
                }
            })
    }

    const editCalendarEvent = (eventConfig) => {
        setShowEditModal(false);
        let newEvent = {
            EventId: selectedEvent.eventId,
            EventName: eventConfig.eventName,
            Description: eventConfig.description,
            GroupId: currentGroup.groupId,
            EventDate: eventConfig.eventDate.replace('T', ' '),
            Repeats: (eventConfig.repeats == null) ? "never" : eventConfig.repeats,
            Type: (eventConfig.type == null) ? "public" : eventConfig.type,
            Reminder: (eventConfig.reminder == null) ? "none" : eventConfig.reminder,
            Color: eventConfig.color == "same" ? selectedEvent.color : eventConfig.color,
            CreatedBy: selectedEvent.createdBy
        }
        console.log("updating event...", newEvent);
        axios.post('calendar/EditGroupEvent', newEvent)
            .then(res => {
                var isSuccessful = res.data['isSuccessful']
                if (isSuccessful) {
                    successCalendarView(res.data['message']);
                    refreshCalendar();
                }
                else {
                    failureCalendarView(res.data['message']);
                }
            })
    }

    const deleteCalendarEvent = (eventConfig) => {
        setShowEditModal(false);
        let eventToDelete = {
            EventId: selectedEvent.eventId,
            EventName: eventConfig.eventName,
            Description: eventConfig.description,
            GroupId: currentGroup.groupId,
            EventDate: eventConfig.eventDate.replace('T', ' '),
            Repeats: (eventConfig.repeats == null) ? "never" : eventConfig.repeats,
            Type: (eventConfig.type == null) ? "public" : eventConfig.type,
            Reminder: (eventConfig.reminder == null) ? "none" : eventConfig.reminder,
            Color: eventConfig.color == "same" ? selectedEvent.color : eventConfig.color,
            CreatedBy: selectedEvent.createdBy,
            RemovedBy: currentUser.username
        }
        console.log("deleting event...", eventToDelete);
        axios.post('calendar/DeleteGroupEvent', eventToDelete)
            .then(res => {
                var isSuccessful = res.data['isSuccessful']
                if (isSuccessful) {
                    successCalendarView(res.data['message']);
                    refreshCalendar();
                }
                else {
                    failureCalendarView(res.data['message']);
                }
            })
    }

    const onPanelChange = (value, mode) => {
        console.log(mode);
        setMode(mode);
    };

    const onSelectDate = (value) => {
        setValue(value);
        let selected = new Date(value);
        setSelectedDate(selected);
    }

    const selectEvent = (event) => {
        setSelectedEvent(event);
        setShowEditModal(true);
    }

    const headerRender = ({ value, type, onChange, onTypeChange }) => {
        const start = 0;
        const end = 12;
        const monthOptions = [];
        let current = value.clone();
        const localeData = value.localeData();
        const months = [];
        for (let i = 0; i < 12; i++) {
            current = current.month(i);
            months.push(localeData.monthsShort(current));
        }
        for (let i = start; i < end; i++) {
            monthOptions.push(
                <Select.Option key={i} value={i} className="month-item">
                    {months[i]}
                </Select.Option>,
            );
        }
        const year = value.year();
        const month = value.month();
        const options = [];
        for (let i = year - 10; i < year + 10; i += 1) {
            options.push(
                <Select.Option key={i} value={i} className="year-item">
                    {i}
                </Select.Option>,
            );
        }
        return (
            <div style={{ padding: 8 }}>
                <h2 className="mulish-font"><b>{months[month]}  {year}</b></h2>
                <div className="padding-bottom">
                    <Select
                        size="small"
                        dropdownMatchSelectWidth={false}
                        className="my-year-select"
                        value={year}
                        onChange={(newYear) => {
                            const now = value.clone().year(newYear);
                            onChange(now);
                        }}
                    >{options}
                    </Select>
                    <Select
                        size="small"
                        dropdownMatchSelectWidth={false}
                        value={month}
                        onChange={(newMonth) => {
                            const now = value.clone().month(newMonth);
                            onChange(now);
                        }}
                    >{monthOptions}
                    </Select>
                </div>
                <Row gutter={24}>
                    <Col span={16}>
                        <Radio.Group
                            style={Styles.calendarViewToggleGroup}
                            onChange={(e) => onTypeChange(e.target.value)}
                            value={type}>
                            <Radio.Button style={Styles.calendarViewToggle}
                                value="day">Day
                            </Radio.Button>
                            <Radio.Button style={Styles.calendarViewToggle}
                                value="week">Week
                            </Radio.Button>
                            <Radio.Button style=
                                {Styles.calendarViewToggle}
                                value="month">Month
                            </Radio.Button>
                            <Radio.Button style=
                                {Styles.calendarViewToggle}
                                value="year">Year
                            </Radio.Button>
                        </Radio.Group>
                            
                    </Col>
                    <Col span={4} offset={4}>
                        <Button
                            id="add-file"
                            style={Styles.addButtonStyle}
                            shape="round"
                            icon={<PlusCircleOutlined />}
                            size={'large'}
                            onClick={() => setShowAddModal(true)}>
                            Add event
                        </Button>
                    </Col>
                </Row>
            </div>
        );
    }

    const isSameDay = (eventDate, value) => {
        let date = new Date(eventDate);
        date.setHours(0, 0, 0, 0);
        let compareDate = new Date(Date.UTC(value.year(), value.month(), value.date()+1));
        compareDate.setHours(0, 0, 0, 0);
        let sameDay = (date.valueOf() == compareDate.valueOf()) ? true : false;
        return sameDay;
    }

    const dateCellRender = (value) => {
        return (
            <ul className="events">
                {events.map((event) => (
                    (isSameDay(event.eventDate, value) && !event.isDeleted) ? 
                        (<li className="event" key={event.eventId} onClick={() => selectEvent(event)}>
                            <Badge status={event.type} color={event.color} text={event.eventName} />
                    </li>) : null
                ))}
            </ul>
        );
    }

    const successCalendarView = (successMessage) => {
        notification.open({
            message: successMessage,
            duration: 10,
            placement: "topLeft"
        });
    }


    const failureCalendarView = (failureMessage) => {
        notification.open({
            message: "Sorry, an error occurred.",
            description: failureMessage,
            duration: 10,
            placement: "topLeft"
        });
    }

    const cellRender = (current, info) => {
        return dateCellRender(current);
    };

    return (
        <div style={Styles.body}>
            <div>
                <Calendar
                    style={{ fontFamily: 'Mulish' }}
                    headerRender={headerRender}
                    dateCellRender={cellRender}
                    onPanelChange={onPanelChange}
                    onSelect={onSelectDate}
                    />
                </div>
            {showAddModal && <AddEventModal show={showAddModal} close={() => setShowAddModal(false)} confirm={addCalendarEvent} date={selectedDate} />}
            {showEditModal && <EditEventModal show={showEditModal} close={() => setShowEditModal(false)} event={selectedEvent} confirm={editCalendarEvent} remove={deleteCalendarEvent} />}
        </div>
    );
};

export default CalendarView;