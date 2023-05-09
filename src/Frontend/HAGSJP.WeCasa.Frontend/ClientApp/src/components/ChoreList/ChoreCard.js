import React, { Component, useState, useEffect } from 'react';
import { Card, Avatar, Button, notification} from 'antd';
import * as Styles from '../../styles/ConstStyles.js';
import '../../styles/System.css';
import '../../index.css';
import axios from 'axios';
import defaultImage from '../../assets/defaultimgs/wecasatemp.jpg';
import * as ValidationFuncs from '../../scripts/InputValidation.js';
import { SendOutlined, EllipsisOutlined, SettingOutlined, CheckSquareOutlined } from '@ant-design/icons';
import Nudge from '../Nudge/Nudge';
import image1 from '../../assets/profileimgs/1.jpg';
import image2 from '../../assets/profileimgs/2.jpg';
import image3 from '../../assets/profileimgs/3.jpg';
import image4 from '../../assets/profileimgs/4.jpg';
import image5 from '../../assets/profileimgs/5.jpg';
import image6 from '../../assets/profileimgs/6.jpg';
import ChoreEditModal from './ChoreEditModal'
const { Meta } = Card;


export const ChoreCard = (props) => {
    const images = [image1, image2, image3, image4, image5, image6];
    const [showEditModal, setShowEditModal] = useState(false);

    const getAssignedUsernames = (assignments) => {
        var arr = []
        for (let i = 0; i < assignments.length; i++) {
            var userProfile = assignments[i]
            var username = userProfile['username']
            arr.push(username)
        }
        return arr;
    }

    const assignmentProfileIcons = (assignments) => {
        return (<div>
              {assignments.map((user, i) =>
                <Avatar style={{ borderColor: 'black', marginLeft: -15, marginTop: -15}} className='padding' src={images[user['image']]} />
            )}
        </div>);
    }

    const assignmentLabel = (assignments) => {
        var label = ''
        for (let i = 0; i < assignments.length; i++) {
            var userProfile = assignments[i]
            var firstName = userProfile['firstName']
            var assigned = (firstName == '' || firstName == undefined) ? userProfile['username'] : firstName;
            if (i == assignments.length - 1) {
                label += assigned
            } else {
                label += assigned + ', '
            }
        }
        return label
    }

    const completeChore = (chore) => {
        console.log(chore)
        let choreForm = {
            CurrentUser: props.user['username'],
            GroupId: chore['groupId'], 
            Name: chore['name'],
            Notes: chore['notes'],
            Repeats: chore['repeats'],
            Days: chore['days'],
            AssignedTo: getAssignedUsernames(chore['assignedTo']),
            ChoreId: chore['choreId'],
            ChoreDate: chore['choreDate']
        }

        axios.post('chorelist/CompleteChore', choreForm)
            .then(res => {
                var isSuccessful = res.data['isSuccessful'];
                if (isSuccessful) {
                    toast('Chore successfully completed. Chore now in History Tab. ')
                    props.setUpdate(true)
                } else {
                    toast(res.data['message'])
                }
            })
            .catch((error => {
                toast('Error performing operation: Complete Chore')
            }));
    };

    const updateChore = (choreForm) => {
        console.log(choreForm)

        axios.post('chorelist/EditChore', choreForm)
            .then(res => {
                var isSuccessful = res.data['isSuccessful'];
                if (isSuccessful) {
                    toast('Chore successfully updated.')
                    props.setUpdate(true)
                } else {
                    toast(res.data['message'])
                }
            })
            .catch((error => {
                toast('Error performing operation: Edit Chore.')
            }));
    };

    const deleteChore = () => {
        let choreForm = {
            CurrentUser: props.user['username'],
            GroupId: props.chore['groupId'],
            Name: props.chore['name'],
            Notes: props.chore['notes'],
            Repeats: props.chore['repeats'],
            Days: props.chore['days'],
            AssignedTo: getAssignedUsernames(props.chore['assignedTo']),
            ChoreId: props.chore['choreId'],
            ChoreDate: props.chore['choreDate']
        }

        axios.post('chorelist/DeleteChore', choreForm)
            .then(res => {
                var isSuccessful = res.data['isSuccessful'];
                if (isSuccessful) {
                    toast('Chore was deleted.')
                    props.setUpdate(true)
                } else {
                    toast(res.data['message'])
                }
            })
            .catch((error => {
                toast('Error performing operation: Delete Chore.')
            }));
    };

    const toast = (title, desc = '') => {
        notification.open({
            message: title,
            description: desc,
            duration: 5,
            placement: "bottom",
        });
    }

    return (
        <div className="padding-bottom">
        <Card
            style={{
                marginBottom: 10,
                width: 120,
                borderColor: 'black'
            }}
            actions={[
                <Nudge key="nudge" assignedUser="Assignee" />,
                <Button shape="circle" icon={<SettingOutlined />} onClick={() => setShowEditModal(true)}/>,
                <Button shape="circle" icon={<CheckSquareOutlined />} onClick={() => completeChore(props.chore)}  />
                ]}>
                {assignmentProfileIcons(props.chore['assignedTo'])}
                <h6 className="mulish-font" style={{
                    marginTop: 10,
                    fontSize: 15,
                    marginLeft: -10,
                    overflowWrap: 'break-word'
                }}><b>{props.chore['name']}</b></h6>
                <p className="mulish-font" style={{
                    fontSize: 11,
                    marginLeft: -10,
                    overflowWrap: 'break-word'
                }}>{(assignmentLabel(props.chore['assignedTo']))}</p>
                <p className="mulish-font" style={{
                    color: 'gray',
                    fontSize: 11,
                    marginLeft: -10,
                    overflowWrap: 'break-word'
                }}><i>{props.chore['notes']}</i></p>
            </Card>
            <ChoreEditModal show={showEditModal} close={() => setShowEditModal(false)} confirm={updateChore} deleteChore={deleteChore} user={props.user} chore={props.chore}/>
    </div>);
};

export default ChoreCard;