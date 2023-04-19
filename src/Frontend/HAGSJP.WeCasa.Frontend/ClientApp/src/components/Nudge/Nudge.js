import React, { useState } from 'react';
import { Button, Popover, Radio, notification } from 'antd';
import { SendOutlined } from '@ant-design/icons';
import './Nudge.css';

const NudgeButton = ({ assignedUser }) => {
    const [selectedOption, setSelectedOption] = useState(null);

    const textOptions = [
        'Option 1: Friendly reminder',
        'Option 2: Hey, time is running out',
        'Option 3: Please finish this task',
        'Option 4: Urgent reminder',
    ];

    const handleChange = (e) => {
        setSelectedOption(e.target.value);
    };

    const handleSend = () => {
        console.log(`Nudge sent with message: ${selectedOption}`);
        showNotification();
    };

    const showNotification = () => {
        notification.open({
            message: 'Sent',
            placement: 'bottomCenter',
            duration: 2,
        });
    };

    const nudgeContent = (
        <div>
            <p>Send a friendly reminder to {assignedUser}!</p>
            <Radio.Group onChange={handleChange} value={selectedOption} style={{ width: '100%' }}>
                {textOptions.map((option, index) => (
                    <Radio.Button
                        key={index}
                        value={option}
                        className="radio-button"
                    >
                        <Radio value={option}>{option}</Radio>
                    </Radio.Button>
                ))}
            </Radio.Group>
            <br />
            <Button type="primary" disabled={!selectedOption} onClick={handleSend} className="send-button">
                Send Nudge
      </Button>
        </div>
    );

    return (
        <Popover content={nudgeContent} title={`Nudge ${assignedUser}`} trigger="click">
            <Button shape="circle" icon={<SendOutlined />} />
        </Popover>
    );
};

export default NudgeButton;
