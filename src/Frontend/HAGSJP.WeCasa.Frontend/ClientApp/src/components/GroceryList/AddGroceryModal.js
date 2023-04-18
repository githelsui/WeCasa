import React, { Component, useState, useEffect } from 'react';
import { Modal, ConfigProvider, Button, Row, Col, Image, Space, Input, Form, Switch, notification, Spin, Card, Checkbox } from 'antd';
import * as Styles from '../../styles/ConstStyles.js';
import '../../styles/System.css';
import '../../index.css';
import * as ValidationFuncs from '../../scripts/InputValidation.js';
import axios from 'axios';

export const AddGroceryModal = (props) => {
    const [form] = Form.useForm();

    useEffect(() => {
       
    }, []);

    return (
        <Modal
            open={props.show}
            closable={false}
            centered="true"
            footer={null}
            maskClosable="false"
        >
            <div className="padding">
                <h2 className="padding-bottom mulish-font">Add Item</h2>
                <Form id="addGroceryForm" onFinish={props.confirm} form={form}>

                    <div style={{ marginLeft: 80 }}>
                        <Button key="cancel" onClick={props.close} type="default" style={Styles.defaultButtonModal}>Exit</Button>
                        <Button key="create" htmlType="submit" type="primary" style={Styles.primaryButtonModal}>Save</Button>
                    </div>
                </Form>
            </div>
        </Modal>
    );
};

export default AddGroceryModal;