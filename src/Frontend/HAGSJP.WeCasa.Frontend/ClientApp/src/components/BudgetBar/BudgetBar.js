import React, { useState, Component } from 'react';
import axios from 'axios';
import {Bill} from './utils'
import { Table } from 'antd';
import NavMenu from '../NavMenu';

export class BudgetBar extends Component {
    static displayName = BudgetBar.name;

    constructor(props) {
        super(props);
        this.state = {
         
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
            //     paymentStatus: false,
            //     isRepeated: false,
            //     isDeleted: false,
            //     dateDeleted: null,
            //     photoFileName: null
            // }],
          }
        };

    componentDidMount() {
        let username = "Jan";
        // {
        //     "Username": "Jan",
        //     "BillId": "12345",
        //     "dateEntered": "2019-01-06T17:16:40",
        //     "BillName": "trash",
        //     "BillDescription": "description",
        //     "Amount": 1000,
        //     "PaymentStatus": true,
        //     "IsRepeated": true,
        //     "IsDeleted": false,
        //     "dateDeleted": null,
        //     "PhotoFileName": "dfafs"
        //    }
        // this.populateInitialView(username);
        this.fetchTable(username);
    }

    render() {
        const dataSource = [
            
            {
              key: '1',
              date: this.state.activeBills[0].dateEntered,
              name: this.state.activeBills[0].billName,
              age: 32,
              address: '10 Downing Street',
            },
            {
              key: '2',
              name: 'John',
              age: 42,
              address: '10 Downing Street',
            },
          ];
          
          const columns = [
            {
              title: '',
              dataIndex: 'name',
              key: "",
            },
            {
              title: 'Date',
              dataIndex: 'date',
              key: 'age',
            },
            {
              title: 'Name',
              dataIndex: 'billName',
              key: 'address',
            },
            {
                title: 'Owner',
                dataIndex: 'username',
                key: 'age',
            },
            {
              title: 'Description',
              dataIndex: 'description',
              key: 'address',
            },
            {
              title: 'Amount',
              dataIndex: 'amount',
              key: 'age',
            },
            {
              title: 'Status',
              dataIndex: 'paymentStatus',
              key: 'address',
            },
            {
              title: 'Receipt',
              dataIndex: 'receipt',
              key: 'address',
            }
          ];
        return (
        <div>
            <NavMenu/>
            <Table dataSource={dataSource} columns={columns} />;
            { <p>contents budget: {this.state.budget}</p>}
            <p>contents group: {this.state.group.join(", ")}</p>
            <p>contents spent: {this.state.groupTotal}</p>
            <p>contents totalSpentPerMember: {this.state.totalSpentPerMember["jan"]}</p>
            <p>contents activeBills: {this.state.activeBills[0].amount}</p>
            <p>contents deletedBills: {this.state.deletedBills[0].amount}</p> 
            <p>contents total for {this.state.username}: {this.state.total}</p> 
            {/* <p>contents result: {String(this.state.result)}</p> */}
        </div>
        );
    }

    fetchBudgetBar(username) 
    {
        axios.get(`finances/${username}`).then((response) => {
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
        axios.get(`finances/${username}`).then((response) => {
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
        axios.put(`finance/EditBill`).then(res => {
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
