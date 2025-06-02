import React, { useState, useEffect, useMemo } from 'react';
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
  const [selectedAccessories, setSelectedAccessories] = useState([]);
  const [interiorEquipment, setInteriorEquipment] = useState([]);
  const [selectedInteriorEquipment, setSelectedInteriorEquipment] = useState([]);
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
          
          // Pobierz akcesoria - spróbuj różnych endpointów
          console.log('Próba pobrania akcesorii dla modelu:', carModelsData[0].name);
          
          // Najpierw spróbuj dla konkretnego modelu
          let accessoriesData = [];
          try {
            const accessoriesResponse = await fetch(`${apiUrl}/api/car-accessories/model/${encodeURIComponent(carModelsData[0].name)}`);
            if (accessoriesResponse.ok) {
              accessoriesData = await accessoriesResponse.json();
              console.log('Akcesoria dla modelu:', accessoriesData);
            }
          } catch (err) {
            console.log('Błąd pobierania akcesorii dla modelu:', err);
          }
          
          // Jeśli brak akcesorii dla modelu, pobierz wszystkie
          if (accessoriesData.length === 0) {
            try {
              const allAccessoriesResponse = await fetch(`${apiUrl}/api/car-accessories`);
              if (allAccessoriesResponse.ok) {
                const allAccessoriesData = await allAccessoriesResponse.json();
                console.log('Wszystkie akcesoria:', allAccessoriesData);
                accessoriesData = allAccessoriesData;
              }
            } catch (err) {
              console.log('Błąd pobierania wszystkich akcesorii:', err);
            }
          }
          
          // Jeśli nadal brak danych, użyj przykładowych akcesorii
          if (accessoriesData.length === 0) {
            accessoriesData = [
              {
                id: '1',
                name: 'Dywaniki gumowe',
                description: 'Wysokiej jakości dywaniki gumowe dopasowane do modelu',
                price: 299,
                category: 'Wnętrze',
                manufacturer: 'BMW',
                isOriginalBMWPart: true,
                isInStock: true,
                imageUrl: null
              },
              {
                id: '2',
                name: 'Bagażnik dachowy',
                description: 'Uniwersalny bagażnik dachowy o pojemności 400L',
                price: 1299,
                category: 'Zewnętrzne',
                manufacturer: 'Thule',
                isOriginalBMWPart: false,
                isInStock: true,
                imageUrl: null
              },
              {
                id: '3',
                name: 'Zestaw zimowy',
                description: 'Kompletny zestaw opon zimowych z felgami',
                price: 2499,
                category: 'Koła',
                manufacturer: 'BMW',
                isOriginalBMWPart: true,
                isInStock: false,
                imageUrl: null
              },
              {
                id: '4',
                name: 'System nawigacji',
                description: 'Zaawansowany system nawigacji z mapami Europy',
                price: 1899,
                category: 'Elektronika',
                manufacturer: 'BMW',
                isOriginalBMWPart: true,
                isInStock: true,
                imageUrl: null
              },
              {
                id: '5',
                name: 'Foteliki dziecięce',
                description: 'Bezpieczne foteliki dla dzieci 9-36kg',
                price: 899,
                category: 'Bezpieczeństwo',
                manufacturer: 'BMW',
                isOriginalBMWPart: true,
                isInStock: true,
                imageUrl: null
              }
            ];
            console.log('Używam przykładowych akcesorii');
          }
          
          setAccessories(accessoriesData.map(acc => ({ ...acc, selected: false })));
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

  // W useEffect, dodaj pobieranie wyposażenia wnętrza:
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
          
          // Pobierz akcesoria - spróbuj różnych endpointów
          console.log('Próba pobrania akcesorii dla modelu:', carModelsData[0].name);
          
          // Najpierw spróbuj dla konkretnego modelu
          let accessoriesData = [];
          try {
            const accessoriesResponse = await fetch(`${apiUrl}/api/car-accessories/model/${encodeURIComponent(carModelsData[0].name)}`);
            if (accessoriesResponse.ok) {
              accessoriesData = await accessoriesResponse.json();
              console.log('Akcesoria dla modelu:', accessoriesData);
            }
          } catch (err) {
            console.log('Błąd pobierania akcesorii dla modelu:', err);
          }
          
          // Jeśli brak akcesorii dla modelu, pobierz wszystkie
          if (accessoriesData.length === 0) {
            try {
              const allAccessoriesResponse = await fetch(`${apiUrl}/api/car-accessories`);
              if (allAccessoriesResponse.ok) {
                const allAccessoriesData = await allAccessoriesResponse.json();
                console.log('Wszystkie akcesoria:', allAccessoriesData);
                accessoriesData = allAccessoriesData;
              }
            } catch (err) {
              console.log('Błąd pobierania wszystkich akcesorii:', err);
            }
          }
          
          // Jeśli nadal brak danych, użyj przykładowych akcesorii
          if (accessoriesData.length === 0) {
            accessoriesData = [
              {
                id: '1',
                name: 'Dywaniki gumowe',
                description: 'Wysokiej jakości dywaniki gumowe dopasowane do modelu',
                price: 299,
                category: 'Wnętrze',
                manufacturer: 'BMW',
                isOriginalBMWPart: true,
                isInStock: true,
                imageUrl: null
              },
              {
                id: '2',
                name: 'Bagażnik dachowy',
                description: 'Uniwersalny bagażnik dachowy o pojemności 400L',
                price: 1299,
                category: 'Zewnętrzne',
                manufacturer: 'Thule',
                isOriginalBMWPart: false,
                isInStock: true,
                imageUrl: null
              },
              {
                id: '3',
                name: 'Zestaw zimowy',
                description: 'Kompletny zestaw opon zimowych z felgami',
                price: 2499,
                category: 'Koła',
                manufacturer: 'BMW',
                isOriginalBMWPart: true,
                isInStock: false,
                imageUrl: null
              },
              {
                id: '4',
                name: 'System nawigacji',
                description: 'Zaawansowany system nawigacji z mapami Europy',
                price: 1899,
                category: 'Elektronika',
                manufacturer: 'BMW',
                isOriginalBMWPart: true,
                isInStock: true,
                imageUrl: null
              },
              {
                id: '5',
                name: 'Foteliki dziecięce',
                description: 'Bezpieczne foteliki dla dzieci 9-36kg',
                price: 899,
                category: 'Bezpieczeństwo',
                manufacturer: 'BMW',
                isOriginalBMWPart: true,
                isInStock: true,
                imageUrl: null
              }
            ];
            console.log('Używam przykładowych akcesorii');
          }
          
          setAccessories(accessoriesData.map(acc => ({ ...acc, selected: false })));
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
      
      // Pobierz akcesoria dla nowego modelu
      console.log('Zmiana modelu - pobieranie akcesorii dla:', selectedModel.name);
      
      let accessoriesData = [];
      try {
        const accessoriesResponse = await fetch(`${apiUrl}/api/car-accessories/model/${encodeURIComponent(selectedModel.name)}`);
        if (accessoriesResponse.ok) {
          accessoriesData = await accessoriesResponse.json();
        }
      } catch (err) {
        console.log('Błąd pobierania akcesorii dla nowego modelu:', err);
      }
      
      // Jeśli brak akcesorii, pobierz wszystkie
      if (accessoriesData.length === 0) {
        try {
          const allAccessoriesResponse = await fetch(`${apiUrl}/api/car-accessories`);
          if (allAccessoriesResponse.ok) {
            accessoriesData = await allAccessoriesResponse.json();
          }
        } catch (err) {
          console.log('Błąd pobierania wszystkich akcesorii przy zmianie modelu:', err);
        }
      }
      
      setAccessories(accessoriesData.map(acc => ({ ...acc, selected: false })));
      setSelectedAccessories([]);
    } catch (err) {
      console.error('Błąd zmiany modelu:', err);
    }
  };

  // Funkcja do obsługi wyboru akcesorii
  const handleAccessoryToggle = (accessoryId) => {
    setAccessories(prev => 
      prev.map(acc => 
        acc.id === accessoryId 
          ? { ...acc, selected: !acc.selected }
          : acc
      )
    );
    
    setSelectedAccessories(prev => {
      const accessory = accessories.find(acc => acc.id === accessoryId);
      const isSelected = prev.some(acc => acc.id === accessoryId);
      
      if (isSelected) {
        return prev.filter(acc => acc.id !== accessoryId);
      } else {
        return [...prev, accessory];
      }
    });
  };

  // Funkcja do obsługi wyboru wyposażenia wnętrza
  const handleInteriorEquipmentToggle = (equipmentId) => {
    setInteriorEquipment(prev => 
      prev.map(eq => 
        eq.id === equipmentId 
          ? { ...eq, selected: !eq.selected }
          : eq
      )
    );
    
    setSelectedInteriorEquipment(prev => {
      const equipment = interiorEquipment.find(eq => eq.id === equipmentId);
      const isSelected = prev.some(eq => eq.id === equipmentId);
      
      if (isSelected) {
        return prev.filter(eq => eq.id !== equipmentId);
      } else {
        return [...prev, equipment];
      }
    });
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
  const totalPrice = useMemo(() => {
    const basePrice = selectedCarModel?.basePrice || 120000;
    const enginePrice = selectedEngine?.additionalPrice || 0;
    const accessoriesPrice = selectedAccessories.reduce((sum, acc) => sum + (acc?.price || 0), 0);
    const interiorEquipmentPrice = selectedInteriorEquipment.reduce((sum, eq) => sum + (eq?.additionalPrice || 0), 0);
    const exteriorColorPrice = exteriorColors.find(c => c.value === carColor)?.price || 0;
    const wheelTypePrice = wheelTypes.find(w => w.value === wheelType)?.price || 0;
    const interiorColorPrice = interiorColors.find(i => i.value === interiorColor)?.price || 0;
    
    return basePrice + enginePrice + accessoriesPrice + interiorEquipmentPrice + exteriorColorPrice + wheelTypePrice + interiorColorPrice;
  }, [selectedCarModel, selectedEngine, selectedAccessories, selectedInteriorEquipment, exteriorColors, wheelTypes, interiorColors, carColor, wheelType, interiorColor]);

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
            accessories={accessories}
            selectedAccessories={selectedAccessories}
            onAccessoryToggle={handleAccessoryToggle}
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
            selectedAccessories={selectedAccessories}
            selectedInteriorEquipment={selectedInteriorEquipment}
            totalPrice={totalPrice}
            onInteriorEquipmentToggle={handleInteriorEquipmentToggle}
            carColor={carColor}
            setCarColor={setCarColor}
            wheelType={wheelType}
            setWheelType={setWheelType}
            interiorColor={interiorColor}
            setInteriorColor={setInteriorColor}
          />
        </div>
      </div>
    </div>
  );
};

export default CarConfigurator;