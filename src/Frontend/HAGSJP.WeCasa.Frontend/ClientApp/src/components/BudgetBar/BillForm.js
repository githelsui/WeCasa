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

export const BillForm = (flow) => {

  const [form] = Form.useForm();
  const [members, setMembers] = useState([]);
  const [name, setName] = useState('');
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

    // Flow: 1
    const persistAddForm = () =>
    {
      // TEST DATA
      let request =  {          
            Usernames : members,
            Owner: 'frost@gmail.com',
            BillId: 13243,
            GroupId: 123456,
            BillName: name,
            BillDescription: description,
            Amount: amount,
            PaymentStatus: paymentStatus,
            IsRepeated: isRepeated,
            PhotoFileName: photoFileName
      }  
       console.log(request)
      axios.post('budgetbar/AddBill', request).then(res => {
          var response = res.data;
            console.log(response);
      })
      .catch((error => { console.error(error) }));
      setopen(false);
    };

    // Flow: 2
    const persistEditForm = () =>
    {
      // TEST DATA
      let request =  {
        Bill : {
            Usernames : members,
            BillId: 13243,
            BillName: 'name',
            BillDescription: 'description',
            Amount: amount,
            PaymentStatus: paymentStatus,
            IsRepeated: isRepeated,
            PhotoFileName: photoFileName
        }
      } 

      axios.put('budgetbar/EditBill', request).then(res => {
          var response = res.data;
            console.log(response);
      })
      .catch((error => { console.error(error) }));
      setopen(false);
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
      {/* <Button style={Styles.primaryButtonModal} onClick={() => { setopen(true);}}>Add Bill</Button> */}
        <Modal
            open={open}
            title="Add Bill"
            okText="Save"
            cancelText="Cancel"
            onCancel={() => {setopen(false);}}
            onOk={() => {setopen(false);}}
            footer={[
              <Button key="submit" style={Styles.primaryButtonModal} type="primary" onClick={() => {persistAddForm()}}>Save</Button>,
              <Button key="cancel" style={Styles.defaultButtonModal} type="default" onClick={() => setopen(false)}>Cancel</Button>
            ]}>
            <Form name="billForm" {...formItemLayout} >
                <Form.Item name="name" label=" Bill Name" 
                  rules={[
                      {
                      required: true,
                      message: 'Please input a bill name!',
                      },
                  ]}>
                  <Input onChange={e => setName(e.target.value)}/>
                </Form.Item>

                <Form.Item name="description" label="Description">
                  <Input onChange={e => setDescription(e.target.value)}/>
                </Form.Item> 

                <Form.Item name="input-number" label="Amount">
                  <InputNumber min={0} max={1000000000} onChange={value => setAmount(value)}/>
                </Form.Item>

                <Form.Item name="isRepeated" label="Repeat" valuePropName="checked">
                  <Checkbox value="M" style={{ lineHeight: '32px' }} onChange={() => setIsRepeated(true)}>Monthly</Checkbox>
                </Form.Item>

                <Form.Item name="members" label="Members">
                  <Select mode="multiple" onChange={e => setMembers(e)}>
                    <Option value="captain@gmail.com" >captain@gmail.com</Option>
                    <Option value="wendy@gmail.com">wendy@gmail.com</Option>
                    <Option value="strange@gmail.com">strange@gmail.com</Option>
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


  
export default BillForm;