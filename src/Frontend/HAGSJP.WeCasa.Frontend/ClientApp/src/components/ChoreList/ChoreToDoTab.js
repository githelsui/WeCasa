import React, { Component, useState, useEffect } from 'react';
import { Modal, ConfigProvider, Button, Row, Col, Image, Space, Input, Form, Switch, notification, Card } from 'antd';
import * as Styles from '../../styles/ConstStyles.js';
import '../../styles/System.css';
import '../../index.css';
import ChoreWeek from './ChoreWeek'

export const ChoreToDoTab = (props) => {
    return (<div>
        <ChoreWeek />
         </div>);
};

export default ChoreToDoTab;