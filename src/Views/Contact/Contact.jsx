import React, { useState } from 'react';
import styles from './Contact.module.css';

export default function Contact() {
  const [formData, setFormData] = useState({
    name: '',
    email: '',
    phone: '',
    subject: 'info',
    message: ''
  });
  const [submitted, setSubmitted] = useState(false);

  const handleChange = (e) => {
    const { id, value } = e.target;
    setFormData(prevState => ({
      ...prevState,
      [id]: value
    }));
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    // Tu można dodać integrację z API do wysyłania formularza
    console.log('Wysyłanie formularza:', formData);
    setSubmitted(true);
  };

  return (
    <div className={styles.page}>
      <main className={styles.main}>
        <div className={styles.container}>
          <h1 className={styles.pageTitle}>Kontakt</h1>
          
          <div className={styles.contactWrapper}>
            {/* Informacje kontaktowe */}
            <div className={styles.contactInfo}>
              <h2 className={styles.sectionTitle}>Dane kontaktowe</h2>
              
              <div className={styles.infoItem}>
                <strong>Email:</strong>
                <p>info@autokonfigurator.pl</p>
              </div>
              
              <div className={styles.infoItem}>
                <strong>Telefon:</strong>
                <p>+48 123 456 789</p>
              </div>
              
              <div className={styles.infoItem}>
                <strong>Adres:</strong>
                <p>ul. Motoryzacyjna 10</p>
                <p>00-001 Warszawa</p>
              </div>
              
              <div className={styles.workingHours}>
                <h3 className={styles.subSectionTitle}>Godziny otwarcia</h3>
                <p>Poniedziałek - Piątek: 8:00 - 18:00</p>
                <p>Sobota: 9:00 - 14:00</p>
                <p>Niedziela: Zamknięte</p>
              </div>
            </div>
            
            {/* Formularz kontaktowy */}
            <div className={styles.contactForm}>
              <h2 className={styles.sectionTitle}>Napisz do nas</h2>
              
              {submitted ? (
                <div className={styles.successMessage}>
                  <h3 className={styles.successTitle}>Dziękujemy za wiadomość!</h3>
                  <p className={styles.successDescription}>Odpowiemy najszybciej jak to możliwe.</p>
                </div>
              ) : (
                <form onSubmit={handleSubmit}>
                  <div className={styles.formGroup}>
                    <label htmlFor="name">Imię i nazwisko</label>
                    <input 
                      type="text" 
                      id="name" 
                      value={formData.name}
                      onChange={handleChange}
                      required 
                    />
                  </div>
                  
                  <div className={styles.formGroup}>
                    <label htmlFor="email">Email</label>
                    <input 
                      type="email" 
                      id="email" 
                      value={formData.email}
                      onChange={handleChange}
                      required 
                    />
                  </div>
                  
                  <div className={styles.formGroup}>
                    <label htmlFor="phone">Telefon</label>
                    <input 
                      type="tel" 
                      id="phone" 
                      value={formData.phone}
                      onChange={handleChange}
                    />
                  </div>
                  
                  <div className={styles.formGroup}>
                    <label htmlFor="subject">Temat</label>
                    <select 
                      id="subject"
                      value={formData.subject}
                      onChange={handleChange}
                    >
                      <option value="info">Informacje ogólne</option>
                      <option value="config">Konfiguracja samochodu</option>
                      <option value="test">Jazda testowa</option>
                      <option value="other">Inne</option>
                    </select>
                  </div>
                  
                  <div className={styles.formGroup}>
                    <label htmlFor="message">Wiadomość</label>
                    <textarea 
                      id="message" 
                      rows="5"
                      value={formData.message}
                      onChange={handleChange}
                      required
                    ></textarea>
                  </div>
                  
                  <button type="submit" className={styles.submitBtn}>
                    Wyślij wiadomość
                  </button>
                </form>
              )}
            </div>
          </div>
        </div>
      </main>
    </div>
  );
}