import React, { Component, useState, useEffect } from 'react';
import { Divider, Modal, ConfigProvider, Button, Row, Col, Image, Space, Input, Form, Switch, notification, Card } from 'antd';
import * as Styles from '../../styles/ConstStyles.js';
import '../../styles/System.css';
import '../../index.css';
import ChoreCard from './ChoreCard'
import axios from 'axios';
import defaultImage from '../../assets/defaultimgs/wecasatemp.jpg';
import * as ValidationFuncs from '../../scripts/InputValidation.js';

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

export const ChoreWeek = (props) => {
    const [monChores, setMonChores] = useState([]);
    const [tuesChores, setTuesChores] = useState([]);
    const [wedChores, setWedChores] = useState([]);
    const [thursChores, setThursChores] = useState([]);
    const [friChores, setFriChores] = useState([]);
    const [satChores, setSatChores] = useState([]);
    const [sunChores, setSunChores] = useState([]);
    
    useEffect(() => {
        //setMonChores(props.toDoList['MON'])
        //setTuesChores(props.toDoList['TUES'])
        //setWedChores(props.toDoList['WED'])
        //setThursChores(props.toDoList['THURS'])
        //setFriChores(props.toDoList['FRI'])
        //setSatChores(props.toDoList['SAT'])
        //setSunChores(props.toDoList['SUN'])
    }, []);

    return (<div style={{ paddingTop: 20 }}>
        <Row gutter={[8, 8]} align="center" justify="space-around" className="todo-chores-header">
            <Col span={2}>
                <p className="mulish-font" style={{ borderRadius: 5, backgroundColor: 'gray', width: '50%', padding: 5 }}>MON</p>
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
                    <ChoreCard chore={item}/>)}
                </div>
            </Col>
            <Col span={1}>
                <Divider type="vertical" style={{ border: '0.2px solid gray', height: '100%' }} />
            </Col>
            <Col span={2} style={{ paddingTop: 20 }} >
                <div>{tuesChores.map((item, i) =>
                    <ChoreCard chore={item}/>)}
                </div>
            </Col>
            <Col span={1}>
                <Divider type="vertical" style={{ border: '0.2px solid gray', height: '100%' }} />
            </Col>
            <Col span={2} style={{ paddingTop: 20 }} >
                <div>{wedChores.map((item, i) =>
                    <ChoreCard chore={item}/>)}
                </div>
            </Col>
            <Col span={1}>
                <Divider type="vertical" style={{ border: '0.2px solid gray', height: '100%' }} />
            </Col>
            <Col span={2} style={{ paddingTop: 20 }} >
                <div>{thursChores.map((item, i) =>
                    <ChoreCard chore={item}/>)}
                </div>
            </Col>
            <Col span={1}>
                <Divider type="vertical" style={{ border: '0.2px solid gray', height: '100%' }} />
            </Col>
            <Col span={2} style={{ paddingTop: 20 }} >
                <div>{friChores.map((item, i) =>
                    <ChoreCard chore={item}/>)}
                </div>
            </Col>
            <Col span={1}>
                <Divider type="vertical" style={{ border: '0.2px solid gray', height: '100%' }} />
            </Col>
            <Col span={2} style={{ paddingTop: 20 }} >
                <div>{satChores.map((item, i) =>
                    <ChoreCard chore={item}/>)}
                </div>
            </Col>
            <Col span={1}>
                <Divider type="vertical" style={{ border: '0.2px solid gray', height: '100%' }} />
            </Col>
            <Col span={2} style={{ paddingTop: 20 }} >
                <div>{sunChores.map((item, i) =>
                    <ChoreCard chore={item}/>)}
                </div>
            </Col>
        </Row>
    </div>);
};

export default ChoreWeek;