import React from 'react';
// import { Router, Routes, Route } from 'react-router-dom';
import { BrowserRouter, Router,Routes,  Route } from 'react-router-dom';
import Header from './components/Header';
import DataTable from './components/DataTable';
import ColorCodedBar from './components/ColorCodedBar';
import EditBudgetModal from './components/EditBar';

const App = () => {
  return (
    <div>
      <Header />
      <DataTable />
      <ColorCodedBar />
    </div>
  );
};



export default App;

