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

export const BillForm = () => {

  const [form] = Form.useForm();
  const [members, setMembers] = useState([]);
  const [name, setName] = useState("");
  const [description, setDescription] = useState("");
  const [amount, setAmount] = useState(0);
  const [paymentStatus, setPaymentStatus] = useState(false);
  const [isRepeated, setIsRepeated] = useState(false);
  const [photoFileName, setPhotoFileName] = useState("");

  const [open, setopen] = useState(false);

    const onCreate = (values) => {
      console.log('Received values of form: ', values);
      setopen(false);
    };

    const formItemLayout = {
      labelCol: { span: 6 },
      wrapperCol: { span: 14 },
    };

    const persistAddForm = () =>
    {
      // let request =  {
      //   Usernames : members,
      //   Bill : {
      //       Username: "Jan",
      //       BillId: 13243,
      //       GroupId: 123456,
      //       DateEntered: Date.UTC,
      //       BillName: name,
      //       BillDescription: description,
      //       Amount: amount,
      //       PaymentStatus: paymentStatus,
      //       IsRepeated: isRepeated,
      //       IsDeleted: 0,
      //       DateDeleted: Date.UTC,
      //       PhotoFileName: photoFileName
      //   }
      // }  

      let request =  {
        Usernames : members,
        Bill : {
            Username: "Jan",
            BillId: 13243,
            GroupId: 123456,
            BillName: name,
            BillDescription: description,
            Amount: amount,
            PaymentStatus: paymentStatus,
            IsRepeated: isRepeated,
            PhotoFileName: photoFileName
        }
      } 
      
      console.log(request);

      axios.post(`budgetbar/AddBill`, request).then(res => {
          var isSuccessful = res.data;
          if (isSuccessful) {
            console.log(res);
          } else {
            console.log(res);
          }
      })
      .catch((error) => { console.error(error) });
      setopen(false);
    }

    // const BillModal = () => {
    //     const [open, setopen] = useState(false);

    //     const onCreate = (values) => {
    //       console.log('Received values of form: ', values);
    //       setopen(false);
    //     };
      
    //     return (
    //       <div>
    //         <Button
    //           style={Styles.primaryButtonModal}
    //           onClick={() => { setopen(true);}}>
    //           Add Bill
    //         </Button>
    //         <BillForm
    //           open={open}
    //           onCreate={onCreate}
    //           onCancel={() => {setopen(false);}}
    //         />
    //       </div>
    //     );
    //   };

    const normFile = (e) => {
      console.log('Upload event:', e);
      if (Array.isArray(e)) {
          return e;
      }
      return e?.fileList;
    }
    

    return (
      <div>
        <Button style={Styles.primaryButtonModal} onClick={() => { setopen(true);}}> Add Bill</Button>
        <Modal 
            open={open}
            title="Add Bill"
            okText="Save"
            cancelText="Cancel"
            onCancel={() => {setopen(false);}}
            onOk={() => {setopen(false);}}>
            <Form name="billForm" {...formItemLayout}  onFinish={() => persistAddForm()}>
                <Form.Item name="name" label="Name"
                rules={[
                    {
                    required: true,
                    message: 'Please input Name!',
                    },
                ]}>
                <Input onChange={e => setName(e.target.value)}/>
                </Form.Item>

                <Form.Item name="description" label="Description">
                    <Input onChange={value => setDescription(value)}/>
                </Form.Item> 

                <Form.Item name="amount" label="Amount"
                    rules={[
                        {
                        required: true,
                        message: 'Please input Name!',
                        },
                    ]}>
                    <InputNumber min={0} max={1000000000} onChange={value => setAmount(value)}/>
                </Form.Item>

                <Form.Item name="isRepeated" label="Repeat" valuePropName="checked">
                    <Checkbox value="M" style={{ lineHeight: '32px' }} onChange={value => setIsRepeated(value)}>
                    Monthly
                    </Checkbox>
                </Form.Item>

                <Form.Item name="members" label="Members">
                <Select mode="multiple" >
                    <Option value="red" onChange={value => members.push(value)}>Red</Option>
                    <Option value="green">Green</Option>
                    <Option value="blue">Blue</Option>
                </Select>
                </Form.Item>

                <Form.Item name="paymentStatus" label="Radio.Button"
                rules={[{ required: true, message: 'Please pick an item!' }]}
                >
                <Radio.Group>
                    <Radio.Button value="a" onChange={value => setPaymentStatus(true)}>PAID</Radio.Button>
                    <Radio.Button value="b" onChange={value => setPaymentStatus(false)}>UNPAID</Radio.Button>
                </Radio.Group>
                </Form.Item>

                <Form.Item label="Receipt">
                <Form.Item name="receiptFile" valuePropName="fileList" getValueFromEvent={normFile} noStyle>
                    <Upload.Dragger name="files" action="/upload.do" onChange={value => setPhotoFileName(value)}>
                    <p className="ant-upload-drag-icon">
                        <InboxOutlined />
                    </p>
                    <p className="ant-upload-text">Upload new image</p>
                    </Upload.Dragger>
                </Form.Item>
                </Form.Item>
                <Button style={Styles.primaryButtonStyle} type="default" htmlType="submit" onClick={(value) => {persistAddForm(value)}}>Save</Button>
                {/* <Button style={Styles.primaryButtonStyle} type="default" htmlType="cancel" onClick={setopen(false)}>Cancel</Button> */}

            </Form>
        </Modal>
      </div>
    );
}
      

  
export default BillForm;