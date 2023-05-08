import React, { Component, useState, useEffect } from 'react';
import { NavLink, Navbar, NavItem } from 'reactstrap';
import { Link } from 'react-router-dom';
import '../styles/NavMenu.css';
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