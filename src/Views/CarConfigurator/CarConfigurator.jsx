import React, { useState, useEffect, useMemo } from 'react';
import { useLocation } from 'react-router-dom';
import styles from './CarConfigurator.module.css';
import CarViewer from '../../components/CarConfigurator/CarViewer/CarViewer';
import ConfigTabs from '../../components/CarConfigurator/ConfigTabs/ConfigTabs';
import ConfigOptions from '../../components/CarConfigurator/ConfigOptions/ConfigOptions';
import SummaryPanel from '../../components/CarConfigurator/SummaryPanel/SummaryPanel';
import adminApiService from '../../services/adminApiService';

const CarConfigurator = () => {
  const location = useLocation();
  const modelFromNavigation = location.state?.modelId;
  
  const [activeTab, setActiveTab] = useState('engine');
  const [carColor, setCarColor] = useState('#000000');
  
  // Nowe stany dla danych z API
  const [carModels, setCarModels] = useState([]);
  const [selectedCarModel, setSelectedCarModel] = useState(null);
  const [engines, setEngines] = useState([]);
  const [selectedEngine, setSelectedEngine] = useState(null);
  const [accessories, setAccessories] = useState([]);
  const [selectedAccessories, setSelectedAccessories] = useState([]);
  const [interiorEquipment, setInteriorEquipment] = useState([]);
  const [selectedInteriorEquipment, setSelectedInteriorEquipment] = useState([]);
  const [availableColors, setAvailableColors] = useState([]);
  const [colorPrices, setColorPrices] = useState({});
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
        
        // Wybierz model na podstawie nawigacji lub pierwszy dostępny
        let selectedModel = null;
        if (modelFromNavigation) {
          selectedModel = carModelsData.find(model => model.id === modelFromNavigation);
        }
        if (!selectedModel && carModelsData.length > 0) {
          selectedModel = carModelsData[0];
        }
        
        if (selectedModel) {
          setSelectedCarModel(selectedModel);
          
          // Pobierz dostępne kolory dla modelu
          try {
            console.log(`Pobieranie dostępnych kolorów dla modelu: ${selectedModel.id}`);
            const availableColorsData = await adminApiService.getAvailableColorsForModel(selectedModel.id);
            console.log('Otrzymane dostępne kolory:', availableColorsData);
            setAvailableColors(availableColorsData || []);
            
            // Pobierz ceny kolorów
            try {
              const colorPricesData = await adminApiService.getColorPricesForModel(selectedModel.id);
              console.log('Otrzymane ceny kolorów:', colorPricesData);
              setColorPrices(colorPricesData || {});
            } catch (err) {
              console.log('Błąd pobierania cen kolorów:', err);
              setColorPrices({});
            }
            
            // Ustaw domyślny kolor jeśli dostępne
            if (availableColorsData && availableColorsData.length > 0) {
              const defaultColorName = availableColorsData[0];
              console.log(`Ustawianie domyślnego koloru: ${defaultColorName}`);
              const staticColor = staticExteriorColors.find(c => c.name === defaultColorName);
              console.log('Znaleziony statyczny kolor:', staticColor);
              if (staticColor) {
                setCarColor(staticColor.value);
                console.log(`Ustawiono kolor samochodu na: ${staticColor.value}`);
              }
            } else {
              console.log('Brak dostępnych kolorów dla tego modelu');
              // Jeśli brak kolorów i aktywna jest zakładka exterior, przełącz na engine
              if (activeTab === 'exterior') {
                setActiveTab('engine');
              }
            }
          } catch (err) {
            console.error('Błąd pobierania dostępnych kolorów:', err);
            setAvailableColors([]);
          }
          
          // Pobierz silniki dla wybranego modelu
          const enginesResponse = await fetch(`${apiUrl}/api/car-models/${selectedModel.id}/engines`);
          if (enginesResponse.ok) {
            const enginesData = await enginesResponse.json();
            setEngines(enginesData);
            if (enginesData.length > 0) {
              setSelectedEngine(enginesData[0]);
            }
          }
          
          // Pobierz akcesoria
          let accessoriesData = [];
          try {
            const accessoriesResponse = await fetch(`${apiUrl}/api/car-accessories`);
            if (accessoriesResponse.ok) {
              accessoriesData = await accessoriesResponse.json();
            }
          } catch (err) {
            console.log('Błąd pobierania akcesorii:', err);
          }
          
          setAccessories(accessoriesData.map(acc => ({ ...acc, selected: false })));
          
          // Pobierz wyposażenie wnętrza
          let interiorEquipmentData = [];
          try {
            const interiorResponse = await fetch(`${apiUrl}/api/car-interior-equipment`);
            if (interiorResponse.ok) {
              interiorEquipmentData = await interiorResponse.json();
            }
          } catch (err) {
            console.log('Błąd pobierania wyposażenia wnętrza:', err);
          }
          
          setInteriorEquipment(interiorEquipmentData.map(eq => ({ ...eq, selected: false })));
        }
        
      } catch (err) {
        setError(err.message);
        console.error('Błąd pobierania danych:', err);
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, [apiUrl, modelFromNavigation]);


  // Funkcja do obsługi zmiany silnika
  const handleEngineChange = (engine) => {
    setSelectedEngine(engine);
  };

  // Funkcja do obsługi wyboru akcesorii
  const handleAccessoryToggle = (accessoryId) => {
    const clickedAccessory = accessories.find(acc => acc.id === accessoryId);
    if (!clickedAccessory) return;

    // Logika dla felg - tylko jedna może być wybrana
    if (clickedAccessory.type === 'AlloyWheels') {
      setAccessories(prev => 
        prev.map(acc => {
          if (acc.type === 'AlloyWheels') {
            return { ...acc, selected: acc.id === accessoryId };
          }
          return acc;
        })
      );
      
      setSelectedAccessories(prev => {
        // Usuń wszystkie felgi i dodaj tylko wybraną
        const withoutWheels = prev.filter(acc => acc.type !== 'AlloyWheels');
        return [...withoutWheels, clickedAccessory];
      });
    } 
    // Logika dla innych akcesoriów - jeden na typ
    else {
      setAccessories(prev => 
        prev.map(acc => {
          if (acc.type === clickedAccessory.type) {
            return { ...acc, selected: acc.id === accessoryId };
          }
          return acc;
        })
      );
      
      setSelectedAccessories(prev => {
        // Usuń wszystkie akcesoria tego samego typu i dodaj wybrane
        const withoutSameType = prev.filter(acc => acc.type !== clickedAccessory.type);
        return [...withoutSameType, clickedAccessory];
      });
    }
  };

  // Funkcja do obsługi wyboru wyposażenia wnętrza
  const handleInteriorEquipmentToggle = (equipmentId) => {
    const clickedEquipment = interiorEquipment.find(eq => eq.id === equipmentId);
    if (!clickedEquipment) return;

    // Logika - tylko jedna opcja na typ wyposażenia
    setInteriorEquipment(prev => 
      prev.map(eq => {
        if (eq.type === clickedEquipment.type) {
          return { ...eq, selected: eq.id === equipmentId };
        }
        return eq;
      })
    );
    
    setSelectedInteriorEquipment(prev => {
      // Usuń wszystkie wyposażenia tego samego typu i dodaj wybrane
      const withoutSameType = prev.filter(eq => eq.type !== clickedEquipment.type);
      return [...withoutSameType, clickedEquipment];
    });
  };

  // Funkcja do zapisania konfiguracji
  const saveConfiguration = async () => {
    if (!selectedCarModel) {
      alert('Wybierz model samochodu przed zapisaniem konfiguracji');
      return;
    }

    // Zapytaj użytkownika o nazwę konfiguracji
    const configurationName = prompt('Podaj nazwę dla swojej konfiguracji:', 
      `${selectedCarModel.name} - ${new Date().toLocaleDateString()}`);
    
    if (!configurationName || configurationName.trim() === '') {
      return;
    }
    
    try {
      // Przygotuj dane konfiguracji
      const exteriorColorInfo = exteriorColors.find(c => c.value === carColor) || 
                                staticExteriorColors.find(c => c.value === carColor);
      const configurationData = {
        configurationName: configurationName.trim(),
        carModelId: selectedCarModel.id,
        engineId: selectedEngine?.engineId || null,
        exteriorColor: carColor,
        exteriorColorName: exteriorColorInfo?.name || 'Nieznany kolor',
        accessoryIds: selectedAccessories.map(acc => acc.id),
        interiorEquipmentIds: selectedInteriorEquipment.map(eq => eq.id),
        totalPrice: totalPrice
      };

      console.log('Zapisuję konfigurację:', configurationData);
      
      // Wyślij do nowego endpointa
      const response = await fetch(`${apiUrl}/api/user-configurations`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${localStorage.getItem('accessToken')}`
        },
        body: JSON.stringify(configurationData)
      });
      
      if (response.ok) {
        const result = await response.json();
        alert(`Konfiguracja "${configurationName}" została zapisana w Twoim koncie!`);
        console.log('Konfiguracja zapisana z ID:', result.Id);
      } else if (response.status === 401) {
        alert('Musisz być zalogowany, aby zapisać konfigurację. Przejdź do logowania.');
      } else {
        const errorData = await response.json().catch(() => ({}));
        throw new Error(errorData.message || 'Błąd zapisywania konfiguracji');
      }
    } catch (err) {
      console.error('Błąd zapisywania konfiguracji:', err);
      alert(`Wystąpił błąd podczas zapisywania konfiguracji: ${err.message}`);
    }
  };

  const staticExteriorColors = [
    { name: 'Czarny', value: '#000000', price: 0 },
    { name: 'Biały', value: '#ffffff', price: 0 },
    { name: 'Srebrny', value: '#c0c0c0', price: 2000 },
    { name: 'Szary', value: '#808080', price: 1500 },
    { name: 'Niebieski', value: '#0000ff', price: 3000 },
    { name: 'Czerwony', value: '#ff0000', price: 3000 },
    { name: 'Zielony', value: '#008000', price: 3000 },
    { name: 'Żółty', value: '#ffff00', price: 3500 },
    { name: 'Pomarańczowy', value: '#ffa500', price: 3500 },
    { name: 'Brązowy', value: '#8b4513', price: 2500 },
    { name: 'Beżowy', value: '#f5f5dc', price: 1500 },
    { name: 'Złoty', value: '#ffd700', price: 4000 },
    { name: 'Granatowy', value: '#000080', price: 2500 },
    { name: 'Fioletowy', value: '#800080', price: 3500 },
  ];

  // Filtruj kolory na podstawie dostępnych kolorów dla modelu
  const exteriorColors = useMemo(() => {
    console.log('Filtrowanie kolorów - dostępne kolory:', availableColors);
    console.log('Filtrowanie kolorów - ceny kolorów:', colorPrices);
    console.log('Filtrowanie kolorów - statyczne kolory:', staticExteriorColors);
    
    if (availableColors.length === 0) {
      console.log('Brak dostępnych kolorów - zwracanie pustej listy');
      return [];
    }
    
    const filtered = staticExteriorColors.filter(color => 
      availableColors.includes(color.name)
    ).map(color => ({
      ...color,
      // Użyj ceny z API jeśli dostępna, w przeciwnym razie użyj ceny domyślnej
      price: colorPrices[color.name] ?? color.price
    }));
    
    console.log('Przefiltrowane kolory z cenami z API:', filtered);
    return filtered;
  }, [availableColors, colorPrices]);




  // Obliczanie ceny z uwzględnieniem danych z API
  const totalPrice = useMemo(() => {
    const basePrice = selectedCarModel?.basePrice || 120000;
    const enginePrice = selectedEngine?.additionalPrice || 0;
    const accessoriesPrice = selectedAccessories.reduce((sum, acc) => sum + (acc?.price || 0), 0);
    const interiorEquipmentPrice = selectedInteriorEquipment.reduce((sum, eq) => sum + (eq?.additionalPrice || 0), 0);
    
    // Użyj ceny koloru z exteriorColors (która już zawiera ceny z API)
    const exteriorColorPrice = exteriorColors.find(c => c.value === carColor)?.price || 0;
    
    return basePrice + enginePrice + accessoriesPrice + interiorEquipmentPrice + exteriorColorPrice;
  }, [selectedCarModel, selectedEngine, selectedAccessories, selectedInteriorEquipment, carColor, exteriorColors]);

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
      
      {/* Wyświetlanie wybranego modelu */}
      {selectedCarModel && (
        <div className={styles.selectedModelInfo}>
          <h2>{selectedCarModel.name}</h2>
          <p>Cena bazowa: {selectedCarModel.basePrice?.toLocaleString()} zł</p>
        </div>
      )}
      
      <div className={styles.configuratorContent}>
        <div className={styles.configuratorLeftPanel}>
          <CarViewer
            selectedCarModel={selectedCarModel}
            carColor={carColor}
          />
          <ConfigTabs 
            activeTab={activeTab} 
            setActiveTab={setActiveTab} 
            availableColors={availableColors}
          />
          <ConfigOptions
            activeTab={activeTab}
            exteriorColors={exteriorColors}
            accessories={accessories}
            interiorEquipment={interiorEquipment}
            selectedAccessories={selectedAccessories}
            selectedInteriorEquipment={selectedInteriorEquipment}
            onAccessoryToggle={handleAccessoryToggle}
            onInteriorEquipmentToggle={handleInteriorEquipmentToggle}
            engines={engines}
            selectedEngine={selectedEngine}
            onEngineChange={handleEngineChange}
            carColor={carColor}
            setCarColor={setCarColor}
          />
        </div>
        <div className={styles.configuratorRightPanel}>
          <SummaryPanel
            selectedCarModel={selectedCarModel}
            selectedEngine={selectedEngine}
            exteriorColors={exteriorColors}
            selectedAccessories={selectedAccessories}
            selectedInteriorEquipment={selectedInteriorEquipment}
            totalPrice={totalPrice}
            onSaveConfiguration={saveConfiguration}
            carColor={carColor}
          />
        </div>
      </div>
    </div>
  );
};

export default CarConfigurator;