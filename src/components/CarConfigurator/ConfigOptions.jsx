import React from 'react';

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
      <div className="config-grid">
        {exteriorColors.map((color) => (
          <div
            key={color.value}
            onClick={() => setCarColor(color.value)}
            className={`config-item ${carColor === color.value ? 'selected' : ''}`}
          >
            <div
              className="color-box"
              style={{ backgroundColor: color.value }}
            />
            <div>{color.name}</div>
            <div style={{ fontSize: '14px', color: '#666' }}>
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
      <div className="config-grid">
        {wheelTypes.map((wheel) => (
          <div
            key={wheel.value}
            onClick={() => setWheelType(wheel.value)}
            className={`config-item ${wheelType === wheel.value ? 'selected' : ''}`}
          >
            <div>
              <img
                src="/api/placeholder/100/100"
                alt={wheel.name}
                className="wheel-image"
              />
            </div>
            <div>{wheel.name}</div>
            <div style={{ fontSize: '14px', color: '#666' }}>
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
      <div className="config-grid">
        {interiorColors.map((interior) => (
          <div
            key={interior.value}
            onClick={() => setInteriorColor(interior.value)}
            className={`config-item ${interiorColor === interior.value ? 'selected' : ''}`}
          >
            <div
              className="color-box"
              style={{ backgroundColor: interior.value }}
            />
            <div>{interior.name}</div>
            <div style={{ fontSize: '14px', color: '#666' }}>
              +{interior.price.toLocaleString()} zł
            </div>
          </div>
        ))}
      </div>
    );
  }

  // Akcesoria
  if (activeTab === 'accessories') {
    return (
      <div className="text-center">
        Wkrótce dostępne nowe akcesoria...
      </div>
    );
  }

  return null;
};

export default ConfigOptions;
