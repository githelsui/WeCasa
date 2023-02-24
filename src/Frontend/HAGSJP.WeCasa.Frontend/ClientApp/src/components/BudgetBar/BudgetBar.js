import React, { useState, useEffect, Component, EditButton, onClose } from 'react';
import axios from 'axios';
import {Bill} from './utils'
import { Table, Progress, Tabs, Button} from 'antd';
import NavMenu from '../NavMenu';
import {MultiColorProgressBar} from './ProgressBar';
import BillForm from './BillForm';
import BudgetForm from './BudgetForm';
import ButtonIcon from './ButtonIcon';
import { useParams } from 'react-router-dom';
import { EditOutlined } from '@ant-design/icons';
import * as Styles from '../../styles/ConstStyles';

export const BudgetBar = (user) => {
  // const { auth, currentUser } = useAuth();
  const currentUser = 'Jan'; //TEST DATA
  const groupId = 123456; //TEST DATA
  const [showBillForm, setShowBillForm] = useState(false);
  const [budget, setBudget] = useState(0)
  // const [budgetBarUsers, setBudgetBarUsers] = useState([...{Username:"", firstName: "", totalSpent: 0, activeBills: [Bill], deletedBills: [Bill]}])
    const [budgetBarUsers, setBudgetBarUsers] = useState([])
  const [groupTotal, setGroupTotal] = useState(0)
  const [dataSource, setDataSource] = useState( [
    {
      key: '1',
      date: 2,//this.state.activeBills[0].dateEntered,
      billName: "dsfaddsaf",//this.state.activeBills[0].billName,
      owner: 32,
      description: '10 Downing Street',
      amount: 0,
      paymentStatus: "paid",
    }
  ])

  const columns = [
    {
      title: '',
      render: () => {
        return (<EditOutlined onClick={()=>{setShowBillForm(!showBillForm);}}/>)}    
    },
    {
      title: 'Date',
      dataIndex: 'date',
    },
    {
      title: 'Name',
      dataIndex: 'billName',
    },
    {
        title: 'Owner',
        dataIndex: 'owner',
    },
    {
      title: 'Description',
      dataIndex: 'description',
    },
    {
      title: 'Amount',
      dataIndex: 'amount',
    },
    {
      title: 'Status',
      dataIndex: 'paymentStatus',
    },
    {
      title: 'Receipt',
      dataIndex: 'receipt',
    }
  ];

  const color = [
    '#7e7e7e', '#a88e7a', '#e5e3d7', '#cbc3ba', '#a88e7a', '#a6a998', '#d9c2b0'
  ]
    
  let readings = [
  {
    name: 'Apples',
    value: 60,
    color: color[0]
  },
  {
    name: 'Blueberries',
    value: 7,
    color: color[1]
  },
  {
    name: 'Guavas',
    value: 23,
    color: color[2]
  },
  {
    name: 'Grapes',
    value: 10,
    color: color[3]
  }
  ]

  const items = [
    {
      key: '1',
      label:  <b>Current</b>,
      children: <Table dataSource={dataSource} columns={columns} />,
    },
    {
      key: '2',
      label: <b>History</b>,
      children: <Table dataSource={dataSource} columns={columns} />,
    }
  ];

  const getBudgetBarData = ()  =>
  {
     axios.get(`budgetbar/${groupId}`).then((response) => { 
       var res = response.data
       var activeB = []
       var deletedB = []
       res["group"].forEach(function (item) {
        item.activeBills.forEach(function(bill) {
          activeB.push({
            usernames: bill.usernames,
            owner: bill.owner,
            billName: bill.billName,
            groupid: bill.groupId,
            amount: bill.amount,
            billId: bill.billId,
            dateEntered: bill.dateEntered,
            billDescription: bill.billDescription,
            paymentStatus: bill.paymentStatus,
            isRepeated: bill.isRepeated,
            isDeleted: bill.isDeleted,
            dateDeleted: bill.dateDeleted,
            photoFileName: bill.photoFileName
          })
        })
        item.deletedBills.forEach(function(bill) {
          deletedB.push({
            usernames: bill.usernames,
            owner: bill.owner,
            billName: bill.billName,
            groupid: bill.groupId,
            amount: bill.amount,
            billId: bill.billId,
            dateEntered: bill.dateEntered,
            billDescription: bill.billDescription,
            paymentStatus: bill.paymentStatus,
            isRepeated: bill.isRepeated,
            isDeleted: bill.isDeleted,
            dateDeleted: bill.dateDeleted,
            photoFileName: bill.photoFileName
          })
        })
        setBudgetBarUsers(prevState => [{
            username: item.username,
            firstName: item.firstName,
            totalSpent: item.totalSpent,
            activeBills: activeB,
            deletedBills: deletedB
          }, ...prevState])

          activeB = []
          deletedB = []
      })
       setBudget(res["budget"])
       setGroupTotal(res["groupTotal"])
     })
     .catch((error => { console.error(error) }));
  }

   const populateTables = (bills) => {
    for (let i = 0; i < this.state.maxData; i++) {
      let bill = {
        key: i,
        date: bills[i].dateEntered,
        billName: bills[i].billName,
        owner: bills[i].owner,
        description: bills[i].description,
        amount: bills[i].amount,
        paymentStatus: bills[i].paymentStatus
      }
      setDataSource(dataSource.push(bill))
    }
   }

  //  const popTab = (bills) => {
  //   bills.map(bill => {
  //     let b = { key: 1,
  //       date : bills.activeBills[0].dateEntered,
  //       billName : bills.activeBills[0].billName,
  //       owner : bills.activeBills[0].owner,
  //       description : bills.activeBills[0].description,
  //       amount : bills.activeBills[0].amount,
  //       paymentStatus : bills.activeBills[0].paymentStatus}
      
  //   })
  //  }

      let request =  {
          Usernames : ["sam", "man"],
          Bill : {
              GroupId: 0,
              Username: "Jan",
              BillName: "Trip",
              BillDescription: "some-description",
              Amount: 100,
              PaymentStatus: true,
              IsRepeated: true,
              PhotoFileName: "dfafs"
          }
   }

   const persistEditForm = (request) =>
    {
      axios.put(`finance/EditBill`).then(res => {
          var isSuccessful = res.data;
          if (isSuccessful) {

          } else {
          }
      })
      .catch((error) => { console.error(error) });
    }

    return (
      <div>
        {/* {getBudgetBarData()} */}
        {(budgetBarUsers.length === 0) ? getBudgetBarData() : null}
         <NavMenu/>
         {/* <Button shape="round" size='large' onClick={handleClick}></Button> */}
        <ButtonIcon readings={readings} items={items}/>
        <BudgetForm budget={budget} setBudget={setBudget}/>
        <p><strong>Total Budget: ${budget}</strong></p>
        <Progress percent={50} strokeColor = {color[0]} showInfo={false} strokeWidth="30px"/>
        <MultiColorProgressBar readings={readings}/>
        <Button style={Styles.primaryButtonModal} onClick={()=>setShowBillForm(!showBillForm)}>Add Bill</Button>
        {showBillForm && (<BillForm/>)}
        {console.log(budgetBarUsers[1].activeBills)}
        {/* {populateTables(budgetBarUsers[0].activeBills)} */}
        <Tabs defaultActiveKey="1" items={items}/> 
    </div>
    );
}
