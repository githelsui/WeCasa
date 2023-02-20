import React, { useState } from 'react';
import 'bootstrap/dist/css/bootstrap.min.css';
import {
  Form,
  InputNumber,
  Modal,
  Button
} from 'antd';
import * as Styles from '../../styles/ConstStyles';

const BudgetForm = (props) => {
    const [open, setopen] = useState(false);
  
    const onCreate = (values) => {
      console.log('Received values of form: ', values);
      setopen(false);
    };
  
    const onCancel= () => {
      setopen(false);
    }

    const onOk= () => {
        Form
            .validateFields()
            .then((values) => {
            Form.resetFields();
            onCreate(values);
            })
            .catch((info) => {
            console.log('Validate Failed:', info);
            });
    }
  
    return (
        <div>
             <Button
                style={Styles.defaultButtonModal }
                onClick={() => { setopen(true);}}>
                Update Budget
            </Button>
            <Modal onCancel={() => {setopen(false);}}
                open={open}
                title="Update Monthly Budget"
                footer={[
                    <Button key="save" onClick={props.save} type="default" style={Styles.defaultButtonModal}>Save</Button>,
                    <Button key="cancel" onClick={onCancel} type="primary" style={Styles.primaryButtonModal}>Cancel</Button>
                 ]}>
                <Form.Item name="input-number">
                    <InputNumber min={0} max={1000000000} />
                </Form.Item>
            </Modal>
      </div>
    )
}

export default BudgetForm;