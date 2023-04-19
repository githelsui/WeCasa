import React, { Component, useState, useEffect } from 'react';
import { Navigate, useNavigate } from 'react-router-dom'
import { useAuth } from '../AuthContext';
import { Col, Card, Space, Button, Image, Tabs, notification, Pagination } from 'antd';
import { PlusCircleOutlined, FileOutlined } from '@ant-design/icons'
import axios from 'axios';
import * as Styles from '../../styles/ConstStyles.js';
import { FileView } from './FileView.js';
import config from '../../appsettings.json';
const { Meta } = Card;
const TabPane = Tabs.TabPane;

export const Files = () => {
    const { currentUser, currentGroup } = useAuth();
    const [files, setFiles] = useState([]);
    const [deletedFiles, setDeletedFiles] = useState([]);
    const [selectedFile, setSelectedFile] = useState('');
    const [uploadFile, setUploadFile] = useState('');
    const [validInput, setValidInput] = useState(false);
    const [showFile, setShowFile] = useState(false);
    const [refreshSettings, setRefreshSettings] = useState(true);
    const [currentPage, setCurrentPage] = useState(1);
    const fileInputRef = React.createRef();
    const navigate = useNavigate();
    const maxFileSize = 10 * 1024 * 1024; // 10 MB
    const maxBucketSize = 15 * 1024 * 1024 * 1024 // 15 GBS
    const maxFilesPerPage = config.maxFilesPerPage;
    const validFileTypes = config.validFileTypes;
    const validFileExt = config.validFileExt;
    const filesToDisplay = files.slice((currentPage - 1) * maxFilesPerPage, currentPage * maxFilesPerPage);

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
        let groupId = currentGroup['groupId'];
        axios.get('files/GetGroupFiles', { params: { groupId }})
            .then(res => {
                var isSuccessful = res.data['isSuccessful']
                if (isSuccessful) {
                    var fileContents = []
                    fileContents = res.data['returnedObject'].map(file => {
                        // decoding the base-64 string data to binary array
                        const binaryData = atob(file['data']);
                        // creating an array buffer to perform data manipulation on the binary data
                        const arrayBuffer = new ArrayBuffer(binaryData.length);
                        // creating an array of 8-bit unsigned integers necessary for creating the Blob
                        const uint8Array = new Uint8Array(arrayBuffer);
                        // converting binary data into string representation
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

    const getDeletedFiles = () => {
        let groupId = currentGroup['groupId'];
        axios.get('files/GetDeletedFiles', { params: { groupId } })
            .then(res => {
                var isSuccessful = res.data['isSuccessful']
                if (isSuccessful) {
                    var fileContents = []
                    fileContents = res.data['returnedObject'].map(file => {
                        return {
                            ...file,
                            owner: file.fileName.split('/').slice(0, -1).join('/'),
                            fileName: file.fileName.split('/').pop(),
                        }
                    });
                    setDeletedFiles(fileContents);
                }
                else {
                    failureFileView(res.data['message']);
                }
            })
            .catch((error) => {
                console.error(error)
            });
        displayDeletedFiles();
    }

    const getUserFile = () => {
        fileInputRef.current.click();
    }

    const handleFileInputChange = (event) => {
        const file = event.target.files[0];
        if (!validFileTypes.includes(file.type)) {
            setValidInput(false);
            toast('Invalid file type');
            return;
        }
        if (!validFileExt.includes(file.name.split(".").pop())) {
            console.log(file.name.split(".").pop());
            setValidInput(false);
            toast('Invalid file type');
            return;
        }
        if (file.size > maxFileSize) {
            setValidInput(false);
            toast('File is too large');
            return;
        }
        setValidInput(true);
        setUploadFile(file);
        attemptFileUpload(file);
    }

    const handlePageChange = (page) => {
        setCurrentPage(page);
    };

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
                    successFileView(res.data['message']);
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
        var fileList = filesToDisplay.map(function (file, index) {
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
        var deletedFileList = deletedFiles.map(function (file, index) {
            return (
                <div key={index} onClick={() => selectFile(file)}>
                    <Col span={10} style={{ marginTop: 16, marginLeft: 16 }}>
                        <Card
                            hoverable
                            style={{ width: 200 }}>
                            <Meta title={file.fileName}
                                avatar={<FileOutlined className="padding-bottom" />}
                                type="inner"
                                style={{ textAlign: "center", display: "block" }} />
                        </Card>
                    </Col>
                </div>
            );
        });
        return deletedFileList;
    }

    const successFileView = (successMessage) => {
        notification.open({
            message: "",
            description: successMessage,
            duration: 10,
            placement: "topLeft"
        });
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
                            <Pagination
                                current={currentPage}
                                pageSize={maxFilesPerPage}
                                total={files.length}
                                onChange={handlePageChange}
                            />
                            <Button
                                id="add-file"
                                style={Styles.addButtonStyle}
                                shape="round"
                                icon={<PlusCircleOutlined />}
                                size={'large'}
                                onClick={() => getUserFile()}>
                                Add file
                            </Button>
                            <input type="file" ref={fileInputRef} onChange={handleFileInputChange} style={{ display: 'none'}}></input>
                        </Space>
                    </TabPane>
                    <TabPane tab="Deleted Files" key="2">
                        <Space direction="horizonal" size={32}>
                            {getDeletedFiles()}
                            <Pagination
                                current={currentPage}
                                pageSize={maxFilesPerPage}
                                total={files.length}
                                onChange={handlePageChange}
                            />
                        </Space>
                    </TabPane>
                </Tabs>
                <FileView show={showFile} close={() => refreshFiles() } file={selectedFile} />
            </div>
        </div>
    );
};

export default Files;