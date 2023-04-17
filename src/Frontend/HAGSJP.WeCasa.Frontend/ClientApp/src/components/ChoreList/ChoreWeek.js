import React, { Component, useState, useEffect } from 'react';
import { Divider, Modal, ConfigProvider, Button, Row, Col, Image, Space, Input, Form, Switch, notification, Card } from 'antd';
import * as Styles from '../../styles/ConstStyles.js';
import { useAuth } from '../AuthContext.js';
import '../../styles/System.css';
import '../../index.css';
import ChoreCard from './ChoreCard'
import axios from 'axios';
import defaultImage from '../../assets/defaultimgs/wecasatemp.jpg';
import * as ValidationFuncs from '../../scripts/InputValidation.js';

export const ChoreWeek = (props) => {
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
                <ChoreCard />
                <ChoreCard/>
            </Col>
            <Col span={1}>
                <Divider type="vertical" style={{ border: '0.2px solid gray', height: '100%' }} />
            </Col>
            <Col span={2} style={{ paddingTop: 20 }} >
              // Tuesday chores
            </Col>
            <Col span={1}>
                <Divider type="vertical" style={{ border: '0.2px solid gray', height: '100%' }} />
            </Col>
            <Col span={2} style={{ paddingTop: 20 }} >
                // Wed chores
            </Col>
            <Col span={1}>
                <Divider type="vertical" style={{ border: '0.2px solid gray', height: '100%' }} />
            </Col>
            <Col span={2} style={{ paddingTop: 20 }} >
                //Thurs chores
            </Col>
            <Col span={1}>
                <Divider type="vertical" style={{ border: '0.2px solid gray', height: '100%' }} />
            </Col>
            <Col span={2} style={{ paddingTop: 20 }} >
                //Fri chores
            </Col>
            <Col span={1}>
                <Divider type="vertical" style={{ border: '0.2px solid gray', height: '100%' }} />
            </Col>
            <Col span={2} style={{ paddingTop: 20 }} >
                //Sat chores
            </Col>
            <Col span={1}>
                <Divider type="vertical" style={{ border: '0.2px solid gray', height: '100%' }} />
            </Col>
            <Col span={2} style={{ paddingTop: 20 }} >
                //Sun chores
            </Col>
        </Row>
    </div>);
};

export default ChoreWeek;