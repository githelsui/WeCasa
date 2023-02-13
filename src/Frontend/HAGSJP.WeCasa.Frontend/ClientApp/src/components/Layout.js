import React, { Component } from 'react';
import { Container } from 'reactstrap';
import { Header } from './Header';
import { NavMenu } from './NavMenu';

export class Layout extends Component {
    static displayName = Layout.name;

    constructor(props) {
        super(props);

        this.state = {
            isAuthenticated: false,
        };
    }

  render() {
    return (
      <div>
        <Header />
            {(this.state.isAuthenticated == true) ?
                (<NavMenu />) : <div></div>}
        <Container tag="main">
          {this.props.children}
        </Container>
      </div>
    );
  }
}
