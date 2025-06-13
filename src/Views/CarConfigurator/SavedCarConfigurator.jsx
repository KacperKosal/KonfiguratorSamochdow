import React, { useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import styles from './CarConfigurator.module.css';
import CarViewer from '../../components/CarConfigurator/CarViewer/CarViewer';
import ConfigTabs from '../../components/CarConfigurator/ConfigTabs/ConfigTabs';
import ConfigOptions from '../../components/CarConfigurator/ConfigOptions/ConfigOptions';
import SummaryPanel from '../../components/CarConfigurator/SummaryPanel/SummaryPanel';
import { useCarConfigurator } from '../../hooks/useCarConfigurator';

const SavedCarConfigurator = () => {
  const { configurationId } = useParams();
  const navigate = useNavigate();
  
  const {
    // States
    activeTab,
    setActiveTab,
    carColor,
    setCarColor,
    carModels,
    selectedCarModel,
    engines,
    selectedEngine,
    accessories,
    selectedAccessories,
    interiorEquipment,
    selectedInteriorEquipment,
    availableColors,
    loading,
    error,
    isEditMode,
    originalConfiguration,
    pendingConfiguration,
    setPendingConfiguration,
    isLoadingConfiguration,
    setIsLoadingConfiguration,
    isLoadingModelData,
    
    // Computed values
    exteriorColors,
    totalPrice,
    apiUrl,
    
    // Functions
    fetchCarModels,
    loadModelSpecificData,
    loadSavedConfiguration,
    handleEngineChange,
    handleAccessoryToggle,
    handleInteriorEquipmentToggle,
    handleEditConfiguration,
    handleSaveNewConfiguration,
    handleCancelEdit,
    
    // Setters
    setOriginalConfiguration,
    setSelectedCarModel
  } = useCarConfigurator();

  // Pobieranie konfiguracji z API na podstawie configurationId
  useEffect(() => {
    const loadConfigurationFromApi = async () => {
      if (!configurationId || originalConfiguration) return;
      
      if (!loading) {
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
            setPendingConfiguration(apiConfiguration);
            
            // Znajdź i ustaw model samochodu na podstawie konfiguracji
            if (carModels.length > 0) {
              const carModelId = apiConfiguration.carModelId || apiConfiguration.CarModelId;
              const savedModel = carModels.find(model => 
                model.id.toString() === carModelId?.toString()
              );
              if (savedModel) {
                console.log('🚗 Ustawiam model z konfiguracji:', savedModel.name);
                setSelectedCarModel(savedModel);
              }
            }
          } else if (response.status === 404) {
            console.error('Konfiguracja nie została znaleziona');
            alert('Konfiguracja nie została znaleziona lub nie masz uprawnień do jej wyświetlenia.');
            navigate('/car-configurator/new');
          } else {
            console.error('Błąd pobierania konfiguracji z API:', response.status);
            alert('Wystąpił błąd podczas ładowania konfiguracji.');
            navigate('/car-configurator/new');
          }
        } catch (err) {
          console.error('Błąd pobierania konfiguracji:', err);
          alert('Wystąpił błąd podczas ładowania konfiguracji.');
          navigate('/car-configurator/new');
        }
      }
    };

    loadConfigurationFromApi();
  }, [configurationId, loading]);

  // Pobieranie podstawowych danych
  useEffect(() => {
    fetchCarModels();
  }, []);

  // UseEffect do ustawienia modelu gdy modele są załadowane po konfiguracji
  useEffect(() => {
    console.log('🔧 UseEffect carModels triggered');
    console.log('🔧 carModels length:', carModels.length);
    console.log('🔧 pendingConfiguration:', pendingConfiguration);
    console.log('🔧 selectedCarModel:', selectedCarModel);
    
    if (carModels.length > 0 && pendingConfiguration && !selectedCarModel) {
      const carModelId = pendingConfiguration.carModelId || pendingConfiguration.CarModelId;
      const savedModel = carModels.find(model => 
        model.id.toString() === carModelId?.toString()
      );
      if (savedModel) {
        console.log('🔧 Ustawiam model po załadowaniu modeli:', savedModel.name);
        setSelectedCarModel(savedModel);
      }
    }
  }, [carModels, pendingConfiguration, selectedCarModel]);

  // Ładowanie danych specyficznych dla wybranego modelu
  useEffect(() => {
    console.log('🚗 UseEffect loadModelSpecificData triggered');
    console.log('🚗 selectedCarModel:', selectedCarModel);
    console.log('🚗 loading:', loading);
    console.log('🚗 isLoadingConfiguration:', isLoadingConfiguration);
    
    if (selectedCarModel && !loading && !isLoadingConfiguration) {
      console.log('🚗 Warunki spełnione - ładuję dane dla modelu:', selectedCarModel.name);
      loadModelSpecificData(selectedCarModel);
    } else {
      console.log('🚗 Warunki niespełnione - nie ładuję danych dla modelu');
    }
  }, [selectedCarModel, loading, isLoadingConfiguration]);

  // UseEffect do obsługi odłożonej konfiguracji po zmianie modelu
  useEffect(() => {
    console.log('🔄 UseEffect pendingConfiguration triggered');
    console.log('🔄 pendingConfiguration:', pendingConfiguration);
    console.log('🔄 selectedCarModel:', selectedCarModel);
    console.log('🔄 isLoadingConfiguration:', isLoadingConfiguration);
    console.log('🔄 loading:', loading);
    console.log('🔄 isLoadingModelData:', isLoadingModelData);
    
    if (pendingConfiguration && selectedCarModel && !isLoadingConfiguration) {
      // Sprawdź czy model pasuje do konfiguracji
      const configModelId = pendingConfiguration.carModelId || pendingConfiguration.CarModelId;
      console.log('🔄 Sprawdzam czy model pasuje - configModelId:', configModelId, 'selectedCarModel.id:', selectedCarModel.id);
      if (configModelId?.toString() !== selectedCarModel.id?.toString()) {
        console.log('❌ Model nie pasuje do konfiguracji, czekam...');
        return;
      }
      
      let attempts = 0;
      const maxAttempts = 15;
      
      const checkDataReady = () => {
        attempts++;
        console.log(`Sprawdzenie ${attempts}/${maxAttempts}: silniki=${engines.length}, akcesoria=${accessories?.length || 0}, wyposażenie=${interiorEquipment?.length || 0}, loadingModelData=${isLoadingModelData}`);
        
        const dataReady = engines.length > 0 && 
                          accessories !== undefined && 
                          interiorEquipment !== undefined &&
                          !isLoadingModelData;
        
        if (dataReady) {
          console.log('🎯 Wszystkie dane gotowe - ładowanie odłożonej konfiguracji');
          console.log('🎯 PendingConfiguration:', pendingConfiguration);
          setIsLoadingConfiguration(true);
          setTimeout(() => {
            console.log('🚀 Wywołuję loadSavedConfiguration z:', pendingConfiguration);
            loadSavedConfiguration(pendingConfiguration);
            setPendingConfiguration(null);
            setTimeout(() => {
              console.log('🏁 Kończę ładowanie konfiguracji');
              setIsLoadingConfiguration(false);
            }, 200);
          }, 100);
        } else if (attempts < maxAttempts) {
          setTimeout(checkDataReady, 300);
        } else {
          console.warn('Przekroczono maksymalną liczbę prób ładowania konfiguracji');
          setIsLoadingConfiguration(false);
          setPendingConfiguration(null);
        }
      };

      const timeout = setTimeout(checkDataReady, 200);
      return () => clearTimeout(timeout);
    }
  }, [pendingConfiguration, selectedCarModel, engines, accessories, interiorEquipment, isLoadingModelData, isLoadingConfiguration]);

  const handleSaveNew = () => {
    handleSaveNewConfiguration(navigate);
  };

  const handleBackToModels = () => {
    navigate('/car-configurator/new');
  };

  if (loading || isLoadingConfiguration) {
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
        <button onClick={handleBackToModels} className={styles.backButton}>
          Wróć do wyboru modeli
        </button>
      </div>
    );
  }

  if (!selectedCarModel) {
    return (
      <div className={styles.configurator}>
        <div className={styles.loading}>Ładowanie modelu samochodu...</div>
      </div>
    );
  }

  return (
    <div className={styles.configurator}>
      <div className={styles.breadcrumb}>
        <button onClick={handleBackToModels} className={styles.breadcrumbLink}>
          Modele samochodów
        </button>
        <span className={styles.breadcrumbSeparator}> / </span>
        <span>Zapisana konfiguracja: {selectedCarModel.name}</span>
      </div>
      
      <h1 className={styles.configuratorTitle}>
        Zapisana konfiguracja - {selectedCarModel.name}
        {isEditMode && <span className={styles.editModeIndicator}> (Tryb edycji)</span>}
      </h1>
      
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
            disabled={!isEditMode}
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
            onSaveConfiguration={handleSaveNew}
            carColor={carColor}
            configurationId={configurationId}
            isEditMode={isEditMode}
            onEditConfiguration={handleEditConfiguration}
            onSaveNewConfiguration={handleSaveNew}
            onCancelEdit={handleCancelEdit}
            isSavedConfiguration={true}
          />
        </div>
      </div>
    </div>
  );
};

export default SavedCarConfigurator;