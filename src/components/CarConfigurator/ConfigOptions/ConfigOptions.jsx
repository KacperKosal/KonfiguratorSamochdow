import React from 'react';
import styles from './ConfigOptions.module.css';

const ConfigOptions = ({
  activeTab,
  exteriorColors,
  wheelTypes,
  interiorColors,
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

  // Wnętrze
  if (activeTab === 'interior') {
    return (
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
    );
  }

  // Akcesoria
  return (
    <div className={styles.textCenter}>
      <p>Wkrótce dostępne akcesoria...</p>
    </div>
  );
};

export default ConfigOptions;
