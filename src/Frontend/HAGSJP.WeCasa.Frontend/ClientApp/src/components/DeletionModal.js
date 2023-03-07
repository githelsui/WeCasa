import React, { Component } from 'react';
import { Modal, ConfigProvider, Button } from 'antd';
import * as Styles from '../styles/ConstStyles';
// import '../../styles/System.css';
// import '../index.css';

const DeletionModal = (props) => {
    return (
        <Modal
            title={props.message}
            open={props.show}
            // closable={false}
            centered="true"
            footer={[
                <Button key="cancel" onClick={props.close} type="default" style={Styles.defaultButtonModal}>No thanks.</Button>,
                <Button key="confirm" onClick={props.confirm} type="primary" style={Styles.deleteButtonModal}>Delete</Button>
            ]}>
        </Modal>
    );
}

export default DeletionModal;