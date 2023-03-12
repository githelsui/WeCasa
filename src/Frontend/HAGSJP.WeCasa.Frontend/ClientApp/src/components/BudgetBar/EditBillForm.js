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
    {console.log("IN COMPO", props.bill)}
  const [members, setMembers] = useState(props.bill.usernames);
  const [name, setName] = useState(props.bill.billName);
  const [description, setDescription] = useState(props.bill.description);
  const [amount, setAmount] = useState(props.bill.amount);
  const [paymentStatus, setPaymentStatus] = useState(props.bill.paymentStatus);
  const [isRepeated, setIsRepeated] = useState(props.bill.IsRepeated);
  const [photoFileName, setPhotoFileName] = useState('');
  {console.log("DESCRIPTION",isRepeated)}
  const formItemLayout = {
    labelCol: { span: 6 },
    wrapperCol: { span: 14 },
  }

    const persistEditForm = () =>
    {
      // TEST DATA
      let request =  {
            Usernames : members,
            Owner: props.bill.owner,
            BillId: props.bill.billID,
            groupID: props.bill.groupID,
            BillName: name,
            BillDescription: description,
            Amount: amount,
            PaymentStatus: paymentStatus,
            IsRepeated: isRepeated,
            PhotoFileName: photoFileName
      } 
      console.log("EDIT BILL", request)

    axios.put('budgetbar/EditBill', request).then(res => {
          var response = res.data;
            console.log(response);
      })
      .catch((error => { console.error(error) }));
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
                      message: 'Please input a bill name!',
                      },
                  ]}>
                  <Input onChange={e => setName(e.target.value)} defaultValue={name}/>
                </Form.Item>

                <Form.Item name="description" label="Description">
                  <Input onChange={e => e?.target?.value && setDescription(e.target.value)} defaultValue={description}/>
                </Form.Item> 

                <Form.Item name="input-number" label="Amount">
                  <InputNumber min={0} max={1000000000} onChange={value => setAmount(value)} defaultValue={amount}/>
                </Form.Item>

                <Form.Item name="isRepeated" label="Repeat" valuePropName="checked">
                  <Checkbox value="M" style={{ lineHeight: '32px' }} onChange={() => setIsRepeated(true)}>Monthly</Checkbox>
                </Form.Item>

                <Form.Item name="members" label="Members">
                  <Select mode="multiple" defaultValue={members} onChange={e => setMembers(e)}>
                    <Option value="captain@gmail.com" >captain@gmail.com</Option>
                    <Option value="wendy@gmail.com">wendy@gmail.com</Option>
                    <Option value="strange@gmail.com">strange@gmail.com</Option>
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