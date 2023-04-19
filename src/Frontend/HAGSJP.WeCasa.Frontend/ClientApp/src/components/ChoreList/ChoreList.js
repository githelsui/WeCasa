import React, { Component, useState, useEffect } from 'react';
import { Navigate, useNavigate } from 'react-router-dom'
import { useAuth } from '../AuthContext';
import { Col, Card, Row, Space, Avatar, Button, notification, Tabs } from 'antd';
import ChoreToDoTab from './ChoreToDoTab'
import ChoreHistory from './ChoreHistoryTab'
import ChoreCreationModal from './ChoreCreationModal'
import * as Styles from '../../styles/ConstStyles.js';
import axios from 'axios';
const TabPane = Tabs.TabPane;

export const ChoreList = (props) => {
    const [showCreateModal, setShowCreateModal] = useState(false);
    const [fetchedRoommates, setFetchedRoommates] = useState(false);
    const [membersList, setMembersList] = useState([]);
    const { currentGroup, currentUser } = useAuth();

    const tabItemClick = (key) => {
        console.log('tab click', key);
        if (key == 1) {
            // to do tab
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
                   
                }
            })
            .catch((error => { console.error(error) }));

        // Refresh ChoresToDoTab 
    }

    const fetchCurrentRoommates = () => {
        // axios call to fetch current group members UserProfiles
        let groupMemberForm = {
            GroupId: currentGroup['groupId']
        }

        axios.post('chorelist/GetCurrentGroupMembers', groupMemberForm)
            .then(res => {
                var isSuccessful = res.data['isSuccessful'];
                if (isSuccessful) {
                    var memberArrRes = res.data['returnedObject']
                    console.log(memberArrRes)
                    var copyArr = cleanArrayCopy(memberArrRes)
                    setMembersList(copyArr)
                    if (membersList != null) {
                        setFetchedRoommates(true)
                    }
                } 
            })
            .catch((error => { console.error(error) }));
    }

    const cleanArrayCopy = (array) => {
        let copy = []
        for (let i = 0; i < array.length; i++) {
            var member = array[i]
            if (member != null && member['username'] != currentUser['username']) {
                copy.push(array[i])
            }
        }
        setMembersList(copy)
        return copy
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
        setMembersList([])
        fetchCurrentRoommates()
        console.log(membersList)
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
                        <ChoreCreationModal show={showCreateModal} close={() => setShowCreateModal(false)} confirm={attemptChoreCreation} group={currentGroup} currentMembers={membersList} fetchedRoommates={fetchedRoommates} />
                    </Col>
                </Row>
            </div>
            <Tabs defaultActiveKey="1" onChange={tabItemClick} destroyInactiveTabPane>
                <TabPane tab="Current To-do" key="1"><ChoreToDoTab/></TabPane>
                <TabPane tab="History" key="2"><ChoreHistory/></TabPane>
            </Tabs>  </div>
    );
};

export default ChoreList;
