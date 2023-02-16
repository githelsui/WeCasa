import React, { useState, Component } from 'react';
import axios from 'axios';
import {Bill} from './utils'


export class BudgetBar extends Component {
    static displayName = BudgetBar.name;

    constructor(props) {
        super(props);
        this.state = {
            username: "",
            group: [],
            budget: 0,
            groupTotal: 0,
            totalSpentPerMember: [{
              username: "",
              total: 0
            }],
            total: 0,
            activeBills: [Bill],
            deletedBills: [Bill]
            // activeBills: [{
            //     username: "",
            //     billId: "",
            //     dateEntered: null,
            //     billName: "",
            //     billDescription: "",
            //     amount: 0,
            //     percentageOwed: 0,
            //     paymentStatus: false,
            //     isRepeated: false,
            //     isDeleted: false,
            //     dateDeleted: null,
            //     photoFileName: null
            // }],
            // deletedBills: [{
            //     username: "",
            //     billId: "",
            //     dateEntered: null,
            //     billName: "",
            //     billDescription: "",
            //     amount: 0,
            //     percentageOwed: 0,
            //     paymentStatus: false,
            //     isRepeated: false,
            //     isDeleted: false,
            //     dateDeleted: null,
            //     photoFileName: null
            // }],
        };
    }

    componentDidMount() {
        let username = "Jan";
        // {
        //     "Username": "Jan",
        //     "BillId": "12345",
        //     "dateEntered": "2019-01-06T17:16:40",
        //     "BillName": "trash",
        //     "BillDescription": "description",
        //     "Amount": 1000,
        //     "PercentageOwed": 50,
        //     "PaymentStatus": true,
        //     "IsRepeated": true,
        //     "IsDeleted": false,
        //     "dateDeleted": null,
        //     "PhotoFileName": "dfafs"
        //    }
        // this.populateInitialView(username);
        // this.fetchTable(username) 
    }

    render() {
        return (
        <div>
            <h1>Hello, welcome to WeCasa</h1>
            { <p>contents budget: {this.state.budget}</p>}
            <p>contents group: {this.state.group.join(", ")}</p>
            <p>contents spent: {this.state.groupTotal}</p>
            <p>contents totalSpentPerMember: {this.state.totalSpentPerMember["jan"]}</p>
            <p>contents activeBills: {this.state.activeBills[0].amount}</p>
            {/* <p>contents deletedBills: {this.state.deletedBills}</p>  */}
            <p>contents total for {this.state.username}: {this.state.total}</p> 
            {/* <p>contents result: {String(this.state.result)}</p> */}
        </div>
        );
    }

    fetchBudgetBar(username) 
    {
        axios.get(`budgetbar/${username}`).then((response) => {
            var res = response.data
            this.setState({
                            group: res["group"],
                            budget: res["budget"],
                            groupTotal: res["groupTotal"],
                            totalSpentPerMember: res["totalSpentPerMember"]
            });
        });
    }

    fetchTable(username) 
    {
        axios.get(`budgetbar/${username}`).then((response) => {
            var res = response.data
            this.setState({
                            total: res["total"],
                            activeBills: res["activeBills"],
                            deletedBills: res["deletedBills"]
            });
        });
    }


    persistEditForm(bill) 
    {
        axios.put(`budgetbar/EditBill`).then(res => {
            var isSuccessful = res.data;
            if (isSuccessful) {

            } else {
            }
        })
        .catch((error) => { console.error(error) });
    }
}

// const submitLoginForm = (values) => {
//     setInputFailures(false);
//     var failureMessage = '';

//     let billForm = {
//         Name: values.name,
//         Description: values.description,
//         Amount: values.amount,
//         RepeatMonthly: values.repeatMonthly,
//         IsPaid: values.isPaid,
//         Payees: values.Payees,
//         fileName: values.fileName
//     };

//     // -- Client-side Input Validation

//     // Blank User Inputs
//     for (let key in billForm) {
//         if (userAccount[key] == null) {
//             setInputFailures(true);
//             failureMessage = 'Empty fields are not accepted.'
//             break;
//         }
//     }

//     if (!inputFailures) {
//         attemptInitialLogin(userAccount)
//     } else {
//         failureLoginView(failureMessage);
//     }
// };
