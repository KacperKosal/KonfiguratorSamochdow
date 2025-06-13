import React, { useEffect } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';

const CarConfigurator = () => {
  const location = useLocation();
  const navigate = useNavigate();
  const modelFromNavigation = location.state?.modelId;
  const savedConfiguration = location.state?.savedConfiguration;
  const configurationId = location.state?.configurationId;

  useEffect(() => {
    // Przekieruj na odpowiednią nową ścieżkę w zależności od parametrów
    if (configurationId) {
      // Jeśli jest configurationId, przekieruj na ścieżkę zapisanej konfiguracji
      navigate(`/car-configurator/saved/${configurationId}`, { replace: true });
    } else if (modelFromNavigation) {
      // Jeśli jest modelId, przekieruj na nową konfigurację z tym modelem
      navigate(`/car-configurator/new?modelId=${modelFromNavigation}`, { replace: true });
    } else {
      // W przeciwnym razie przekieruj na wybór modelu
      navigate('/car-configurator/new', { replace: true });
    }
  }, [navigate, configurationId, modelFromNavigation]);

  // Wyświetl loading podczas przekierowania
  return (
    <div style={{ 
      display: 'flex', 
      justifyContent: 'center', 
      alignItems: 'center', 
      height: '50vh',
      fontSize: '16px',
      color: '#666'
    }}>
      Przekierowywanie...
    </div>
  );
};

export default CarConfigurator;