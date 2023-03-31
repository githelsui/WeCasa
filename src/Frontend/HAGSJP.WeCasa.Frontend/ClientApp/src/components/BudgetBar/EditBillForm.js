import React, { useState } from 'react';
import 'bootstrap/dist/css/bootstrap.min.css';
import { InboxOutlined } from '@ant-design/icons';
import {
  Button,
  Checkbox,
  Form,
  InputNumber,
  Radio,
  Select,
  Upload,
  Modal,
  Input
} from 'antd';
import axios from 'axios';
import * as Styles from '../../styles/ConstStyles';

const { Option } = Select;

export const EditBillForm = (props) => {
  const [members, setMembers] = useState(props.bill.usernames);
  const [name, setName] = useState(props.bill.billName);
  // const [billID, setBillID] = useState(props.bill.billID);
  // const [groupID, setGroupID] = useState(props.bill.groupID);
  const [description, setDescription] = useState(props.bill.billDescription);
  const [amount, setAmount] = useState(props.bill.amount);
  const [paymentStatus, setPaymentStatus] = useState(props.bill.paymentStatus === 'PAID'? true : false);
  const [isRepeated, setIsRepeated] = useState(props.bill.isRepeated);
  const [photoFileName, setPhotoFileName] = useState(props.bill.photoFileName);
  const formItemLayout = {
    labelCol: { span: 6 },
    wrapperCol: { span: 14 },
  }

    const persistEditForm = () =>
    {
      console.log("PROP BILL", props.bill)
      let request =  {
            usernames : members,
            owner: props.bill.owner,
            billID: props.bill.billID,
            groupID: props.bill.groupID,
            billName: name,
            billDescription: description,
            amount: amount,
            paymentStatus: paymentStatus,
            isRepeated: isRepeated,
            photoFileName: photoFileName
      } 
      console.log("EDIT BILL", request)

    axios.put('budgetbar/EditBill', request).then(res => {
          var response = res.data;
            console.log(response);
      })
      .catch((error => { console.error(error) }));

      paymentStatus? request.paymentStatus = 'PAID' : request.paymentStatus = 'UNPAID'
      request.date = props.bill.date
      let filteredList = props.activeBills.filter(Bill => Bill.billID !== props.bill.billID)
      console.log("FILTERED ACTIVE1", filteredList)
      const newList =  [...filteredList, request]
      console.log("New List", newList)

      props.setActiveBills(newList)
      console.log("ACTIVE3", props.activeBills)
      console.log("BILLID", props.bill.billID)
      props.handleCurrentTable()
      console.log(newList)
      props.setOpen(false);
    };

    const normFile = (e) => {
      console.log('Upload event:', e);
      if (Array.isArray(e)) {
          return e;
      }
      return e?.fileList;
    }
    
    return (
      <div>
        <Modal
            // open={open}
            title="Edit Bill"
            open={props.show}
            onCancel={props.close}
            footer={[
              <Button key="submit" style={Styles.primaryButtonModal} type="primary" onClick={()=>persistEditForm()}>Save</Button>,
              <Button key="cancel" style={Styles.defaultButtonModal} type="default" onClick={props.close}>Cancel</Button>
            ]}>
            <Form name="EditBillForm" {...formItemLayout} >
                <Form.Item name="name" label=" Bill Name" 
                  rules={[
                    {
                    required: true,
                    message: 'Missing bill name',
                    },
                    {
                      pattern: /^[a-zA-Z0-9 ]{1,60}$/,
                      message: 'Invalid bill name',
                    },
                ]}>
                  <Input onChange={e => setName(e.target.value)} defaultValue={name}/>
                </Form.Item>

                <Form.Item name="description" label="Description"
                  rules={[
                    {
                      pattern: /^[a-zA-Z0-9 ]{0,2000}$/,
                      message: 'Description is too long',
                    },
                ]}>
                  <Input onChange={e => e?.target?.value && setDescription(e.target.value)} defaultValue={description}/>
                </Form.Item> 

                <Form.Item name="input-number" label="Amount"
                    rules={[
                      {
                      required: true,
                      message: 'Invalid amount',
                      },
                      {
                        pattern: /^\d+(\.\d{1,2})?$/,
                        message: 'Amount should be in $X.XX format',
                      },
                      {
                        validator(_, input) {
                          if ((parseInt(input) + parseInt(props.groupTotal)) >= props.budget) {
                            return Promise.reject('Amount exceeds budget');
                          }
                          return Promise.resolve();
                        },
                      },
                    ]}>
                  <InputNumber min={0} max={props.budget} onChange={value => setAmount(value)} defaultValue={amount}/>
                </Form.Item>

                <Form.Item name="isRepeated" label="Repeat" valuePropName="checked">
                  <Checkbox value="M" style={{ lineHeight: '32px' }} onChange={() => setIsRepeated(true)}>Monthly</Checkbox>
                </Form.Item>

                <Form.Item name="members" label="Members">
                  <Select mode="multiple" onChange={e => setMembers(e)} >
                    {props.members.map(member => (
                      <Option key={member.username} value={member.username}>{member.name}</Option>
                    ))}
                  </Select>
                </Form.Item>

                <Form.Item name="paymentStatus" label="Radio.Button"
                  rules={[{ required: true, message: 'Please pick an item!' }]}>
                <Radio.Group defaultValue={paymentStatus? "a" : "b"}>
                    <Radio.Button value="a" onChange={() => setPaymentStatus(true)}>PAID</Radio.Button>
                    <Radio.Button value="b" onChange={() => setPaymentStatus(false)}>UNPAID</Radio.Button>
                </Radio.Group>
                </Form.Item>

                <Form.Item label="Receipt">
                <Form.Item name="dragger" valuePropName="fileList" getValueFromEvent={normFile} noStyle>
                    <Upload.Dragger name="files" action="/upload.do" onChange={(value) => setPhotoFileName(value)}>
                      <p className="ant-upload-drag-icon" >
                        <InboxOutlined />
                    </p>
                    <p className="ant-upload-text">Upload new image</p>
                    </Upload.Dragger>
                </Form.Item>
                </Form.Item>
            </Form>
        </Modal>
      </div>
    );
};


  
export default EditBillForm;