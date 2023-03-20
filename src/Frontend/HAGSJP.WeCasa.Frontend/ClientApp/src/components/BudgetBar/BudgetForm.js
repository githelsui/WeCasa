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


const BudgetForm = ({budget, setBudget, group}) => {
    const groupId = group.groupId // TEST DATA
    const [open, setopen] = useState(false);
    const [tempBudget, setTempBudget] = useState(0);
  
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

    const updateBudget = () => {
      let request = {
          GroupId: groupId,
          Amount: tempBudget
      }

      axios.put('budgetbar/UpdateBudget', request).then(res => {
          var response = res.data;
            console.log(response);
      })
      .catch((error => { console.error(error) }));
      setopen(false);
      setBudget(tempBudget)
    }
  
    return (
        <div style={Styles.body}>
             <Button
                id="update-budget-btn"
                style={Styles.addBudgetButton}
                onClick={() => { setopen(true);}}>
                Update Budget
            </Button>
            <Modal onCancel={() => {setopen(false);}}
                id="budget-form"
                open={open}
                title="Update Monthly Budget"
                footer={[
                    <Button key="save" type="default" style={Styles.defaultButtonModal} onClick={()=>updateBudget()}>Save</Button>,
                    <Button key="cancel" onClick={onCancel} type="primary" style={Styles.primaryButtonModal}>Cancel</Button>
                 ]}>
                <Form>
                <Form.Item name="input-number">
                    <InputNumber min={0} max={1000000000} onChange={(value)=>setTempBudget(value)}/>
                </Form.Item>
                </Form>
            </Modal>
      </div>
    )
}

export default BudgetForm;