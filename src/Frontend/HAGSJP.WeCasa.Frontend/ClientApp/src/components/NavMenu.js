﻿import React from 'react';
import { Collapse, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';
import '../styles/NavMenu.css';
import * as Styles from '../styles/ConstStyles.js';

export const NavMenu = ()  => {    
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

export default NavMenu;