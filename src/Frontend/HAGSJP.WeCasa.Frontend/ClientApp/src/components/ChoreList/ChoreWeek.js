import React, { Component, useState, useEffect } from 'react';
import { Divider, Modal, ConfigProvider, Button, Row, Col, Image, Space, Input, Form, Switch, notification, Card } from 'antd';
import * as Styles from '../../styles/ConstStyles.js';
import '../../styles/System.css';
import '../../index.css';
import ChoreCard from './ChoreCard'
import axios from 'axios';
import defaultImage from '../../assets/defaultimgs/wecasatemp.jpg';
import * as ValidationFuncs from '../../scripts/InputValidation.js';

export const ChoreWeek = (props) => {
    const [monChores, setMonChores] = useState([]);
    const [tuesChores, setTuesChores] = useState([]);
    const [wedChores, setWedChores] = useState([]);
    const [thursChores, setThursChores] = useState([]);
    const [friChores, setFriChores] = useState([]);
    const [satChores, setSatChores] = useState([]);
    const [sunChores, setSunChores] = useState([]);

    const organizeChores = () => {
        setMonChores(props.toDoList['MON'] == undefined ? [] : props.toDoList['MON'])
        setTuesChores(props.toDoList['TUES'] == undefined ? [] : props.toDoList['TUES'])
        console.log(thursChores)
        setWedChores(props.toDoList['WED'] == undefined ? [] : props.toDoList['WED'])
        setThursChores(props.toDoList['THURS'] == undefined ? [] : props.toDoList['THURS'])
        setFriChores(props.toDoList['FRI'] == undefined ? [] : props.toDoList['FRI'])
        setSatChores(props.toDoList['SAT'] == undefined ? [] : props.toDoList['SAT'])
        setSunChores(props.toDoList['SUN'] == undefined ? [] : props.toDoList['SUN'])
    }
    
    useEffect(() => {
        organizeChores()
        
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