import React from 'react';
import './Header.css';

const Header = () => {
  return (
    <header>
      <nav>
        <div className="logo">WeCasa</div>
        <ul>
          <li>
            Bulletin Board
          </li>
          <li>
            Calendar
          </li>
          <li>
            Finances
          </li>
          <li>
            Group Members
          </li>
        </ul>
        <button>Profile</button>
      </nav>
    </header>
  );
};

export default Header;







