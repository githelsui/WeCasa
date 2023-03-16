import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { Table, Progress, Tabs, Button, Space } from 'antd';
import NavMenu from '../NavMenu';
import {MultiColorProgressBar} from './ProgressBar';
import BillForm from './BillForm';
import BudgetForm from './BudgetForm';
import DeletionModal from '../DeletionModal';
import { useAuth } from '../AuthContext';
import { DeleteOutlined, EditOutlined } from '@ant-design/icons';
import * as Styles from '../../styles/ConstStyles';
import { EditBillForm } from './EditBillForm';

export const BudgetBar = () => {
  const { auth, currentUser, currentGroup } = useAuth()
  let user = currentUser != null? currentUser.username : null
  let authorized = auth
  let group = currentGroup

  // TODO: temporary fix for refresh issue
  if (authorized===null) authorized = true
  if (user===null) user = 'joy@gmail.com'
  if (group===null) group = {groupId: '1235467'}

  const [selectedUser, setSelectedUser] = useState(user);
  const [showAddForm, setShowAddForm] = useState(false);
  const [showEditForm, setShowEditForm] = useState(false);
  const [deleteBill, setDeleteBill] = useState(null);
  const [editBill, setEditBill] = useState(null);
  
  const [budget, setBudget] = useState(0)
  const [groupTotal, setGroupTotal] = useState(0)
  const [users, setUsers] = useState([])
  const [activeBills, setActiveBills] = useState([])
  const [deletedBills, setDeletedBills] = useState([])

  const errorPage = () => {
    return (
      <div name="ErrorPage">
        <p><strong>Oops, something went wrong!</strong></p>
        <p>Please try again!</p>
        <p>For further assistance, please submit a User Feedback ticket.</p>
      </div>
    )
  }

  useEffect(() => {
    axios.get(`budgetbar/${group.groupId}`).then((response) => { 
       var res = response.data
       setBudget(res["budget"])
       setGroupTotal(res["groupTotal"])
       let newUserList = [];
       let colorCounter = 0;
       res["group"].forEach(function (item) {
         const newObj =   {
           username: item.username,
           name: item.firstName,
           value: Math.floor(res["userTotals"][item.username]/res["groupTotal"] * 100),
           color: color[colorCounter++]
         }
         newUserList.push(newObj)
       })
       setUsers(newUserList)
       let activeBillList = []
       res["activeBills"].forEach(function (bill) {
         let date = new Date(bill.dateEntered);
         let formattedDate = date.getMonth() + "/" + date.getDay() + "/" + date.getFullYear();
         let newObj = {
           amount: bill.amount,
           description: bill.billDescription,
           billID: bill.billId,
           billName: bill.billName,
           date: formattedDate,
           groupID: bill.groupId,
           IsRepeated: bill.isRepeated,
           owner: bill.owner,
           paymentStatus: bill.paymentStatus ? "PAID" : "UNPAID",
           usernames: bill.usernames,
           PhotoFileName: bill.photoFileName
         }
         activeBillList.push(newObj)
       })
       setActiveBills(activeBillList)
       let deletedBillList = []
       res["deletedBills"].forEach(function (bill) {
         let date = new Date(bill.dateEntered);
         let formattedDate = date.getMonth() + "/" + date.getDay() + "/" + date.getFullYear();
         let newObj = {
           amount: bill.amount,
           description: bill.billDescription,
           billID: bill.billId,
           billName: bill.billName,
           date: formattedDate,
           groupID: bill.groupId,
           IsRepeated: bill.isRepeated,
           owner: bill.owner,
           paymentStatus: bill.paymentStatus ? "PAID" : "UNPAID",
           usernames: bill.usernames,
           PhotoFileName: bill.photoFileName

         }
         deletedBillList.push(newObj)
       })
       setDeletedBills(deletedBillList)
     console.log("GET SUCCESS")
 }).catch( (error) => {
   console.log(error)
   errorPage()});
 }, []);
  
  const handleRestoreButton = (bill) => {
    const billId = bill.billID
    if (billId !== null) {
      axios.put(`budgetbar/Restore/${billId}`).then((response) => { 
        let res = response
        if (res) {
          console.log('Restore Successful')
        } else {
          console.log('Restore Failed')
        }
      }).catch((error => { console.error(error) }));
    }
  }

  const historyColumns = [
    {
      key: "1",
      title: '',
      render: (bill) => {
        if (bill.paymentStatus === 'UNPAID')
        {
          return ( <Button onClick={()=>{handleRestoreButton(bill)}}>Restore</Button>)
        }    
      }
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
   '#7e7e7e', '#c4bdac', '#e5e3d7', '#cbc3ba','#a88e7a', '#cbdae1', '#a88e7a', '#ebcfc4', '#a6a998', '#d9c2b0'
  ]

  const handleDelete = (billId) => {
    axios.delete(`budgetbar/Delete/${billId}`).then((response) => { 
      let res = response
      if (res) {
        console.log('Delete Successful')
      } else {
        console.log('Delete Failed')
      }
    }).catch(() => { alert('Delete Failed') });
    setDeleteBill(false)
  }

  const columns = [
    {
      key: "1",
      title: '',
      render: (bill) => {
        return(
        <Space size="middle">
          {(currentUser===bill.owner || user===bill.owner) && <EditOutlined onClick={()=>{
            setShowEditForm(true)
            setEditBill(bill)
            }}/> }
           {(user===bill.owner) &&  <DeleteOutlined onClick={()=>setDeleteBill(bill)}/> }
          {deleteBill && <DeletionModal message='Are you sure you want to delete this bill?' show={deleteBill} close={()=>setDeleteBill(false)} confirm={()=>handleDelete(deleteBill.billID)} />}
          {showEditForm && <EditBillForm show={showEditForm} bill={editBill} members={users} setActiveBills={setActiveBills} close={()=>setShowEditForm(false)} setOpen={setShowEditForm}/> }
        </Space>
        )}    
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

  const handleCurrentTable = () => {
    const filteredBills = activeBills.filter(bill => bill.usernames.includes(selectedUser));
    return <Table dataSource={filteredBills} columns={columns} rowKey={record => record.billID}/>
  }

  const handleHistoryTable = () => {
    const filteredBills = deletedBills.filter(bill => bill.usernames.includes(selectedUser));
    return <Table dataSource={filteredBills} columns={historyColumns} rowKey={record => record.billID}/>
  }

  const tabs = [
    {
      key: 'current',
      label:  <b>Current</b>,
      children: handleCurrentTable(),
    },
    {
      key: 'history',
      label: <b>History</b>,
      children: handleHistoryTable()
    }
  ];

    const displayButtonIcons = () => {
      return users.map(user => (
          <div style={{ display: 'inline-flex', margin: '10px' }}  key={user.username}>
            <Button shape="round" style={{ background: user.color, width: `50px`, height: `50px` }} size='large' onClick={() => setSelectedUser(user.username)}></Button>
            <span>{user.name}</span>
          </div>
      ));
    }

    return (
      (authorized) ?
        <div>
          <div>{(auth == null) ? <NavMenu/> : null}</div>
          {displayButtonIcons() }
          <BudgetForm budget={budget} setBudget={setBudget} group={group} style="margin-top: 20px"/>
          <p><strong>Total Budget: ${budget}</strong></p>
          <Progress percent={(groupTotal/budget)*100} strokeColor = {color[0]} showInfo={false} strokeWidth="30px"/>
          <MultiColorProgressBar  readings={users} />
          <Button style={Styles.addFormButton} onClick={()=>setShowAddForm(!showAddForm)}>Add Bill</Button>
          {showAddForm && (<BillForm budget={budget} groupTotal={groupTotal} user={user} group={group} members={users}/>)}
          <Tabs defaultActiveKey="1" items={tabs} /> 
        </div>
      : errorPage()
    );
  }
