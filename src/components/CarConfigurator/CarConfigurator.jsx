import React, { useState } from 'react';
import CarViewer from './CarViewer';

const CarConfigurator = () => {
  // Stany konfiguracji
  const [currentRotation, setCurrentRotation] = useState(0);

  const rotateLeft = () => {
    setCurrentRotation((prev) => (prev - 45) % 360);
  };

  const rotateRight = () => {
    setCurrentRotation((prev) => (prev + 45) % 360);
  };

  return (
    <div style={{ display: 'flex', flexDirection: 'column', minHeight: '100vh' }}>

      <main className="main-container">
        <h1 className="title">Konfigurator modelu X-Drive GT</h1>
        <div className="configurator-wrapper">
          {/* Lewa kolumna: Wizualizacja i opcje konfiguracji */}
          <div className="left-panel">
            <CarViewer
              currentRotation={currentRotation}
              rotateLeft={rotateLeft}
              rotateRight={rotateRight}
            />

            {/* Przyciski wizualizacji */}
            <div className="toolbar">
              <button className="toolbar-button">Obróć model</button>
              <button className="toolbar-button">Zrzut ekranu</button>
              <button className="toolbar-button">Pobierz konfigurację</button>
            </div>
          </div>

          {/* Prawa kolumna: Podsumowanie konfiguracji */}
        </div>
      </main>

    </div>
  );
};

export default CarConfigurator;
