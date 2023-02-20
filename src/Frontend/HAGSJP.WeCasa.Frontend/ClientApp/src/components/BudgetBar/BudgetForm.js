import React, { useState } from 'react';
import 'bootstrap/dist/css/bootstrap.min.css';
import {
  Form,
  InputNumber,
  Modal,
  Button
} from 'antd';
import * as Styles from '../../styles/ConstStyles';
import axios from 'axios';


const BudgetForm = ({budget, setBudget}) => {
    const [open, setopen] = useState(false);
    // const [amount, setAmount] = useState(0)
  
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

    const submitBudget = (value) => 
    {
        let request = {
          GroupId: 12334,
          Amount: value.budget
        }

        console.log(request);
        axios.put(`budgetbar/UpdateBudget`, request).then(res => {
            var isSuccessful = res.data;
            if (isSuccessful) {
              console.log("Updated Budget!")
            } else {
              console.log("Updated Budget Failed!")
            }
        })
        .catch((error) => { console.error(error) });
        setopen(false);
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
                    <Button key="save" onClick={(values) => submitBudget(values)} type="default" style={Styles.defaultButtonModal}>Save</Button>,
                    <Button key="cancel" onClick={onCancel} type="primary" style={Styles.primaryButtonModal}>Cancel</Button>
                 ]}>
                <Form.Item name="amount">
                    {/* <InputNumber min={0} max={1000000000} placeholder="0" onChange={value => setBudget(value)} type="text" value={budget}/> */}
                    <InputNumber min={0} max={1000000000} placeholder="0" onChange={value => setBudget(value)} type="text" value={budget}/>
                </Form.Item>
            </Modal>
      </div>
    )
}

export default BudgetForm;