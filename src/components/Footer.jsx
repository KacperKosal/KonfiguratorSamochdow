import React from 'react';

const Footer = () => {
  return (
    <footer className="footer">
      <div className="footer-container">
        <div>
          <h3 className="footer-title">AutoKonfigurator</h3>
          <p className="footer-text">
            Twój samochód, Twoje zasady. Konfiguruj i personalizuj swój wymarzony samochód online.
          </p>
        </div>
        <div>
          <h3 className="footer-title">Nawigacja</h3>
          <ul className="footer-text" style={{ listStyle: 'none', paddingLeft: 0 }}>
            <li className="nav-item">Strona główna</li>
            <li className="nav-item">Modele</li>
            <li className="nav-item">Konfigurator</li>
            <li className="nav-item">Akcesoria</li>
          </ul>
        </div>
        <div>
          <h3 className="footer-title">Kontakt</h3>
          <ul className="footer-text" style={{ listStyle: 'none', paddingLeft: 0 }}>
            <li>info@autokonfigurator.pl</li>
            <li>+48 123 456 789</li>
            <li>ul. Motoryzacyjna 10</li>
            <li>00-001 Warszawa</li>
          </ul>
        </div>
        <div>
          <h3 className="footer-title">Newsletter</h3>
          <p className="footer-text">
            Zapisz się, aby otrzymywać najnowsze oferty i aktualizacje.
          </p>
          <div style={{ display: 'flex' }}>
            <input
              type="email"
              placeholder="Twój e-mail"
              style={{ flex: 1, padding: '8px', borderRadius: '4px 0 0 4px', border: '1px solid #ccc' }}
            />
            <button
              style={{
                padding: '8px 16px',
                borderRadius: '0 4px 4px 0',
                border: 'none',
                backgroundColor: '#1d4ed8',
                color: '#fff',
                cursor: 'pointer'
              }}
            >
              Zapisz
            </button>
          </div>
        </div>
      </div>
      <div className="footer-bottom">
        <p>© 2025 AutoKonfigurator. Wszelkie prawa zastrzeżone.</p>
      </div>
    </footer>
  );
};

export default Footer;
