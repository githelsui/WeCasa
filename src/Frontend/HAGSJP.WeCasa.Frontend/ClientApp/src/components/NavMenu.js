import React, { Component } from 'react';
import { Collapse, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';
import '../styles/NavMenu.css';
import * as Styles from '../styles/ConstStyles.js';

export class NavMenu extends Component {
  static displayName = NavMenu.name;

  constructor (props) {
    super(props);

    this.toggleNavbar = this.toggleNavbar.bind(this);
    this.state = {
        collapsed: true,
        loggedIn: false
    };
  }

  //TODO: Update navbar when logged in
  toggleNavbar () {
    this.setState({
        collapsed: !this.state.collapsed,
        loggedIn: this.state.loggedIn
    });
  }

  render() {
    return (
        <header>
            <Navbar className={Styles.defaultNavbarStyle} container light>
                <NavbarBrand tag={Link} to="/">WeCasa</NavbarBrand>
                <NavbarToggler onClick={this.toggleNavbar} className="mr-2" />
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
                                <ul className="navbar-nav flex-grow">
                                    <NavItem>
                                        <NavLink tag={Link} className="text-dark" to="/home">Home</NavLink>
                                    </NavItem>
                                    <NavItem>
                                        <NavLink tag={Link} className="text-dark" to="/profile">Settings</NavLink>
                                    </NavItem>
                                    <NavItem>
                                        <NavLink tag={Link} className="text-dark" to="/logout">Logout</NavLink>
                                    </NavItem>
                                </ul>
                            )

                    }
                </Collapse>
            </Navbar>
      </header>
    );
  }
}
