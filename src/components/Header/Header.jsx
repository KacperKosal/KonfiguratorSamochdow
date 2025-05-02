import React from 'react';
import styles from './Header.module.css';
import { NavLink } from 'react-router-dom';

const Header = () => {
  return (
    <header className={styles.header}>
      <div className={styles.headerContainer}>
        <div className={styles.headerLogo}>AutoKonfigurator</div>
        <nav>
          <ul className={styles.navList}>
            <li className={styles.navItem}><NavLink to="/home">Modele</NavLink></li>
            <li className={styles.navItem}><NavLink to="/car-configurator">Konfigurator</NavLink></li>
            <li className={styles.navItem}>Salony</li>
            <li className={styles.navItem}>Finansowanie</li>
            <li className={styles.navItem}><NavLink to="/contact">Kontakt</NavLink></li>
            <li className={styles.navItem}><NavLink to="/login">Logowanie</NavLink></li>
          </ul>
        </nav>
      </div>
    </header>
  );
};

export default Header;
