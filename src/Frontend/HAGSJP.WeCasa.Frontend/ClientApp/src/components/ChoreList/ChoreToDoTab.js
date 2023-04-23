import React, { Component, useState, useEffect } from 'react';
import { Modal, ConfigProvider, Button, Row, Col, Image, Space, Input, Form, Switch, notification, Card } from 'antd';
import * as Styles from '../../styles/ConstStyles.js';
import '../../styles/System.css';
import '../../index.css';
import ChoreWeek from './ChoreWeek'
import axios from 'axios';

export const ChoreToDoTab = (props) => {
    const [count, setCount] = useState(0); //For fetching
    const [chores, setChores] = useState(null)
    const [successfulFetch, setSuccessFetch] = useState(false);

    const fetchChores = () => {
        let groupForm = {
            GroupId: props.group['groupId']
        }

        axios.post('chorelist/GetGroupToDoChores', groupForm)
            .then(res => {
                var isSuccessful = res.data['isSuccessful'];
                if (isSuccessful) {
                    var items = res.data['returnedObject']
                    console.log(items)
                    setChores(items)
                    if (chores != null) {
                        setSuccessFetch(true)
                    }
                    console.log(chores)
                } else {
                    setSuccessFetch(true)
                }
            })
            .catch((error => { console.error(error) }));
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
        //if (props.update) {
        //    fetchChores()
        //} else {
        //    setChores([])
        //}

        // initial fetch
        if (!successfulFetch) {
            fetchChores()
            setCount(count + 1);
        }

        // when user adds new chore
        //if (props.update) {
        //    if (!successfulFetch) {
        //        fetchChores()
        //        setCount(count + 1);
        //    }
        //}
    }, [count]);

    return (<div>
        <ChoreWeek toDoList={chores} />
         </div>);
};

export default ChoreToDoTab;