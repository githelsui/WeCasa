import React, { Component, useState, useEffect } from 'react';
import { Collapse, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink, UncontrolledDropdown, DropdownToggle, DropdownMenu, DropdownItem } from 'reactstrap';
import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from '../AuthContext';
import '../styles/NavMenu.css';
import axios from 'axios'
import { notification, Avatar } from 'antd';
import { UserOutlined } from '@ant-design/icons'
import * as Styles from '../../styles/ConstStyles.js';

export const Calendar = () => {
    const { setAuth, auth, setCurrentUser, currentUser, setCurrentGroup } = useAuth();
    const [showModal, setShowModal] = useState(false);
    const [collapsed, setCollapsed] = useState(true);

    return (
        
    );
};

export default Calendar;