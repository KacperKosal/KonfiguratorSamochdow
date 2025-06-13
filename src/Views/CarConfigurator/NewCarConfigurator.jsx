import React, { useEffect, useState } from 'react';
import { useNavigate, useSearchParams } from 'react-router-dom';
import styles from './CarConfigurator.module.css';
import CarViewer from '../../components/CarConfigurator/CarViewer/CarViewer';
import ConfigTabs from '../../components/CarConfigurator/ConfigTabs/ConfigTabs';
import ConfigOptions from '../../components/CarConfigurator/ConfigOptions/ConfigOptions';
import SummaryPanel from '../../components/CarConfigurator/SummaryPanel/SummaryPanel';
import { useCarConfigurator } from '../../hooks/useCarConfigurator';

const NewCarConfigurator = () => {
  const navigate = useNavigate();
  const [searchParams] = useSearchParams();
  const modelId = searchParams.get('modelId');
  
  const [showModelSelector, setShowModelSelector] = useState(!modelId);
  
  const {
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
    loading,
    error,
    isLoadingModelData,
    
    // Computed values
    exteriorColors,
    totalPrice,
    
    // Functions
    fetchCarModels,
    loadModelSpecificData,
    handleEngineChange,
    handleAccessoryToggle,
    handleInteriorEquipmentToggle,
    saveConfiguration
  } = useCarConfigurator();

  // Pobieranie podstawowych danych
  useEffect(() => {
    fetchCarModels();
  }, []);

  // Wybór modelu na podstawie parametru URL
  useEffect(() => {
    if (modelId && carModels.length > 0 && !selectedCarModel) {
      const model = carModels.find(m => m.id.toString() === modelId);
      if (model) {
        setSelectedCarModel(model);
        setShowModelSelector(false);
      } else {
        console.warn(`Model o ID ${modelId} nie został znaleziony`);
        setShowModelSelector(true);
      }
    }
  }, [modelId, carModels, selectedCarModel]);

  // Ładowanie danych specyficznych dla wybranego modelu
  useEffect(() => {
    if (selectedCarModel && !loading) {
      loadModelSpecificData(selectedCarModel);
    }
  }, [selectedCarModel, loading]);

  const handleModelSelect = (model) => {
    setSelectedCarModel(model);
    setShowModelSelector(false);
    // Aktualizuj URL bez przeładowania strony
    navigate(`/car-configurator/new?modelId=${model.id}`, { replace: true });
  };

  const handleChangeModel = () => {
    setShowModelSelector(true);
  };

  const handleSaveConfiguration = () => {
    saveConfiguration(navigate);
  };

  const handleBackToHome = () => {
    navigate('/');
  };

  if (loading) {
    return (
      <div className={styles.configurator}>
        <div className={styles.loading}>Ładowanie modeli samochodów...</div>
      </div>
    );
  }

  if (error) {
    return (
      <div className={styles.configurator}>
        <div className={styles.error}>Błąd: {error}</div>
        <button onClick={handleBackToHome} className={styles.backButton}>
          Wróć do strony głównej
        </button>
      </div>
    );
  }

  // Selector modeli
  if (showModelSelector) {
    return (
      <div className={styles.configurator}>
        <div className={styles.breadcrumb}>
          <button onClick={handleBackToHome} className={styles.breadcrumbLink}>
            Strona główna
          </button>
          <span className={styles.breadcrumbSeparator}> / </span>
          <span>Wybór modelu samochodu</span>
        </div>
        
        <h1 className={styles.configuratorTitle}>Wybierz model samochodu</h1>
        
        <div className={styles.modelSelector}>
          {carModels.length === 0 ? (
            <div className={styles.noModels}>Brak dostępnych modeli samochodów</div>
          ) : (
            <div className={styles.modelsGrid}>
              {carModels.map((model) => (
                <div 
                  key={model.id} 
                  className={styles.modelCard}
                  onClick={() => handleModelSelect(model)}
                >
                  {model.imageUrl && (
                    <img 
                      src={model.imageUrl} 
                      alt={model.name}
                      className={styles.modelImage}
                      onError={(e) => {
                        e.target.style.display = 'none';
                      }}
                    />
                  )}
                  <div className={styles.modelInfo}>
                    <h3 className={styles.modelName}>{model.name}</h3>
                    {model.basePrice && (
                      <p className={styles.modelPrice}>
                        Od {model.basePrice.toLocaleString()} PLN
                      </p>
                    )}
                    {model.description && (
                      <p className={styles.modelDescription}>{model.description}</p>
                    )}
                  </div>
                </div>
              ))}
            </div>
          )}
        </div>
      </div>
    );
  }

  // Główny konfigurator po wyborze modelu
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
        <button onClick={handleBackToHome} className={styles.breadcrumbLink}>
          Strona główna
        </button>
        <span className={styles.breadcrumbSeparator}> / </span>
        <button onClick={handleChangeModel} className={styles.breadcrumbLink}>
          Modele samochodów
        </button>
        <span className={styles.breadcrumbSeparator}> / </span>
        <span>{selectedCarModel.name}</span>
      </div>
      
      <h1 className={styles.configuratorTitle}>
        Konfigurator - {selectedCarModel.name}
      </h1>
      
      <div className={styles.configuratorActions}>
        <button 
          onClick={handleChangeModel}
          className={styles.changeModelButton}
        >
          Zmień model
        </button>
      </div>
      
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
            onSaveConfiguration={handleSaveConfiguration}
            carColor={carColor}
            isEditMode={false}
            isSavedConfiguration={false}
          />
        </div>
      </div>
    </div>
  );
};

export default NewCarConfigurator;