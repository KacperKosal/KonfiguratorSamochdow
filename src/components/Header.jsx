import React from 'react';

const Header = () => {
  return (
    <header className="header">
      <div className="header-container">
        <div className="header-logo">AutoKonfigurator</div>
        <nav>
          <ul className="nav-list">
            <li className="nav-item">Modele</li>
            <li className="nav-item">Konfigurator</li>
            <li className="nav-item">Salony</li>
            <li className="nav-item">Finansowanie</li>
            <li className="nav-item">Kontakt</li>
            <li className="nav-item">Logowanie</li>
          </ul>
        </nav>
      </div>
    </header>
  );
};

export default Header;
