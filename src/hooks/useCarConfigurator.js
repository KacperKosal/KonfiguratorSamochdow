import { useState, useEffect, useMemo } from 'react';
import adminApiService from '../services/adminApiService';

export const useCarConfigurator = () => {
  const [activeTab, setActiveTab] = useState('engine');
  const [carColor, setCarColor] = useState('#000000');
  
  // Stany dla danych z API
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
  const [isEditMode, setIsEditMode] = useState(false);
  const [originalConfiguration, setOriginalConfiguration] = useState(null);
  const [pendingConfiguration, setPendingConfiguration] = useState(null);
  const [isLoadingConfiguration, setIsLoadingConfiguration] = useState(false);
  const [isLoadingModelData, setIsLoadingModelData] = useState(false);
  const [loadedConfigurationId, setLoadedConfigurationId] = useState(null);

  // Konfiguracja API
  const apiUrl = import.meta.env.VITE_API_URL || 'https://localhost:7020';

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
    if (availableColors.length === 0) {
      return [];
    }
    
    const filtered = staticExteriorColors.filter(color => 
      availableColors.includes(color.name)
    ).map(color => ({
      ...color,
      price: colorPrices[color.name] ?? color.price
    }));
    
    return filtered;
  }, [availableColors, colorPrices]);

  // Obliczanie ceny
  const totalPrice = useMemo(() => {
    const basePrice = selectedCarModel?.basePrice || 120000;
    const enginePrice = selectedEngine?.additionalPrice || 0;
    const accessoriesPrice = selectedAccessories.reduce((sum, acc) => sum + (acc?.price || 0), 0);
    const interiorEquipmentPrice = selectedInteriorEquipment.reduce((sum, eq) => sum + (eq?.additionalPrice || 0), 0);
    const exteriorColorPrice = exteriorColors.find(c => c.value === carColor)?.price || 0;
    
    return basePrice + enginePrice + accessoriesPrice + interiorEquipmentPrice + exteriorColorPrice;
  }, [selectedCarModel, selectedEngine, selectedAccessories, selectedInteriorEquipment, carColor, exteriorColors]);

  // Pobieranie podstawowych danych (modeli samochodów)
  const fetchCarModels = async () => {
    try {
      console.log('🚗 Rozpoczynam pobieranie modeli samochodów...');
      setLoading(true);
      const carModelsResponse = await fetch(`${apiUrl}/api/car-models`);
      if (!carModelsResponse.ok) throw new Error('Błąd pobierania modeli samochodów');
      const carModelsData = await carModelsResponse.json();
      console.log('🚗 Pobrano modele samochodów:', carModelsData);
      setCarModels(carModelsData);
      return carModelsData;
    } catch (err) {
      setError(err.message);
      console.error('❌ Błąd pobierania modeli samochodów:', err);
      throw err;
    } finally {
      console.log('🚗 Zakończono pobieranie modeli samochodów, loading = false');
      setLoading(false);
    }
  };

  // Ładowanie danych specyficznych dla modelu
  const loadModelSpecificData = async (model) => {
    console.log('⚙️ Rozpoczynam ładowanie danych dla modelu:', model?.name);
    if (!model || isLoadingModelData) {
      console.log('⚙️ Pomijam ładowanie - brak modelu lub już ładuję');
      return;
    }
    
    console.log('⚙️ Ustawiam isLoadingModelData = true');
    setIsLoadingModelData(true);
    
    try {
      // Pobierz dostępne kolory dla modelu
      try {
        const availableColorsData = await adminApiService.getAvailableColorsForModel(model.id);
        setAvailableColors(availableColorsData);
        
        try {
          const colorPricesData = await adminApiService.getColorPricesForModel(model.id);
          setColorPrices(colorPricesData);
        } catch (err) {
          console.log('Błąd pobierania cen kolorów:', err);
          setColorPrices({});
        }
        
        // Ustaw domyślny kolor jeśli dostępne
        if (!pendingConfiguration && !originalConfiguration && availableColorsData && availableColorsData.length > 0) {
          const defaultColorName = availableColorsData[0];
          const staticColor = staticExteriorColors.find(c => c.name === defaultColorName);
          if (staticColor) {
            setCarColor(staticColor.value);
          }
        }
      } catch (err) {
        console.error('Błąd pobierania dostępnych kolorów:', err);
        setAvailableColors([]);
      }
      
      // Pobierz silniki dla wybranego modelu
      const enginesResponse = await fetch(`${apiUrl}/api/car-models/${model.id}/engines`);
      if (enginesResponse.ok) {
        const enginesData = await enginesResponse.json();
        setEngines(enginesData);
        if (!pendingConfiguration && !originalConfiguration && enginesData.length > 0) {
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
      
      // Aktualizuj stany
      setAccessories(prev => {
        const newData = accessoriesData.map(item => {
          const existing = prev.find(p => p.id === item.id);
          const selected = existing ? existing.selected : false;
          const imageUrl = item.imageUrl && !item.imageUrl.startsWith('http') 
            ? `${apiUrl}/api/images/${item.imageUrl}` 
            : item.imageUrl;
          return { ...item, selected, imageUrl };
        });
        return newData;
      });
      
      setInteriorEquipment(prev => {
        const newData = interiorEquipmentData.map(item => {
          const existing = prev.find(p => p.id === item.id);
          const selected = existing ? existing.selected : false;
          return { ...item, selected };
        });
        return newData;
      });
      
    } catch (err) {
      console.error('❌ Błąd ładowania danych modelu:', err);
    } finally {
      console.log('⚙️ Zakończono ładowanie danych modelu, isLoadingModelData = false');
      setIsLoadingModelData(false);
    }
  };

  // Funkcja do wczytywania konfiguracji
  const loadSavedConfiguration = (configuration, retryCount = 0) => {
    if (!configuration) {
      console.log('❌ Brak konfiguracji do załadowania');
      return;
    }
    
    if (isLoadingConfiguration) {
      console.log('❌ Ładowanie konfiguracji już w toku, pomijam');
      return;
    }
    
    const configId = configuration.id || configuration.Id;
    if (configId && loadedConfigurationId === configId && retryCount === 0) {
      console.log('✅ Konfiguracja już załadowana, pomijam:', configId);
      return;
    }
    
    console.log('=== ŁADOWANIE KONFIGURACJI ===');
    console.log('🔄 Konfiguracja do załadowania:', configuration);
    console.log('🔄 Retry count:', retryCount);
    console.log('🔄 Car models length:', carModels.length);
    console.log('🔄 Engines length:', engines.length);
    console.log('🔄 Accessories length:', accessories.length);
    console.log('🔄 Interior equipment length:', interiorEquipment.length);
    
    // Przy pierwszym wywołaniu ustaw oryginalną konfigurację i wyczyść aktualne listy
    if (retryCount === 0) {
      console.log('🔧 Ustawianie oryginalnej konfiguracji i czyszczenie list');
      setOriginalConfiguration(configuration);
      setLoadedConfigurationId(configId);
      setSelectedAccessories([]);
      setSelectedInteriorEquipment([]);
      
      setAccessories(prev => prev.map(acc => ({ ...acc, selected: false })));
      setInteriorEquipment(prev => prev.map(eq => ({ ...eq, selected: false })));
    }
    
    // Ustaw właściwy model samochodu
    const carModelId = configuration.carModelId || configuration.CarModelId;
    console.log('🚗 Próba ustawienia modelu samochodu - ID z konfiguracji:', carModelId);
    console.log('🚗 Dostępne modele:', carModels.map(m => ({ id: m.id, name: m.name })));
    console.log('🚗 Aktualnie wybrany model:', selectedCarModel?.id, selectedCarModel?.name);
    
    if (carModelId && carModels.length > 0) {
      const savedModel = carModels.find(model => 
        model.id.toString() === carModelId.toString()
      );
      console.log('🚗 Znaleziony model dla konfiguracji:', savedModel);
      
      if (savedModel && savedModel.id !== selectedCarModel?.id) {
        console.log('✅ Ustawianie nowego modelu samochodu:', savedModel.name);
        setSelectedCarModel(savedModel);
      } else if (savedModel) {
        console.log('✅ Model już ustawiony poprawnie:', savedModel.name);
      } else {
        console.log('❌ Nie znaleziono modelu o ID:', carModelId);
      }
    } else {
      console.log('❌ Brak ID modelu lub brak dostępnych modeli');
    }
      
    // Ustaw wybrany silnik
    const engineId = configuration.engineId || configuration.EngineId;
    console.log('⚙️ Próba ustawienia silnika - ID z konfiguracji:', engineId);
    console.log('⚙️ Dostępne silniki:', engines.map(e => ({ id: e.id, engineId: e.engineId, name: e.name })));
    console.log('⚙️ Aktualnie wybrany silnik:', selectedEngine?.id, selectedEngine?.name);
    
    if (engineId && engines.length > 0) {
      const savedEngine = engines.find(e => {
        const engineIdField = e.engineId?.toString();
        const id = e.id?.toString();
        const searchId = engineId.toString();
        
        return engineIdField === searchId || id === searchId;
      });
      console.log('⚙️ Znaleziony silnik dla konfiguracji:', savedEngine);
      
      if (savedEngine) {
        console.log('✅ Ustawianie silnika:', savedEngine.name);
        setSelectedEngine(savedEngine);
      } else {
        console.log('❌ Nie znaleziono silnika, ustawiam pierwszy dostępny');
        if (engines.length > 0) {
          setSelectedEngine(engines[0]);
        }
      }
    } else if (!engineId && engines.length > 0) {
      console.log('⚙️ Brak ID silnika, ustawiam pierwszy dostępny');
      setSelectedEngine(engines[0]);
    } else {
      console.log('❌ Brak ID silnika lub brak dostępnych silników');
    }
    
    // Ustaw kolor
    console.log('🎨 Próba ustawienia koloru:', configuration.exteriorColor);
    if (configuration.exteriorColor) {
      console.log('✅ Ustawianie koloru na:', configuration.exteriorColor);
      setCarColor(configuration.exteriorColor);
    } else {
      console.log('❌ Brak koloru w konfiguracji');
    }
    
    // Ustaw akcesoria
    const savedAccessoriesConfig = configuration.selectedAccessories || configuration.SelectedAccessories || [];
    const accessoriesAlreadyLoaded = selectedAccessories.length > 0 && 
      savedAccessoriesConfig.every(savedAcc => {
        const savedId = savedAcc.Id || savedAcc.id || savedAcc;
        return selectedAccessories.some(acc => acc.id?.toString() === savedId?.toString());
      });
    
    if (savedAccessoriesConfig && savedAccessoriesConfig.length > 0 && !accessoriesAlreadyLoaded) {
      if (accessories.length > 0) {
        const selectedAcc = [];
        const updatedAccessories = accessories.map(acc => {
          const isSelected = savedAccessoriesConfig.some(savedAcc => {
            const savedId = savedAcc.Id || savedAcc.id || savedAcc;
            return savedId?.toString() === acc.id?.toString();
          });
          if (isSelected) {
            selectedAcc.push(acc);
          }
          return { ...acc, selected: isSelected };
        });
        
        setAccessories(updatedAccessories);
        setSelectedAccessories(selectedAcc);
      } else if (retryCount < 3) {
        setTimeout(() => {
          if (accessories.length > 0) {
            loadSavedConfiguration(configuration, retryCount + 1);
          }
        }, 500);
      }
    }
    
    // Ustaw wyposażenie wnętrza
    const savedInteriorEquipmentConfig = configuration.selectedInteriorEquipment || configuration.SelectedInteriorEquipment || [];
    const interiorEquipmentAlreadyLoaded = selectedInteriorEquipment.length > 0 && 
      savedInteriorEquipmentConfig.every(savedEq => {
        const savedId = savedEq.Id || savedEq.id || savedEq;
        return selectedInteriorEquipment.some(eq => eq.id?.toString() === savedId?.toString());
      });
    
    if (savedInteriorEquipmentConfig && savedInteriorEquipmentConfig.length > 0 && !interiorEquipmentAlreadyLoaded) {
      if (interiorEquipment.length > 0) {
        const selectedEq = [];
        const updatedInteriorEquipment = interiorEquipment.map(eq => {
          const isSelected = savedInteriorEquipmentConfig.some(savedEq => {
            const savedId = savedEq.Id || savedEq.id || savedEq;
            return savedId?.toString() === eq.id?.toString();
          });
          if (isSelected) {
            selectedEq.push(eq);
          }
          return { ...eq, selected: isSelected };
        });
        
        setInteriorEquipment(updatedInteriorEquipment);
        setSelectedInteriorEquipment(selectedEq);
      } else if (retryCount < 3) {
        setTimeout(() => {
          if (interiorEquipment.length > 0) {
            loadSavedConfiguration(configuration, retryCount + 1);
          }
        }, 500);
      }
    }
    
    console.log('=== KONIEC ŁADOWANIA KONFIGURACJI ===');
    console.log('✅ Wybrany model:', selectedCarModel?.name);
    console.log('✅ Wybrany silnik:', selectedEngine?.name);
    console.log('✅ Wybrany kolor:', carColor);
    console.log('✅ Wybrane akcesoria:', selectedAccessories.map(a => a.name));
    console.log('✅ Wybrane wyposażenie:', selectedInteriorEquipment.map(e => e.name));
  };

  // Funkcja do obsługi zmiany silnika
  const handleEngineChange = (engine) => {
    setSelectedEngine(engine);
  };

  // Funkcja do obsługi wyboru akcesorii
  const handleAccessoryToggle = (accessoryId) => {
    const clickedAccessory = accessories.find(acc => acc.id === accessoryId);
    if (!clickedAccessory) return;

    const isCurrentlySelected = clickedAccessory.selected;

    if (clickedAccessory.type === 'AlloyWheels') {
      setAccessories(prev => 
        prev.map(acc => {
          if (acc.type === 'AlloyWheels') {
            if (acc.id === accessoryId && isCurrentlySelected) {
              return { ...acc, selected: false };
            }
            return { ...acc, selected: acc.id === accessoryId };
          }
          return acc;
        })
      );
      
      setSelectedAccessories(prev => {
        const withoutWheels = prev.filter(acc => acc.type !== 'AlloyWheels');
        if (isCurrentlySelected) {
          return withoutWheels;
        }
        return [...withoutWheels, clickedAccessory];
      });
    } else {
      setAccessories(prev => 
        prev.map(acc => {
          if (acc.type === clickedAccessory.type) {
            if (acc.id === accessoryId && isCurrentlySelected) {
              return { ...acc, selected: false };
            }
            return { ...acc, selected: acc.id === accessoryId };
          }
          return acc;
        })
      );
      
      setSelectedAccessories(prev => {
        const withoutSameType = prev.filter(acc => acc.type !== clickedAccessory.type);
        if (isCurrentlySelected) {
          return withoutSameType;
        }
        return [...withoutSameType, clickedAccessory];
      });
    }
  };

  // Funkcja do obsługi wyboru wyposażenia wnętrza
  const handleInteriorEquipmentToggle = (equipmentId) => {
    const clickedEquipment = interiorEquipment.find(eq => eq.id === equipmentId);
    if (!clickedEquipment) return;

    const isCurrentlySelected = clickedEquipment.selected;

    setInteriorEquipment(prev => 
      prev.map(eq => {
        if (eq.type === clickedEquipment.type) {
          if (eq.id === equipmentId && isCurrentlySelected) {
            return { ...eq, selected: false };
          }
          return { ...eq, selected: eq.id === equipmentId };
        }
        return eq;
      })
    );
    
    setSelectedInteriorEquipment(prev => {
      const withoutSameType = prev.filter(eq => eq.type !== clickedEquipment.type);
      if (isCurrentlySelected) {
        return withoutSameType;
      }
      return [...withoutSameType, clickedEquipment];
    });
  };

  // Funkcja do zapisania konfiguracji
  const saveConfiguration = async (navigate) => {
    if (!selectedCarModel) {
      alert('Wybierz model samochodu przed zapisaniem konfiguracji');
      return;
    }

    const configurationName = prompt('Podaj nazwę dla swojej konfiguracji:', 
      `${selectedCarModel.name} - ${new Date().toLocaleDateString()}`);
    
    if (!configurationName || configurationName.trim() === '') {
      return;
    }
    
    try {
      const exteriorColorInfo = exteriorColors.find(c => c.value === carColor) || 
                                staticExteriorColors.find(c => c.value === carColor);

      const configurationData = {
        configurationName: configurationName.trim(),
        carModelId: selectedCarModel.id.toString(),
        engineId: selectedEngine?.engineId?.toString() || selectedEngine?.id?.toString() || null,
        exteriorColor: carColor,
        exteriorColorName: exteriorColorInfo?.name || 'Nieznany kolor',
        accessoryIds: selectedAccessories.map(acc => acc.id.toString()),
        interiorEquipmentIds: selectedInteriorEquipment.map(eq => eq.id.toString()),
        totalPrice: totalPrice
      };
      
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
        console.log('📁 Odpowiedź z API po zapisaniu:', result);
        alert(`Konfiguracja "${configurationName}" została zapisana w Twoim koncie!`);
        
        if (navigate) {
          // Spróbuj obu wariantów ID
          const configId = result.Id || result.id;
          console.log('📁 Używam ID konfiguracji do redirecta:', configId);
          navigate(`/car-configurator/saved/${configId}`, { replace: true });
        }
        return result;
      } else if (response.status === 401) {
        alert('Musisz być zalogowany, aby zapisać konfigurację. Przejdź do logowania.');
      } else if (response.status === 400) {
        const errorData = await response.json().catch(() => ({}));
        if (errorData.error && errorData.error.includes('maksymalny limit')) {
          alert(errorData.error);
          if (confirm('Czy chcesz przejść do swojego konta, aby usunąć stare konfiguracje?')) {
            navigate('/my-account');
          }
        } else {
          alert(errorData.error || 'Błąd walidacji danych');
        }
      } else {
        const errorData = await response.json().catch(() => ({}));
        throw new Error(errorData.message || 'Błąd zapisywania konfiguracji');
      }
    } catch (err) {
      console.error('Błąd zapisywania konfiguracji:', err);
      alert(`Wystąpił błąd podczas zapisywania konfiguracji: ${err.message}`);
    }
  };

  // Funkcje obsługi trybu edycji
  const handleEditConfiguration = () => {
    setIsEditMode(true);
  };

  const handleSaveNewConfiguration = (navigate) => {
    saveConfiguration(navigate);
  };

  const handleCancelEdit = () => {
    if (originalConfiguration) {
      // Przywróć oryginalną konfigurację
      if (originalConfiguration.engineId && engines.length > 0) {
        const savedEngine = engines.find(e => 
          e.engineId?.toString() === originalConfiguration.engineId?.toString() || 
          e.id?.toString() === originalConfiguration.engineId?.toString()
        );
        if (savedEngine) {
          setSelectedEngine(savedEngine);
        }
      }
      
      if (originalConfiguration.exteriorColor) {
        setCarColor(originalConfiguration.exteriorColor);
      }
      
      // Przywróć akcesoria
      if (originalConfiguration.selectedAccessories) {
        const selectedAcc = [];
        const updatedAccessories = accessories.map(acc => {
          const isSelected = originalConfiguration.selectedAccessories.some(savedAcc => {
            const savedId = savedAcc.Id || savedAcc.id || savedAcc;
            return savedId?.toString() === acc.id?.toString();
          });
          if (isSelected) {
            selectedAcc.push(acc);
          }
          return { ...acc, selected: isSelected };
        });
        setAccessories(updatedAccessories);
        setSelectedAccessories(selectedAcc);
      }
      
      // Przywróć wyposażenie wnętrza
      if (originalConfiguration.selectedInteriorEquipment) {
        const selectedEq = [];
        const updatedInteriorEquipment = interiorEquipment.map(eq => {
          const isSelected = originalConfiguration.selectedInteriorEquipment.some(savedEq => {
            const savedId = savedEq.Id || savedEq.id || savedEq;
            return savedId?.toString() === eq.id?.toString();
          });
          if (isSelected) {
            selectedEq.push(eq);
          }
          return { ...eq, selected: isSelected };
        });
        setInteriorEquipment(updatedInteriorEquipment);
        setSelectedInteriorEquipment(selectedEq);
      }
    }
    setIsEditMode(false);
  };

  return {
    // States
    activeTab,
    setActiveTab,
    carColor,
    setCarColor,
    carModels,
    selectedCarModel,
    setSelectedCarModel,
    engines,
    selectedEngine,
    accessories,
    selectedAccessories,
    interiorEquipment,
    selectedInteriorEquipment,
    availableColors,
    colorPrices,
    loading,
    error,
    isEditMode,
    originalConfiguration,
    pendingConfiguration,
    setPendingConfiguration,
    isLoadingConfiguration,
    setIsLoadingConfiguration,
    isLoadingModelData,
    loadedConfigurationId,
    
    // Computed values
    exteriorColors,
    totalPrice,
    apiUrl,
    staticExteriorColors,
    
    // Functions
    fetchCarModels,
    loadModelSpecificData,
    loadSavedConfiguration,
    handleEngineChange,
    handleAccessoryToggle,
    handleInteriorEquipmentToggle,
    saveConfiguration,
    handleEditConfiguration,
    handleSaveNewConfiguration,
    handleCancelEdit,
    
    // Setters for direct state management
    setOriginalConfiguration,
    setLoadedConfigurationId,
    setSelectedCarModel,
    setSelectedAccessories,
    setSelectedInteriorEquipment,
    setAccessories,
    setInteriorEquipment,
    setEngines,
    setSelectedEngine,
    setAvailableColors,
    setColorPrices,
    setLoading,
    setError
  };
};