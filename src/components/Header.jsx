import React from 'react';

const Header = () => {
  return (
    <header className="header">
      <div className="header-container">
        <div className="header-logo">AutoKonfigurator</div>
        <nav>
          <ul className="nav-list">
            <li className="nav-item">Modele</li>
            <li className="nav-item">Salony</li>
            <li className="nav-item">Finansowanie</li>
            <li className="nav-item">Kontakt</li>
          </ul>
        </nav>
      </div>
    </header>
  );
};

export default Header;
