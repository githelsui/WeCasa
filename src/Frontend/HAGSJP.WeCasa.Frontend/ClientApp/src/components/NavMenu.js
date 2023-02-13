import React, { Component, useState } from 'react';
import { Collapse, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink, UncontrolledDropdown, DropdownToggle, DropdownMenu, DropdownItem } from 'reactstrap';
import LogoutModal from '../components/LogoutModal'
import { Modal } from 'antd'
import { Link } from 'react-router-dom';
import Home, { attemptLogout } from './Home'
import '../styles/NavMenu.css';

export class NavMenu extends Component {
  static displayName = NavMenu.name;

    constructor(props) {    
        super(props);

        this.toggleNavbar = this.toggleNavbar.bind(this);
        this.state = {
            showModal: false,
            collapsed: true,
            loggedIn: false,
        };  
    }

  //TODO: Update navbar when logged in
  toggleNavbar () {
    this.setState({
        collapsed: !this.state.collapsed,
        loggedIn: this.state.loggedIn,
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
            <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" container light>
                <NavbarBrand tag={Link} to="/">WeCasa</NavbarBrand>
                <NavbarToggler onClick={this.toggleNavbar} className="mr-2" />
                <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={!this.state.collapsed} navbar>
                    {
                        (!this.state.loggedIn) ?
                            (
                                <ul className="navbar-nav flex-grow">
                                    <NavItem>
                                        <NavLink tag={Link} className="text-dark" to="/">Register</NavLink>
                                    </NavItem>
                                    <NavItem>
                                        <NavLink tag={Link} className="text-dark" to="/login">Login</NavLink>
                                    </NavItem>
                                    <NavItem>
                                        <NavLink tag={Link} className="text-dark" to="/feedback">Feedback</NavLink>
                                    </NavItem>
                                </ul>
                            ) : (
                                <ul className="navbar-nav flex-grow">
                                    <NavItem>
                                        <NavLink tag={Link} className="text-dark" to="/home">Home</NavLink>
                                    </NavItem>
                                    <NavItem>
                                        <NavLink tag={Link} className="text-dark" to="/finanaces">Finances</NavLink>
                                    </NavItem>
                                    <UncontrolledDropdown>
                                        <DropdownToggle>Profile</DropdownToggle>
                                        <DropdownMenu>
                                            <DropdownItem href="/profile-settings">Settings</DropdownItem>
                                              <DropdownItem onClick={() => this.setShowModal(true)}>Logout</DropdownItem>
                                              <LogoutModal show={this.state.showModal} close={() => this.setShowModal(false)} confirm={() => Home.attemptLogout()} />
                                          </DropdownMenu>
                                    </UncontrolledDropdown>
                                </ul>
                            )
                    }
                </Collapse>
            </Navbar>
        </header>
    );
  }
};

export default NavMenu;