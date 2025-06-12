// CarDetailsModal.jsx
import styles from './CarDetailsModal.module.css';
import btnStyles from '../Form.module/Button.module.css';
import commonStyles from '../Form.module/Common.module.css';

const CarDetailsModal = ({ car, onClose, onEdit, formatPrice }) => (
  <div className={commonStyles.card}>
    <div className={commonStyles.cardHeader}>
      <h2 className={commonStyles.cardTitle}>
        Szczegóły Samochodu
      </h2>
      <button
        onClick={onClose}
        className={commonStyles.closeBtn}
      >
        ×
      </button>
    </div>

    <div className={styles.detailsGrid}>
      {/* Lewa kolumna */}
      <div className={commonStyles.spaceY6}>
        {/* Podstawowe informacje */}
        <div className={styles.detailSection}>
          <h3 className={styles.detailTitle}>
            Podstawowe informacje
          </h3>
          <div className={styles.detailList}>
            <div className={styles.detailItem}>
              <span className={styles.detailLabel}>Marka:</span>
              <span className={styles.detailValue}>{car.marka}</span>
            </div>
            <div className={styles.detailItem}>
              <span className={styles.detailLabel}>Model:</span>
              <span className={styles.detailValue}>{car.model}</span>
            </div>
            <div className={styles.detailItem}>
              <span className={styles.detailLabel}>Rok:</span>
              <span className={styles.detailValue}>{car.rok}</span>
            </div>
            <div className={styles.detailItem}>
              <span className={styles.detailLabel}>Typ:</span>
              <span className={styles.detailValue} style={{textTransform: 'capitalize'}}>{car.typ}</span>
            </div>
            <div className={styles.detailItem}>
              <span className={styles.detailLabel}>Kolor nadwozia:</span>
              <span className={styles.detailValue}>{car.kolorNadwozia}</span>
            </div>
            <div className={styles.detailItem}>
              <span className={styles.detailLabel}>Cena:</span>
              <span className={`${styles.detailValue} ${styles.success}`} style={{fontSize: '1.125rem', fontWeight: 'bold'}}>
                {formatPrice(car.cena)}
              </span>
            </div>
          </div>
        </div>

        {/* Silnik */}
        <div className={styles.detailSection}>
          <h3 className={styles.detailTitle}>
            Dane techniczne
          </h3>
          <div className={styles.detailList}>
            <div className={styles.detailItem}>
              <span className={styles.detailLabel}>Pojemność:</span>
              <span className={styles.detailValue}>{car.pojemnosc}L</span>
            </div>
            <div className={styles.detailItem}>
              <span className={styles.detailLabel}>Moc:</span>
              <span className={styles.detailValue}>{car.moc} KM</span>
            </div>
            <div className={styles.detailItem}>
              <span className={styles.detailLabel}>Napęd 4x4:</span>
              <span className={`${styles.detailValue} ${car.ma4x4 ? styles.success : styles.muted}`}>
                {car.ma4x4 ? 'Tak' : 'Nie'}
              </span>
            </div>
            <div className={styles.detailItem}>
              <span className={styles.detailLabel}>Elektryczny:</span>
              <span className={`${styles.detailValue} ${car.jestElektryczny ? styles.success : styles.muted}`}>
                {car.jestElektryczny ? 'Tak' : 'Nie'}
              </span>
            </div>
          </div>
        </div>

        {/* Zdjęcie */}
        {car.zdjecie && (
          <div className={styles.detailSection}>
            <h3 className={styles.detailTitle}>
              Zdjęcie
            </h3>
            <img 
              src={car.zdjecie} 
              alt={`${car.marka} ${car.model}`}
              className={styles.detailImage}
              onError={(e) => {
                e.target.parentElement.parentElement.style.display = 'none';
              }}
            />
          </div>
        )}
      </div>

      {/* Prawa kolumna */}
      <div className={commonStyles.spaceY6}>
        {/* Wyposażenie wnętrza */}
        <div className={styles.detailSection}>
          <h3 className={styles.detailTitle}>
            Wyposażenie wnętrza
          </h3>
          <p className={styles.detailText}>
            {car.wyposazenieWnetrza}
          </p>
        </div>

        {/* Akcesoria */}
        {car.akcesoria && (
          <div className={styles.detailSection}>
            <h3 className={styles.detailTitle}>
              Akcesoria
            </h3>
            <p className={styles.detailText}>
              {car.akcesoria}
            </p>
          </div>
        )}

        {/* Cechy */}
        {(car.cechy && car.cechy.length > 0) && (
          <div className={styles.detailSection}>
            <h3 className={styles.detailTitle}>
              Cechy dodatkowe
            </h3>
            <div className={styles.detailBadges}>
              {car.cechy.map((cecha, index) => (
                <span
                  key={`modal-${car.id}-feature-${index}-${cecha}`}
                  className={`${styles.detailBadge} ${styles.detailBadgeBlue}`}
                >
                  {cecha}
                </span>
              ))}
            </div>
          </div>
        )}

        {/* Własne cechy */}
        {(car.wlasneCechy && car.wlasneCechy.length > 0) && (
          <div className={styles.detailSection}>
            <h3 className={styles.detailTitle}>
              Cechy specjalne
            </h3>
            <div className={styles.detailBadges}>
              {car.wlasneCechy.map((cecha, index) => (
                <span
                  key={`modal-${car.id}-custom-${index}-${cecha}`}
                  className={`${styles.detailBadge} ${styles.detailBadgeGreen}`}
                >
                  {cecha}
                </span>
              ))}
            </div>
          </div>
        )}

        {/* Opis */}
        <div className={styles.detailSection}>
          <h3 className={styles.detailTitle}>
            Opis
          </h3>
          <p className={styles.detailText}>
            {car.opis}
          </p>
        </div>
      </div>
    </div>

    {/* Przyciski akcji */}
    <div className={styles.detailActions}>
      <button
        onClick={() => onEdit(car)}
        className={`${btnStyles.btn} ${btnStyles.btnWarning}`}
      >
        Edytuj Samochód
      </button>
      <button
        onClick={onClose}
        className={`${btnStyles.btn} ${btnStyles.btnSecondary}`}
      >
        Zamknij
      </button>
    </div>
  </div>
);

export default CarDetailsModal;