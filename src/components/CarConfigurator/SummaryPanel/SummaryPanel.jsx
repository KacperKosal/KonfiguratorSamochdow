import React from 'react';
import { ShoppingCart } from 'lucide-react';
import styles from './SummaryPanel.module.css';

const SummaryPanel = ({
  selectedCarModel,
  selectedEngine,
  exteriorColors,
  wheelTypes,
  interiorColors,
  selectedAccessories,
  selectedInteriorEquipment,
  carColor,
  wheelType,
  interiorColor,
  totalPrice,
  onSaveConfiguration
}) => {
  const selectedExterior = exteriorColors.find(c => c.value === carColor);
  const selectedWheels = wheelTypes.find(w => w.value === wheelType);
  const selectedInterior = interiorColors.find(i => i.value === interiorColor);

  return (
    <div className={styles.summaryPanel}>
      <h2>Podsumowanie konfiguracji</h2>
      
      {/* Model bazowy z danych API */}
      <div className={styles.summarySection}>
        <h3>Model bazowy</h3>
        <p>{selectedCarModel?.name || 'X-Drive GT 2.0 Turbo 250KM'}</p>
        <p className={styles.summaryPrice}>{(selectedCarModel?.basePrice || 120000).toLocaleString()} zł</p>
      </div>
      
      {/* Silnik z danych API */}
      {selectedEngine && (
        <div className={styles.summarySection}>
          <h3>Silnik</h3>
          <p>{selectedEngine.engineName}</p>
          <p className={styles.summaryPrice}>+{selectedEngine.additionalPrice?.toLocaleString()} zł</p>
        </div>
      )}
      
      {/* Pozostałe sekcje bez zmian */}
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
      
      {/* Nowa sekcja akcesorii */}
      {selectedAccessories && selectedAccessories.length > 0 && (
        <div className={styles.summarySection}>
          <h3>Akcesoria</h3>
          {selectedAccessories.map((accessory) => (
            <div key={accessory.id} className={styles.accessoryItem}>
              <p>{accessory.name}</p>
              <p className={styles.summaryPrice}>+{accessory.price?.toLocaleString() || 0} zł</p>
            </div>
          ))}
        </div>
      )}
      
      {/* Sekcja wyposażenia wnętrza */}
      {selectedInteriorEquipment && selectedInteriorEquipment.length > 0 && (
        <div className={styles.summarySection}>
          <h3>Wyposażenie wnętrza</h3>
          {selectedInteriorEquipment.map((equipment) => (
            <div key={equipment.id} className={styles.summaryItem}>
              <div>
                <p className={styles.summaryItemName}>{equipment.value}</p>
                <p className={styles.summaryItemDescription}>{equipment.description}</p>
              </div>
              <p className={styles.summaryPrice}>+{equipment.additionalPrice?.toLocaleString()} zł</p>
            </div>
          ))}
        </div>
      )}
      
      <div className={styles.priceDivider}>
        <div className={styles.totalPrice}>
          <span>Cena łączna</span>
          <span>{(totalPrice && !isNaN(totalPrice) ? totalPrice : 0).toLocaleString()} zł</span>
        </div>
        <div className={styles.vatInfo}>
          Cena zawiera VAT (23%)
        </div>
      </div>
      <div className={styles.buttonGroup}>
        <button 
          className={`${styles.actionButton} ${styles.save}`}
          onClick={onSaveConfiguration}
        >
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
