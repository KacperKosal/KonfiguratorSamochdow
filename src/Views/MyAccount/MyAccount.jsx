import React, { useState, useEffect } from 'react';
import { useStore } from '../../store/useStore';
import axiosInstance from '../../services/axiosConfig';
import { useNavigate } from 'react-router-dom';
import styles from './MyAccount.module.css';

const MyAccount = () => {
  const { accessToken, user, dispatch } = useStore();
  const navigate = useNavigate();
  const [configurations, setConfigurations] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  const apiUrl = import.meta.env.VITE_API_URL || 'https://localhost:7001';

  useEffect(() => {
    fetchUserConfigurations();
  }, []);

  const fetchUserConfigurations = async () => {
    if (!accessToken) {
      setError('Musisz być zalogowany, aby zobaczyć swoje konfiguracje');
      setLoading(false);
      return;
    }

    try {
      setLoading(true);
      
      const response = await axiosInstance.get('/api/user-configurations');
      setConfigurations(response.data);
    } catch (err) {
      console.error('Błąd pobierania konfiguracji:', err);
      if (err.response?.status === 401) {
        setError('Sesja wygasła. Zaloguj się ponownie.');
      } else {
        setError('Wystąpił błąd podczas ładowania konfiguracji');
      }
    } finally {
      setLoading(false);
    }
  };

  const deleteConfiguration = async (configurationId, configurationName) => {
    if (!confirm(`Czy na pewno chcesz usunąć konfigurację "${configurationName}"?`)) {
      return;
    }

    try {
      await axiosInstance.delete(`/api/user-configurations/${configurationId}`);
      alert('Konfiguracja została usunięta');
      // Odśwież listę konfiguracji
      fetchUserConfigurations();
    } catch (err) {
      console.error('Błąd usuwania konfiguracji:', err);
      alert('Wystąpił błąd podczas usuwania konfiguracji');
    }
  };

  const formatPrice = (price) => {
    return new Intl.NumberFormat('pl-PL', {
      style: 'currency',
      currency: 'PLN',
      minimumFractionDigits: 0
    }).format(price);
  };

  const formatDate = (dateString) => {
    return new Date(dateString).toLocaleDateString('pl-PL', {
      year: 'numeric',
      month: 'long',
      day: 'numeric',
      hour: '2-digit',
      minute: '2-digit'
    });
  };

  const handleLogout = () => {
    dispatch({ type: 'LOGOUT' });
    navigate('/home');
  };

  if (loading) {
    return (
      <div className={styles.myAccount}>
        <div className={styles.container}>
          <h1 className={styles.pageTitle}>Moje Konto</h1>
          <div className={styles.loading}>Ładowanie konfiguracji...</div>
        </div>
      </div>
    );
  }

  if (error) {
    return (
      <div className={styles.myAccount}>
        <div className={styles.container}>
          <h1 className={styles.pageTitle}>Moje Konto</h1>
          <div className={styles.error}>
            <p>{error}</p>
            <button 
              onClick={fetchUserConfigurations}
              className={styles.retryButton}
            >
              Spróbuj ponownie
            </button>
          </div>
        </div>
      </div>
    );
  }

  return (
    <div className={styles.myAccount}>
      <div className={styles.container}>
        <h1 className={styles.pageTitle}>Moje Konto</h1>
        
        {user && (
          <div className={styles.userInfo}>
            <div className={styles.userHeader}>
              <div>
                <h2>Witaj{user.name ? `, ${user.name}` : ''}!</h2>
                {user.email && <p>Email: {user.email}</p>}
              </div>
              <button 
                onClick={handleLogout}
                className={styles.logoutButton}
              >
                Wyloguj się
              </button>
            </div>
          </div>
        )}

        <div className={styles.configurationsSection}>
          <h2 className={styles.sectionTitle}>Moje Konfiguracje ({configurations.length})</h2>
          
          {configurations.length === 0 ? (
            <div className={styles.noConfigurations}>
              <p>Nie masz jeszcze żadnych zapisanych konfiguracji.</p>
              <p>Przejdź do <a href="/car-configurator">konfiguratora</a>, aby stworzyć swoją pierwszą konfigurację!</p>
            </div>
          ) : (
            <div className={styles.configurationsGrid}>
              {configurations.map((config) => (
                <div key={config.id} className={styles.configurationCard}>
                  <div className={styles.configHeader}>
                    <h3 className={styles.configName}>{config.configurationName}</h3>
                    <div className={styles.configPrice}>{formatPrice(config.totalPrice)}</div>
                  </div>
                  
                  <div className={styles.configDetails}>
                    <div className={styles.configDetail}>
                      <strong>Model:</strong> {config.carModelName}
                    </div>
                    {config.engineName && (
                      <div className={styles.configDetail}>
                        <strong>Silnik:</strong> {config.engineName}
                      </div>
                    )}
                    <div className={styles.configDetail}>
                      <strong>Kolor:</strong> 
                      <span className={styles.colorInfo}>
                        <div 
                          className={styles.colorSquare}
                          style={{ backgroundColor: config.exteriorColor }}
                        ></div>
                        {config.exteriorColorName}
                      </span>
                    </div>
                    {config.selectedAccessories && config.selectedAccessories.length > 0 && (
                      <div className={styles.configDetail}>
                        <strong>Akcesoria:</strong> {config.selectedAccessories.length} wybranych
                      </div>
                    )}
                    {config.selectedInteriorEquipment && config.selectedInteriorEquipment.length > 0 && (
                      <div className={styles.configDetail}>
                        <strong>Wyposażenie wnętrza:</strong> {config.selectedInteriorEquipment.length} wybranych
                      </div>
                    )}
                  </div>
                  
                  <div className={styles.configFooter}>
                    <div className={styles.configDate}>
                      Utworzona: {formatDate(config.createdAt)}
                    </div>
                    <div className={styles.configActions}>
                      <button
                        className={styles.deleteBtn}
                        onClick={() => deleteConfiguration(config.id, config.configurationName)}
                      >
                        Usuń
                      </button>
                    </div>
                  </div>
                </div>
              ))}
            </div>
          )}
        </div>
      </div>
    </div>
  );
};

export default MyAccount;