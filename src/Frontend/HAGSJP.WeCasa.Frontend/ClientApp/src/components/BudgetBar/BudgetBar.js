import React, { useAuth, useState, Component, EditButton } from 'react';
import axios from 'axios';
import {Bill} from './utils'
import { Table, Progress, Tabs, Button} from 'antd';
import NavMenu from '../NavMenu';
import {MultiColorProgressBar} from './ProgressBar';
import BillForm from './BillForm';
import BudgetForm from './BudgetForm';
import ButtonIcon from './ButtonIcon';
import * as Styles from '../../styles/ConstStyles';

export const BudgetBar = (user) => {
  const [budget, setBudget] = useState(0)
  // const { auth, currentUser } = useAuth();

        // this.state = {
         
        //     group: [],
        //     budget: 0,
        //     groupTotal: 0
        //   }
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
      const dataSource = [
        {
          key: '1',
          date: 0,//this.state.activeBills[0].dateEntered,
          name: "",//this.state.activeBills[0].billName,
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

      const handleClick = () => {
        dataSource[0].name = "RICKKKKK"
        
      }
      
      const columns = [
        {
          title: '',
          dataIndex: 'name',
          key: '',
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
         <NavMenu/>
         {/* <Button shape="round" size='large' onClick={handleClick}></Button> */}
        <ButtonIcon readings={readings} items={items}/>
        <BudgetForm budget={budget} setBudget={setBudget}/>
         <p><strong>Total Budget: {budget}</strong></p>
        <Progress percent={50} strokeColor = {color[0]} showInfo={true} strokeWidth="30px"/>
        <MultiColorProgressBar readings={readings}/>
        {/* <Button style={Styles.primaryButtonModal} onClick={() => { setopen(true);}}>Add Bill</Button> */}
        <BillForm/>
        <Tabs defaultActiveKey="1" items={items}  /> 
        {/* { <p>contents budget: {this.state.budget}</p>} */}
        {/* <p>contents group: {this.state.group.join(", ")}</p>
        <p>contents spent: {this.state.groupTotal}</p> */}
        {/* <p>contents totalSpentPerMember: {this.state.totalSpentPerMember[""jan""]}</p>
        <p>contents activeBills: {this.state.activeBills[0].amount}</p>
        <p>contents deletedBills: {this.state.deletedBills[0].amount}</p> 
        <p>contents total for {this.state.username}: {this.state.total}</p>  */}
        {/* <p>contents result: {String(this.state.result)}</p> */}
    </div>
    );
}
