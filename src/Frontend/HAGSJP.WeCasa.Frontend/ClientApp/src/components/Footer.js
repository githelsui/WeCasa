import React, { Component, useState, useEffect } from 'react';
import { NavLink, Navbar, NavItem } from 'reactstrap';
import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from './AuthContext';
import '../styles/NavMenu.css';
import axios from 'axios'
import { notification, Avatar } from 'antd';
import { UserOutlined } from '@ant-design/icons'
import * as Styles from '../styles/ConstStyles.js';

export const Footer = () => {
    return (
        <footer>
            <Navbar className={Styles.defaultHeaderStyle} container light>
                <ul className="navbar-nav flex-grow">
                    <NavItem>
                        <NavLink tag={Link} className="text-dark" to="/feedback">Contact Us</NavLink>
                    </NavItem>
                </ul>
            </Navbar>
        </footer>
    );
};

export default Footer;