import React, { Component, useState, useEffect } from 'react';
import { Navigate, useNavigate } from 'react-router-dom'
import { useAuth } from '../AuthContext';
import { Col, Card, Row, Space, Avatar, Button, notification, Tabs, Checkbox } from 'antd';
import * as Styles from '../../styles/ConstStyles.js';
import axios from 'axios';
import AddGroceryModal from './AddGroceryModal'

export const GroceryList = (props) => {
    const [showAddModal, setShowAddModal] = useState(false);
    const { currentGroup, currentUser } = useAuth();

    const addGroceryItem = (modalConfig) => {

    }

    const itemPurchased = () => {

    }

    const fetchGroceries = () => {

    }

    useEffect(() => {
       //fetch grocery items based on group id
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
                    <AddGroceryModal show={showAddModal} close={() => setShowAddModal(false)} confirm={addGroceryItem} group={currentGroup} />
                </Col>
            </Row>
            </div>
            <div className="grocery-board">
                <Checkbox onChange={itemPurchased}>
                    <h6 className="mulish-font">Item
                    <i> (assigned to)</i>
                    </h6>
                </Checkbox>
            </div>
        </div>
    );
};

export default GroceryList;