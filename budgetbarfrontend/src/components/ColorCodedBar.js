import React, { useState } from 'react';
import './ColorCodedBar.css';

const ColorCodedBar = () => {
  const [data] = useState([ //setData
    { color: 'red', value: 50, name: 'Roommate 1' },
    { color: 'blue', value: 10, name: 'Roommate 2' },
    { color: 'green', value: 40, name: 'Roommate 3' },
  ]);
  
  const [totalBudget, setTotalBudget] = useState(
    data.reduce((acc, item) => acc + item.value, 0)
  );
  const totalSpent = data.reduce((acc, item) => acc + item.value, 0);
  const remainingBudget = totalBudget - totalSpent;
  //const remainingBudgetPercentage = (remainingBudget / totalBudget) * 100;

  const [showModal, setShowModal] = useState(false);
  const [newTotalBudget, setNewTotalBudget] = useState(totalBudget);

  const handleSave = () => {
    setTotalBudget(newTotalBudget);
    setShowModal(false);
  };

  return (
    <div className="budget-bar">
  <div className="edit-budget-button" onClick={() => setShowModal(true)}>
    Edit Budget
  </div>
  {showModal && (
    <div className="edit-budget-modal">
      <input
        type="number"
        value={newTotalBudget}
        onChange={(e) => setNewTotalBudget(e.target.value)}
      />
      <button onClick={handleSave}>Save</button>
      <button onClick={() => setShowModal(false)}>Cancel</button>
    </div>
  )}
  <div className="roommates">
    {data.map((item, index) => (
      <div key={index} className="roommate">
        <span className="roommate-color" style={{ backgroundColor: item.color }} />
        <span className="roommate-name">{item.name}</span>
      </div>
    ))}
    <div className="total-budget">
      Total Monthly Budget: ${totalBudget}
    </div>
  </div>
  <div className="budget-bar-container">
    <div
      className="total-spent-bar"
      style={{ width: `${(totalSpent / totalBudget) * 100}%` }}
    >
      {totalSpent / totalBudget > 0 && (
        <div className="total-spent-text">
          Total Spent: ${totalSpent}
        </div>
      )}
    </div>
    <div
      className="remaining-budget-bar"
      style={{ width: `${(remainingBudget / totalBudget) * 100}%` }}
    >
      {remainingBudget / totalBudget > 0 && (
        <div className="remaining-budget-text">
         Remaining Budget: ${remainingBudget}
        </div>
      )}
    </div>
  </div>

  <div className="bar-container">
    {data.map((item, index) => (
      <div
        key={index}
        className="bar-item"
        style={{
          backgroundColor: item.color,

          width: `${(item.value / totalSpent) * 100}%`,
        }}
        >
        <div className="value">${item.value}</div>
      </div>
    ))}
  </div>
</div>

  );
};

export default ColorCodedBar;