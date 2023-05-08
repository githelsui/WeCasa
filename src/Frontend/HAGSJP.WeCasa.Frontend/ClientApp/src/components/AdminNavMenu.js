import React, { useState } from 'react';
import { Collapse, Navbar, NavbarBrand, NavbarToggler } from 'reactstrap';
import { NavLink, Link } from 'react-router-dom';
import '../styles/NavMenu.css';
import * as Styles from '../styles/ConstStyles.js';

export const AdminNavMenu = () => {
    return (
        <header>
            <div className="menu-outer">
                <div className="table">
                    <ul className="navbar-nav flex-grow horizonal-list">
                        <li>
                            <NavLink tag={Link}
                                style={({ isActive }) => ({
                                    ...Styles.navLink,
                                    ...(isActive ? Styles.activeStyle : null)
                                })}
                                to="/home">Home
                            </NavLink>
                        </li>
                        <li>
                            <NavLink tag={Link}
                                style={({ isActive }) => ({
                                    ...Styles.navLink,
                                    ...(isActive ? Styles.activeStyle : null)
                                })}
                                to="/analytics">Analytics
                            </NavLink>
                        </li>
                    </ul>
                </div>
            </div>
        </header>
    )
}

export default AdminNavMenu;