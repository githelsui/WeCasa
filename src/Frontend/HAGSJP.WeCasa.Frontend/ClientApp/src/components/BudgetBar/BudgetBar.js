import React, { useState, useEffect, Component, EditButton, onClose } from 'react';
import axios from 'axios';
import { Table, Progress, Tabs, Button, Avatar} from 'antd';
import NavMenu from '../NavMenu';
import {MultiColorProgressBar} from './ProgressBar';
import BillForm from './BillForm';
import BudgetForm from './BudgetForm';
import ButtonIcon from './ButtonIcon';
import { EditOutlined } from '@ant-design/icons';
import * as Styles from '../../styles/ConstStyles';
// const { Schema } = require('mongoose');

const Bill = {
  owner: String,
  billName: String,
  groupid: Number,
  amount: Number,
  billId: Number,
  dateEntered: Date,
  billDescription: String,
  paymentStatus: String,
  isRepeated: Boolean,
  isDeleted: Boolean,
  dateDeleted: Date,
  photoFileName: String}

export const BudgetBar = (user) => {
  // const { auth, currentUser } = useAuth();
  const currentUser = 'frost@gmail.com'; //TEST DATA
  const groupId = 123456; //TEST DATA
  const [GET, setGet] = useState();
  const [showBillForm, setShowBillForm] = useState(false);
  const [billStatus, setBillStatus] = useState(false);
  const [budget, setBudget] = useState(0)
  const [groupTotal, setGroupTotal] = useState(0)
  // const [budgetBarUsers, setBudgetBarUsers] = useState([...{Username:"", firstName: "", totalSpent: 0, activeBills: [Bill], deletedBills: [Bill]}])
  // const [users, setUsers] = useState([{
  //   username : {
  //     firstName: "",
  //     totalSpent: 0,
  //     billIds: []
  //     }
  // }])
  const [users, setUsers] = useState(new Map())
  const [activeBills, setActiveBills] = useState(new Map())
  const [deletedBills, setDeletedBills] = useState(new Map())
  const [activeBillIds, setActiveBillIds] = useState(new Map())
  const [deletedBillIds, setDeletedBillIds] = useState(new Map())


  const [dataSource, setDataSource] = useState([])

  const columns = [
    {
      key: "1",
      title: '',
      render: () => {
        return (<EditOutlined onClick={()=>{setShowBillForm(!showBillForm);}}/>)}    
    },
    {
      key: "2",
      title: 'Date',
      dataIndex: 'date',
    },
    {
      key: "3",
      title: 'Name',
      dataIndex: 'billName',
    },
    {
      key: "4",
      title: 'Owner',
      dataIndex: 'owner',
    },
    {
      key: "5",
      title: 'Description',
      dataIndex: 'description',
    },
    {
      key: "6",
      title: 'Amount',
      dataIndex: 'amount',
    },
    {
      key: "7",
      title: 'Status',
      dataIndex: 'paymentStatus',
    },
    {
      key: "8",
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

  const tabs = [
    {
      key: 'current',
      label:  <b>Current</b>,
      children: <Table dataSource={dataSource} columns={columns} rowKey={record => record.billId}/>,
    },
    {
      key: 'history',
      label: <b>History</b>,
      children: <Table dataSource={dataSource} columns={columns} rowKey={(record) => record.billId}/>,
    }
  ];

  useEffect(() => {
     axios.get(`budgetbar/${groupId}`).then((response) => { 
        var res = response.data
        res["group"].forEach(function (item) {
          let newMap = users
          newMap.set(item.username, item)
          setUsers(newMap)
            item.activeBills.forEach(function (bill) {
              let newMap = activeBills
              let date = new Date(bill.dateEntered);
              let formattedDate = date.getMonth() + "/" + date.getDay() + "/" + date.getFullYear();
              newMap.set(bill.billId, {...bill, dateEntered: formattedDate})
              setActiveBills(newMap)
              bill.usernames.forEach(function (username) {
                  let prevIds = activeBillIds.get(username) ? activeBillIds[username] : []
                  prevIds.push(bill.billId)
                  setActiveBillIds(map => new Map(map.set(username, prevIds)))
              })
            })
            // console.log(activeBillIds)
            item.deletedBills.forEach(function (bill) {
              let date = new Date(bill.dateEntered);
              let formattedDate = date.getMonth() + "/" + date.getDay() + "/" + date.getFullYear();
              setDeletedBills(map => new Map(map.set(bill.billId, {...bill, dateEntered: formattedDate})))
            })
        })
        console.log("USer", users)
        console.log("USer", users["captain@gmail.com"])
      // console.log("ACtive", activeBills)
      setBudget(res["budget"])
      setGroupTotal(res["groupTotal"])
      setGet(true)
      console.log("SUCCESS")
  }).catch((error => { console.error(error) }));
  }, []);

  // const getBudgetBarData = ()  =>
  // {
  //    axios.get(`budgetbar/${groupId}`).then((response) => { 
  //      var res = response.data
  //     //  var activeB = []
  //     //  var deletedB = []
  //      res["group"].forEach(function (item) {
  //       var activeB = []
  //       var deletedB = []
  //       item.activeBills.forEach(function(bill) {
  //         activeB.push({
  //           usernames: bill.usernames,
  //           owner: bill.owner,
  //           billName: bill.billName,
  //           groupid: bill.groupId,
  //           amount: bill.amount,
  //           billId: bill.billId,
  //           dateEntered: bill.dateEntered,
  //           billDescription: bill.billDescription,
  //           paymentStatus: bill.paymentStatus,
  //           isRepeated: bill.isRepeated,
  //           isDeleted: bill.isDeleted,
  //           dateDeleted: bill.dateDeleted,
  //           photoFileName: bill.photoFileName
  //         })
  //       })
  //       item.deletedBills.forEach(function(bill) {
  //         deletedB.push({
  //           usernames: bill.usernames,
  //           owner: bill.owner,
  //           billName: bill.billName,
  //           groupid: bill.groupId,
  //           amount: bill.amount,
  //           billId: bill.billId,
  //           dateEntered: bill.dateEntered,
  //           billDescription: bill.billDescription,
  //           paymentStatus: bill.paymentStatus,
  //           isRepeated: bill.isRepeated,
  //           isDeleted: bill.isDeleted,
  //           dateDeleted: bill.dateDeleted,
  //           photoFileName: bill.photoFileName
  //         })
  //       })
  //       setBudgetBarUsers(prevState => [{
  //         username: item.username,
  //         firstName: item.firstName,
  //         totalSpent: item.totalSpent,
  //         activeBills: activeB,
  //         deletedBills: deletedB
  //       }, ...prevState])
  //     })
  //      setBudget(res["budget"])
  //      setGroupTotal(res["groupTotal"])
  //    })
  //    .catch((error => { console.error(error) }));
  //   //  console.log(budgetBarUsers[1].activeBills)
  // }

   const populateTables =  (billIds, bills) => {
    console.log('populate')
    let billList = []
    billIds.forEach(billId => {
      setDataSource(prevState => [...prevState, {
        billId: billId,
        date: bills.get(billId).dateEntered,
        billName: bills.get(billId).billName,
        owner: users.get(bills.get(billId).owner).firstName,
        description: bills.get(billId).description,
        amount: bills.get(billId).amount,
        paymentStatus: bills.get(billId).paymentStatus ? 'PAID' : 'UNPAID',
        receipt: bills.get(billId).receipt
      }])
    })
    console.log(billList)
    setDataSource(prevState => [...prevState, billList])
    setBillStatus(false)
   };

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

    const onChange = (key) => {
      switch(key) {
        case 'current':
          console.log(activeBillIds)
          if(activeBillIds.size > 0 && activeBills.size > 0) populateTables(activeBillIds, activeBills)
          break
        case 'history':
          if (billStatus) populateTables([12357], deletedBills)
          break
        default:
          break
      }
    };

    return (
      <div>
        <NavMenu/>
        {/* {(users) ? console.log(users.activeBills) : null} */}
        {/* {(!get) ? getBudgetBarData() : null} */}
        {/* {console.log(activeBills)} */}
        {/* {console.log(users.get("frost@gmail.com").activeBillIds)} */}
         {/* {(billStatus) ? populateTables([12354, 12355], activeBills): null} */}
         {/* {(billStatus) ? populateTables([12357], deletedBills): null} */}
        {/* {console.log(users["frost@gmail.com"].activeBillIds)} */}
         {/* <Button shape="round" size='large' onClick={handleClick}></Button> */}
        <ButtonIcon readings={readings} items={tabs}/>
        <BudgetForm budget={budget} setBudget={setBudget}/>
        <p><strong>Total Budget: ${budget}</strong></p>
        <Progress percent={50} strokeColor = {color[0]} showInfo={false} strokeWidth="30px"/>
        <MultiColorProgressBar readings={readings}/>
        <Button style={Styles.primaryButtonModal} onClick={()=>setShowBillForm(!showBillForm)}>Add Bill</Button>
        {showBillForm && (<BillForm/>)}
        {/* {console.log(budgetBarUsers[0].username)} */}

        {/* { {console.log(budgetBarUsers.useState())} */}
        {/* {budgetBarUsers[1].activeBills? console.log(budgetBarUsers[1].activeBills): null}  */}
        {/* {(budgetBarUsers.length === 0) ? populateTables(budgetBarUsers[0].activeBills) : null} */}
        <Tabs defaultActiveKey="1" items={tabs} onChange={onChange}/> 
    </div>
    );
}
