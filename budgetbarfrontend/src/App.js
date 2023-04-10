import React from 'react';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import BudgetBar from './budgetbar/budgetbar';
import BulletinBoard from './bulletinboard/bulletinboard';

const App = () => {
  return (
    // <BrowserRouter>
    //   <Routes>
    //     <Route path="/budgetbar" element={<BudgetBar />} />
    //     <Route path="/bulletinboard" element={<BulletinBoard />} />
    //   </Routes>
    // </BrowserRouter>
    // <BulletinBoard/>
    <BulletinBoard/>
  );
};

export default App;


