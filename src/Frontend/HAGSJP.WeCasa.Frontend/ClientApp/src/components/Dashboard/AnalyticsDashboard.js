import React, { Component, useState, useEffect } from 'react';
import { Navigate, useNavigate } from 'react-router-dom'
import { useAuth } from '../Auth/AuthContext';
import { Col, Card, Row, Space, Avatar, Button, notification, Tabs, Layout, Menu } from 'antd';
import * as Styles from '../../styles/ConstStyles.js';
import { NotFound } from "../NotFound";
import { KPIContent } from "./KPIContent";
import axios from 'axios';
const TabPane = Tabs.TabPane;
const { Header, Content, Footer, Sider } = Layout;

export const AnalyticsDashboard = (props) => {
    const kpiList = ["Logins Per Day", "Registrations Per Day", "Daily Active Users", "Sessions Per User", "Most Used Features", "Error Rate", "Retention Rate", "Customer Service Ratings", "User Rating Reviews"];
    const { currentGroup, currentUser, auth, admin } = useAuth();
    const [selectedKPI, setSelectedKPI] = useState("")
    const [currentKPI, setCurrentKPI] = useState("Logins Per Day")

    const selectKPI = (item) => {
        setCurrentKPI(item);
    };


    useEffect(() => {
        // Update the selected key after the component has re-rendered
        setSelectedKPI(currentKPI);
    }, [currentKPI]);

    return (
        <div>
            {(auth && admin && currentGroup == null) ?
                (<div>
                    <Layout style={{ backgroundColor: 'white' }}>
                        <Sider
                            theme="light"
                            breakpoint="lg"
                            collapsedWidth="0"
                            onBreakpoint={(broken) => {
                                console.log(broken);
                            }}
                            onCollapse={(collapsed, type) => {
                                console.log(collapsed, type);
                            }}>
                            <Menu
                                selectedKeys={currentKPI}
                                theme="light"
                                mode="vertical"
                                >
                                {kpiList.map((item) => (
                                    <Menu.Item key={item} onClick={() => selectKPI(item)}>
                                    {item}
                                </Menu.Item>
                            ))}
                        </Menu>
                        </Sider>
                        <Content style={{ marginLeft: '30px', backgroundColor: 'white' }}>
                            <KPIContent kpiLabel={currentKPI}/>
                        </Content>
                    </Layout>
                </div>) :
                (<NotFound />)}
        </div>
    );
};

export default AnalyticsDashboard;
