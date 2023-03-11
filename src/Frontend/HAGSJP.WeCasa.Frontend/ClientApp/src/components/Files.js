import React, { Component, useState, useEffect } from 'react';
import { Navigate, useNavigate } from 'react-router-dom'
import { useAuth } from './AuthContext';
import { Col, Card, Row, Space, ConfigProvider, Button, Image, Tabs, notification } from 'antd';
import { PlusCircleOutlined } from '@ant-design/icons'
import axios from 'axios';
import * as Styles from '../styles/ConstStyles.js';
import { FileView } from './FileView.js';
import image2 from '../assets/profileimgs/2.jpg';
import NavMenu from './NavMenu';
const { Meta } = Card;
const TabPane = Tabs.TabPane;

export const Files = () => {
    const { auth, currentGroup } = useAuth();
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

    const getBlobType = (fileType) => {
        var blobType = '';
        switch (fileType) {
            case '.jpg' || '.png' || '.gif':
                blobType = `image/${fileType}`;
            case '.txt':
                blobType = 'text/plain';
            case '.html':
                blobType = 'text/html';
            case '.pdf':
                blobType = 'application/pdf';
            case '.doc':
                blobType = 'application/msword';
            case '.docx':
                blobType = 'application/vnd.openxmlformats-officedocument.wordprocessingml.document';
        }
        return blobType;
    }

    const getFiles = () => {
        axios.get('files/GetGroupFiles')
            .then(res => {
                var fileContents = []
                fileContents = res.data['returnedObject'].map(file => {
                    const binaryData = atob(file['data']);
                    const arrayBuffer = new ArrayBuffer(binaryData.length);
                    const uint8Array = new Uint8Array(arrayBuffer);
                    for (let i = 0; i < binaryData.length; i++) {
                        uint8Array[i] = binaryData.charCodeAt(i);
                    }
                    const blob = new Blob([uint8Array], { type: getBlobType(file.contentType) })
                    return {
                        ...file,
                        data: binaryData,
                        url: URL.createObjectURL(blob)
                    }
                });
                setFiles(fileContents);
            })
            .catch((error) => {
                console.error(error)
                // failure file display view
            });
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
                            style={{ width: 150 }}
                            cover={
                                <Image
                                    src={file.url}
                                    onError={() => console.error(`Error loading image ${file.url}`)}
                                    preview={false}/>}>
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
                    <TabPane tab="Group Files" key="1">
                        <Space direction="horizonal" size={32}>
                            {displayFileView()}
                            <Button
                                style={Styles.fileButtonStyle}
                                shape="round"
                                icon={<PlusCircleOutlined />}
                                size={'large'}
                                onClick={() => attemptFileUpload()}>
                                Add file
                            </Button>
                        </Space>
                    </TabPane>
                    <TabPane tab="Deleted Files" key="2">{displayDeletedFiles()}</TabPane>
                </Tabs>
                <FileView show={showFile} close={() => setShowFile(false)} file={selectedFile} />
            </div>
        </div>
    );
};

export default Files;