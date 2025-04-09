import React from 'react';
import styles from './Footer.module.css';

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
          <ul className={styles.footerText}>
            <li className={styles.navItem}>Strona główna</li>
            <li className={styles.navItem}>Modele</li>
            <li className={styles.navItem}>Konfigurator</li>
            <li className={styles.navItem}>Akcesoria</li>
          </ul>
        </div>
        <div>
          <h3 className={styles.footerTitle}>Kontakt</h3>
          <ul className={styles.footerText}>
            <li>info@autokonfigurator.pl</li>
            <li>+48 123 456 789</li>
            <li>ul. Motoryzacyjna 10</li>
            <li>00-001 Warszawa</li>
          </ul>
        </div>
        <div>
          <h3 className={styles.footerTitle}>Newsletter</h3>
          <p className={styles.footerText}>
            Zapisz się, aby otrzymywać najnowsze oferty i aktualizacje.
          </p>
          <div className={styles.newsletterForm}>
            <input
              type="email"
              placeholder="Twój e-mail"
              className={styles.newsletterInput}
            />
            <button className={styles.newsletterButton}>
              Zapisz się
            </button>
          </div>
        </div>
      </div>
      <div className={styles.footerBottom}>
        © 2024 AutoKonfigurator. Wszelkie prawa zastrzeżone.
      </div>
    </footer>
  );
};

export default Footer;
