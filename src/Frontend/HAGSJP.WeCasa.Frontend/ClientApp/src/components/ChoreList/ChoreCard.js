import React, { Component, useState, useEffect } from 'react';
import { Card, Avatar} from 'antd';
import * as Styles from '../../styles/ConstStyles.js';
import '../../styles/System.css';
import '../../index.css';
import axios from 'axios';
import defaultImage from '../../assets/defaultimgs/wecasatemp.jpg';
import * as ValidationFuncs from '../../scripts/InputValidation.js';
import { SendOutlined, EllipsisOutlined, SettingOutlined, CheckSquareOutlined } from '@ant-design/icons';
import IconStack from './IconStack'
import Nudge from '../Nudge/Nudge';
const { Meta } = Card;


export const ChoreCard = (props) => {
    return (<div className="padding-bottom">
        <Card
            style={{
                marginBottom: 10,
                width: 120,
                borderColor: 'black'
            }}
            actions={[
                <Nudge key="nudge" assignedUser="Assignee" />,
                <CheckSquareOutlined key="complete"/>
            ]}
        >
            <Avatar className='padding' src="https://xsgames.co/randomusers/avatar.php?g=pixel" />
            <h6 className="mulish-font"><b>Chore name</b></h6>
            <p className="mulish-font">assigned usernames</p>
            <p className="mulish-font" style={{ color: 'gray' }}><i>Notes</i></p>
        </Card>
    </div>);
};

export default ChoreCard;