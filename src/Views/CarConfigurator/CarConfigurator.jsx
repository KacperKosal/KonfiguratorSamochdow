import React, { useState, useEffect, useMemo } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import styles from './CarConfigurator.module.css';
import CarViewer from '../../components/CarConfigurator/CarViewer/CarViewer';
import ConfigTabs from '../../components/CarConfigurator/ConfigTabs/ConfigTabs';
import ConfigOptions from '../../components/CarConfigurator/ConfigOptions/ConfigOptions';
import SummaryPanel from '../../components/CarConfigurator/SummaryPanel/SummaryPanel';
import adminApiService from '../../services/adminApiService';

const CarConfigurator = () => {
  const location = useLocation();
  const navigate = useNavigate();
  const modelFromNavigation = location.state?.modelId;
  const savedConfiguration = location.state?.savedConfiguration;
  const configurationId = location.state?.configurationId;
  
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
  const [isEditMode, setIsEditMode] = useState(false);
  const [originalConfiguration, setOriginalConfiguration] = useState(null);
  const [pendingConfiguration, setPendingConfiguration] = useState(null);
  const [isLoadingConfiguration, setIsLoadingConfiguration] = useState(false);
  const [isLoadingModelData, setIsLoadingModelData] = useState(false);

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
        
        // Wybierz model na podstawie nawigacji, zapisanej konfiguracji lub pierwszy dostępny
        let selectedModel = null;
        if (savedConfiguration && savedConfiguration.carModelId) {
          selectedModel = carModelsData.find(model => model.id === savedConfiguration.carModelId);
        } else if (modelFromNavigation) {
          selectedModel = carModelsData.find(model => model.id === modelFromNavigation);
        }
        if (!selectedModel && carModelsData.length > 0) {
          selectedModel = carModelsData[0];
        }
        
        if (selectedModel) {
          console.log('Wybrany model samochodu:', selectedModel);
          setSelectedCarModel(selectedModel);
          // Dane specyficzne dla modelu będą załadowane przez osobny useEffect
        } else {
          // Jeśli nie ma wybranego modelu, załaduj globalne dane
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
          
          setAccessories(accessoriesData.map(acc => {
            // Konstruuj pełny URL dla zdjęcia jeśli jest to tylko nazwa pliku
            const imageUrl = acc.imageUrl && !acc.imageUrl.startsWith('http') 
              ? `${apiUrl}/api/images/${acc.imageUrl}` 
              : acc.imageUrl;
            return { ...acc, selected: false, imageUrl };
          }));
          
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
  
  // Pobieranie konfiguracji z API na podstawie configurationId
  useEffect(() => {
    const loadConfigurationFromApi = async () => {
      // Nie ładuj konfiguracji jeśli jest pendingConfiguration (czeka na dane nowego modelu)
      // lub jeśli konfiguracja już została załadowana
      if (pendingConfiguration || originalConfiguration) return;
      
      if (configurationId && !loading) {
        try {
          console.log('Pobieranie konfiguracji z API dla ID:', configurationId);
          const response = await fetch(`${apiUrl}/api/user-configurations/${configurationId}`, {
            headers: {
              'Authorization': `Bearer ${localStorage.getItem('accessToken')}`
            }
          });
          
          if (response.ok) {
            const apiConfiguration = await response.json();
            console.log('Pobrana konfiguracja z API:', apiConfiguration);
            setOriginalConfiguration(apiConfiguration);
            
            // Wczytaj konfigurację
            setIsLoadingConfiguration(true);
            setTimeout(() => {
              loadSavedConfiguration(apiConfiguration);
              setTimeout(() => setIsLoadingConfiguration(false), 100);
            }, 50);
          } else {
            console.error('Błąd pobierania konfiguracji z API:', response.status);
            // Fallback - użyj danych z location.state
            if (savedConfiguration) {
              setIsLoadingConfiguration(true);
              loadSavedConfiguration(savedConfiguration);
              setIsLoadingConfiguration(false);
            }
          }
        } catch (err) {
          console.error('Błąd pobierania konfiguracji:', err);
          // Fallback - użyj danych z location.state
          if (savedConfiguration) {
            setIsLoadingConfiguration(true);
            loadSavedConfiguration(savedConfiguration);
            setIsLoadingConfiguration(false);
          }
        }
      } else if (savedConfiguration && !loading) {
        // Jeśli nie ma configurationId, ale jest savedConfiguration
        setIsLoadingConfiguration(true);
        loadSavedConfiguration(savedConfiguration);
        setIsLoadingConfiguration(false);
      }
    };

    loadConfigurationFromApi();
  }, [configurationId, loading, carModels]);

  // UseEffect do ładowania danych specyficznych dla wybranego modelu
  useEffect(() => {
    const loadModelSpecificData = async () => {
      if (!selectedCarModel || loading || isLoadingConfiguration || isLoadingModelData) return;
      
      console.log(`=== ŁADOWANIE DANYCH MODELU: ${selectedCarModel.id} ===`);
      console.log('Timestamp:', new Date().toISOString());
      
      setIsLoadingModelData(true);
      
      try {
        // Pobierz dostępne kolory dla modelu
        try {
          console.log(`Pobieranie dostępnych kolorów dla modelu: ${selectedCarModel.id}`);
          const availableColorsData = await adminApiService.getAvailableColorsForModel(selectedCarModel.id);
          console.log('Otrzymane dostępne kolory:', availableColorsData);
          setAvailableColors(availableColorsData);
          
          try {
            const colorPricesData = await adminApiService.getColorPricesForModel(selectedCarModel.id);
            console.log('Otrzymane ceny kolorów:', colorPricesData);
            setColorPrices(colorPricesData);
          } catch (err) {
            console.log('Błąd pobierania cen kolorów:', err);
            setColorPrices({});
          }
          
          // Ustaw domyślny kolor jeśli dostępne (tylko gdy nie ma pendingConfiguration ani originalConfiguration)
          if (!pendingConfiguration && !originalConfiguration && availableColorsData && availableColorsData.length > 0) {
            const defaultColorName = availableColorsData[0];
            console.log(`Ustawianie domyślnego koloru: ${defaultColorName}`);
            const staticColor = staticExteriorColors.find(c => c.name === defaultColorName);
            console.log('Znaleziony statyczny kolor:', staticColor);
            if (staticColor) {
              setCarColor(staticColor.value);
              console.log(`Ustawiono kolor samochodu na: ${staticColor.value}`);
            }
          }
        } catch (err) {
          console.error('Błąd pobierania dostępnych kolorów:', err);
          setAvailableColors([]);
        }
        
        // Pobierz silniki dla wybranego modelu
        console.log(`Pobieranie silników dla modelu: ${selectedCarModel.id}`);
        const enginesResponse = await fetch(`${apiUrl}/api/car-models/${selectedCarModel.id}/engines`);
        if (enginesResponse.ok) {
          const enginesData = await enginesResponse.json();
          console.log('Otrzymane silniki:', enginesData);
          setEngines(enginesData);
          if (!pendingConfiguration && !originalConfiguration && enginesData.length > 0) {
            setSelectedEngine(enginesData[0]);
            console.log('Wybrany domyślny silnik:', enginesData[0]);
          }
        } else {
          console.error('Błąd pobierania silników:', enginesResponse.status, enginesResponse.statusText);
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
        
        // Tylko zaktualizuj jeśli dane rzeczywiście się zmieniły, zachowaj istniejące zaznaczenia
        setAccessories(prev => {
          console.log('loadModelSpecificData - aktualizacja akcesoriów');
          console.log('Poprzednie akcesoria:', prev.map(p => ({ id: p.id, name: p.name, selected: p.selected })));
          const newData = accessoriesData.map(item => {
            const existing = prev.find(p => p.id === item.id);
            const selected = existing ? existing.selected : false;
            // Konstruuj pełny URL dla zdjęcia jeśli jest to tylko nazwa pliku
            const imageUrl = item.imageUrl && !item.imageUrl.startsWith('http') 
              ? `${apiUrl}/api/images/${item.imageUrl}` 
              : item.imageUrl;
            return { ...item, selected, imageUrl };
          });
          console.log('Nowe akcesoria:', newData.map(p => ({ id: p.id, name: p.name, selected: p.selected })));
          if (JSON.stringify(prev.map(p => ({ ...p, selected: false }))) !== JSON.stringify(newData.map(p => ({ ...p, selected: false })))) {
            return newData;
          }
          return prev;
        });
        
        setInteriorEquipment(prev => {
          console.log('loadModelSpecificData - aktualizacja wyposażenia wnętrza');
          console.log('Poprzednie wyposażenie:', prev.map(p => ({ id: p.id, value: p.value, selected: p.selected })));
          const newData = interiorEquipmentData.map(item => {
            const existing = prev.find(p => p.id === item.id);
            const selected = existing ? existing.selected : false;
            return { ...item, selected };
          });
          console.log('Nowe wyposażenie:', newData.map(p => ({ id: p.id, value: p.value, selected: p.selected })));
          if (JSON.stringify(prev.map(p => ({ ...p, selected: false }))) !== JSON.stringify(newData.map(p => ({ ...p, selected: false })))) {
            return newData;
          }
          return prev;
        });
        
      } catch (err) {
        console.error('Błąd ładowania danych modelu:', err);
      } finally {
        setIsLoadingModelData(false);
      }
    };

    loadModelSpecificData();
  }, [selectedCarModel, apiUrl, loading, pendingConfiguration, isLoadingConfiguration, originalConfiguration]);

  // UseEffect do obsługi odłożonej konfiguracji po zmianie modelu
  useEffect(() => {
    if (pendingConfiguration && selectedCarModel) {
      let attempts = 0;
      const maxAttempts = 10; // Maksymalnie 10 prób (2 sekundy)
      
      // Sprawdź czy wszystkie potrzebne dane są już załadowane
      const checkDataReady = () => {
        attempts++;
        // Sprawdź czy dane zostały już załadowane
        // Musimy poczekać na dane z API - sprawdź czy tablice zostały faktycznie załadowane
        // (nie tylko zdefiniowane jako puste tablice)
        const dataReady = engines.length > 0 && 
                          accessories !== undefined && 
                          interiorEquipment !== undefined &&
                          !isLoadingModelData &&
                          (accessories.length > 0 || interiorEquipment.length > 0 || attempts > 3);
        console.log('Sprawdzanie czy dane są gotowe:', {
          attempt: attempts,
          maxAttempts,
          engines: engines.length,
          accessories: accessories.length,
          interiorEquipment: interiorEquipment.length,
          isLoadingModelData,
          dataReady
        });
        
        if (dataReady) {
          console.log('Wszystkie dane gotowe - ładowanie odłożonej konfiguracji po zmianie modelu');
          setIsLoadingConfiguration(true);
          setTimeout(() => {
            loadSavedConfiguration(pendingConfiguration);
            setPendingConfiguration(null);
            setTimeout(() => setIsLoadingConfiguration(false), 100);
          }, 50);
        } else if (attempts < maxAttempts) {
          // Spróbuj ponownie za chwilę
          setTimeout(checkDataReady, 200);
        } else {
          console.warn('Przekroczono maksymalną liczbę prób ładowania konfiguracji');
          setPendingConfiguration(null);
        }
      };

      // Pierwsza próba po 100ms
      const timeout = setTimeout(checkDataReady, 100);
      return () => clearTimeout(timeout);
    }
  }, [pendingConfiguration, selectedCarModel, engines, accessories, interiorEquipment, isLoadingModelData]);

  // Funkcja do wczytywania konfiguracji
  const loadSavedConfiguration = (configuration) => {
    if (!configuration || isLoadingConfiguration) return;
    
    console.log('=== ŁADOWANIE KONFIGURACJI ===');
    console.log('Timestamp:', new Date().toISOString());
    console.log('Konfiguracja do załadowania:', configuration);
    console.log('Właściwości konfiguracji:', Object.keys(configuration));
    console.log('carModelId/CarModelId:', configuration.carModelId, '/', configuration.CarModelId);
    console.log('engineId/EngineId:', configuration.engineId, '/', configuration.EngineId);
    console.log('selectedAccessories/SelectedAccessories:', configuration.selectedAccessories, '/', configuration.SelectedAccessories);
    console.log('selectedInteriorEquipment/SelectedInteriorEquipment:', configuration.selectedInteriorEquipment, '/', configuration.SelectedInteriorEquipment);
    console.log('Aktualny stan:');
    console.log('- selectedCarModel:', selectedCarModel?.id, selectedCarModel?.name);
    console.log('- engines length:', engines.length);
    console.log('- accessories length:', accessories.length, accessories.map(a => ({ id: a.id, name: a.name, selected: a.selected })));
    console.log('- interiorEquipment length:', interiorEquipment.length, interiorEquipment.map(i => ({ id: i.id, value: i.value, selected: i.selected })));
    console.log('================================');
    
    setOriginalConfiguration(configuration);
    
    // Ustaw właściwy model samochodu
    const carModelId = configuration.carModelId || configuration.CarModelId;
    if (carModelId && carModels.length > 0) {
      const savedModel = carModels.find(model => 
        model.id.toString() === carModelId.toString()
      );
      console.log('Szukam modelu o ID:', carModelId, 'Znaleziony:', savedModel);
      if (savedModel && savedModel.id !== selectedCarModel?.id) {
        console.log('Przełączam na model:', savedModel);
        setSelectedCarModel(savedModel);
        // Zaznacz że konfiguracja powinna być załadowana po zmianie modelu
        setPendingConfiguration(configuration);
        console.log(configuration);
        return; // Zakończ tutaj, żeby przeładować dane dla nowego modelu
      }
    }
      
    // Ustaw wybrany silnik
    const engineId = configuration.engineId || configuration.EngineId;
    if (engineId && engines.length > 0) {
      console.log('Szukam silnika o ID:', engineId);
      console.log('Dostępne silniki:', engines);
      const savedEngine = engines.find(e => 
        e.carModelEngineId === engineId || 
        e.carModelEngineId === parseInt(engineId) ||
        e.engineId === engineId ||
        e.engineId === parseInt(engineId) ||
        e.id === engineId ||
        e.id === parseInt(engineId)
      );
      console.log('Znaleziony silnik:', savedEngine);
      if (savedEngine) {
        setSelectedEngine(savedEngine);
      } else {
        console.warn(`Silnik o ID ${engineId} nie został znaleziony. Dostępne silniki:`, engines.map(e => ({
          carModelEngineId: e.carModelEngineId,
          engineId: e.engineId,
          id: e.id,
          name: e.engineName
        })));
      }
    }
    
    // Ustaw kolor
    if (configuration.exteriorColor) {
      setCarColor(configuration.exteriorColor);
    }
    
    // Ustaw akcesoria
    const selectedAccessories = configuration.selectedAccessories || configuration.SelectedAccessories || [];
    if (selectedAccessories && selectedAccessories.length > 0 && accessories.length > 0) {
      console.log('Wczytywanie akcesoriów:', selectedAccessories);
      console.log('Dostępne akcesoria do mapowania:', accessories);
      const selectedAcc = [];
      setAccessories(prev => prev.map(acc => {
        const isSelected = selectedAccessories.some(savedAcc => {
          // Sprawdź różne warianty ID - API zwraca duże I
          const savedId = savedAcc.Id || savedAcc.id || savedAcc;
          console.log(`Porównuję akcesorium ${acc.id} z zapisanym ${savedId}`);
          return savedId === acc.id || savedId.toString() === acc.id.toString();
        });
        if (isSelected) {
          selectedAcc.push(acc);
          console.log(`Znaleziono akcesorium: ${acc.name} (ID: ${acc.id})`);
        }
        return { ...acc, selected: isSelected };
      }));
      setSelectedAccessories(selectedAcc);
      console.log('Wybrane akcesoria po mapowaniu:', selectedAcc);
    } else if (selectedAccessories && selectedAccessories.length > 0) {
      console.log('POMINIĘTO mapowanie akcesoriów - brak dostępnych akcesoriów w stanie');
      console.log('Zapisane akcesoria do załadowania:', selectedAccessories);
      console.log('Długość tablicy accessories:', accessories.length);
    }
    
    // Ustaw wyposażenie wnętrza
    const selectedInteriorEquipment = configuration.selectedInteriorEquipment || configuration.SelectedInteriorEquipment || [];
    if (selectedInteriorEquipment && selectedInteriorEquipment.length > 0 && interiorEquipment.length > 0) {
      console.log('Wczytywanie wyposażenia wnętrza:', selectedInteriorEquipment);
      console.log('Dostępne wyposażenie do mapowania:', interiorEquipment);
      const selectedEq = [];
      setInteriorEquipment(prev => prev.map(eq => {
        const isSelected = selectedInteriorEquipment.some(savedEq => {
          // Sprawdź różne warianty ID - API zwraca duże I
          const savedId = savedEq.Id || savedEq.id || savedEq;
          console.log(`Porównuję wyposażenie ${eq.id} z zapisanym ${savedId}`);
          return savedId === eq.id || savedId.toString() === eq.id.toString();
        });
        if (isSelected) {
          selectedEq.push(eq);
          console.log(`Znaleziono wyposażenie: ${eq.value} (ID: ${eq.id})`);
        }
        return { ...eq, selected: isSelected };
      }));
      setSelectedInteriorEquipment(selectedEq);
      console.log('Wybrane wyposażenie wnętrza po mapowaniu:', selectedEq);
    } else if (selectedInteriorEquipment && selectedInteriorEquipment.length > 0) {
      console.log('POMINIĘTO mapowanie wyposażenia wnętrza - brak dostępnego wyposażenia w stanie');
      console.log('Zapisane wyposażenie do załadowania:', selectedInteriorEquipment);
      console.log('Długość tablicy interiorEquipment:', interiorEquipment.length);
    }
  };


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
      console.log('Wybrany model:', selectedCarModel);
      console.log('Wybrany silnik:', selectedEngine);
      console.log('Wybrane akcesoria:', selectedAccessories);
      console.log('Wybrane wyposażenie wnętrza:', selectedInteriorEquipment);
      console.log('Wybrany kolor:', carColor, exteriorColorInfo);
      console.log('Całkowita cena:', totalPrice);

      const configurationData = {
        configurationName: configurationName.trim(),
        carModelId: selectedCarModel.id.toString(),
        engineId: selectedEngine?.carModelEngineId?.toString() || selectedEngine?.engineId?.toString() || selectedEngine?.id?.toString() || null,
        exteriorColor: carColor,
        exteriorColorName: exteriorColorInfo?.name || 'Nieznany kolor',
        accessoryIds: selectedAccessories.map(acc => acc.id.toString()),
        interiorEquipmentIds: selectedInteriorEquipment.map(eq => eq.id.toString()),
        totalPrice: totalPrice
      };

      console.log('Dane do wysłania:', configurationData);
      
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
        
        // Przejdź do konfiguratora z zapisaną konfiguracją - użyj tylko configurationId
        navigate('/car-configurator', { 
          state: { 
            configurationId: result.Id
          },
          replace: true
        });
      } else if (response.status === 401) {
        alert('Musisz być zalogowany, aby zapisać konfigurację. Przejdź do logowania.');
      } else if (response.status === 400) {
        const errorData = await response.json().catch(() => ({}));
        if (errorData.error && errorData.error.includes('maksymalny limit')) {
          alert(errorData.error);
          // Opcjonalnie: przekieruj do strony konta gdzie użytkownik może usunąć stare konfiguracje
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

  const handleSaveNewConfiguration = () => {
    saveConfiguration(); // Zapisz jako nową konfigurację
  };

  const handleCancelEdit = () => {
    if (originalConfiguration) {
      // Przywróć oryginalną konfigurację
      if (originalConfiguration.engineId && engines.length > 0) {
        const savedEngine = engines.find(e => e.carModelEngineId === originalConfiguration.engineId);
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
        setAccessories(prev => prev.map(acc => {
          const isSelected = originalConfiguration.selectedAccessories.some(savedAcc => 
            savedAcc.id === acc.id || savedAcc === acc.id
          );
          if (isSelected) {
            selectedAcc.push(acc);
          }
          return { ...acc, selected: isSelected };
        }));
        setSelectedAccessories(selectedAcc);
      }
      
      // Przywróć wyposażenie wnętrza
      if (originalConfiguration.selectedInteriorEquipment) {
        const selectedEq = [];
        setInteriorEquipment(prev => prev.map(eq => {
          const isSelected = originalConfiguration.selectedInteriorEquipment.some(savedEq => 
            savedEq.id === eq.id || savedEq === eq.id
          );
          if (isSelected) {
            selectedEq.push(eq);
          }
          return { ...eq, selected: isSelected };
        }));
        setSelectedInteriorEquipment(selectedEq);
      }
    }
    setIsEditMode(false);
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
            configurationId={configurationId}
            isEditMode={isEditMode}
            onEditConfiguration={handleEditConfiguration}
            onSaveNewConfiguration={handleSaveNewConfiguration}
            onCancelEdit={handleCancelEdit}
          />
        </div>
      </div>
    </div>
  );
};

export default CarConfigurator;