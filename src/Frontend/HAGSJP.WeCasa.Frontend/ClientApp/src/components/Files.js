import React, { Component, useState, useEffect } from 'react';
import { Navigate, useNavigate } from 'react-router-dom'
import { useAuth } from './AuthContext';
import { Col, Card, Row, Space, Avatar, Button, Tabs, notification } from 'antd';
import { PlusCircleOutlined } from '@ant-design/icons'
import axios from 'axios';
import * as Styles from '../styles/ConstStyles.js';
import NavMenu from './NavMenu';
const { Meta } = Card;
const TabPane = Tabs.TabPane;



export const Files = () => {
    const [loading, setLoading] = useState(true);
    const { currentUser, currentGroup } = useAuth();
    const [files, setFiles] = useState([]);
    const [refreshSettings, setRefreshSettings] = useState(true);
    const [refreshFiles, setRefreshFiles] = useState(true);
    const navigate = useNavigate();

    useEffect(() => { getFiles(); }, []);

    const tabItemClick = (key) => {
        console.log('tab click', key);
        if (key == 1) {
            setRefreshFiles(true)
            setRefreshSettings(false)
        } else {
            setRefreshFiles(false)
            setRefreshSettings(true)
        }
    };

    const getFiles = () => {
        
    }

    const attemptFileUpload = () => {
        
    }

    const displayFileView = () => {
        return files.map(file => (
            <div key={file.fileId}>
                <Col span={10} style={{marginTop:16, marginLeft:16}}>
                    <Card>
                        
                    </Card>
                </Col>
            </div>
        ));
    }

    const displayDeletedFiles = () => {

    }

    const failureFileView = (failureMessage) => {
        notification.open({
            message: "Sorry, an error occurred.",
            description: failureMessage,
            duration: 10,
            placement: "topLeft"
        });
    }

    return (
        <div>
            <div>
                <Tabs defaultActiveKey="1" onChange={tabItemClick} destroyInactiveTabPane>
                    <TabPane tab="Group Files" key="1">{displayFileView()}</TabPane>
                    <TabPane tab="Deleted Files" key="2">{displayDeletedFiles()}</TabPane>
                </Tabs> 
                               
              </div>
        </div>
    );
}; 

export default Files;