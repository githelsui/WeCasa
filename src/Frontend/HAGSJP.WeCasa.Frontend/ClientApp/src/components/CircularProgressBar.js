import React, { Component, useState, useEffect } from 'react';
import '../styles/CircularProgressBar.css';
import '../styles/System.css';
import '../index.css';

export const CircularProgressBar = (props) => {
    const report = props.report;

    const calcChoreProgress = (report) => {
        console.log(report);
        var result;
        if (report == null || report.length == 0 || report.length > 1) {
            result = "-";
        } else {
            if (report.completedChores != 0 && report.incompleteChores === 0) {
                result = 100
            } else {
                result = (parseInt(report.completedChores) / (parseInt(report.completedChores) + parseInt(report.incompleteChores))) * 100;
                if (Number.isInteger(result)) {
                    return result;
                } else if (Number.isNaN(result)) {
                    result = "-";
                } else {
                    result = result.toFixed(0);
                }
            }
        }
        console.log(report.username, " Chore compltetion percentage:", result);
        return result;
    }

    const value = calcChoreProgress(report[0]);

    return (
        <div>
            <div role="circularbar"
                aria-valuemin="0"
                aria-valuemax="100"
                style={{ "--value": value }}>
            </div>
            {value == "-" ? 
                (<div style={{ textAlign: "center" }}>
                    <p>No chores assigned :(</p>
                </div>) : (
                <div style={{ textAlign: "center" }}>
                    <p>chores completed</p>
                </div>)}    
        </div>
    );
};

export default CircularProgressBar;