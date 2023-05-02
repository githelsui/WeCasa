import React, { Component } from 'react';
import { Container } from 'reactstrap';
import { Route, Routes, BrowserRouter } from 'react-router-dom';
import AppRoutes from './AppRoutes';
import { Header } from './components/Header';
import { NavMenu } from './components/NavMenu';
import { useAuth } from './components/Auth/AuthContext';
import './custom.css';

function App() {
    const baseUrl = document.getElementsByTagName('base')[0].getAttribute('href');
    const { auth, currentGroup } = useAuth();

    return (
        <BrowserRouter basename={baseUrl}>
        <Header />
            {(auth && currentGroup != null) ? <NavMenu /> : <div />}
        <Container tag="main">
        <Routes>
          {AppRoutes.map((route, index) => {
            const { element, ...rest } = route;
            return <Route key={index} {...rest} element={element} />;
          })}
        </Routes>
        </Container>
      </BrowserRouter>
    );
}

export default App;
