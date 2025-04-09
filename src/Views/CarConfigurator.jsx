import React, { useState } from 'react';
import { useParams } from 'react-router-dom';
import CarViewer from '../components/CarConfigurator/CarViewer/CarViewer';
import ConfigTabs from '../components/CarConfigurator/ConfigTabs/ConfigTabs';
import ConfigOptions from '../components/CarConfigurator/ConfigOptions/ConfigOptions';
import SummaryPanel from '../components/CarConfigurator/SummaryPanel/SummaryPanel';

const CarConfiguratorView = () => {
  const { modelId } = useParams();
  const [activeTab, setActiveTab] = useState('exterior');
  const [carColor, setCarColor] = useState('red');
  const [wheelType, setWheelType] = useState('standard');
  const [interiorColor, setInteriorColor] = useState('black');
  const [currentRotation, setCurrentRotation] = useState(0);

  const exteriorColors = [
    { name: 'Czerwony Metalik', value: 'red', price: 0 },
    { name: 'Srebrny Metalik', value: 'silver', price: 2000 },
    { name: 'Niebieski Metalik', value: 'blue', price: 2500 },
    { name: 'Czarny Perłowy', value: 'black', price: 3000 },
    { name: 'Biały Perłowy', value: 'white', price: 2800 }
  ];

  const wheelTypes = [
    { name: 'Standardowe 17"', value: 'standard', price: 0 },
    { name: 'Aluminiowe 18"', value: 'aluminum', price: 3500 },
    { name: 'Sportowe 19"', value: 'sport', price: 5000 }
  ];

  const interiorColors = [
    { name: 'Czarna skóra', value: 'black', price: 0 },
    { name: 'Beżowa skóra', value: 'beige', price: 1500 },
    { name: 'Brązowa skóra', value: 'brown', price: 2000 }
  ];

  const rotateLeft = () => {
    setCurrentRotation((prev) => (prev - 45) % 360);
  };

  const rotateRight = () => {
    setCurrentRotation((prev) => (prev + 45) % 360);
  };

  const calculateTotalPrice = () => {
    const basePrice = 120000;
    const selectedColor = exteriorColors.find(c => c.value === carColor);
    const selectedWheels = wheelTypes.find(w => w.value === wheelType);
    const selectedInterior = interiorColors.find(i => i.value === interiorColor);

    return basePrice + selectedColor.price + selectedWheels.price + selectedInterior.price;
  };

  return (
    <div className="car-configurator-page">
      <main className="main-container">
        <h1 className="title">Konfigurator modelu {modelId}</h1>
        <div className="configurator-wrapper">
          <div className="left-panel">
            <CarViewer
              currentRotation={currentRotation}
              rotateLeft={rotateLeft}
              rotateRight={rotateRight}
            />

            <div className="toolbar">
              <button className="toolbar-button">Obróć model</button>
              <button className="toolbar-button">Zrzut ekranu</button>
              <button className="toolbar-button">Pobierz konfigurację</button>
            </div>

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

          <div className="right-panel">
            <SummaryPanel
              exteriorColors={exteriorColors}
              wheelTypes={wheelTypes}
              interiorColors={interiorColors}
              carColor={carColor}
              wheelType={wheelType}
              interiorColor={interiorColor}
              totalPrice={calculateTotalPrice()}
            />
          </div>
        </div>
      </main>
    </div>
  );
};

export default CarConfiguratorView; 