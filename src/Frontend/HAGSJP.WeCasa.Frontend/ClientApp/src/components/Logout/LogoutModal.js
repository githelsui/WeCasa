import React, { Component } from 'react';
import { Modal, ConfigProvider, Button } from 'antd';
import * as Styles from '../../styles/ConstStyles.js';
import '../../styles/System.css';
import '../../index.css';

const LogoutModal = (props) => {
    return (
           <Modal 
               title="Are you sure you want to logout?"
               open={props.show}
               closable={false}
               centered="true"
               footer={[
                   <Button key="cancel" onClick={props.close} type="default" style={Styles.defaultButtonModal}>No thanks.</Button>,
                   <Button key="logout" onClick={props.confirm} type="primary" style={Styles.primaryButtonModal}>Logout</Button>
                ]}>
            </Modal>
    );
}

export default LogoutModal;