import React, { Component, useState, useEffect } from 'react';
import '../styles/CircularProgressBar.css';
import '../styles/System.css';
import '../index.css';

export const CircularProgressBar = (props) => {
    const progress = props.choreProgress;

    const calcChoreProgress = (report) => {
        if (report == null) {
            return "-";
        } else {
            let result = report['completedChores'] / report['incompleteChores'];
            if (Number.isInteger(result)) {
                return result;
            } else if (Number.isNAN) {
                return '-';
            } else {
                return result.toFixed(1);
            }
        }
    }

    return (
        <div>
        <div role="circularbar"
            aria-valuemin="0"
            aria-valuemax="100"
                style={{ "--value": calcChoreProgress(progress) }}>
        </div>
            <p>chores completed</p>
        </div>
    );
};

export default CircularProgressBar;