import React from 'react';
import Header from '../components/Header';
import DataTable from '../components/DataTable';
import ColorCodedBar from '../components/ColorCodedBar';

const BudgetBar = () => {
  return (
    <div>
      <Header />
      <DataTable />
      <ColorCodedBar />
    </div>
  );
};

export default BudgetBar;
