import React, { Component, useState, useEffect } from 'react';
import { Collapse, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink, UncontrolledDropdown, DropdownToggle, DropdownMenu, DropdownItem } from 'reactstrap';
import LogoutModal from './LogoutModal'
import { Link } from 'react-router-dom';
import { useAuth } from './AuthContext';
import '../styles/NavMenu.css';
import axios from 'axios'
import { notification } from 'antd';
import * as Styles from '../styles/ConstStyles.js';

export const Header = () => {
    const { setAuth, auth, currentUser } = useAuth();
    const [showModal, setShowModal] = useState(false);
    const [collapsed, setCollapsed] = useState(true);

    const toggleHeader = () => {
        setCollapsed(!collapsed);
    }

    const attemptLogout = () => {
        console.log("Attempting logout...");
        console.log(currentUser);
        axios.post('logout/AttemptLogout', currentUser)
            .then(res => {
                console.log(res.data)
                var isSuccessful = res.data['isSuccessful'];
                if (isSuccessful) {
                    this.state.logoutResults = true;
                    // return to main page
                    setAuth(false);
                } else {
                    failureLogoutView(res.data['message']);
                }
            })
            .catch((error) => { console.error(error) });
    };

    const failureLogoutView = (failureMessage) => {
        // Accounts for user failure cases and system errors
        this.state.logoutResults = false;
        notification.open({
            message: "Try again.",
            description: failureMessage,
            duration: 10,
            placement: "topRight",
        });
    }

    return (
        <header>
            <Navbar className={Styles.defaultHeaderStyle} container light>
                <NavbarBrand tag={Link} to="/">WeCasa</NavbarBrand>
                <NavbarToggler onClick={toggleHeader} className="mr-2" />
                <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={!collapsed} navbar>
                    {(!auth) ?
                        (<ul className="navbar-nav flex-grow">
                            <NavItem>
                                <NavLink tag={Link} className="text-dark" to="/feedback">Feedback</NavLink>
                            </NavItem>
                        </ul>
                        ) : (<UncontrolledDropdown>
                                <DropdownToggle>Profile</DropdownToggle>
                                <DropdownMenu>
                                    <DropdownItem disabled>Settings</DropdownItem>
                                    <DropdownItem href="/edit-profile">Edit Profile</DropdownItem>
                                    <DropdownItem href="/settings">Account Settings</DropdownItem>
                                    <DropdownItem href="/feedback">Help</DropdownItem>
                                    <DropdownItem onClick={() => setShowModal(true)}>Logout</DropdownItem>
                                    <LogoutModal show={showModal} close={() => setShowModal(false)} confirm={attemptLogout} />
                                </DropdownMenu>
                            </UncontrolledDropdown>)
                    }
                </Collapse>
            </Navbar>
        </header>
    );
};

export default Header;