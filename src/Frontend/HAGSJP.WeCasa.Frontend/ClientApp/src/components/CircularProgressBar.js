import React, { Component, useState, useEffect } from 'react';
import { Modal, ConfigProvider, Button, Form, Input, Row, Col, notification, Card, Image, Space} from 'antd';
import * as Styles from '../styles/ConstStyles.js';
import '../styles/CircularProgressBar.css';
import '../styles/System.css';
import '../index.css';
import { useAuth } from './AuthContext.js';
import axios from 'axios';
const { Meta } = Card;

export const CircularProgressBar = (props) => {
    const value = props.percentage;

    return (
        <div role="progressbar"
            aria-valuenow={value}
            aria-valuemin="0"
            aria-valuemax="100">
        </div>
    );
};

export default CircularProgressBar;