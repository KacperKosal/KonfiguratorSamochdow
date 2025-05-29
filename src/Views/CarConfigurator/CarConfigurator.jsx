import React, { useState, useEffect } from 'react';
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
  
  // Nowe stany dla danych z API
  const [carModels, setCarModels] = useState([]);
  const [selectedCarModel, setSelectedCarModel] = useState(null);
  const [engines, setEngines] = useState([]);
  const [selectedEngine, setSelectedEngine] = useState(null);
  const [accessories, setAccessories] = useState([]);
  const [interiorEquipment, setInteriorEquipment] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  // Konfiguracja API
  const apiUrl = import.meta.env.VITE_API_URL || 'https://localhost:7001';

  // Pobieranie danych z API
  useEffect(() => {
    const fetchData = async () => {
      try {
        setLoading(true);
        
        // Pobierz modele samochodów
        const carModelsResponse = await fetch(`${apiUrl}/api/car-models`);
        if (!carModelsResponse.ok) throw new Error('Błąd pobierania modeli samochodów');
        const carModelsData = await carModelsResponse.json();
        setCarModels(carModelsData);
        
        // Ustaw pierwszy model jako domyślny
        if (carModelsData.length > 0) {
          setSelectedCarModel(carModelsData[0]);
          
          // Pobierz silniki dla wybranego modelu
          const enginesResponse = await fetch(`${apiUrl}/api/car-models/${carModelsData[0].id}/engines`);
          if (enginesResponse.ok) {
            const enginesData = await enginesResponse.json();
            setEngines(enginesData);
            if (enginesData.length > 0) {
              setSelectedEngine(enginesData[0]);
            }
          }
        }
        
        // Pobierz wszystkie silniki
        const allEnginesResponse = await fetch(`${apiUrl}/api/engines`);
        if (allEnginesResponse.ok) {
          const allEnginesData = await allEnginesResponse.json();
          // Możesz użyć tych danych do wyświetlenia opcji silników
        }
        
      } catch (err) {
        setError(err.message);
        console.error('Błąd pobierania danych:', err);
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, [apiUrl]);

  // Funkcja do zmiany modelu samochodu
  const handleCarModelChange = async (modelId) => {
    try {
      const selectedModel = carModels.find(model => model.id === modelId);
      setSelectedCarModel(selectedModel);
      
      // Pobierz silniki dla nowego modelu
      const enginesResponse = await fetch(`${apiUrl}/api/car-models/${modelId}/engines`);
      if (enginesResponse.ok) {
        const enginesData = await enginesResponse.json();
        setEngines(enginesData);
        setSelectedEngine(enginesData.length > 0 ? enginesData[0] : null);
      }
    } catch (err) {
      console.error('Błąd zmiany modelu:', err);
    }
  };

  // Funkcja do zapisania konfiguracji
  const saveConfiguration = async () => {
    if (!selectedCarModel) return;
    
    try {
      const configurationData = {
        carModelId: selectedCarModel.id,
        engineId: selectedEngine?.engineId,
        exteriorColor: carColor,
        wheelType: wheelType,
        interiorColor: interiorColor,
        accessories: accessories.filter(acc => acc.selected).map(acc => acc.id),
        interiorEquipment: interiorEquipment.filter(eq => eq.selected).map(eq => eq.id)
      };
      
      const response = await fetch(`${apiUrl}/api/car-configurations/${selectedCarModel.id}`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(configurationData)
      });
      
      if (response.ok) {
        alert('Konfiguracja została zapisana!');
      } else {
        throw new Error('Błąd zapisywania konfiguracji');
      }
    } catch (err) {
      console.error('Błąd zapisywania konfiguracji:', err);
      alert('Wystąpił błąd podczas zapisywania konfiguracji');
    }
  };

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

  // Obliczanie ceny z uwzględnieniem danych z API
  const basePrice = selectedCarModel?.basePrice || 120000;
  const enginePrice = selectedEngine?.additionalPrice || 0;
  const totalPrice = basePrice + enginePrice +
    exteriorColors.find(c => c.value === carColor)?.price +
    wheelTypes.find(w => w.value === wheelType)?.price +
    interiorColors.find(i => i.value === interiorColor)?.price;

  if (loading) {
    return (
      <div className={styles.configurator}>
        <div className={styles.loading}>Ładowanie konfiguracji...</div>
      </div>
    );
  }

  if (error) {
    return (
      <div className={styles.configurator}>
        <div className={styles.error}>Błąd: {error}</div>
      </div>
    );
  }

  return (
    <div className={styles.configurator}>
      <h1 className={styles.configuratorTitle}>Konfigurator samochodu</h1>
      
      {/* Selektor modelu samochodu */}
      {carModels.length > 0 && (
        <div className={styles.modelSelector}>
          <label htmlFor="carModel">Wybierz model:</label>
          <select 
            id="carModel"
            value={selectedCarModel?.id || ''}
            onChange={(e) => handleCarModelChange(e.target.value)}
            className={styles.modelSelect}
          >
            {carModels.map(model => (
              <option key={model.id} value={model.id}>
                {model.name} - {model.basePrice?.toLocaleString()} zł
              </option>
            ))}
          </select>
        </div>
      )}
      
      {/* Selektor silnika */}
      {engines.length > 0 && (
        <div className={styles.engineSelector}>
          <label htmlFor="engine">Wybierz silnik:</label>
          <select 
            id="engine"
            value={selectedEngine?.engineId || ''}
            onChange={(e) => {
              const engine = engines.find(eng => eng.engineId === e.target.value);
              setSelectedEngine(engine);
            }}
            className={styles.engineSelect}
          >
            {engines.map(engine => (
              <option key={engine.engineId} value={engine.engineId}>
                {engine.engineName} - +{engine.additionalPrice?.toLocaleString()} zł
              </option>
            ))}
          </select>
        </div>
      )}
      
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
            selectedCarModel={selectedCarModel}
            selectedEngine={selectedEngine}
            exteriorColors={exteriorColors}
            wheelTypes={wheelTypes}
            interiorColors={interiorColors}
            carColor={carColor}
            wheelType={wheelType}
            interiorColor={interiorColor}
            totalPrice={totalPrice}
            onSaveConfiguration={saveConfiguration}
          />
        </div>
      </div>
    </div>
  );
};

export default CarConfigurator;