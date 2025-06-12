import React, { useState, useEffect } from 'react';
import { useStore } from '../../store/useStore';
import axiosInstance from '../../services/axiosConfig';
import { useNavigate } from 'react-router-dom';
import { Eye, EyeOff } from 'lucide-react';
import styles from './MyAccount.module.css';

const MyAccount = () => {
  const { accessToken, user, dispatch } = useStore();
  const navigate = useNavigate();
  const [configurations, setConfigurations] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [showChangePassword, setShowChangePassword] = useState(false);
  const [passwordData, setPasswordData] = useState({
    currentPassword: '',
    newPassword: '',
    confirmPassword: ''
  });
  const [passwordError, setPasswordError] = useState(null);
  const [passwordLoading, setPasswordLoading] = useState(false);
  const [showPasswords, setShowPasswords] = useState({
    currentPassword: false,
    newPassword: false,
    confirmPassword: false
  });

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

  const validatePasswordData = () => {
    const errors = [];
    
    if (!passwordData.currentPassword.trim()) {
      errors.push('Obecne hasło jest wymagane');
    }
    
    if (!passwordData.newPassword.trim()) {
      errors.push('Nowe hasło jest wymagane');
    }
    
    if (!passwordData.confirmPassword.trim()) {
      errors.push('Potwierdzenie hasła jest wymagane');
    }
    
    if (passwordData.newPassword.length < 6) {
      errors.push('Nowe hasło musi mieć co najmniej 6 znaków');
    }
    
    if (passwordData.newPassword.length > 128) {
      errors.push('Nowe hasło nie może być dłuższe niż 128 znaków');
    }
    
    if (passwordData.newPassword !== passwordData.confirmPassword) {
      errors.push('Nowe hasła nie są identyczne');
    }
    
    if (passwordData.currentPassword === passwordData.newPassword) {
      errors.push('Nowe hasło musi być różne od obecnego hasła');
    }
    
    // Sprawdzenie siły hasła
    const passwordStrengthRegex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]/;
    if (passwordData.newPassword.length >= 6 && !passwordStrengthRegex.test(passwordData.newPassword)) {
      errors.push('Nowe hasło powinno zawierać co najmniej jedną małą literę, jedną wielką literę, jedną cyfrę i jeden znak specjalny');
    }
    
    return errors;
  };

  const handlePasswordChange = async (e) => {
    e.preventDefault();
    
    const validationErrors = validatePasswordData();
    if (validationErrors.length > 0) {
      setPasswordError(validationErrors.join('. '));
      return;
    }

    try {
      setPasswordLoading(true);
      setPasswordError(null);

      await axiosInstance.post('/change-password', {
        currentPassword: passwordData.currentPassword,
        newPassword: passwordData.newPassword
      });

      alert('Hasło zostało zmienione pomyślnie');
      setShowChangePassword(false);
      setPasswordData({
        currentPassword: '',
        newPassword: '',
        confirmPassword: ''
      });
      setShowPasswords({
        currentPassword: false,
        newPassword: false,
        confirmPassword: false
      });
    } catch (err) {
      console.error('Błąd zmiany hasła:', err);
      
      let errorMessage = 'Wystąpił błąd podczas zmiany hasła';
      
      if (err.response?.data) {
        // Jeśli API zwraca obiekt z errorMessage (błędy zmiany hasła)
        if (err.response.data.errorMessage) {
          errorMessage = err.response.data.errorMessage;
        }
        // Jeśli API zwraca JSON z message
        else if (err.response.data.message) {
          errorMessage = err.response.data.message;
        }
        // Jeśli API zwraca string JSON (potrzebuje parsowania)
        else if (typeof err.response.data === 'string') {
          try {
            const parsed = JSON.parse(err.response.data);
            if (parsed.errorMessage) {
              errorMessage = parsed.errorMessage;
            } else if (parsed.message) {
              errorMessage = parsed.message;
            } else {
              errorMessage = err.response.data;
            }
          } catch {
            errorMessage = err.response.data;
          }
        }
        // Jeśli API zwraca obiekt z errors (walidacja)
        else if (err.response.data.errors) {
          const errors = Object.values(err.response.data.errors).flat();
          errorMessage = errors.join('. ');
        }
        // Jeśli API zwraca array błędów walidacji
        else if (Array.isArray(err.response.data)) {
          const messages = err.response.data.map(error => error.Message || error.message || error.errorMessage).filter(Boolean);
          if (messages.length > 0) {
            errorMessage = messages.join('. ');
          }
        }
      } 
      // Fallback dla kodów statusu
      else if (err.response?.status === 401) {
        errorMessage = 'Sesja wygasła. Zaloguj się ponownie.';
      } else if (err.response?.status === 400) {
        errorMessage = 'Nieprawidłowe dane formularza';
      }
      
      setPasswordError(errorMessage);
    } finally {
      setPasswordLoading(false);
    }
  };

  const handlePasswordInputChange = (field, value) => {
    setPasswordData(prev => ({
      ...prev,
      [field]: value
    }));
    setPasswordError(null);
  };

  const togglePasswordVisibility = (field) => {
    setShowPasswords(prev => ({
      ...prev,
      [field]: !prev[field]
    }));
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
              <div className={styles.userActions}>
                <button 
                  onClick={() => setShowChangePassword(!showChangePassword)}
                  className={styles.changePasswordButton}
                >
                  Zmień hasło
                </button>
                <button 
                  onClick={handleLogout}
                  className={styles.logoutButton}
                >
                  Wyloguj się
                </button>
              </div>
            </div>
          </div>
        )}

        {showChangePassword && (
          <div className={styles.changePasswordSection}>
            <h2 className={styles.sectionTitle}>Zmiana hasła</h2>
            <form onSubmit={handlePasswordChange} className={styles.passwordForm}>
              {passwordError && (
                <div className={styles.passwordError}>
                  {passwordError}
                </div>
              )}
              
              <div className={styles.inputGroup}>
                <label htmlFor="currentPassword">Obecne hasło:</label>
                <div className={styles.passwordInputWrapper}>
                  <input
                    type={showPasswords.currentPassword ? "text" : "password"}
                    id="currentPassword"
                    value={passwordData.currentPassword}
                    onChange={(e) => handlePasswordInputChange('currentPassword', e.target.value)}
                    required
                    disabled={passwordLoading}
                    className={styles.passwordInput}
                  />
                  <button
                    type="button"
                    className={styles.passwordToggle}
                    onClick={() => togglePasswordVisibility('currentPassword')}
                    disabled={passwordLoading}
                    title={showPasswords.currentPassword ? "Ukryj hasło" : "Pokaż hasło"}
                  >
                    {showPasswords.currentPassword ? <EyeOff size={20} /> : <Eye size={20} />}
                  </button>
                </div>
              </div>

              <div className={styles.inputGroup}>
                <label htmlFor="newPassword">Nowe hasło:</label>
                <div className={styles.passwordInputWrapper}>
                  <input
                    type={showPasswords.newPassword ? "text" : "password"}
                    id="newPassword"
                    value={passwordData.newPassword}
                    onChange={(e) => handlePasswordInputChange('newPassword', e.target.value)}
                    required
                    disabled={passwordLoading}
                    minLength="6"
                    maxLength="128"
                    placeholder="Minimum 6 znaków"
                    className={styles.passwordInput}
                  />
                  <button
                    type="button"
                    className={styles.passwordToggle}
                    onClick={() => togglePasswordVisibility('newPassword')}
                    disabled={passwordLoading}
                    title={showPasswords.newPassword ? "Ukryj hasło" : "Pokaż hasło"}
                  >
                    {showPasswords.newPassword ? <EyeOff size={20} /> : <Eye size={20} />}
                  </button>
                </div>
              </div>

              <div className={styles.inputGroup}>
                <label htmlFor="confirmPassword">Potwierdź nowe hasło:</label>
                <div className={styles.passwordInputWrapper}>
                  <input
                    type={showPasswords.confirmPassword ? "text" : "password"}
                    id="confirmPassword"
                    value={passwordData.confirmPassword}
                    onChange={(e) => handlePasswordInputChange('confirmPassword', e.target.value)}
                    required
                    disabled={passwordLoading}
                    minLength="6"
                    maxLength="128"
                    className={styles.passwordInput}
                  />
                  <button
                    type="button"
                    className={styles.passwordToggle}
                    onClick={() => togglePasswordVisibility('confirmPassword')}
                    disabled={passwordLoading}
                    title={showPasswords.confirmPassword ? "Ukryj hasło" : "Pokaż hasło"}
                  >
                    {showPasswords.confirmPassword ? <EyeOff size={20} /> : <Eye size={20} />}
                  </button>
                </div>
              </div>

              <div className={styles.passwordFormActions}>
                <button
                  type="button"
                  onClick={() => {
                    setShowChangePassword(false);
                    setPasswordData({
                      currentPassword: '',
                      newPassword: '',
                      confirmPassword: ''
                    });
                    setPasswordError(null);
                    setShowPasswords({
                      currentPassword: false,
                      newPassword: false,
                      confirmPassword: false
                    });
                  }}
                  className={styles.cancelButton}
                  disabled={passwordLoading}
                >
                  Anuluj
                </button>
                <button
                  type="submit"
                  className={styles.submitButton}
                  disabled={passwordLoading}
                >
                  {passwordLoading ? 'Zmieniam...' : 'Zmień hasło'}
                </button>
              </div>
            </form>
          </div>
        )}

        <div className={styles.configurationsSection}>
          <h2 className={styles.sectionTitle}>Moje Konfiguracje ({configurations.length})</h2>
          
          {configurations.length === 0 ? (
            <div className={styles.noConfigurations}>
              <p>Nie masz jeszcze żadnych zapisanych konfiguracji. Przejdź do <a href="/home">modeli</a>, aby wybrać i stworzyć swoją pierwszą konfigurację!</p>
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
                        className={styles.viewBtn}
                        onClick={() => {
                          navigate('/car-configurator', { 
                            state: { 
                              configurationId: config.id
                            } 
                          });
                        }}
                      >
                        Wyświetl
                      </button>
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