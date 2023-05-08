import React, { Component, useState, useEffect, useRef } from 'react';
import * as d3 from 'd3';
import { useLocation } from 'react-router-dom';
import { Modal, ConfigProvider, Button, Row, Col, Image, Space, Input, Form, Switch, Tabs } from 'antd';
import * as Styles from '../../styles/ConstStyles.js';
import '../../styles/System.css';
import '../../index.css';
import axios from 'axios';
import { useAuth } from '../Auth/AuthContext';
const TabPane = Tabs.TabPane;

// temp data for line charts
const data = [
    { date: '2022-01-01', value: 10 },
    { date: '2022-01-02', value: 20 },
    { date: '2022-01-03', value: 30 },
    { date: '2022-01-04', value: 15 },
    { date: '2022-01-05', value: 95 },
];

export const Charts = (props) => {
    const { admin, auth, currentUser } = useAuth();
    //const [ data, setData] = useState([]) 

    const fetchAnalyticsData = () => {
        let kpiForm = {
            TimeFrame: props.timeFrame,
            CurrentUser: currentUser["username"]
        }

        axios.post(('analytics/' + props.endpoint), kpiForm)
            .then(res => {
                var isSuccessful = res.data['isSuccessful'];
                if (isSuccessful) {
                    console.log("api went through")
                    //setData(res.data.returnedObject)
                } else {

                }
            })
            .catch((error => { console.error(error) }));
    }

    //KPI: Most Used Features
    const renderBarChart = () => {

    }

    //All other KPIs
    const renderLineChart = () => {
        // set the dimensions and margins of the graph
        var margin = { top: 20, right: 20, bottom: 50, left: 70 },
            width = 960 - margin.left - margin.right,
            height = 500 - margin.top - margin.bottom;
        // append the svg object to the body of the page
        d3.select("#d3-line-chart").html("");
        var svg = d3.select("#d3-line-chart").append("svg")
            .attr("width", width + margin.left + margin.right)
            .attr("height", height + margin.top + margin.bottom)
            .append("g")
            .attr("transform", `translate(${margin.left},     ${margin.top})`);

        // Add X axis and Y axis
        var x = d3.scaleTime().range([0, width]);
        var y = d3.scaleLinear().range([height, 0]);
        x.domain(d3.extent(data, d => new Date(d.date)));
        y.domain([0, d3.max(data, (d) => { return d.value; })]);

        const xAxisGenerator = d3.axisBottom(x)
            .tickFormat(d3.timeFormat("%x"));
        svg.append('g')
            .attr('transform', `translate(0, ${height})`)
            .call(xAxisGenerator);
        svg.append("g")
            .call(d3.axisLeft(y));

        // add the Line
        var valueLine = d3.line()
            .x((d) => { return x(new Date(d.date)); })
            .y((d) => { return y(d.value); });
        svg.append("path")
            .data([data])
            .attr("class", "line")
            .attr("fill", "none")
            .attr("stroke", "steelblue")
            .attr("stroke-width", 1.5)
            .attr("d", valueLine)
    };

    useEffect(() => {
        console.log("endpoint: " + props.endpoint)
        fetchAnalyticsData();
        renderLineChart();
        console.log(props.kpiLabel + ' in ' + props.timeFrame)
    }, [props.endpoint]);

    //useEffect(() => {
    //    //renderLineChart();
    //}, [data]);

    return (
        <div style={{ paddingTop: '20px' }}>
            <h6 className="chart-header" style={{ textAlign: 'center' }}>{props.kpiLabel + ' in ' + props.timeFrame}</h6>
            <Row><div id="d3-line-chart"></div></Row>
        </div>
    );
};

export default Charts;