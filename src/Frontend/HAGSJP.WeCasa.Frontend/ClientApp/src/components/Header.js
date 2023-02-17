import React, { Component, useState, useEffect } from 'react';
import { Collapse, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink, UncontrolledDropdown, DropdownToggle, DropdownMenu, DropdownItem } from 'reactstrap';
import LogoutModal from './LogoutModal'
import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from './AuthContext';
import '../styles/NavMenu.css';
import axios from 'axios'
import { notification } from 'antd';
import * as Styles from '../styles/ConstStyles.js';

export const Header = () => {
    const { setAuth, auth, setCurrentUser, currentUser } = useAuth();
    const [showModal, setShowModal] = useState(false);
    const [collapsed, setCollapsed] = useState(true);
    const [logoutResults, setLogoutResults] = useState(false);

    const toggleHeader = () => {
        setCollapsed(!collapsed);
    }


    const attemptLogout = () => {
        console.log("Attempting logout...");
        let account = {
            Username: currentUser
        }
        axios.post('home/AttemptLogout', account)
            .then(res => {
                console.log(res.data)
                var isSuccessful = res.data['isSuccessful'];
                if (isSuccessful) {
                    setLogoutResults(true);
                    // return to main page
                    setAuth(false);
                    setCurrentUser(null);
                    successLogoutView(res.data['message']);
                } else {
                    failureLogoutView(res.data['message']);
                }
            })
            .catch((error) => { console.error(error) });
    };

    const successLogoutView = (successMessage) => {
        // display confirmation message
        notification.open({
            message: successMessage,
            description: '',
            duration: 10,
            placement: "topRight",
        });
        setShowModal(false);
    }

    const failureLogoutView = (failureMessage) => {
        // Accounts for user failure cases and system errors
        setLogoutResults(false);
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