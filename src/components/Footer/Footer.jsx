import React from 'react';
import styles from './Footer.module.css';
import { NavLink } from 'react-router-dom';

const Footer = () => {
  return (
    <footer className={styles.footer}>
      <div className={styles.footerContainer}>
        <div>
          <h3 className={styles.footerTitle}>AutoKonfigurator</h3>
          <p className={styles.footerText}>
            Twój samochód, Twoje zasady. Konfiguruj i personalizuj swój wymarzony samochód online.
          </p>
        </div>
        <div>
          <h3 className={styles.footerTitle}>Nawigacja</h3>
          <ul className={styles.footerNav}>
            <li><NavLink to="/home" className={styles.navItem}>Modele</NavLink></li>
            <li><NavLink to="/contact" className={styles.navItem}>Kontakt</NavLink></li>
            <li><NavLink to="/login" className={styles.navItem}>Zaloguj się</NavLink></li>
            <li><NavLink to="/register" className={styles.navItem}>Zarejestruj się</NavLink></li>
          </ul>
        </div>
        <div>
          <h3 className={styles.footerTitle}>Kontakt</h3>
          <ul className={styles.footerText}>
            <li>konfiguratorsamochodwy@gmail.com</li>
            <li>+48 123 456 789</li>
            <li>ul. Motoryzacyjna 10</li>
            <li>00-001 Warszawa</li>
          </ul>
        </div>
        
      </div>
      <div className={styles.footerBottom}>
        © 2025 AutoKonfigurator. Wszelkie prawa zastrzeżone.
      </div>
    </footer>
  );
};

export default Footer;
