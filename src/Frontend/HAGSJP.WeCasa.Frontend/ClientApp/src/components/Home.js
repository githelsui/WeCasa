import React, { Component } from 'react';
import { Navigate } from 'react-router-dom'
import { Modal, notification } from 'antd';
import axios from 'axios';

export class Home extends Component {
    static displayName = Home.name;

    constructor(props) {
        super(props);

        this.state = {
            logoutResults: false,
            currentUser: '',
            loggedIn: false
        };
    }

    componentDidMount() {
        console.log(this.props);
        this.setState({
            ...this.state,
            currentUser: this.props.account,
            loggedIn: (this.props.account == null) ? false : true
        })
    }


    static attemptLogout() {
        console.log("Attempting logout...");
        axios.post('logout/AttemptLogout', this.state.currentUser)
            .then(res => {
                console.log(res.data)
                var isSuccessful = res.data['isSuccessful'];
                if (isSuccessful) {
                    this.state.logoutResults = true;
                    // return to main page
                    this.state.currentUser = '';
                    this.state.loggedIn = false;
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

    setCurrentUser = (userAccount) => {
        this.setState({
            currentUser: userAccount,
            loggedIn: true
        })
    }

  render() {
    return (
        <div>
            (<div>
                {(!this.state.loggedIn) ? 
                    (<Navigate to='/login'></Navigate>)
                    : (<div><h1>Hello, welcome to WeCasa</h1>
                        <p>{this.state.currentUser.Username}</p></div>)}
            </div>)
        </div>
    );
  }
} 

export default Home;