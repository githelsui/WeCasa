import React from 'react';
import './Header.css';

const Header = () => {
  return (
    <header>
      <nav>
        <div className="logo">Logo</div>
        <ul>
          <li>
            <a href="#">Home</a>
          </li>
          <li>
            <a href="#">Bulletin Board</a>
          </li>
          <li>
            <a href="#">Calendar</a>
          </li>
          <li>
            <a href="#">Finances</a>
          </li>
          <li>
            <a href="#">Group Members</a>
          </li>
        </ul>
        <button>Profile</button>
      </nav>
    </header>
  );
};

export default Header;

