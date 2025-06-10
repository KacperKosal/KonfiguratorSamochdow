import React from 'react';
import styles from './ConfigOptions.module.css';
import { translateInteriorEquipmentType, translateAccessoryCategory } from '../../../utils/translations';

const ConfigOptions = ({
  activeTab,
  exteriorColors,
  accessories,
  selectedAccessories,
  onAccessoryToggle,
  interiorEquipment,
  selectedInteriorEquipment,
  onInteriorEquipmentToggle,
  carColor,
  setCarColor,
  engines,
  selectedEngine,
  onEngineChange,
}) => {
  // Silnik
  if (activeTab === 'engine') {
    if (!engines || engines.length === 0) {
      return (
        <div className={styles.textCenter}>
          <p>Brak dostępnych silników dla tego modelu.</p>
        </div>
      );
    }

    return (
      <div className={styles.configGrid}>
        {engines.map((engine) => (
          <div
            key={engine.engineId}
            onClick={() => onEngineChange(engine)}
            className={`${styles.configItem} ${styles.engineItem} ${
              selectedEngine?.engineId === engine.engineId ? styles.selected : ''
            }`}
          >
            <div className={styles.engineIcon}>🔧</div>
            <div className={styles.engineInfo}>
              <div className={styles.engineName}>{engine.engineName}</div>
              <div className={styles.engineDetails}>
                <div>Moc: {engine.power} KM</div>
                <div>Moment obrotowy: {engine.torque} Nm</div>
                <div>Typ: {engine.fuelType}</div>
                {engine.displacement && (
                  <div>Pojemność: {engine.displacement}L</div>
                )}
              </div>
              <div className={styles.priceText}>
                +{engine.additionalPrice?.toLocaleString() || 0} zł
              </div>
            </div>
            {selectedEngine?.engineId === engine.engineId && (
              <div className={styles.selectedIndicator}>✓</div>
            )}
          </div>
        ))}
      </div>
    );
  }

  // Nadwozie
  if (activeTab === 'exterior') {
    return (
      <div className={styles.configGrid}>
        {exteriorColors.map((color) => (
          <div
            key={color.value}
            onClick={() => setCarColor(color.value)}
            className={`${styles.configItem} ${carColor === color.value ? styles.selected : ''}`}
          >
            <div
              className={styles.colorBox}
              style={{ backgroundColor: color.value }}
            />
            <div>{color.name}</div>
            <div className={styles.priceText}>
              +{color.price.toLocaleString()} zł
            </div>
          </div>
        ))}
      </div>
    );
  }

  // Felgi
  if (activeTab === 'wheels') {
    // Filter accessories to get only AlloyWheels
    const wheelAccessories = accessories ? accessories.filter(acc => acc.type === 'AlloyWheels') : [];
    
    if (!wheelAccessories || wheelAccessories.length === 0) {
      return (
        <div className={styles.textCenter}>
          <p>Brak dostępnych felg dla tego modelu.</p>
          <p style={{fontSize: '12px', color: '#666', marginTop: '8px'}}>
            Dodaj felgi w panelu administratora, aby wyświetlić je tutaj.
          </p>
        </div>
      );
    }
    
    return (
      <div className={styles.configContainer}>
        {/* Wheel accessories section */}
        <div className={styles.sectionTitle}>Dostępne felgi</div>
        <div className={styles.configGrid}>
          {wheelAccessories.map((wheelAcc) => (
            <div
              key={wheelAcc.id}
              onClick={() => onAccessoryToggle(wheelAcc.id)}
              className={`${styles.configItem} ${styles.accessoryItem} ${
                wheelAcc.selected ? styles.selected : ''
              } ${!wheelAcc.isInStock ? styles.outOfStock : ''}`}
            >
              {wheelAcc.imageUrl && (
                <div className={styles.accessoryImageContainer}>
                  <img
                    src={wheelAcc.imageUrl}
                    alt={wheelAcc.name}
                    className={styles.wheelImage}
                    onError={(e) => {
                      e.target.style.display = 'none';
                    }}
                  />
                </div>
              )}
              <div className={styles.accessoryInfo}>
                <div className={styles.accessoryName}>{wheelAcc.name}</div>
                {wheelAcc.size && (
                  <div className={styles.wheelSize}>Rozmiar: {wheelAcc.size}"</div>
                )}
                {wheelAcc.pattern && (
                  <div className={styles.wheelPattern}>Wzór: {wheelAcc.pattern}</div>
                )}
                {wheelAcc.description && (
                  <div className={styles.accessoryDescription}>
                    {wheelAcc.description}
                  </div>
                )}
                <div className={styles.accessoryDetails}>
                  {wheelAcc.manufacturer && (
                    <div className={styles.manufacturer}>
                      {wheelAcc.manufacturer}
                    </div>
                  )}
                  {wheelAcc.isOriginalBMWPart && (
                    <div className={styles.originalPart}>Oryginalna część BMW</div>
                  )}
                </div>
                <div className={styles.priceText}>
                  +{wheelAcc.price?.toLocaleString() || 0} zł
                </div>
                {!wheelAcc.isInStock && (
                  <div className={styles.stockStatus}>Brak w magazynie</div>
                )}
              </div>
              {wheelAcc.selected && (
                <div className={styles.selectedIndicator}>✓</div>
              )}
            </div>
          ))}
        </div>
      </div>
    );
  }

  // Wnętrze - wyposażenie wnętrza
  if (activeTab === 'interior') {
    return (
      <div className={styles.configContainer}>
        {/* Sekcja wyposażenia wnętrza */}
        <div className={styles.sectionTitle}>Wyposażenie wnętrza</div>
        {!interiorEquipment || interiorEquipment.length === 0 ? (
          <div className={styles.textCenter}>
            <p>Brak dostępnego wyposażenia wnętrza dla tego modelu.</p>
          </div>
        ) : (
          <div className={styles.equipmentContainer}>
            {/* Grupowanie wyposażenia według typu */}
            {Object.entries(
              interiorEquipment.reduce((groups, equipment) => {
                const type = equipment.type || 'Inne';
                if (!groups[type]) {
                  groups[type] = [];
                }
                groups[type].push(equipment);
                return groups;
              }, {})
            ).map(([type, equipmentList]) => (
              <div key={type} className={styles.equipmentCategory}>
                <h4 className={styles.categoryTitle}>{translateInteriorEquipmentType(type)}</h4>
                <div className={styles.equipmentGrid}>
                  {equipmentList.map((equipment) => {
                    const isSelected = selectedInteriorEquipment.some(eq => eq.id === equipment.id);
                    return (
                      <div
                        key={equipment.id}
                        onClick={() => onInteriorEquipmentToggle(equipment.id)}
                        className={`${styles.equipmentItem} ${isSelected ? styles.selected : ''}`}
                      >
                        <div className={styles.equipmentInfo}>
                          <h5 className={styles.equipmentName}>{equipment.value}</h5>
                          <p className={styles.equipmentDescription}>{equipment.description}</p>
                          <div className={styles.equipmentFeatures}>
                            {equipment.hasNavigation && (
                              <span className={styles.feature}>📍 Nawigacja</span>
                            )}
                            {equipment.hasPremiumSound && (
                              <span className={styles.feature}>🔊 Premium Audio</span>
                            )}
                            {equipment.controlType && (
                              <span className={styles.feature}>⚙️ {equipment.controlType}</span>
                            )}
                          </div>
                          <div className={styles.equipmentPrice}>
                            +{equipment.additionalPrice?.toLocaleString()} zł
                          </div>
                        </div>
                        {isSelected && (
                          <div className={styles.selectedIndicator}>✓</div>
                        )}
                      </div>
                    );
                  })}
                </div>
              </div>
            ))}
          </div>
        )}
      </div>
    );
  }

  // Akcesoria
  if (activeTab === 'accessories') {
    console.log('Renderowanie akcesorii:', accessories);
    console.log('Liczba akcesorii:', accessories?.length);
    
    // Filter out AlloyWheels accessories (they are shown in wheels tab)
    const nonWheelAccessories = accessories ? accessories.filter(acc => acc.type !== 'AlloyWheels') : [];
    
    if (!nonWheelAccessories || nonWheelAccessories.length === 0) {
      return (
        <div className={styles.textCenter}>
          <p>Brak dostępnych akcesorii dla tego modelu.</p>
          <p style={{fontSize: '12px', color: '#666', marginTop: '8px'}}>
            Debug: total accessories = {accessories?.length}, non-wheel accessories = {nonWheelAccessories.length}
          </p>
        </div>
      );
    }

    // Grupowanie akcesorii według typu (bez felg) - jeden wybór na typ
    const accessoriesByType = nonWheelAccessories.reduce((groups, accessory) => {
      const type = accessory.type || 'Inne';
      if (!groups[type]) {
        groups[type] = [];
      }
      groups[type].push(accessory);
      return groups;
    }, {});

    return (
      <div className={styles.accessoriesContainer}>
        {Object.entries(accessoriesByType).map(([type, typeAccessories]) => (
          <div key={type} className={styles.categorySection}>
            <h3 className={styles.categoryTitle}>{type}</h3>
            <div className={styles.configGrid}>
              {typeAccessories.map((accessory) => (
                <div
                  key={accessory.id}
                  onClick={() => onAccessoryToggle(accessory.id)}
                  className={`${styles.configItem} ${styles.accessoryItem} ${
                    accessory.selected ? styles.selected : ''
                  } ${!accessory.isInStock ? styles.outOfStock : ''}`}
                >
                  {accessory.imageUrl && (
                    <div className={styles.accessoryImageContainer}>
                      <img
                        src={accessory.imageUrl}
                        alt={accessory.name}
                        className={styles.accessoryImage}
                        onError={(e) => {
                          e.target.style.display = 'none';
                        }}
                      />
                    </div>
                  )}
                  <div className={styles.accessoryInfo}>
                    <div className={styles.accessoryName}>{accessory.name}</div>
                    {accessory.description && (
                      <div className={styles.accessoryDescription}>
                        {accessory.description}
                      </div>
                    )}
                    <div className={styles.accessoryDetails}>
                      {accessory.manufacturer && (
                        <div className={styles.manufacturer}>
                          {accessory.manufacturer}
                        </div>
                      )}
                      {accessory.isOriginalBMWPart && (
                        <div className={styles.originalPart}>Oryginalna część BMW</div>
                      )}
                    </div>
                    <div className={styles.priceText}>
                      +{accessory.price?.toLocaleString() || 0} zł
                    </div>
                    {!accessory.isInStock && (
                      <div className={styles.stockStatus}>Brak w magazynie</div>
                    )}
                  </div>
                  {accessory.selected && (
                    <div className={styles.selectedIndicator}>✓</div>
                  )}
                </div>
              ))}
            </div>
          </div>
        ))}
      </div>
    );
  }

  return (
    <div className={styles.textCenter}>
      <p>Wybierz kategorię konfiguracji.</p>
    </div>
  );
};

export default ConfigOptions;
