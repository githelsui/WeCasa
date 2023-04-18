import React, { useState } from 'react';
import { Collapse, Navbar, NavbarBrand, NavbarToggler } from 'reactstrap';
import { NavLink, Link } from 'react-router-dom';
import '../styles/NavMenu.css';
import * as Styles from '../styles/ConstStyles.js';

export const NavMenu = () => {
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
                                to="/bulletin">Bulletin Board
                            </NavLink>
                        </li>
                        <li>
                            <NavLink tag={Link}
                                style={({ isActive }) => ({
                                    ...Styles.navLink,
                                    ...(isActive ? Styles.activeStyle : null)
                                })}
                                to="/grocerylist">Groceries
                            </NavLink>
                        </li>
                        <li>
                            <NavLink tag={Link}
                                style={({ isActive }) => ({
                                    ...Styles.navLink,
                                    ...(isActive ? Styles.activeStyle : null)
                                })}
                                to="/calendar">Calendar</NavLink>
                        </li>
                        <li>
                            <NavLink tag={Link}
                                style={({ isActive }) => ({
                                    ...Styles.navLink,
                                    ...(isActive ? Styles.activeStyle : null)
                                })}
                                to="/finances">Finances</NavLink>
                        </li>
                        <li>
                            <NavLink tag={Link}
                                style={({ isActive }) => ({
                                    ...Styles.navLink,
                                    ...(isActive ? Styles.activeStyle : null)
                                })}
                                to="/files">Files</NavLink>
                        </li>
                        <li>
                            <NavLink
                                tag={Link}
                                style={({ isActive }) => ({
                                    ...Styles.navLink,
                                    ...(isActive ? Styles.activeStyle : null)
                                })}
                                to="/group-settings">Group Members</NavLink>
                        </li>
                    </ul>
                </div>
            </div>
        </header>
    )
}

export default NavMenu;