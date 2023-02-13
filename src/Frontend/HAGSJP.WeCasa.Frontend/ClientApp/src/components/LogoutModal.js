import React, { Component } from 'react';
import { Modal } from 'antd';

const LogoutModal = (props) => {
   return (
        <Modal title="Are you sure?"
           open={props.show}
           centered="true"
           onOk={props.confirm}
            onCancel={props.close}>
            <p>Please confirm you want to logout by pressing OK.</p>
        </Modal>
    );
}

export default LogoutModal;