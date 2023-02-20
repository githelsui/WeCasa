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
import * as Styles from '../../styles/ConstStyles';

const { Option } = Select;

const BillForm = () => {
    const [open, setopen] = useState(false);

    const onCreate = (values) => {
      console.log('Received values of form: ', values);
      setopen(false);
    };
  
    return (
      <div>
        <Button
          style={Styles.primaryButtonModal}
          onClick={() => { setopen(true);}}>
          Add Bill
        </Button>
        <CreateForm
          open={open}
          onCreate={onCreate}
          onCancel={() => {setopen(false);}}
        />
      </div>
    );
  };

const CreateForm = ({ open, onCreate, onCancel }) => {
    const [form] = Form.useForm();

    const onFinish = (values) => {
        console.log('Received values of form: ', values);
    };

    return (
        <Modal
            open={open}
            title="Add Bill"
            okText="Save"
            cancelText="Cancel"
            onCancel={onCancel}
            onOk={() => {
            form
                .validateFields()
                .then((values) => {
                form.resetFields();
                onCreate(values);
                })
                .catch((info) => {
                console.log('Validate Failed:', info);
                });
            }}>
            <Form
                name="validate_other"
                {...formItemLayout}
                onFinish={onFinish}
                initialValues={{ 'input-number': 3, 'checkbox-group': ['A', 'B'], rate: 3.5 }}
                style={{ maxWidth: 600 }}
                >

                <Form.Item
                name="name"
                label="Name"
                rules={[
                    {
                    required: true,
                    message: 'Please input Name!',
                    },
                ]}>
                <Input/>
                </Form.Item>

                <Form.Item
                name="description"
                label="Description">
                    <Input />
                </Form.Item> 

                <Form.Item name="input-number" label="Amount">
                    <InputNumber min={0} max={1000000000} />
                </Form.Item>

                <Form.Item name="checkbox-group" label="Repeat">
                    <Checkbox value="F" style={{ lineHeight: '32px' }}>
                    Monthly
                    </Checkbox>
                </Form.Item>

                <Form.Item
                name="select-multiple" label="Members"
                >
                <Select mode="multiple" >
                    <Option value="red">Red</Option>
                    <Option value="green">Green</Option>
                    <Option value="blue">Blue</Option>
                </Select>
                </Form.Item>

                <Form.Item
                name="radio-button"
                label="Radio.Button"
                rules={[{ required: true, message: 'Please pick an item!' }]}
                >
                <Radio.Group>
                    <Radio.Button value="a">PAID</Radio.Button>
                    <Radio.Button value="b">UNPAID</Radio.Button>
                </Radio.Group>
                </Form.Item>

                <Form.Item label="Dragger">
                <Form.Item name="dragger" valuePropName="fileList" getValueFromEvent={normFile} noStyle>
                    <Upload.Dragger name="files" action="/upload.do">
                    <p className="ant-upload-drag-icon">
                        <InboxOutlined />
                    </p>
                    <p className="ant-upload-text">Upload new image</p>
                    </Upload.Dragger>
                </Form.Item>
                </Form.Item>
            </Form>
        </Modal>
    );
};

const formItemLayout = {
    labelCol: { span: 6 },
    wrapperCol: { span: 14 },
  };
  
const normFile = (e) => {
  console.log('Upload event:', e);
  if (Array.isArray(e)) {
      return e;
  }
  return e?.fileList;
};

  
export default BillForm;