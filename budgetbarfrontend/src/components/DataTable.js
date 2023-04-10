import React, { useState } from 'react';
import './DataTable.css';

const DataTable = () => {
  const [tableData, setTableData] = useState([

  ]);

  const [modalOpen, setModalOpen] = useState(false);

  const [updateModalOpen, setUpdateModalOpen] = useState(false);

  const [formData, setFormData] = useState({
    date: '',
    name: '',
    owner: '',
    description: '',
    amount: '',
    status: '',
    receipt: ''
  });

  const [currentIndex, setCurrentIndex] = useState(-1);

  const handleModalOpen = () => {
    setModalOpen(true);
  };

  const handleModalClose = () => {
    setModalOpen(false);
  };

  const handleDeleteBill = () => {
    const updatedTableData = [...tableData];
    updatedTableData.splice(currentIndex, 1);
    setTableData(updatedTableData);
    setUpdateModalOpen(false);
  };

  const handleUpdateModalOpen = id => {
    const billIndex = tableData.findIndex(bill => bill.id === id);
    setCurrentIndex(billIndex);
    setFormData(tableData[billIndex]);
    setUpdateModalOpen(true);
  };
  

  const handleUpdateModalClose = () => {
    setUpdateModalOpen(false);
  };

  const handleFormDataChange = event => {
    setFormData({
      ...formData,
      [event.target.name]: event.target.value
    });
  };

  const handleAddBill = () => {
    const newBill = {
      id: tableData.length + 1,
      date: formData.date,
      name: formData.name,
      owner: formData.owner,
      description: formData.description,
      amount: formData.amount,
      status: formData.status,
      receipt: formData.receipt
    };

    setTableData([...tableData, newBill]);
    setModalOpen(false);
  };

  const handleUpdateBill = () => {
    const updatedTableData = [...tableData];
    updatedTableData[currentIndex] = {
      ...formData
    };

    setTableData(updatedTableData);
    setUpdateModalOpen(false);

  };

  const data1 = [
    
  ];

  const data2 = [
    
  ];


  const handleTable1 = () => {
    setTableData(data1);
  };

  const handleTable2 = () => {
    setTableData(data2);
  };

  return (
    <div className="datatable">
      {updateModalOpen && (
  <div className="modal">
    <div className="modal-content">
      <span className="close" onClick={handleUpdateModalClose}>&times;</span>
      <form>
        <input
          type="text"
          name="date"
          placeholder="Date"
          value={formData.date}
          onChange={handleFormDataChange}
        />
        <input
          type="text"
          name="name"
          placeholder="Name"
          value={formData.name}
          onChange={handleFormDataChange}
        />
        <input
          type="text"
          name="owner"
          placeholder="Owner"
          value={formData.owner}
          onChange={handleFormDataChange}
        />
        <input
          type="text"
          name="description"
          placeholder="Description"
          value={formData.description}
          onChange={handleFormDataChange}
        />
        <input
          type="text"
          name="amount"
          placeholder="Amount"
          value={formData.amount}
          onChange={handleFormDataChange}
        />
        <input
          type="text"
          name="status"
          placeholder="Status"
          value={formData.status}
          onChange={handleFormDataChange}
        />
        <input
          type="text"
          name="receipt"
          placeholder="Receipt"
          value={formData.receipt}
          onChange={handleFormDataChange}
        />
      </form>
      <button onClick={handleUpdateBill}>Save</button>
      <button onClick={handleDeleteBill}>Delete</button>
    </div>
  </div>
)}

      <div className="buttons">
        <button onClick={handleTable1}>Table 1</button>
        <button onClick={handleTable2}>Table 2</button>
        <button onClick={handleModalOpen}>Add Bill</button>
      </div>
      {modalOpen && (
    <div className="modal">
      <div className="modal-content">
        <span className="close" onClick={handleModalClose}>&times;</span>
        <form>
          <input
            type="text"
            name="date"
            placeholder="Date"
            value={formData.date}
            onChange={handleFormDataChange}
          />
          <input
            type="text"
            name="name"
            placeholder="Name"
            value={formData.name}
            onChange={handleFormDataChange}
          />
          <input
            type="text"
            name="owner"
            placeholder="Owner"
            value={formData.owner}
            onChange={handleFormDataChange}
          />
          <input
            type="text"
            name="description"
            placeholder="Description"
            value={formData.description}
            onChange={handleFormDataChange}
          />
          <input
            type="text"
            name="amount"
            placeholder="Amount"
            value={formData.amount}
            onChange={handleFormDataChange}
          />
          <input
            type="text"
            name="status"
            placeholder="Status"
            value={formData.status}
            onChange={handleFormDataChange}
          />
          <input
            type="text"
            name="receipt"
            placeholder="Receipt"
            value={formData.receipt}
            onChange={handleFormDataChange}
          />
          <button onClick={handleAddBill}>Add Bill</button>
        </form>
      </div>
    </div>
  )}
      <table>
  <thead>
    <tr>
      <th>ID</th>
      <th>Date</th>
      <th>Name</th>
      <th>Owner</th>
      <th>Description</th>
      <th>Amount</th>
      <th>Status</th>
      <th>Receipt</th>
      <th>Actions</th>
    </tr>
  </thead>
  <tbody>
  {tableData.map((row, index) => (
  <tr key={row.id}>
        <td>{row.id}</td>
        <td>{row.date}</td>
        <td>{row.name}</td>
        <td>{row.owner}</td>
        <td>{row.description}</td>
        <td>{row.amount}</td>
        <td>{row.status}</td>
        <td>{row.receipt}</td>
        <td>
        <button onClick={() => handleUpdateModalOpen(row.id)}>Update</button>


    </td>

      </tr>
    ))}
  </tbody>
</table>

    </div>
  );
};

export default DataTable;