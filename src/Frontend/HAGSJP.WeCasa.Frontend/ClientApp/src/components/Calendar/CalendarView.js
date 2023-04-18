import React, { Component, useState, useEffect } from 'react';
import { useAuth } from '../AuthContext';
import { Calendar, Col, Radio, Space, Row, Select, Button, Typography, theme } from 'antd';
import { PlusCircleOutlined } from '@ant-design/icons'
import dayjs from 'dayjs';
import 'dayjs/locale/zh-cn';
import dayLocaleData from 'dayjs/plugin/localeData';
import * as Styles from '../../styles/ConstStyles.js';

export const CalendarView = () => {
    const { currentGroup } = useAuth();
    const [value, setValue] = useState();
    const [selectedDate, setSelectedDate] = useState(value);
    const [showModal, setShowModal] = useState(false);
    const [collapsed, setCollapsed] = useState(true);
    const currentDate = new Date();
    dayjs.extend(dayLocaleData);

    const onPanelChange = (value, mode) => {
        console.log(value.format('YYYY-MM-DD'), mode);
    };

    const onSelectDate = (value) => {
        setValue(value);
        setSelectedDate(value);
    }

    const addCalendarEvent = (date) => {
        console.log("adding event")
    }

    return (
        <div style={Styles.body}>
            <Calendar
                onSelect={onSelectDate}
                style={{ fontFamily: 'Mulish' }}
                headerRender={({ value, type, onChange, onTypeChange }) => {
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
                        <div
                            style={{
                                padding: 8,
                            }}
                        >
                            <Row gutter={8}>
                                <h2 className="padding-button mulish-font"><b>{months[month]}  {year}</b></h2>
                                <Col span={4} offset={4}>
                                    <Space
                                        direction="horizontal"
                                    >
                                        <Button style={Styles.calendarViewToggle} onClick={(e) => onTypeChange(e.target.value)} value="month">Month</Button>
                                        <Button style={Styles.calendarViewToggle} onClick={(e) => onTypeChange(e.target.value)} value="year">Year</Button>
                                    </Space>
                                </Col>
                                <Col offset={2}>
                                    <Select
                                        style={Styles.defaultDropdown}
                                        size="small"
                                        dropdownMatchSelectWidth={false}
                                        className="my-year-select"
                                        value={year}
                                        onChange={(newYear) => {
                                            const now = value.clone().year(newYear);
                                            onChange(now);
                                        }}
                                    >
                                        {options}
                                    </Select>
                                </Col>
                                <Col>
                                    <Select
                                        style={Styles.defaultDropdown}
                                        size="small"
                                        dropdownMatchSelectWidth={false}
                                        value={month}
                                        onChange={(newMonth) => {
                                            const now = value.clone().month(newMonth);
                                            onChange(now);
                                        }}
                                    >
                                        {monthOptions}
                                    </Select>
                                </Col>
                                <Col span={4} push={2}>
                                    <Button
                                        id="add-file"
                                        style={Styles.fileButtonStyle}
                                        shape="round"
                                        icon={<PlusCircleOutlined />}
                                        size={'large'}
                                        onClick={() => addCalendarEvent()}>
                                        Add event
                                    </Button>
                                </Col>
                            </Row>
                        </div>
                    );
                }}
                onPanelChange={onPanelChange} />
        </div>
    );
};

export default CalendarView;