import React, { Component, useState, useEffect } from 'react';
import { Collapse, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink, UncontrolledDropdown, DropdownToggle, DropdownMenu, DropdownItem } from 'reactstrap';
import LogoutModal from './LogoutModal'
import { Link } from 'react-router-dom';
import Home from './Home'
import '../styles/NavMenu.css';
import * as Styles from '../styles/ConstStyles.js';

export class NavMenu extends Component {
    static displayName = NavMenu.name;

    constructor(props) {
        super(props);

        this.state = {
            showModal: false,
            collapsed: true,
        };
    }


    render() {
        return (
            <header>
                <div className="menu-outer">
                    <div className="table">
                        <ul className="navbar-nav flex-grow horizonal-list">
                            <NavItem>
                                <NavLink tag={Link} className="text-dark" to="/home">Home</NavLink>
                            </NavItem>
                            <NavItem>
                                <NavLink tag={Link} className="text-dark" to="/bulletin">Bulletin Board</NavLink>
                            </NavItem>
                            <NavItem>
                                <NavLink tag={Link} className="text-dark" to="/calendar">Calendar</NavLink>
                            </NavItem>
                            <NavItem>
                                <NavLink tag={Link} className="text-dark" to="/finanaces">Finances</NavLink>
                            </NavItem>
                            <NavItem>
                                <NavLink tag={Link} className="text-dark" to="/group">Group Members</NavLink>
                            </NavItem>
                        </ul>
                    </div>
                </div>
            </header>
        )
    }
}