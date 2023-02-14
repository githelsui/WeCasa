import React, { Component, useState, useEffect } from 'react';
import { Collapse, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink, UncontrolledDropdown, DropdownToggle, DropdownMenu, DropdownItem } from 'reactstrap';
import LogoutModal from './LogoutModal'
import { Link } from 'react-router-dom';
import Home from './Home'
import '../styles/NavMenu.css';
import * as Styles from '../styles/ConstStyles.js';

export class Header extends Component {
  static displayName = Header.name;

    constructor(props) {    
        super(props);

        this.toggleHeader = this.toggleHeader.bind(this);
        this.state = {
            showModal: false,
            collapsed: true,
            loggedIn: false
        }; 
    }

  //TODO: Update navbar
  toggleHeader (props) {
    this.setState({
        collapsed: !props.isAuthenticated,
        loggedIn: props.isAuthenticated
    });
    }

    setShowModal(show) {
        this.setState({
            ...this.state,
            showModal: show
        });
    }

  render() {
    return (
        <header>
            <Navbar className={Styles.defaultHeaderStyle} container light>
                <NavbarBrand tag={Link} to="/">WeCasa</NavbarBrand>
                <NavbarToggler onClick={this.toggleHeader} className="mr-2" />
                <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={!this.state.collapsed} navbar>
                    {
                        (!this.state.loggedIn) ?
                            (
                                <ul className="navbar-nav flex-grow">
                                    <NavItem>
                                        <NavLink tag={Link} className="text-dark" to="/feedback">Feedback</NavLink>
                                    </NavItem>
                                </ul>
                            ) : (
                                <UncontrolledDropdown>
                                    <DropdownToggle>Profile</DropdownToggle>
                                    <DropdownMenu>
                                        <DropdownItem disabled>Settings</DropdownItem>
                                        <DropdownItem href="/edit-profile">Edit Profile</DropdownItem>
                                        <DropdownItem href="/settings">Account Settings</DropdownItem>
                                        <DropdownItem href="/feedback">Help</DropdownItem>
                                            <DropdownItem onClick={() => this.setShowModal(true)}>Logout</DropdownItem>
                                            <LogoutModal show={this.state.showModal} close={() => this.setShowModal(false)} confirm={this.props.logout} />
                                        </DropdownMenu>
                                </UncontrolledDropdown>
                            )
                    }
                </Collapse>
            </Navbar>
        </header>
    );
  }
};

export default Header;