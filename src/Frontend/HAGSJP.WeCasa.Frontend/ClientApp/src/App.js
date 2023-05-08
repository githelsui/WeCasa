import React, { Component } from 'react';
import { Container } from 'reactstrap';
import { Route, Routes, BrowserRouter } from 'react-router-dom';
import AppRoutes from './AppRoutes';
import { Header } from './components/Header';
import { NavMenu } from './components/NavMenu';
import { AdminNavMenu } from './components/AdminNavMenu.js';
import { Footer } from './components/Footer';
import { useAuth } from './components/Auth/AuthContext';
import './custom.css';

function App() {
    const baseUrl = document.getElementsByTagName('base')[0].getAttribute('href');
    const { auth, admin, currentGroup } = useAuth();

    return (
        <BrowserRouter basename={baseUrl}>
        <Header />
            {(auth && admin && currentGroup == null) ? (<div><AdminNavMenu /></div>) : (<div></div>)}
            {(auth && currentGroup != null) ? <NavMenu /> : <div />}
        <Container tag="main">
        <Routes>
          {AppRoutes.map((route, index) => {
            const { element, ...rest } = route;
            return <Route key={index} {...rest} element={element} />;
          })}
        </Routes>
        </Container>
        <Footer />
      </BrowserRouter>
    );
}

export default App;
