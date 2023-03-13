import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { Table, Progress, Tabs, Button, Space } from 'antd';
import NavMenu from '../NavMenu';
import {MultiColorProgressBar} from './ProgressBar';
import BillForm from './BillForm';
import BudgetForm from './BudgetForm';
import DeletionModal from '../DeletionModal';
import { DeleteOutlined, EditOutlined } from '@ant-design/icons';
import * as Styles from '../../styles/ConstStyles';
import { EditBillForm } from './EditBillForm';

export const BudgetBar = (user) => {
  // const { auth, currentUser } = useAuth();
  const currentUser = 'frost@gmail.com'; //TEST DATA
  const groupId = 123456; //TEST DATA
  const [selectedUser, setSelectedUser] = useState(currentUser);
  const [showAddForm, setShowAddForm] = useState(false);
  const [showEditForm, setShowEditForm] = useState(false);
  const [deleteBill, setDeleteBill] = useState(null);
  const [editBill, setEditBill] = useState(null);
  
  const [budget, setBudget] = useState(0)
  const [groupTotal, setGroupTotal] = useState(0)
  const [users, setUsers] = useState([])
  const [activeBills, setActiveBills] = useState([])
  const [deletedBills, setDeletedBills] = useState([])

  useEffect(() => {
    axios.get(`budgetbar/${groupId}`).then((response) => { 
       var res = response.data
       setBudget(res["budget"])
       setGroupTotal(res["groupTotal"])
       console.log("GROUPTOTAL", res["groupTotal"])
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
       console.log("NEW OBJECT WITH COLOR", users)
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
       console.log(activeBillList)
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
     console.log("SUCCESS")
 }).catch( (error) => {
   console.log(error)
   alert("Finances failed to load.")});
 }, []);
  
  const handleRestoreButton = (billId) => {
    console.log("RESROTE", billId)
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
        return (<Button onClick={()=>{handleRestoreButton(bill.billID)}}>Restore</Button>)}    
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
          {(currentUser===bill.owner) && <EditOutlined onClick={()=>{
            setShowEditForm(true)
            setEditBill(bill)
            }}/> }
           {(currentUser===bill.owner) &&  <DeleteOutlined onClick={()=>setDeleteBill(bill)}/> }
          {deleteBill && <DeletionModal message='Are you sure you want to delete this bill?' show={deleteBill} close={()=>setDeleteBill(false)} confirm={()=>handleDelete(deleteBill.billID)} />}
          {showEditForm && <EditBillForm show={showEditForm} bill={editBill} members={users} close={()=>setShowEditForm(false)} setOpen={setShowEditForm}/> }
            {console.log("BILL OWNRE", bill.owner)}
            {console.log("currentUser", currentUser)}
            {console.log("currentUser", currentUser===bill.owner)}
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
        <div>
        <NavMenu/>
        {displayButtonIcons() }
        <BudgetForm budget={budget} setBudget={setBudget} style="margin-top: 20px"/>
        <p><strong>Total Budget: ${budget}</strong></p>
        <Progress percent={(groupTotal/budget)*100} strokeColor = {color[0]} showInfo={false} strokeWidth="30px"/>
        <MultiColorProgressBar  readings={users} />
        <Button style={Styles.addFormButton} onClick={()=>setShowAddForm(!showAddForm)}>Add Bill</Button>
        {showAddForm && (<BillForm budget={budget} groupTotal={groupTotal} members={users}/>)}
        {/* {showEditForm && (<EditBillForm/>)} */}
        <Tabs defaultActiveKey="1" items={tabs} /> 
    </div>
    );
  }
