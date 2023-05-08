﻿import React, { Component, ReactDOM } from 'react';
import { Button, Form } from 'antd';
import * as Styles from '../styles/ConstStyles.js';
// Reference: https://medium.com/@ahmedaffan311/otp-input-in-react-js-3b36ed67e360

class OTPInput extends Component {

    constructor(props) {
        super(props);
        this.state = {
            value: '',
            otp1: "",
            otp2: "",
            otp3: "",
            otp4: "",
            otp5: "",
            otp6: "",
            otp7: "",
            otp8: "",
            otp9: "",
            otp10: "",
            disable: true,
            validChars: "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789.-@"
        };
        this.handleChange = this.handleChange.bind(this);
    }

    handleChange(value1, event) {
        this.setState({ [value1]: event.target.value });
    }

    handleSubmit() {
        this.state.value = this.state.otp1 + this.state.otp2 + this.state.otp3 + this.state.otp4 + this.state.otp5 + this.state.otp6 + this.state.otp7 + this.state.otp8 + this.state.otp9 + this.state.otp10;
        this.props.submit(this.state.value);
    }

    inputfocus = (elmnt) => {
        if (elmnt.key === "Delete" || elmnt.key === "Backspace" || elmnt.key === "ArrowLeft") {
            const next = elmnt.target.tabIndex - 2;
            if (next > -1) {

                elmnt.target.form.elements[next].focus()
            }
        }
        else if (elmnt.key === "ArrowRight" || this.state.validChars.includes(elmnt.key)) {
                console.log("next");

            const next = elmnt.target.tabIndex;
            if (next < 10) {
                elmnt.target.form.elements[next].focus()
            }
        }

    }

    render() {
        return (
            <Form id="otpForm" onFinish={() => this.handleSubmit() }>
                <div className="otpContainer" style={{marginBottom:"10px"}}>
                    <input
                        name="otp1"
                        type="text"
                        autoComplete="off"
                        className="otpInput"
                        value={this.state.otp1}
                        style={Styles.otpInput}
                        onKeyPress={this.keyPressed}
                        onChange={e => this.handleChange("otp1", e)}
                        tabIndex="1" maxLength="1" onKeyUp={e => this.inputfocus(e)}

                    />
                    <input
                        name="otp2"
                        type="text"
                        autoComplete="off"
                        className="otpInput"
                        value={this.state.otp2}
                        style={Styles.otpInput}
                        onChange={e => this.handleChange("otp2", e)}
                        tabIndex="2" maxLength="1" onKeyUp={e => this.inputfocus(e)}

                    />
                    <input
                        name="otp3"
                        type="text"
                        autoComplete="off"
                        className="otpInput"
                        value={this.state.otp3}
                        style={Styles.otpInput}
                        onChange={e => this.handleChange("otp3", e)}
                        tabIndex="3" maxLength="1" onKeyUp={e => this.inputfocus(e)}

                    />
                    <input
                        name="otp4"
                        type="text"
                        autoComplete="off"
                        className="otpInput"
                        value={this.state.otp4}
                        style={Styles.otpInput}
                        onChange={e => this.handleChange("otp4", e)}
                        tabIndex="4" maxLength="1" onKeyUp={e => this.inputfocus(e)}
                    />
                    <input
                        name="otp5"
                        type="text"
                        autoComplete="off"
                        className="otpInput"
                        value={this.state.otp5}
                        style={Styles.otpInput}
                        onChange={e => this.handleChange("otp5", e)}
                        tabIndex="5" maxLength="1" onKeyUp={e => this.inputfocus(e)}
                    />
                    <input
                        name="otp6"
                        type="text"
                        autoComplete="off"
                        className="otpInput"
                        value={this.state.otp6}
                        style={Styles.otpInput}
                        onChange={e => this.handleChange("otp6", e)}
                        tabIndex="6" maxLength="1" onKeyUp={e => this.inputfocus(e)}
                    />
                    <input
                        name="otp7"
                        type="text"
                        autoComplete="off"
                        className="otpInput"
                        value={this.state.otp7}
                        style={Styles.otpInput}
                        onChange={e => this.handleChange("otp7", e)}
                        tabIndex="7" maxLength="1" onKeyUp={e => this.inputfocus(e)}
                    />
                    <input
                        name="otp8"
                        type="text"
                        autoComplete="off"
                        className="otpInput"
                        value={this.state.otp8}
                        style={Styles.otpInput}
                        onChange={e => this.handleChange("otp8", e)}
                        tabIndex="8" maxLength="1" onKeyUp={e => this.inputfocus(e)}
                    />
                    <input
                        name="otp9"
                        type="text"
                        autoComplete="off"
                        className="otpInput"
                        value={this.state.otp9}
                        style={Styles.otpInput}
                        onChange={e => this.handleChange("otp9", e)}
                        tabIndex="9" maxLength="1" onKeyUp={e => this.inputfocus(e)}
                    />
                    <input
                        name="otp10"
                        type="text"
                        autoComplete="off"
                        className="otpInput"
                        value={this.state.otp10}
                        style={Styles.otpInput}
                        onChange={e => this.handleChange("otp10", e)}
                        tabIndex="10" maxLength="1" onKeyUp={e => this.inputfocus(e)}
                    />
                </div>
                <Button key="create" htmlType="submit" type="primary" style={Styles.primaryButtonModal}>Submit</Button>
                <Button key="cancel" onClick={this.props.cancel} type="default" style={Styles.defaultButtonModal}>Resend code</Button>
            </Form>
        );
    }
}


export default OTPInput;