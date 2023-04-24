import React, { Component, useState, useEffect } from 'react';
import { Modal, ConfigProvider, Button, Row, Col, Image, Space, Input, Form, Switch, notification, Card, Divider } from 'antd';
import * as Styles from '../../styles/ConstStyles.js';
import '../../styles/System.css';
import '../../index.css';
import ChoreCard from './ChoreCard'
import axios from 'axios';

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
    const [initialFetch, setInitialFetch] = useState(false);
    const [successfulFetch, setSuccessFetch] = useState(false);
    const [error, setError] = useState(true);
    // Display
    const [monChores, setMonChores] = useState([]);
    const [tuesChores, setTuesChores] = useState([]);
    const [wedChores, setWedChores] = useState([]);
    const [thursChores, setThursChores] = useState([]);
    const [friChores, setFriChores] = useState([]);
    const [satChores, setSatChores] = useState([]);
    const [sunChores, setSunChores] = useState([]);

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

    const getCurrentDay = () => {
        // fetch current day
        const d = new Date();
        let day = d.getDay();
        // update label on display
    }

    const resetStates = () => {
        setSuccessFetch(false)
        setError(false)
        setCheckChores(null)
    }

    const fetchData = () => {
        console.log('fetching data...')
        // initial fetch
        if (!initialFetch) {
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
                setInitialFetch(true)
            }
        } else {
            resetStates()
        }
    }

    const refreshList = () => {
        console.log('refreshing data...')

        if (!successfulFetch || props.update) {
            fetchChores()
            organizeChores()
            setCount(count + 1);
        }

        // fetch returned backend error -> output message only once
        if (successfulFetch && error) {
            toast('Refresh page. Error fetching data.')
        }
    }

    useEffect(() => {
        fetchData()
    }, [count]);

    //Any changes to chore list
    useEffect(() => {
        console.log(props.update)
        refreshList()
    }, [props.update]);

    return (<div style={{ paddingTop: 20 }}>
        <Row gutter={[8, 8]} align="center" justify="space-around" className="todo-chores-header">
            <Col span={2}>
                <p className="mulish-font" style={{ borderRadius: 5, backgroundColor: 'gray', color: 'white', width: '50%', padding: 5 }}>MON</p>
            </Col>
            <Col span={2}>
                <p className="mulish-font" style={{ padding: 5 }}>TUES</p>
            </Col>
            <Col span={2}>
                <p className="mulish-font" style={{ padding: 5 }}>WED</p>
            </Col>
            <Col span={2}>
                <p className="mulish-font" style={{ padding: 5 }}>THURS</p>
            </Col>
            <Col span={2}>
                <p className="mulish-font" style={{ padding: 5 }}>FRI</p>
            </Col>
            <Col span={2}>
                <p className="mulish-font" style={{ padding: 5 }}>SAT</p>
            </Col>
            <Col span={2}>
                <p className="mulish-font" style={{ padding: 5 }}>SUN</p>
            </Col>
        </Row>
        <Divider orientation="center" style={{ border: '0.2px solid gray', marginBottom: 0, marginTop: 5 }} />
        <Row gutter={[8, 8]} align="center" justify="space-around" className="todo-chores-board">
            <Col span={2} style={{ paddingTop: 20 }} >
                <div>{monChores.map((item, i) =>
                    <ChoreCard chore={item} user={props.currentUser} fetchData={fetchData}/>)}
                </div>
            </Col>
            <Col span={1}>
                <Divider type="vertical" style={{ border: '0.2px solid gray', height: '100%' }} />
            </Col>
            <Col span={2} style={{ paddingTop: 20 }} >
                <div>{tuesChores.map((item, i) =>
                    <ChoreCard chore={item} user={props.currentUser} fetchData={fetchData}/>)}
                </div>
            </Col>
            <Col span={1}>
                <Divider type="vertical" style={{ border: '0.2px solid gray', height: '100%' }} />
            </Col>
            <Col span={2} style={{ paddingTop: 20 }} >
                <div>{wedChores.map((item, i) =>
                    <ChoreCard chore={item} user={props.currentUser} fetchData={fetchData}/>)}
                </div>
            </Col>
            <Col span={1}>
                <Divider type="vertical" style={{ border: '0.2px solid gray', height: '100%' }} />
            </Col>
            <Col span={2} style={{ paddingTop: 20 }} >
                <div>{thursChores.map((item, i) =>
                    <ChoreCard chore={item} user={props.currentUser} fetchData={fetchData}/>)}
                </div>
            </Col>
            <Col span={1}>
                <Divider type="vertical" style={{ border: '0.2px solid gray', height: '100%' }} />
            </Col>
            <Col span={2} style={{ paddingTop: 20 }} >
                <div>{friChores.map((item, i) =>
                    <ChoreCard chore={item} user={props.currentUser} fetchData={fetchData}/>)}
                </div>
            </Col>
            <Col span={1}>
                <Divider type="vertical" style={{ border: '0.2px solid gray', height: '100%' }} />
            </Col>
            <Col span={2} style={{ paddingTop: 20 }} >
                <div>{satChores.map((item, i) =>
                    <ChoreCard chore={item} user={props.currentUser} fetchData={fetchData}/>)}
                </div>
            </Col>
            <Col span={1}>
                <Divider type="vertical" style={{ border: '0.2px solid gray', height: '100%' }} />
            </Col>
            <Col span={2} style={{ paddingTop: 20 }} >
                <div>{sunChores.map((item, i) =>
                    <ChoreCard chore={item} user={props.currentUser} fetchData={fetchData}/>)}
                </div>
            </Col>
        </Row>
    </div>);
};

export default ChoreToDoTab;