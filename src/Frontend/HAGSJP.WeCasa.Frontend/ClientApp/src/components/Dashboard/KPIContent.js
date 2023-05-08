import React, { Component, useState, useEffect } from 'react';
import { useLocation } from 'react-router-dom';
import { Modal, ConfigProvider, Button, Row, Col, Image, Space, Input, Form, Switch, Tabs } from 'antd';
import * as Styles from '../../styles/ConstStyles.js';
import '../../styles/System.css';
import '../../index.css';
import { useAuth } from '../Auth/AuthContext';
import Charts from './Charts'
const TabPane = Tabs.TabPane;

export const KPIContent = (props) => {
    const timeFrames = ["1 Week", "1 Month", "3 Months", "6 Months"]
    const { admin, auth, currentUser } = useAuth();
    const [currentKPI, setCurrentKPI] = useState('')
    const [timeFrame, setTimeFrame] = useState('')
    const [endpoint, setEndpoint] = useState("")

    const updateEndpoint = () => {
        if (props.kpiLabel == "Logins Per Day") {
            setEndpoint("GetLoginsPerDay")
        }

        if (props.kpiLabel == "Registrations Per Day") {
            setEndpoint("GetRegistrationsPerDay")
        }

        if (props.kpiLabel == "Daily Active Users") {
            setEndpoint("GetDailyActiveUsers")
        }

        if (props.kpiLabel == "Sessions Per User") {
            setEndpoint("GetSessionsPerUser")
        }

        if (props.kpiLabel == "Most Used Features") {
            setEndpoint("GetMostUsedFeatures")
        }

        if (props.kpiLabel == "Error Rate") {
            setEndpoint("GetErrorRate")
        }

        if (props.kpiLabel == "Retention Rate") {
            setEndpoint("GetRetentionRate")
        }

        if (props.kpiLabel == "Customer Service Ratings") {
            setEndpoint("GetCustomerServiceRatings")
        }

        if (props.kpiLabel == "User Rating Reviews") {
            setEndpoint("GetUserRatingReviews")
        }

        //console.log("currentKPI: " + currentKPI)
        //console.log("selectedKPI: " + selectedKPI)
    }

    const tabItemClick = (key) => {
        console.log('tab: ' + key);
        setTimeFrame(timeFrames[key])
        console.log(timeFrame)
    };

    useEffect(() => {
        updateEndpoint();
        setCurrentKPI(props.kpiLabel)
        console.log("currentKPI: " + props.kpiLabel)
    }, [props.kpiLabel, endpoint]);

    return (
        <div className="kpi-content">
            <Tabs defaultActiveKey="1" onChange={tabItemClick} destroyInactiveTabPane>
                <TabPane tab="1 Week" id="one-week" key="1"><Charts timeFrame="1 Week" kpiLabel={currentKPI} endpoint={endpoint}/></TabPane>
                <TabPane tab="1 Month" id="one-month" key="2"><Charts timeFrame="1 Month" kpiLabel={currentKPI} endpoint={endpoint}/></TabPane>
                <TabPane tab="3 Months" id="three-months" key="3"><Charts timeFrame="3 Months" kpiLabel={currentKPI} endpoint={endpoint}/></TabPane>
                <TabPane tab="6 Months" id="six-months" key="4"><Charts timeFrame="6 Months" kpiLabel={currentKPI} endpoint={endpoint}/></TabPane>
        </Tabs>
        </div>
    );
};

export default KPIContent;