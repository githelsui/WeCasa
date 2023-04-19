import React, { Component, useState, useEffect } from 'react';
import { Modal, ConfigProvider, Button, Row, Col, Image, Space, Input, Form, Switch, notification, Card } from 'antd';
import * as Styles from '../../styles/ConstStyles.js';
import { useAuth } from '../AuthContext.js';
import '../../styles/System.css';
import '../../index.css';
import axios from 'axios';
import defaultImage from '../../assets/defaultimgs/wecasatemp.jpg';
import * as ValidationFuncs from '../../scripts/InputValidation.js';
import IconSelectorModal from "../IconSelectorModal.js";
import ChoreWeek from './ChoreWeek'

export const ChoreToDoTab = (props) => {
    return (<div>
        <ChoreWeek />
         </div>);
};

export default ChoreToDoTab;