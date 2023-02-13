import React, { Component } from 'react';
import { Modal, notification } from 'antd';
import axios from 'axios';

export class Home extends Component {
    static displayName = Home.name;

    constructor(props) {
        super(props);

        this.state = {
            logoutResults: false,
            account: '',
        };
    }


    static attemptLogout() {
        console.log("Attempting logout...");
        axios.post('logout/AttemptLogout', this.props.account)
            .then(res => {
                console.log(res.data)
                var isSuccessful = res.data['isSuccessful'];
                if (isSuccessful) {
                    this.state.logoutResults = true;
                    // return to main page
                    this.props.history.push('/');
                } else {
                    this.failureLogoutView(res.data['message']);
                }
            }) 
            .catch((error) => { console.error(error) });
    };

    failureLogoutView = (failureMessage) => {
        // Accounts for user failure cases and system errors
        this.state.logoutResults = false;
        notification.open({
            message: "Try again.",
            description: failureMessage,
            duration: 10,
            placement: "topRight",
        });
    }

  render() {
    return (
      <div>
        <h1>Hello, welcome to WeCasa</h1>
            <p>Username</p>
      </div>
    );
  }
} 

export default Home;