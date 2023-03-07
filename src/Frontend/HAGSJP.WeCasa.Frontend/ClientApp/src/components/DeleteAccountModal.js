import React, { Component } from 'react';
import { Modal, ConfigProvider, Button } from 'antd';
import * as Styles from '../styles/ConstStyles.js';
import '../styles/System.css';
import '../index.css';

const DeleteAccountModal = (props) => {
    return (
           <Modal 
               title="Are you sure you want to delete your account?"
               open={props.show}
               closable={false}
               centered="true"
               footer={[
                   <Button key="cancel" onClick={props.close} type="default" style={Styles.defaultButtonModal}>No</Button>,
                   <Button key="delete" onClick={props.confirm} type="primary" style={Styles.primaryButtonModal}>Yes</Button>
                ]}>
            </Modal>
    );
}

export default DeleteAccountModal;