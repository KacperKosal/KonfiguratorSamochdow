import React from 'react';
import styles from './Header.module.css';

const Header = () => {
  return (
    <header className={styles.header}>
      <div className={styles.headerContainer}>
        <div className={styles.headerLogo}>AutoKonfigurator</div>
        <nav>
          <ul className={styles.navList}>
            <li className={styles.navItem}>Modele</li>
            <li className={styles.navItem}>Konfigurator</li>
            <li className={styles.navItem}>Salony</li>
            <li className={styles.navItem}>Finansowanie</li>
            <li className={styles.navItem}>Kontakt</li>
            <li className={styles.navItem}>Logowanie</li>
          </ul>
        </nav>
      </div>
    </header>
  );
};

export default Header;
