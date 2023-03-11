import React, { Component, useState, useEffect } from 'react';
import { Navigate, useNavigate } from 'react-router-dom'
import { useAuth } from './AuthContext';
import { Col, Card, Row, Space, Avatar, Button, Image, Tabs, notification } from 'antd';
import { PlusCircleOutlined } from '@ant-design/icons'
import axios from 'axios';
import * as Styles from '../styles/ConstStyles.js';
import { FileView } from './FileView.js';
import image2 from '../assets/profileimgs/2.jpg';
import NavMenu from './NavMenu';
const { Meta } = Card;
const TabPane = Tabs.TabPane;



export const Files = () => {
    const { currentUser, currentGroup } = useAuth();
    const [files, setFiles] = useState([]);
    const [selectedFile, setSelectedFile] = useState('');
    const [showFile, setShowFile] = useState(false);
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
        axios.get('files/GetGroupFiles')
            .then(res => {
                console.log(res.data);
                setFiles(res.data['returnedObject']);
            })
            .catch((error) => { console.error(error) });
    }

    const attemptFileUpload = () => {

    }

    const selectFile = (file) => {
        setSelectedFile(file);
        setShowFile(true);
    }

    const displayFileView = () => {
        var fileList = files.map(function (file, index) {
            return (
                <div key={index} onClick={() => selectFile(file)}>
                    <Col span={10} style={{ marginTop: 16, marginLeft: 16 }}>
                        <Card
                            hoverable
                            style={{ width: 200, height: 200 }}
                            cover={
                                <Image
                                    style={{
                                        borderRadius: '5%',
                                        display: 'flex',
                                        margin: '5%',
                                    }}
                                    src={image2}
                                    preview={false} height="120px" width="120px" />}>
                            <Meta title={file.fileName}
                                type="inner"
                                style={{ textAlign: "center", display: "flex" }} />
                        </Card>
                    </Col>
                </div>
            );
        });
        return fileList;
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
                    <TabPane tab="Group Files" key="1"><Space direction="horizonal" size={32}>{displayFileView()}</Space></TabPane>
                    <TabPane tab="Deleted Files" key="2">{displayDeletedFiles()}</TabPane>
                </Tabs>
                <FileView show={showFile} close={() => setShowFile(false)} file={selectedFile}/>

            </div>
        </div>
    );
};

export default Files;