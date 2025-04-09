import React, { useState } from 'react';
import styles from './CarConfigurator.module.css';
import CarViewer from '../../components/CarConfigurator/CarViewer/CarViewer';
import ConfigTabs from '../../components/CarConfigurator/ConfigTabs/ConfigTabs';
import ConfigOptions from '../../components/CarConfigurator/ConfigOptions/ConfigOptions';
import SummaryPanel from '../../components/CarConfigurator/SummaryPanel/SummaryPanel';

const CarConfigurator = () => {
  const [activeTab, setActiveTab] = useState('exterior');
  const [carColor, setCarColor] = useState('#000000');
  const [wheelType, setWheelType] = useState('standard');
  const [interiorColor, setInteriorColor] = useState('#ffffff');
  const [currentRotation, setCurrentRotation] = useState(0);

  const exteriorColors = [
    { name: 'Czarny', value: '#000000', price: 0 },
    { name: 'Biały', value: '#ffffff', price: 0 },
    { name: 'Srebrny', value: '#c0c0c0', price: 2000 },
    { name: 'Niebieski', value: '#0000ff', price: 3000 },
    { name: 'Czerwony', value: '#ff0000', price: 3000 },
  ];

  const wheelTypes = [
    { name: 'Standardowe', value: 'standard', image: '/api/placeholder/100/100', price: 0 },
    { name: 'Sportowe', value: 'sport', image: '/api/placeholder/100/100', price: 5000 },
    { name: 'Premium', value: 'premium', image: '/api/placeholder/100/100', price: 8000 },
  ];

  const interiorColors = [
    { name: 'Czarny', value: '#000000', price: 0 },
    { name: 'Beżowy', value: '#f5f5dc', price: 2000 },
    { name: 'Brązowy', value: '#8b4513', price: 2000 },
  ];

  const rotateLeft = () => {
    setCurrentRotation((prev) => (prev - 45 + 360) % 360);
  };

  const rotateRight = () => {
    setCurrentRotation((prev) => (prev + 45) % 360);
  };

  const totalPrice = 120000 + 
    exteriorColors.find(c => c.value === carColor)?.price +
    wheelTypes.find(w => w.value === wheelType)?.price +
    interiorColors.find(i => i.value === interiorColor)?.price;

  return (
    <div className={styles.configurator}>
      <h1 className={styles.configuratorTitle}>Konfigurator samochodu</h1>
      <div className={styles.configuratorContent}>
        <div className={styles.configuratorLeftPanel}>
          <CarViewer
            currentRotation={currentRotation}
            rotateLeft={rotateLeft}
            rotateRight={rotateRight}
          />
          <ConfigTabs activeTab={activeTab} setActiveTab={setActiveTab} />
          <ConfigOptions
            activeTab={activeTab}
            exteriorColors={exteriorColors}
            wheelTypes={wheelTypes}
            interiorColors={interiorColors}
            carColor={carColor}
            setCarColor={setCarColor}
            wheelType={wheelType}
            setWheelType={setWheelType}
            interiorColor={interiorColor}
            setInteriorColor={setInteriorColor}
          />
        </div>
        <div className={styles.configuratorRightPanel}>
          <SummaryPanel
            exteriorColors={exteriorColors}
            wheelTypes={wheelTypes}
            interiorColors={interiorColors}
            carColor={carColor}
            wheelType={wheelType}
            interiorColor={interiorColor}
            totalPrice={totalPrice}
          />
        </div>
      </div>
    </div>
  );
};

export default CarConfigurator; 