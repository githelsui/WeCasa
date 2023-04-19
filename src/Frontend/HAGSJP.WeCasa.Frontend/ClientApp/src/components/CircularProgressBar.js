import React, { Component, useState, useEffect } from 'react';
import '../styles/CircularProgressBar.css';
import '../styles/System.css';
import '../index.css';

export const CircularProgressBar = (props) => {
    // const value = props.percentage;
    const value = 33;

    return (
        <div>
        <div role="progressbar"
            aria-valuemin="0"
            aria-valuemax="100"
            style={{ "--value": value }}>
        </div>
            <p>chores completed</p>
        </div>
    );
};

export default CircularProgressBar;