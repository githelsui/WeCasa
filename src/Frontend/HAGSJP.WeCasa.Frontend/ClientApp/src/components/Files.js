import React, { Component, useState, useEffect } from 'react';
import { Navigate, useNavigate } from 'react-router-dom'
import { useAuth } from './AuthContext';
import { Col, Card, Row, Space, ConfigProvider, Button, Image, Tabs, notification } from 'antd';
import { PlusCircleOutlined } from '@ant-design/icons'
import axios from 'axios';
import * as Styles from '../styles/ConstStyles.js';
import { FileView } from './FileView.js';
const { Meta } = Card;
const TabPane = Tabs.TabPane;

export const Files = () => {
    const { currentUser, currentGroup } = useAuth();
    const [files, setFiles] = useState([]);
    const [selectedFile, setSelectedFile] = useState('');
    const [uploadFile, setUploadFile] = useState('');
    const [validInput, setValidInput] = useState(false);
    const [showFile, setShowFile] = useState(false);
    const [refreshSettings, setRefreshSettings] = useState(true);
    const fileInputRef = React.createRef();
    const navigate = useNavigate();
    const validFileTypes = ['image/jpg', 'image/jpeg', 'image/png', 'image/gif', 'text/plain', 'text/html', 'application/pdf', 'application/msword', 'application / vnd.openxmlformats - officedocument.wordprocessingml.document'];
    const validFileExt = ['.jpg', 'jpeg', '.png', '.gif', '.txt', '.html', '.pdf', '.doc', '.docx'];
    const maxFileSize = 10 * 1024 * 1024; // 10 MB
    const maxBucketSize = 15 * 1024 * 1024 * 1024 // 15 GB

    useEffect(() => { getFiles(); }, []);

    const tabItemClick = (key) => {
        console.log('tab click', key);
        if (key == 1) {
            setRefreshSettings(false)
        } else {
            setRefreshSettings(true)
        }
    };

    const getBlobType = (fileType) => {
        var blobType = '';
        switch (fileType) {
            case '.jpg' || '.jpeg' || '.png' || '.gif':
                blobType = `image/${fileType.slice(1)}`;
                break;
            case '.txt':
                blobType = 'text/plain';
                break;
            case '.html':
                blobType = 'text/html';
                break;
            case '.pdf':
                blobType = 'application/pdf';
                break;
            case '.doc':
                blobType = 'application/msword';
                break;
            case '.docx':
                blobType = 'application/vnd.openxmlformats-officedocument.wordprocessingml.document';
                break;
        }
        return blobType;
    }

    const getFiles = () => {
        console.log("Getting group files...");
        let groupId = currentGroup['groupId'];
        axios.get('files/GetGroupFiles', { params: { groupId }})
            .then(res => {
                var isSuccessful = res.data['isSuccessful']
                if (isSuccessful) {
                    var fileContents = []
                    fileContents = res.data['returnedObject'].map(file => {
                        const binaryData = atob(file['data']);
                        const arrayBuffer = new ArrayBuffer(binaryData.length);
                        const uint8Array = new Uint8Array(arrayBuffer);
                        for (let i = 0; i < binaryData.length; i++) {
                            uint8Array[i] = binaryData.charCodeAt(i);
                        }
                        const blobType = getBlobType(file.contentType);
                        const blob = new Blob([uint8Array], { type: blobType })
                        return {
                            ...file,
                            owner: file.fileName.split('/').slice(0, -1).join('/'),
                            fileName: file.fileName.split('/').pop(),
                            data: binaryData,
                            blobType: blobType,
                            url: URL.createObjectURL(blob)
                        }
                    });
                    setFiles(fileContents);
                }
                else {
                    failureFileView(res.data['message']);
                }
            })
            .catch((error) => {
                console.error(error)
            });
    }

    const getUserFile = () => {
        fileInputRef.current.click();
    }

    const handleFileInputChange = (event) => {
        const file = event.target.files[0];
        if (!validFileTypes.includes(file.type)) {
            setValidInput(false);
            toast('Invalid file type');
        }
        if (!validFileExt.includes(file.name.split(".").pop())) {
            setValidInput(false);
            toast('Invalid file type');
        }
        if (file.size > maxFileSize) {
            setValidInput(false);
            toast('File is too large');
        }
        if (validInput) {
            setUploadFile(file);
            attemptFileUpload(file);
        }
    }

    const attemptFileUpload = (file) => {
        const formData = new FormData();
        formData.append('file', file);
        formData.append('name', file.name);
        formData.append('owner', currentUser['username']);
        formData.append('groupId', currentGroup['groupId']);
        
        axios.post('files/UploadFile', formData, { headers: { 'Content-Type': 'multipart/form-data' }})
            .then(res => {
                var isSuccessful = res.data['isSuccessful'];
                if (isSuccessful) {
                    getFiles();
                }
                else {
                    failureFileView(res.data['message']);
                }
            })
            .catch((error) => {
                console.error(error);
            });
    }

    const selectFile = (file) => {
        setSelectedFile(file);
        setShowFile(true);
    }

    const refreshFiles = () => {
        setShowFile(false);
        getFiles();
    }

    const displayFileView = () => {
        var fileList = files.map(function (file, index) {
            return (
                <div key={index} onClick={() => selectFile(file)}>
                    <Col span={10} style={{ marginTop: 16, marginLeft: 16 }}>
                        <Card
                            hoverable
                            style={{ width: 180, overflow: "hidden"}}
                            cover={(file.contentType == ".pdf" || file.contentType == ".txt" || file.contentType == ".doc" || file.contentType == ".docx") ?
                                (<embed src={file.url} type={file.blobType}></embed>) :
                                (<Image
                                    src={file.url}
                                    onError={() => console.error(`Error loading image ${file.url}`)}
                                    preview={false} />)}>
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

    const toast = (title, desc = '') => {
        notification.open({
            message: title,
            description: desc,
            duration: 5,
            placement: 'bottom',
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
                                onClick={() => getUserFile()}>
                                Add file
                            </Button>
                            <input type="file" ref={fileInputRef} onChange={handleFileInputChange} style={{ display: 'none'}}></input>
                        </Space>
                    </TabPane>
                    <TabPane tab="Deleted Files" key="2">{displayDeletedFiles()}</TabPane>
                </Tabs>
                <FileView show={showFile} close={() => refreshFiles() } file={selectedFile} />
            </div>
        </div>
    );
};

export default Files;