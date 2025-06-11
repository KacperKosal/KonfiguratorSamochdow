import React from 'react';
import { ShoppingCart } from 'lucide-react';
import styles from './SummaryPanel.module.css';

const SummaryPanel = ({
  selectedCarModel,
  selectedEngine,
  exteriorColors,
  selectedAccessories,
  selectedInteriorEquipment,
  carColor,
  totalPrice,
  onSaveConfiguration
}) => {
  const selectedExterior = exteriorColors.find(c => c.value === carColor);

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
        <div className={styles.colorItem}>
          <div 
            className={styles.colorSquare}
            style={{ backgroundColor: selectedExterior?.value || carColor }}
          ></div>
          <p>{selectedExterior?.name}</p>
        </div>
        <p className={styles.summaryPrice}>+{selectedExterior?.price.toLocaleString()} zł</p>
      </div>
      {/* Sekcja felg */}
      {(() => {
        const selectedWheels = selectedAccessories ? selectedAccessories.filter(acc => acc.type === 'AlloyWheels') : [];
        return selectedWheels.length > 0 && (
          <div className={styles.summarySection}>
            <h3>Felgi</h3>
            {selectedWheels.map((wheel) => (
              <div key={wheel.id} className={styles.accessoryItem}>
                <div>
                  <p>{wheel.name}</p>
                  {wheel.size && <p className={styles.wheelDetails}>Rozmiar: {wheel.size}"</p>}
                  {wheel.pattern && <p className={styles.wheelDetails}>Wzór: {wheel.pattern}</p>}
                </div>
                <p className={styles.summaryPrice}>+{wheel.price?.toLocaleString() || 0} zł</p>
              </div>
            ))}
          </div>
        );
      })()}
      
      
      {/* Sekcja akcesoriów (bez felg) */}
      {(() => {
        const nonWheelAccessories = selectedAccessories ? selectedAccessories.filter(acc => acc.type !== 'AlloyWheels') : [];
        return nonWheelAccessories.length > 0 && (
          <div className={styles.summarySection}>
            <h3>Akcesoria</h3>
            {nonWheelAccessories.map((accessory) => (
              <div key={accessory.id} className={styles.accessoryItem}>
                <p>{accessory.name}</p>
                <p className={styles.summaryPrice}>+{accessory.price?.toLocaleString() || 0} zł</p>
              </div>
            ))}
          </div>
        );
      })()}
      
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
      </div>
    </div>
  );
};

export default SummaryPanel;
