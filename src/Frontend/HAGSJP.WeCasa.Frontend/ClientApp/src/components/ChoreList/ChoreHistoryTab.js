import React, { Component, useState, useEffect } from 'react';
import { Modal, ConfigProvider, Button, Row, Col, Image, Space, Input, Form, Switch, notification, Card, Divider } from 'antd';
import * as Styles from '../../styles/ConstStyles.js';
import '../../styles/System.css';
import '../../index.css';
import CompletedChoreCard from './CompletedChoreCard'
import axios from 'axios';

//dev only
const tempData = {
    '2023-04-23': [{
        name: 'chore item 1',
        notes: '',
        assignedTo: [{ 'username': 'githelsuico@gmail.com', 'image': 0, 'firstName': 'test' }, { 'username': 'githelsuico@gmail.com', 'image': 2, 'firstName': 'test' }],
        isCompleted: true
    },
    {
        name: 'chore item 2',
        notes: 'test notes',
        assignedTo: [{ 'username': 'githelsuico@gmail.com', 'image': 0, 'firstName': 'test' }, { 'username': 'githelsuico@gmail.com', 'image': 2, 'firstName': 'test' }],
  IsCompleted: true,
    }],
    '2023-01-23': [{
        name: 'chore item 3',
        notes: 'test notes',
        assignedTo: [{ 'username': 'githelsuico@gmail.com', 'image': 0, 'firstName': 'test' }, { 'username': 'githelsuico@gmail.com', 'image': 2, 'firstName': 'test' }],
   IsCompleted: true
    }]
}

export const ChoreHistoryTab = (props) => {
    // Fetching
    const [count, setCount] = useState(0);
    const [checkChores, setCheckChores] = useState(null)
    const [chores, setChores] = useState([])
    const [successfulFetch, setSuccessFetch] = useState(false);
    const [error, setError] = useState(true);

    const fetchChores = () => {
        let groupForm = {
            GroupId: props.group['groupId']
        }

        axios.post('chorelist/GetGroupCompletedChores', groupForm)
            .then(res => {
                var isSuccessful = res.data['isSuccessful'];
                if (isSuccessful) {
                    var items = res.data['returnedObject']
                    console.log(items)
                    setCheckChores(items)
                    setChores(items)
                    if (checkChores != null) {
                        props.setUpdate(false)
                        setSuccessFetch(true)
                        setError(false)
                    }
                } else {
                    props.setUpdate(false)
                    setSuccessFetch(true)
                    setError(true)
                }
            })
            .catch((error => {
                props.setUpdate(false)
                setSuccessFetch(true)
                setError(true)
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

    const resetStates = () => {
        setSuccessFetch(false)
        setError(false)
        setCheckChores(null)
    }

    const fetchData = () => {
        // initial fetch
        if (!successfulFetch) {
            fetchChores()
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

    return (<div style={{ paddingTop: 20 }}>
        {(Object.keys(chores).length == 0) ?
            (<div><h6>Group has no completed chores.</h6></div>) :
            (<div>{
                Object.entries(chores).map(([date, choreVals]) => (
                    <div>
                        <Row align="top" justify="start" gutter={[2, 2]}>
                            <Col span={4} className="date-title mulish-font"><h5>{date}</h5></Col>
                            <Col span={20} className="chore-history-list">
                                <div>{choreVals.map((item) =>
                                    <CompletedChoreCard chore={item} user={props.currentUser} fetchData={fetchData} />
                                )}
                                </div>
                            </Col>
                        </Row>
                    </div>
                ))
            }</div>)
        }
    </div>);
};

export default ChoreHistoryTab;