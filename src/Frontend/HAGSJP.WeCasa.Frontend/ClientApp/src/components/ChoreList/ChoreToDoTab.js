﻿import React, { Component, useState, useEffect } from 'react';
import { Modal, ConfigProvider, Button, Row, Col, Image, Space, Input, Form, Switch, notification, Card, Divider } from 'antd';
import * as Styles from '../../styles/ConstStyles.js';
import '../../styles/System.css';
import '../../index.css';
import ChoreCard from './ChoreCard'
import axios from 'axios';
import { RightCircleOutlined, LeftCircleOutlined } from '@ant-design/icons';


//dev only
const data = {
    'MON': [{
        Name: 'chore item 1',
        Notes: '',
        Assignments: ['githelsuico@gmail.com'],
        IsCompleted: false
    },
    {
        Name: 'chore item 2',
        Notes: 'test notes',
        Assignments: ['githelsuico@gmail.com, new8@gmail.com'],
        IsCompleted: true,
    }],
    'TUES': [{
        Name: 'chore item 3',
        Notes: 'test notes',
        Assignments: ['new8@gmail.com'],
        IsCompleted: false
    }],
    'WED': [],
    'THURS': [],
    'FRI': [],
    'SAT': [{
        Name: 'chore item 3',
        Notes: 'test notes',
        Assignments: ['new8@gmail.com'],
        IsCompleted: false
    },
    {
        Name: 'chore item 3',
        Notes: 'test notes',
        Assignments: ['new8@gmail.com'],
        IsCompleted: false
    }],
    'SUN': [{
        Name: 'chore item 3',
        Notes: 'test notes',
        Assignments: ['new8@gmail.com'],
        IsCompleted: false
    }]
}

