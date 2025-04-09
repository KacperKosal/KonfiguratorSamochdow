import React from 'react';
import { ShoppingCart } from 'lucide-react';
import styles from './SummaryPanel.module.css';

const SummaryPanel = ({
  exteriorColors,
  wheelTypes,
  interiorColors,
  carColor,
  wheelType,
  interiorColor,
  totalPrice,
}) => {
  const selectedExterior = exteriorColors.find(c => c.value === carColor);
  const selectedWheels = wheelTypes.find(w => w.value === wheelType);
  const selectedInterior = interiorColors.find(i => i.value === interiorColor);

  return (
    <div>
      <h2 className={styles.summaryTitle}>Twoja konfiguracja</h2>
      <div className={styles.summarySection}>
        <h3>Model bazowy</h3>
        <p>X-Drive GT 2.0 Turbo 250KM</p>
        <p className={styles.summaryPrice}>120 000 zł</p>
      </div>
      <div className={styles.summarySection}>
        <h3>Kolor nadwozia</h3>
        <p>{selectedExterior?.name}</p>
        <p className={styles.summaryPrice}>+{selectedExterior?.price.toLocaleString()} zł</p>
      </div>
      <div className={styles.summarySection}>
        <h3>Felgi</h3>
        <p>{selectedWheels?.name}</p>
        <p className={styles.summaryPrice}>+{selectedWheels?.price.toLocaleString()} zł</p>
      </div>
      <div className={styles.summarySection}>
        <h3>Wnętrze</h3>
        <p>{selectedInterior?.name}</p>
        <p className={styles.summaryPrice}>+{selectedInterior?.price.toLocaleString()} zł</p>
      </div>
      <div className={styles.priceDivider}>
        <div className={styles.totalPrice}>
          <span>Cena łączna</span>
          <span>{totalPrice.toLocaleString()} zł</span>
        </div>
        <div className={styles.vatInfo}>
          Cena zawiera VAT (23%)
        </div>
      </div>
      <div className={styles.buttonGroup}>
        <button className={`${styles.actionButton} ${styles.save}`}>
          <ShoppingCart size={20} />
          Zapisz konfigurację
        </button>
        <button className={`${styles.actionButton} ${styles.testDrive}`}>
          Umów jazdę testową
        </button>
        <button className={`${styles.actionButton} ${styles.availability}`}>
          Sprawdź dostępność
        </button>
      </div>
    </div>
  );
};

export default SummaryPanel;
