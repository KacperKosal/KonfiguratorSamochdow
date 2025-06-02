import React from 'react';
import styles from './ConfigOptions.module.css';

const ConfigOptions = ({
  activeTab,
  exteriorColors,
  wheelTypes,
  interiorColors,
  accessories,
  selectedAccessories,
  onAccessoryToggle,
  interiorEquipment,
  selectedInteriorEquipment,
  onInteriorEquipmentToggle,
  carColor,
  setCarColor,
  wheelType,
  setWheelType,
  interiorColor,
  setInteriorColor,
}) => {
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
    return (
      <div className={styles.configGrid}>
        {wheelTypes.map((wheel) => (
          <div
            key={wheel.value}
            onClick={() => setWheelType(wheel.value)}
            className={`${styles.configItem} ${wheelType === wheel.value ? styles.selected : ''}`}
          >
            <div>
              <img
                src={wheel.image}
                alt={wheel.name}
                className={styles.wheelImage}
              />
            </div>
            <div>{wheel.name}</div>
            <div className={styles.priceText}>
              +{wheel.price.toLocaleString()} zł
            </div>
          </div>
        ))}
      </div>
    );
  }

  // Wnętrze - rozszerzone o wyposażenie
  if (activeTab === 'interior') {
    return (
      <div className={styles.configContainer}>
        {/* Sekcja kolorów wnętrza */}
        <div className={styles.sectionTitle}>Kolory wnętrza</div>
        <div className={styles.configGrid}>
          {interiorColors.map((color) => (
            <div
              key={color.value}
              onClick={() => setInteriorColor(color.value)}
              className={`${styles.configItem} ${interiorColor === color.value ? styles.selected : ''}`}
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
                <h4 className={styles.categoryTitle}>{type}</h4>
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
    
    if (!accessories || accessories.length === 0) {
      return (
        <div className={styles.textCenter}>
          <p>Brak dostępnych akcesorii dla tego modelu.</p>
          <p style={{fontSize: '12px', color: '#666', marginTop: '8px'}}>
            Debug: accessories = {JSON.stringify(accessories)}
          </p>
        </div>
      );
    }

    // Grupowanie akcesorii według kategorii
    const accessoriesByCategory = accessories.reduce((groups, accessory) => {
      const category = accessory.category || 'Inne';
      if (!groups[category]) {
        groups[category] = [];
      }
      groups[category].push(accessory);
      return groups;
    }, {});

    return (
      <div className={styles.accessoriesContainer}>
        {Object.entries(accessoriesByCategory).map(([category, categoryAccessories]) => (
          <div key={category} className={styles.categorySection}>
            <h3 className={styles.categoryTitle}>{category}</h3>
            <div className={styles.configGrid}>
              {categoryAccessories.map((accessory) => (
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
