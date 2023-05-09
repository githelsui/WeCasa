import React, { useState } from 'react';
import { Button, Popover, Radio, notification } from 'antd';
import { SendOutlined } from '@ant-design/icons';
import './Nudge.css';
import { useAuth } from '../Auth/AuthContext';
import axios from 'axios';

const Nudge = ({ assignedUser, groupId, choreId, senderEmail, recipientEmail, onSendNudge }) => { //button


    const [selectedOption, setSelectedOption] = useState(null);
    const { currentGroup, currentUser } = useAuth();

    const textOptions = [
        'Option 1: Friendly reminder',
        'Option 2: Hey, time is running out',
        'Option 3: Please finish this task',
        'Option 4: Urgent reminder',
    ];

    const handleChange = (e) => {
        setSelectedOption(e.target.value);
    };

    const logErrorToDatabase = async (errorMessage, stackTrace) => {
        const logData = {
            Message: errorMessage,
            LogLevel: 'Error',
            Category: 'Nudge',
            Username: currentUser.email,
            Operation: 'Sending Nudge',
            Success: false,
        };

        try {
            const response = await axios.post('nudge/LogData', logData);
            if (response.data.isSuccessful) {
                console.log('Error logged successfully');
            } else {
                console.log('Failed to log error:', response.data.message);
            }
        } catch (error) {
            console.error('Error logging error:', error.message);
        }
    };


    const handleSend = async () => {
        //onSendNudge(); // Add this line to call the onSendNudge function


        let nudgeData = {
            //nudgeId: choreId,
            GroupId: groupId,
            ChoreId: choreId,
            SenderUsername: senderEmail,
            ReceiverUsername: recipientEmail, // Update this property name
            Message: selectedOption,
            //SentAt: "2023-05-07T12:30:00Z",
            IsComplete: false
        };


        try {
            //console.log(nudgeData);
            const response = await axios.post('nudge/SendNudge', nudgeData);
            if (response.data.isSuccessful) {
                console.log("Nudge sent successfully");
                showNotification();
                //onSendNudge(); // Add this line to call the onSendNudge function
            } else {
                console.log("Failed to send Nudge:", response.data.message);
                showNotificationTooMany();
            }
        } catch (error) {
            if (error.response && error.response.status === 429) {
                console.log("Error sending Nudge: Too many Nudges");
                showNotificationTooMany();
            } else {
                console.log("Error sending Nudge:", error.response ? error.response.data.errorMessage : error.message);
                console.log("Stack trace:", error.response ? error.response.data.stackTrace : '');
                showNotificationFail();
                logErrorToDatabase(error.response ? error.response.data.errorMessage : error.message);
            }
        }

    };

    const showNotificationFail = () => {
        notification.open({
            message: 'Nudge Failed. Reload page and try again. Or attempt account recovery',
            placement: 'bottomCenter',
            duration: 2,
        });
    };

    const showNotification = () => {
        notification.open({
            message: 'Sent',
            placement: 'bottomCenter',
            duration: 2,
        });
    };

    const showNotificationTooMany = () => {
        notification.open({
            message: 'Too many Nudges',
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

export default Nudge; //button