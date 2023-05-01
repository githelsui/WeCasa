import React, { Component, useState, useEffect } from 'react';
import { useAuth } from '../Auth/AuthContext';
import { Calendar, Col, Row, Select, Button, Table, Radio, Badge, notification } from 'antd';
import { PlusCircleOutlined } from '@ant-design/icons'
import dayjs from 'dayjs';
import axios from 'axios';
//import { google } from 'google-auth-library';
import 'dayjs/locale/zh-cn';
import dayLocaleData from 'dayjs/plugin/localeData';
import '../../styles/System.css';
import '../../styles/Calendar.css';
import * as Styles from '../../styles/ConstStyles.js';
import AddEventModal from './AddEventModal.js';
// References:
// http://github.com/react-component/calendar/issues/221
// http://ant.design/components/calendar


export const CalendarView = () => {
    const { currentUser, currentGroup } = useAuth();
    const [value, setValue] = useState(new Date());
    const [events, setEvents] = useState([]);
    const [mode, setMode] = useState('month');
    const [selectedDate, setSelectedDate] = useState(value);
    const [showModal, setShowModal] = useState(false);
    const [collapsed, setCollapsed] = useState(true);
    dayjs.extend(dayLocaleData);

    useEffect(() => { getEvents();}, []);

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
                    console.log(events);
                }
                else {
                    failureCalendarView(res.data['message']);
                }
            })
            .catch((error) => {
                console.error(error)
            });
    }

    const addCalendarEvent = (eventConfig) => {
        let newEvent = {
            EventName: eventConfig.eventName,
            Description: (eventConfig.description == null) ? "" : eventConfig.description,
            GroupId: currentGroup.groupId,
            EventDate: eventConfig.eventDate,
            Repeats: (eventConfig.repeats == null) ? "never" : eventConfig.repeats,
            Type: (eventConfig.type == null) ? "public" : eventConfig.type,
            Reminder: (eventConfig.reminder == null) ? "none" : eventConfig.reminder,
            Color: (eventConfig.color == null) ? "#0256D4" : eventConfig.color,
            CreatedBy: currentUser.username
        }
        console.log("adding event...", newEvent);
        axios.post('calendar/AddGroupEvent', newEvent)
            .then(res => {
                var isSuccessful = res.data['IsSuccessful']
                if (isSuccessful) {
                    // refresh events
                    successCalendarView(res.data['message']);
                }
                else {
                    failureCalendarView(res.data['message']);
                }
            })
            .catch((error) => {
                console.error(error)
            });
    }

    const onPanelChange = (value, mode) => {
        console.log(mode);
        setMode(mode);
    };

    const onSelectDate = (value) => {
        setValue(value);
        setSelectedDate(value);
    }

    const viewEvent = (event) => {
        console.log(event);
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
                            onClick={() => setShowModal(true)}>
                            Add event
                        </Button>
                    </Col>
                </Row>
            </div>
        );
    }

    const isSameDay = (eventDate, value) => {
        let sameDay = (
            (dayjs(eventDate).diff(value, 'day') === 0)
            && (dayjs(eventDate).diff(value, 'month') === 0)
            && (dayjs(eventDate).diff(value, 'year') === 0)
        )
        console.log(sameDay);
        return sameDay;
    }

    const dateCellRender = (value) => {
        return (
            <ul className="events">
                {events.map((event) => (
                    isSameDay(event.eventDate, value) ? 
                        (<li key={event.eventId} onClick={() => viewEvent(event)}>
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

    /*const getDayHoursEvents = (value) => {
    //const dayList = getDayEvents(value);
    const dayList = [];
    const endDate = value.clone().endOf('date').unix();
    const events = [];
    for (let hour = value.clone().startOf('date'); hour.unix() < endDate; hour.add(1, 'h')) {
        events.push({
            id: hour.unix(),
            hour: hour.format('hh a'),
            events: dayList ? dayList.filter(event => hour.unix() <= event.startTime && event.endTime < hour.clone().add(1, 'h').unix()) : <div />,
        });
    }
    return events;
}*/

    /*const dayColumns = [
        {
            title: '',
            dataIndex: 'hour',
            key: 'hour',
            className: Styles.hourColumn,
        },
        {
            title: '',
            dataIndex: 'events',
            key: 'events',
            render: text => dateCellRender(text),
        }
    ];*/

    const cellRender = (current, info) => {
        return dateCellRender(current);
    };

    return (
        <div style={Styles.body}>
            <Calendar
                onSelect={onSelectDate}
                style={{ fontFamily: 'Mulish' }}
                headerRender={headerRender}
                dateCellRender={cellRender}
                onPanelChange={onPanelChange} />
            <AddEventModal show={showModal} close={() => setShowModal(false)} confirm={addCalendarEvent} reject={failureCalendarView} />
        </div>
    );
};

export default CalendarView;