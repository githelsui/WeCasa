import React, { Component } from 'react';
import { Modal } from 'antd';
import * as Styles from '../styles/ConstStyles.js';

const LogoutModal = (props) => {
   return (
        <Modal title="Are you sure?"
           open={props.show}
           centered="true"
           okText="Logout"
           cancelText="No, thank you"
           onOk={props.confirm}
            onCancel={props.close}>
        </Modal>
    );
}

export default LogoutModal;