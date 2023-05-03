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

export const BillForm = (props) => {
  const [members, setMembers] = useState(props.members);
  const [name, setName] = useState('');
  const [owner, setOwner] = useState(props.user);
  const [groupId, setGroupId] = useState(props.group.groupId);
  const [description, setDescription] = useState('');
  const [amount, setAmount] = useState(0);
  const [paymentStatus, setPaymentStatus] = useState(false);
  const [isRepeated, setIsRepeated] = useState(false);
  const [photoFileName, setPhotoFileName] = useState('');
  const [open, setopen] = useState(true);

  const formItemLayout = {
    labelCol: { span: 6 },
    wrapperCol: { span: 14 },
  }

    const persistAddForm = () =>
    {
      let date = new Date();
      let formattedDate = date.getMonth() + "/" + date.getDay() + "/" + date.getFullYear();
      let request =  {          
            usernames: members,
            owner: owner,
            billId: 0,
            groupId: groupId,
            billName: name,
            billDescription: description,
            amount: amount,
            paymentStatus: paymentStatus,
            isRepeated: isRepeated,
            photoFileName: photoFileName
      }
       console.log(request)
      axios.post('budgetbar/AddBill', request).then(res => {
          var response = res.data;
          console.log(response);
          props.setGroupTotal(props.groupTotal + amount)
      })
      .catch((error => { console.error(error) }));
      request.date = formattedDate
      paymentStatus? request.paymentStatus = 'PAID' : request.paymentStatus = 'UNPAID'
      const newList =  [...props.activeBills, request];
      props.setActiveBills(newList)
      props.handleCurrentTable()
      setopen(false);
    };
    
    return (
      <div>
        <Modal
            open={open}
            title="Add Bill"
            onCancel={() => {setopen(false);}}
            footer={[
              <Button key="submit" style={Styles.primaryButtonModal} type="primary" onClick={() => {persistAddForm()}}>Save</Button>,
              <Button key="cancel" style={Styles.defaultButtonModal} type="default" onClick={() => setopen(false)}>Cancel</Button>
            ]}>
            <Form name="billForm" {...formItemLayout} >
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
                  <Input onChange={e => setName(e.target.value)}/>
                </Form.Item>

                <Form.Item name="description" label="Description"
                  rules={[
                    {
                      pattern: /^[a-zA-Z0-9 ]{0,2000}$/,
                      message: 'Description is too long',
                    },
                ]}>
                  <Input onChange={e => setDescription(e.target.value)}/>
                </Form.Item> 

                <Form.Item name="input-number" label="Amount" 
                    rules={[
                      {
                      required: true,
                      message: 'Amount is required',
                      },
                      {
                        pattern: /^\d+(\.\d{1,2})?$/,
                        message: 'Amount should be in $X.XX format',
                      },
                      {
                        validator(_, input) {
                          if (input + props.groupTotal >= props.budget) {
                            return Promise.reject('Amount exceeds budget');
                          }
                          return Promise.resolve();
                        },
                      },
                  ]}>
                  <InputNumber step={0.01} min={0} onChange={value => setAmount(value)}/>
                </Form.Item>

                <Form.Item name="isRepeated" label="Repeat" valuePropName="checked">
                  <Checkbox value="M" style={{ lineHeight: '32px' }} onChange={() => setIsRepeated(true)}>Monthly</Checkbox>
                </Form.Item>

                <Form.Item name="members" label="Members" rules={[
                      {
                      required: true,
                      message: 'Missing Members',
                      }
                  ]}>
                  <Select mode="multiple" onChange={e => setMembers(e)} >
                    {props.members.map(member => (
                      <Option key={member.username} value={member.username}>{member.name}</Option>
                    ))}
                  </Select>
                </Form.Item>

                <Form.Item name="paymentStatus" label="Radio.Button"
                  rules={[{ required: true, message: 'Please pick an item!' }]}>
                <Radio.Group>
                    <Radio.Button value="a" onChange={() => setPaymentStatus(true)}>PAID</Radio.Button>
                    <Radio.Button value="b" onChange={() => setPaymentStatus(false)}>UNPAID</Radio.Button>
                </Radio.Group>
                </Form.Item>

                <Form.Item label="Receipt">
                <Form.Item name="dragger" valuePropName="fileList" getValueFromEvent={(e)=>e?.fileList} noStyle>
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
  
export default BillForm;