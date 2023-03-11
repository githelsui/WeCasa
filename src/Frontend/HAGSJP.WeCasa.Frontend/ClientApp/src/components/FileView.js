import React, { Component, useState, useEffect } from 'react';
import { Modal, ConfigProvider, Button, Row, Col, Image, notification } from 'antd';
import axios from 'axios';
import { DeleteOutlined } from '@ant-design/icons';
import * as Styles from '../styles/ConstStyles.js';
import defaultImage from '../assets/defaultimgs/wecasatemp.jpg';
import '../styles/System.css';
import '../index.css';
import image1 from '../assets/profileimgs/1.jpg';
import image2 from '../assets/profileimgs/2.jpg';
import image3 from '../assets/profileimgs/3.jpg';
import image4 from '../assets/profileimgs/4.jpg';
import image5 from '../assets/profileimgs/5.jpg';
import image6 from '../assets/profileimgs/6.jpg';

export const FileView = (props) => {
    const images = [image1, image2, image3, image4, image5, image6];
    const file = props.file;

    // Source: https://javascript.info/date
    const formatDate = (dateString) => {
        const date = new Date(dateString);
        const month = date.toLocaleString('default', { month: 'long' });
        const day = date.getDate();
        const year = date.getFullYear();
        const time = date.toLocaleString('en-US', {
            hour: 'numeric',
            minute: 'numeric',
            hour12: true
        });
        return `${month} ${day}, ${year} ${time}`;
    }

    const formatFileType = (fileType) => {
        if (fileType != null) {
            return fileType.slice(1).toUpperCase();
        } else {
            return '';
        }
    }

    const deleteFile = () => {
        alert('delete')
        let fileForm = {
            FileName: file.fileName
        }

        axios.post('files/DeleteFile', fileForm)
            .then(res => {
                console.log(res.data);
                props.close();
                toast('File deleted successfully.')
            })
            .catch((error) => { console.error(error) });
    }

    const downloadFile = () => {
        alert('download')
        let fileForm = {
            FileName: file.fileName
        }

        axios.post('files/DownloadFile', fileForm)
            .then(res => {
                console.log(res.data);
                props.close();
                toast('File downloading successfully.')
            })
            .catch((error) => { console.error(error) });
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
        <Modal
            open={props.show}
            closable={true}
            onCancel={props.close}
            maskClosable="false"
            footer={null}
            width={1000}
            centered="true">
            <div className="padding">
                <Row gutter={[24, 24]} align="middle">
                    <Col span={8} className="file-name">
                        <h4 className="mulish-font"><b>{file.fileName}</b></h4>
                    </Col>
                    <Col span={10} className="delete-file-button">
                        <Button size="large" shape="circle" style={{ margin: 20, border: 'none' }} onClick={deleteFile}><DeleteOutlined /></Button>
                    </Col>
                </Row>

                <Row gutter={[24, 24]} align="top">
                    <Col span={16} className="file-object">
                        <Image src={file.url} preview={false} />
                    </Col>
                    <Col span={6}>
                        <div className="file-info-section">
                            <h6 style={{ color: "gray" }} className="mulish-font bold-font">Owner</h6>
                            <Row gutter={[10, 10]} align="top" className="owner-info">
                                <Col span={7}>
                                    <Image style={{
                                        borderRadius: '50%',
                                        objectFit: 'cover',
                                    }}
                                        src={images[4 - 1]}
                                        preview={false} height="50px" width="50px" />
                                </Col>
                                <Col span={17}>
                                    <h6 className="mulish-font bold-font"><b>Firstname Lastname</b></h6>
                                    <p className="mulish-font">Username</p>
                                </Col>
                            </Row> 
                            
                            <h6 style={{ color: "gray" }} className="mulish-font bold-font padding-top">Last updated</h6>
                            <p className="mulish-font">{formatDate(file.lastUpdated)}</p>

                            <h6 style={{ color: "gray" }} className="mulish-font bold-font padding-top">File type</h6>
                            <p className="mulish-font">{formatFileType(file.contentType)}</p>

                            <h6 style={{ color: "gray" }} className="mulish-font bold-font padding-top">File size</h6>
                            <p className="mulish-font">{file.size}</p>
                        </div>
                        <Button onClick={props.download} style={Styles.defaultButtonStyle} type="default" onClick={downloadFile}>Download</Button>
                    </Col>
                </Row>
            </div>
        </Modal>
    );
}

export default FileView;