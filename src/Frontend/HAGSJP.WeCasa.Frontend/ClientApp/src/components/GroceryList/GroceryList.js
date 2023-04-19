import React, { Component, useState, useEffect } from 'react';
import { Navigate, useNavigate } from 'react-router-dom'
import { useAuth } from '../AuthContext';
import { Col, Card, Row, Space, Avatar, Button, notification, Tabs, Checkbox } from 'antd';
import * as Styles from '../../styles/ConstStyles.js';
import axios from 'axios';
import AddGroceryModal from './AddGroceryModal'

const data = [
    {
        Name: 'grocery item 1',
        Notes: '',
        Assignments: ['githelsuico@gmail.com']
    },
    {
        Name: 'grocery item 2',
        Notes: 'test notes',
        Assignments: ['githelsuico@gmail.com, new8@gmail.com']
    },
    {
        Name: 'grocery item 3',
        Notes: 'test notes',
        Assignments: ['new8@gmail.com']
    },
]


export const GroceryList = (props) => {
    const [count, setCount] = useState(0); //For rerendering
    const [showAddModal, setShowAddModal] = useState(false);
    const { currentGroup, currentUser } = useAuth();
    const [groceryItems, setGroceryItems] = useState([])

    const addGroceryItem = (modalConfig) => {
        let groceryForm = {
            CurrentUser: currentUser['username'],
            GroupId: currentGroup['groupId'],
            Name: modalConfig['Name'],
            Notes: modalConfig['Notes'],
            Assignments: modalConfig['Assignments']
        }

        console.log(groceryForm)

        // Web api call
        axios.post('grocerylist/AddGroceryItem', groceryForm)
            .then(res => {
                var isSuccessful = res.data['isSuccessful'];
                console.log(res.data)
                if (isSuccessful) {
                    toast('Successfully added grocery item!')
                    // fetchGroceries()
                } else {
                    toast(res.data['message'])
                }
            })
            .catch((error => { console.error(error) }));
    }

    const itemPurchased = (item) => {
        // get grocery_id from item obj
    }

    const fetchGroceries = () => {
        let groupForm = {
            GroupId: currentGroup['groupId']
        }

        axios.post('grocerylist/GetGroceryItems', groupForm)
            .then(res => {
                var isSuccessful = res.data['isSuccessful'];
                console.log(res.data)
                if (isSuccessful) {
                    // setGroceryItems(returnedobject)
                }
            })
            .catch((error => { console.error(error) }));
    }

    const assignmentLabel = (assignments) => {
        if (assignments.length == 1 && assignments.includes(currentUser['username'])) {
            return '';
        } else {
            var label = '('
            for (let i = 0; i < assignments.length; i++) {
                if (i == assignments.length - 1) {
                    label += assignments[i] + ')'
                } else {
                    label += assignments[i]
                }
            }
            console.log(label)
            return label;
        }
    }

    const toast = (title, desc = '') => {
        notification.open({
            message: title,
            description: desc,
            duration: 2,
            placement: 'bottom',
        });
    }

    useEffect(() => {
        //fetchGroceries()

        //dev only
        //setGroceryItems(data)
    }, []);

    return (
        <div>
            <div className="header">
            <Row gutter={[8, 8]} align="middle">
                <Col span={18}>
                   <h3 className="mulish-font">Grocery List  🛒</h3>
                </Col>
                <Col span={6}>
                        <Button style={Styles.defaultButtonStyle} onClick={() => setShowAddModal(true)}>Add item</Button>
                        <AddGroceryModal show={showAddModal} close={() => setShowAddModal(false)} confirm={addGroceryItem} group={currentGroup} user={currentUser}/>
                </Col>
            </Row>
            </div>
            <div className="grocery-board">

                {(groceryItems.length > 0) ?
                    (<div>{groceryItems.map((item, i) =>
                        <Row gutter={24,24}>
                            <Checkbox onChange={itemPurchased(item)}>
                                <h6 className="mulish-font">
                                    {item['Name']} <i> {(assignmentLabel(item['Assignments']))}</i>
                                </h6>
                            </Checkbox></Row>)}
                       </div>) :
                    (<p className='mulish-font'>No grocery items for this group</p>)}
            </div>
        </div>
    );
};

export default GroceryList;