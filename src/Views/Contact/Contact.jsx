import React, { useState } from 'react';
import styles from './Contact.module.css';
import emailjs from '@emailjs/browser';

export default function Contact() {
  const [formData, setFormData] = useState({
    name: '',
    email: '',
    phone: '',
    subject: 'info',
    message: ''
  });
  const [submitted, setSubmitted] = useState(false);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState('');
  
  // Konfiguracja EmailJS
  const emailJSConfig = {
    serviceId: 'Konfigurator',  // Zaktualizowane ID
    templateId: 'template_6btbzs2', // Zaktualizowane ID
    publicKey: 'EhTnACi248sTCiJ2U'
  };

  // Funkcja wysyłania przez EmailJS
  const sendViaEmailJS = async (formData) => {
    try {
      console.log('🚀 Wysyłam email przez EmailJS...');
      console.log('📧 Docelowy email: konfiguratorsamochodwy@gmail.com');
      console.log('👤 Od:', formData.name, '-', formData.email);
      
      const templateParams = {
        from_name: formData.name,
        from_email: formData.email,
        phone: formData.phone || 'Nie podano',
        subject: getSubjectText(formData.subject),
        message: formData.message,
        to_email: 'konfiguratorsamochodwy@gmail.com'
      };

      console.log('📋 Template params:', templateParams);

      const response = await emailjs.send(
        emailJSConfig.serviceId,
        emailJSConfig.templateId,
        templateParams,
        emailJSConfig.publicKey
      );
      
      console.log('✅ Email wysłany pomyślnie przez EmailJS!', response);
      console.log('📬 Email powinien pojawić się w skrzynce: konfiguratorsamochodwy@gmail.com');
      return true;
    } catch (error) {
      console.error('❌ Błąd EmailJS:', error);
      return false;
    }
  };

  const getSubjectText = (subject) => {
    const subjects = {
      'info': 'Informacje ogólne',
      'config': 'Konfiguracja samochodu',
      'test': 'Jazda testowa',
      'other': 'Inne zapytanie'
    };
    return subjects[subject] || 'Nowe zapytanie';
  };


  const handleChange = (e) => {
    const { id, value } = e.target;
    setFormData(prevState => ({
      ...prevState,
      [id]: value
    }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setIsLoading(true);
    setError('');

    console.log('Rozpoczynam wysyłanie formularza:', formData);

    try {
      console.log('Wysyłam przez EmailJS...');
      const emailJSSuccess = await sendViaEmailJS(formData);
      
      if (emailJSSuccess) {
        setSubmitted(true);
        console.log('Wiadomość wysłana pomyślnie przez EmailJS');
      } else {
        setError('Nie udało się wysłać wiadomości. Spróbuj ponownie za chwilę lub skontaktuj się bezpośrednio na konfiguratorsamochodwy@gmail.com');
      }
    } catch (error) {
      console.error('EmailJS error:', error);
      setError('Wystąpił błąd podczas wysyłania wiadomości. Spróbuj ponownie później lub skontaktuj się bezpośrednio na konfiguratorsamochodwy@gmail.com');
    } finally {
      setIsLoading(false);
    }
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
                <p>konfiguratorsamochodwy@gmail.com</p>
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
                <div>
                  {error && (
                    <div className={styles.errorMessage}>
                      <p>{error}</p>
                    </div>
                  )}
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
                  
                  <button type="submit" className={styles.submitBtn} disabled={isLoading}>
                    {isLoading ? 'Wysyłanie...' : 'Wyślij wiadomość'}
                  </button>
                </form>
                </div>
              )}
            </div>
          </div>
        </div>
      </main>
    </div>
  );
}