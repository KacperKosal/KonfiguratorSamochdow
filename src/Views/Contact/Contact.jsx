import React, { useState } from 'react';

import { Mail, Phone, MapPin, Clock, AlertCircle, Send, Facebook, Twitter, Instagram, Linkedin, HelpCircle, ChevronDown, ChevronUp } from 'lucide-react';
import styles from './contact.module.css';  // Upewnij się, że plik znajduje się w tej samej lokalizacji

const ContactPage = () => {
  // ... Twoja logika formularza, obsługa FAQ, itd.
  // Zamiast klas Tailwind użyj stylów z modułu: className={styles.nazwaKlasy}
  
  return (
    <div className={styles.contactContainer}>
      <header className={styles.header}>
        <div className={styles.headerContent}>
          <div className={styles.logo}>AutoKonfigurator</div>
          <nav>
            <ul className={styles.navList}>
              <li className={styles.navItem}>Strona główna</li>
              <li className={styles.navItem}>Modele</li>
              <li className={styles.navItem}>Konfigurator</li>
              <li className={styles.navItem}>Akcesoria</li>
              <li className={styles.navItem}>Kontakt</li>
            </ul>
          </nav>
        </div>
      </header>

      <div className={styles.banner}>
        <div className={styles.bannerContent}>
          <h1 className={styles.bannerTitle}>Kontakt</h1>
          <p className={styles.bannerSubtitle}>Jesteśmy do Twojej dyspozycji. Skontaktuj się z nami w dowolnej sprawie.</p>
        </div>
      </div>

      <main className={styles.main}>
        <div className={styles.gridTwoCols}>
          <div className={styles.formContainer}>
            <h2 className={styles.formTitle}>Wyślij wiadomość</h2>
            {/* ...reszta formularza... */}
          </div>
          <div className={styles.contactInfo}>
            <h2 className={styles.infoTitle}>Dane kontaktowe</h2>
            <div className={styles.infoItem}>
              <MapPin className={styles.infoIcon} />
              <div className={styles.infoText}>
                AutoKonfigurator Sp. z o.o.<br />
                ul. Motoryzacyjna 10<br />
                00-001 Warszawa
              </div>
            </div>
            <div className={styles.infoItem}>
              <Phone className={styles.infoIcon} />
              <div className={styles.infoText}>
                Sprzedaż: +48 123 456 789<br />
                Serwis: +48 123 456 788<br />
                Fax: +48 123 456 787
              </div>
            </div>
            <div className={styles.infoItem}>
              <Mail className={styles.infoIcon} />
              <div className={styles.infoText}>
                Sprzedaż: sprzedaz@autokonfigurator.pl<br />
                Serwis: serwis@autokonfigurator.pl<br />
                Ogólny: info@autokonfigurator.pl
              </div>
            </div>
            <div className={styles.infoItem}>
              <Clock className={styles.infoIcon} />
              <div className={styles.infoText}>
                Poniedziałek - Piątek: 8:00 - 18:00<br />
                Sobota: 9:00 - 14:00<br />
                Niedziela: Zamknięte
              </div>
            </div>
            {/* Social media, mapa, FAQ można również przenieść do modułu */}
          </div>
        </div>
      </main>

      <footer className={styles.footer}>
        <div className={styles.footerContent}>
          <p className={styles.footerText}>© 2025 AutoKonfigurator. Wszelkie prawa zastrzeżone.</p>
          <div className={styles.footerLinks}>
            <a href="#" className={styles.footerLink}>Polityka prywatności</a>
            <a href="#" className={styles.footerLink}>Warunki korzystania</a>
            <a href="#" className={styles.footerLink}>Kontakt</a>
          </div>
        </div>
      </footer>
    </div>
  );
};

export default ContactPage;
