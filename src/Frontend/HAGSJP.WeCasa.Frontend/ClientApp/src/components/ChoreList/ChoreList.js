import React, { Component, useState, useEffect } from 'react';
import { Navigate, useNavigate } from 'react-router-dom'
import { useAuth } from '../Auth/AuthContext';
import { Col, Card, Row, Space, Avatar, Button, notification, Tabs } from 'antd';
import ChoreToDoTab from './ChoreToDoTab'
import ChoreHistory from './ChoreHistoryTab'
import ChoreCreationModal from './ChoreCreationModal'
import * as Styles from '../../styles/ConstStyles.js';
import axios from 'axios';
const TabPane = Tabs.TabPane;

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

export const ChoreList = (props) => {
    const [showCreateModal, setShowCreateModal] = useState(false);
    const { currentGroup, currentUser } = useAuth();

    const tabItemClick = (key) => {
        console.log('tab click', key);
        if (key == 1) {
            // to do tab
            // setUpdateToDoList(true)
        } else {
            // history tab
        }
    };

    const attemptChoreCreation = (modalConfig) => {
        console.log(currentGroup)

        let choreForm = {
            CurrentUser: currentUser['username'],
            GroupId: currentGroup['groupId'], //or is it GroupId
            Name: modalConfig['ChoreName'],
            Notes: modalConfig['ChoreNotes'],
            Repeats: modalConfig['ChoreRepeats'],
            Days: modalConfig['ChoreDays'],
            AssignedTo: modalConfig['ChoreAssignments']
        }

        // Web api call
        axios.post('chorelist/AddChore', choreForm)
            .then(res => {
                var isSuccessful = res.data['isSuccessful'];
                console.log(res.data)
                if (isSuccessful) {
                    toast('Successfully created chore')
                } else {
                    toast(res.data['message'])
                }
            })
            .catch((error => { console.error(error) }));

        // Refresh ChoresToDoTab 
    }

    const toast = (title, desc = '') => {
        notification.open({
            message: title,
            description: desc,
            duration: 5,
            placement: "bottom",
        });
    }

    useEffect(() => {
        // setUpdateToDoList(true) -> trigger update in ChoreToDoTab
    }, []);

    return (
        <div>
            <div className="header">
                <Row gutter={[8, 8]} align="middle">
                    <Col span={18}>
                        <h3 className="mulish-font">Chore List 📋</h3>
                    </Col>
                    <Col span={6}>
                        <Button style={Styles.defaultButtonStyle} onClick={() => setShowCreateModal(true)}>Add task</Button>
                        <ChoreCreationModal show={showCreateModal} close={() => setShowCreateModal(false)} confirm={attemptChoreCreation} group={currentGroup} user={currentUser} />
                    </Col>
                </Row>
            </div>
            <Tabs defaultActiveKey="1" onChange={tabItemClick} destroyInactiveTabPane>
                <TabPane tab="Current To-do" key="1"><ChoreToDoTab toDoList={data}/></TabPane>
                <TabPane tab="History" key="2"><ChoreHistory/></TabPane>
            </Tabs>  </div>
    );
};

export default ChoreList;
