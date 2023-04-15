import React, { Component } from 'react';
import { Modal, ConfigProvider, Button } from 'antd';
import * as Styles from '../../styles/ConstStyles.js';
import '../../styles/System.css';
import '../../index.css';

const GroupDeletionModal = (props) => {
    return (
        <Modal
            title="Are you sure you want to delete this group?"
            open={props.show}
            closable={false}
            centered="true"
            footer={[
                <Button key="cancel" onClick={props.close} type="default" style={Styles.defaultButtonModal}>No thanks.</Button>,
                <Button key="logout" onClick={props.confirm} type="primary" style={Styles.deleteButtonModal}>Delete Group</Button>
            ]}>
        </Modal>
    );
}

export default GroupDeletionModal;