export const ChoreToDoTab = (props) => {
    // Fetching
    const [count, setCount] = useState(0); 
    const [checkChores, setCheckChores] = useState(null) 
    const [chores, setChores] = useState([])
    const [successfulFetch, setSuccessFetch] = useState(false); //initial fetch
    const [error, setError] = useState(true);
    // Display
    const [monChores, setMonChores] = useState([]);
    const [tuesChores, setTuesChores] = useState([]);
    const [wedChores, setWedChores] = useState([]);
    const [thursChores, setThursChores] = useState([]);
    const [friChores, setFriChores] = useState([]);
    const [satChores, setSatChores] = useState([]);
    const [sunChores, setSunChores] = useState([]);
    const [weekDates, setWeekDates] = useState([]);
    const [currDate, setCurrDate] = useState(null);
    const [currWeek, setCurrWeek] = useState(0); 

    const organizeChores = () => {
        setMonChores(chores['MON'] == undefined ? [] : chores['MON'])
        setTuesChores(chores['TUES'] == undefined ? [] : chores['TUES'])
        console.log(thursChores)
        setWedChores(chores['WED'] == undefined ? [] : chores['WED'])
        setThursChores(chores['THURS'] == undefined ? [] : chores['THURS'])
        setFriChores(chores['FRI'] == undefined ? [] : chores['FRI'])
        setSatChores(chores['SAT'] == undefined ? [] : chores['SAT'])
        setSunChores(chores['SUN'] == undefined ? [] : chores['SUN'])
    }

    const fetchChores = () => {
        let groupForm = {
            GroupId: props.group['groupId']
        }

        axios.post('chorelist/GetGroupToDoChores', groupForm)
            .then(res => {
                var isSuccessful = res.data['isSuccessful'];
                if (isSuccessful) {
                    var items = res.data['returnedObject']
                    setCheckChores(items)
                    setChores(items)
                    if (checkChores != null) {
                        setSuccessFetch(true)
                        setError(false)
                    }
                    console.log(chores)
                } else {
                    setSuccessFetch(true)
                    setError(true)
                }
            })
            .catch((error => {
                setSuccessFetch(true)
                setError(true)
                toast('Refresh page. Error fetching data.')
                console.error(error)
            }));
    }

    const toast = (title, desc = '') => {
        notification.open({
            message: title,
            description: desc,
            duration: 5,
            placement: "bottom",
        });
    }

    const getWeekDates = () => {
        const today = new Date();
        const dayOfWeek = today.getDay();
        const diff = (currWeek * 7) - dayOfWeek + (dayOfWeek === 0 ? -6 : 1);
        const firstDayOfNextWeek = new Date(today.setDate(today.getDate() + diff));
        const dates = [];

        for (let i = 0; i < 7; i++) {
            const date = new Date(firstDayOfNextWeek);
            date.setDate(firstDayOfNextWeek.getDate() + i);
            dates.push(date.toLocaleDateString());
        }

        setWeekDates(dates)
        console.log(dates)

        // fetch current date
        if (currWeek == 0) {
            const currentDate = new Date();
            const currentDayIndex = (currentDate.getDay() + 6) % 7;
            setCurrDate(currentDayIndex)
        } else {
            setCurrDate(null)
        }
    }

    const nextWeek = () => {
        console.log("next btn triggered")
        setCurrWeek(currWeek + 1);
        console.log("current week = " + currWeek)
        getWeekDates()
    }

    const previousWeek = () => {
        console.log("previous btn triggered")
        setCurrWeek(currWeek - 1);
        console.log("current week = " + currWeek)
        getWeekDates()
    }

    const resetStates = () => {
        setSuccessFetch(false)
        setError(false)
        setCheckChores(null)
    }

    const fetchData = () => {
        console.log('fetching data...')
        if (!successfulFetch) {
            fetchChores()
            organizeChores()
            setCount(count + 1);
        }

        // fetch returned backend error -> output message only once
        if (successfulFetch && error) {
            toast('Refresh page. Error fetching data.')
        }

        if (checkChores != null) {
            props.setUpdate(false)
        }
    }

    useEffect(() => {
        setCurrWeek(0)
        getWeekDates()
    }, []);

    useEffect(() => {
        getWeekDates()
    }, [currWeek]);

    // Initial fetch
    useEffect(() => {
        fetchData()
    }, [count]);

    //Any changes to chore list
    useEffect(() => {
        if (props.update & successfulFetch) {
            console.log('refreshing data...')
            resetStates()
            setCount(count + 1);
        }
    }, [props.update]);

    return (<div style={{ paddingTop: 20 }}>
        <Row gutter={[48, 2]} align="center" justify="space-around" className="todo-pagination">
            <Col span={12} align="start">
                <LeftCircleOutlined onClick={() => previousWeek()} style={{ fontSize: '24px', cursor: 'pointer' }}/>
            </Col>
            <Col span={12} align="end">
                <RightCircleOutlined onClick={() => nextWeek()} style={{ fontSize: '24px', cursor: 'pointer' }}/>
            </Col>
        </Row>
        <Row gutter={[8, 8]} align="center" justify="space-around" className="todo-chores-header" style={{ marginTop: -30 }}>
            <Col span={2} align="center">
                <p className="mulish-font" style={(currDate != null && currDate == 0) ? { borderRadius: 5, backgroundColor: '#525252', color: 'white', padding: 4 } : { padding: 5 }}>MON</p>
                <p className="mulish-font" style={{ marginTop: -10, fontSize: 15 }}>{weekDates[0]}</p>
            </Col>
            <Col span={2} align="center">
                <p className="mulish-font" style={(currDate != null && currDate == 1) ? { borderRadius: 5, backgroundColor: '#525252', color: 'white', padding: 4 } : { padding: 5 }}>TUES</p>
                <p className="mulish-font" style={{ marginTop: -10, fontSize: 15 }}>{weekDates[1]}</p>
            </Col>
            <Col span={2} align="center">
                <p className="mulish-font" style={(currDate != null && currDate == 2) ? { borderRadius: 5, backgroundColor: '#525252', color: 'white', padding: 4 } : { padding: 5 }}>WED</p>
                <p className="mulish-font" style={{ marginTop: -10, fontSize: 15 }}>{weekDates[2]}</p>
            </Col>
            <Col span={2} align="center">
                <p className="mulish-font" style={(currDate != null && currDate == 3) ? { borderRadius: 5, backgroundColor: '#525252', color: 'white', padding: 4 } : { padding: 5 }}>THURS</p>
                <p className="mulish-font" style={{ marginTop: -10, fontSize: 15 }}>{weekDates[0]}</p>
            </Col>
            <Col span={2} align="center">
                <p className="mulish-font" style={(currDate != null && currDate == 4) ? { borderRadius: 5, backgroundColor: '#525252', color: 'white', padding: 4 } : { padding: 5 }}>FRI</p>
                <p className="mulish-font" style={{ marginTop: -10, fontSize: 15 }}>{weekDates[0]}</p>
            </Col>
            <Col span={2} align="center">
                <p className="mulish-font" style={(currDate != null && currDate == 5) ? { borderRadius: 5, backgroundColor: '#525252', color: 'white', padding: 4 } : { padding: 5 }}>SAT</p>
                <p className="mulish-font" style={{ marginTop: -10, fontSize: 15 }}>{weekDates[0]}</p>
            </Col>
            <Col span={2} align="center">
                <p className="mulish-font" style={(currDate != null && currDate == 6) ? { borderRadius: 5, backgroundColor: '#525252', color: 'white', padding: 4 } : { padding: 5 }}>SUN</p>
                <p className="mulish-font" style={{ marginTop: -10, fontSize: 15}}>{weekDates[0]}</p>
            </Col>
        </Row>
        <Divider orientation="center" style={{ border: '0.2px solid gray', marginBottom: 0, marginTop: 5 }} />
        <Row gutter={[8, 8]} align="center" justify="space-around" className="todo-chores-board">
            <Col span={2} style={{ paddingTop: 20, marginRight: 10  }} >
                <div>{monChores.map((item, i) =>
                    <ChoreCard chore={item} user={props.currentUser} setUpdate={props.setUpdate} />)}
                </div>
            </Col>
            <Col span={1} style={{ marginRight: -40}} >
                <Divider type="vertical" style={{ border: '0.2px solid gray', height: '100%' }} />
            </Col>
            <Col span={2} style={{ paddingTop: 20, marginRight: 10 }} >
                <div>{tuesChores.map((item, i) =>
                    <ChoreCard chore={item} user={props.currentUser} setUpdate={props.setUpdate} />)}
                </div>
            </Col>
            <Col span={1} style={{ marginRight: -40 }} >
                <Divider type="vertical" style={{ border: '0.2px solid gray', height: '100%' }} />
            </Col>
            <Col span={2} style={{ paddingTop: 20, marginRight: 10 }} >
                <div>{wedChores.map((item, i) =>
                    <ChoreCard chore={item} user={props.currentUser} setUpdate={props.setUpdate} />)}
                </div>
            </Col>
            <Col span={1} style={{ marginRight: -40 }} >
                <Divider type="vertical" style={{ border: '0.2px solid gray', height: '100%' }} />
            </Col>
            <Col span={2} style={{ paddingTop: 20, marginRight: 10 }} >
                <div>{thursChores.map((item, i) =>
                    <ChoreCard chore={item} user={props.currentUser} setUpdate={props.setUpdate} />)}
                </div>
            </Col>
            <Col span={1} style={{ marginRight: -40 }} >
                <Divider type="vertical" style={{ border: '0.2px solid gray', height: '100%' }} />
            </Col>
            <Col span={2} style={{ paddingTop: 20, marginRight: 10 }} >
                <div>{friChores.map((item, i) =>
                    <ChoreCard chore={item} user={props.currentUser} setUpdate={props.setUpdate} />)}
                </div>
            </Col>
            <Col span={1} style={{ marginRight: -40 }} >
                <Divider type="vertical" style={{ border: '0.2px solid gray', height: '100%' }} />
            </Col>
            <Col span={2} style={{ paddingTop: 20, marginRight: 10 }} >
                <div>{satChores.map((item, i) =>
                    <ChoreCard chore={item} user={props.currentUser} setUpdate={props.setUpdate} />)}
                </div>
            </Col>
            <Col span={1} style={{ marginRight: -40 }} >
                <Divider type="vertical" style={{ border: '0.2px solid gray', height: '100%' }} />
            </Col>
            <Col span={2} style={{ paddingTop: 20, marginRight: 10 }} >
                <div>{sunChores.map((item, i) =>
                    <ChoreCard chore={item} user={props.currentUser} setUpdate={props.setUpdate} />)}
                </div>
            </Col>
        </Row>
    </div>);
};

export default ChoreToDoTab;