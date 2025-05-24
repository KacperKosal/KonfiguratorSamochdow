import styles from './Header.module.css';
import { NavLink } from 'react-router-dom';
import React, { useContext, useEffect, useState } from 'react';
import { StoreContext } from '../../store/StoreContext';
import { ChevronDown, User, Settings, Car, LogOut, X, Lock, Eye, EyeOff } from 'lucide-react';

const Header = () => {
  const { state, dispatch } = useContext(StoreContext);
  const [isLogged, setIsLogged] = useState(false);
  const [isSidebarOpen, setIsSidebarOpen] = useState(false);
  const [isPasswordModalOpen, setIsPasswordModalOpen] = useState(false);
  const [passwordForm, setPasswordForm] = useState({
    currentPassword: '',
    newPassword: '',
    confirmPassword: ''
  });
  const [showPasswords, setShowPasswords] = useState({
    current: false,
    new: false,
    confirm: false
  });
  const [passwordErrors, setPasswordErrors] = useState({});
  const [isSubmitting, setIsSubmitting] = useState(false);

  const apiUrl = import.meta.env.VITE_API_URL;

  useEffect(() => {
    if (state.accessToken) setIsLogged(true);
    else setIsLogged(false);
  }, [state.accessToken])

  const handleLogout = () => {
    dispatch({ type: 'LOGOUT' });
    setIsSidebarOpen(false);
  };

  const toggleSidebar = () => {
    setIsSidebarOpen(!isSidebarOpen);
  };

  const closeSidebar = () => {
    setIsSidebarOpen(false);
  };

  const openPasswordModal = () => {
    setIsPasswordModalOpen(true);
    setPasswordForm({
      currentPassword: '',
      newPassword: '',
      confirmPassword: ''
    });
    setPasswordErrors({});
  };

  const closePasswordModal = () => {
    setIsPasswordModalOpen(false);
    setPasswordForm({
      currentPassword: '',
      newPassword: '',
      confirmPassword: ''
    });
    setPasswordErrors({});
  };

  const handlePasswordInputChange = (field, value) => {
    setPasswordForm(prev => ({
      ...prev,
      [field]: value
    }));
    // Usuń błąd dla tego pola gdy użytkownik zaczyna pisać
    if (passwordErrors[field]) {
      setPasswordErrors(prev => ({
        ...prev,
        [field]: ''
      }));
    }
  };

  const togglePasswordVisibility = (field) => {
    setShowPasswords(prev => ({
      ...prev,
      [field]: !prev[field]
    }));
  };

  const validatePasswordForm = () => {
    const errors = {};

    if (!passwordForm.currentPassword) {
      errors.currentPassword = 'Wymagane jest podanie obecnego hasła';
    }

    if (!passwordForm.newPassword) {
      errors.newPassword = 'Wymagane jest podanie nowego hasła';
    } else if (passwordForm.newPassword.length < 6) {
      errors.newPassword = 'Nowe hasło musi zawierać co najmniej 6 znaków';
    }

    if (!passwordForm.confirmPassword) {
      errors.confirmPassword = 'Wymagane jest potwierdzenie nowego hasła';
    } else if (passwordForm.newPassword !== passwordForm.confirmPassword) {
      errors.confirmPassword = 'Hasła nie są identyczne';
    }

    if (passwordForm.currentPassword === passwordForm.newPassword) {
      errors.newPassword = 'Nowe hasło musi być różne od obecnego';
    }

    setPasswordErrors(errors);
    return Object.keys(errors).length === 0;
  };

  const handlePasswordSubmit = async (e) => {
    e.preventDefault();
    
    if (!validatePasswordForm()) {
      return;
    }

    setIsSubmitting(true);

    try {
      const response = await fetch(`${apiUrl}/change-password`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${state.accessToken}`
        },
        body: JSON.stringify({
          currentPassword: passwordForm.currentPassword,
          newPassword: passwordForm.newPassword
        })
      });

      const data = await response.json();

      if (response.ok) {
        alert('Hasło zostało pomyślnie zmienione!');
        closePasswordModal();
      } else {
        // Obsługa błędów z serwera
        if (data.message === 'Invalid current password') {
          setPasswordErrors({ currentPassword: 'Nieprawidłowe obecne hasło' });
        } else {
          alert('Wystąpił błąd podczas zmiany hasła: ' + (data.message || 'Nieznany błąd'));
        }
      }
    } catch (error) {
      console.error('Error changing password:', error);
      alert('Wystąpił błąd podczas zmiany hasła. Spróbuj ponownie.');
    } finally {
      setIsSubmitting(false);
    }
  };

  // Zablokuj scroll gdy sidebar lub modal jest otwarty
  useEffect(() => {
    if (isSidebarOpen || isPasswordModalOpen) {
      document.body.style.overflow = 'hidden';
    } else {
      document.body.style.overflow = 'unset';
    }

    return () => {
      document.body.style.overflow = 'unset';
    };
  }, [isSidebarOpen, isPasswordModalOpen]);

  console.log(state)
  return (
    <>
      <header className={styles.header}>
        <div className={styles.headerContainer}>
          <div className={styles.headerLogo}>AutoKonfigurator</div>
          <nav>
            <ul className={styles.navList}>
              <li className={styles.navItem}><NavLink to="/home">Modele</NavLink></li>
              <li className={styles.navItem}>Salony</li>
              <li className={styles.navItem}>Finansowanie</li>
              <li className={styles.navItem}><NavLink to="/contact">Kontakt</NavLink></li>
              {!isLogged && (
                <li className={styles.navItem}><NavLink to="/login">Logowanie</NavLink></li>
              )}
              {isLogged && (
                <li className={styles.navItem}>
                  <button 
                    onClick={toggleSidebar} 
                    className={styles.userButton}
                  >
                    <User size={18} />
                    <span>Moje konto</span>
                  </button>
                </li>
              )}
            </ul>
          </nav>
        </div>
      </header>

      {/* Overlay */}
      {(isSidebarOpen || isPasswordModalOpen) && (
        <div className={styles.overlay} onClick={isSidebarOpen ? closeSidebar : closePasswordModal}></div>
      )}

      {/* Sidebar */}
      <div className={`${styles.sidebar} ${isSidebarOpen ? styles.sidebarOpen : ''}`}>
        <div className={styles.sidebarHeader}>
          <div className={styles.sidebarTitle}>
            <User size={24} />
            <span>Moje konto</span>
          </div>
          <button onClick={closeSidebar} className={styles.closeButton}>
            <X size={20} />
          </button>
        </div>

        <div className={styles.sidebarContent}>
          <div className={styles.sectionTitle}>Twoje konfiguracje</div>
          
          <div className={styles.configList}>
            <div className={styles.configItem}>
              <Car size={20} />
              <div className={styles.configInfo}>
                <div className={styles.configName}>BMW X5 2024</div>
                <div className={styles.configDate}>Zapisano: 15.01.2024</div>
                <div className={styles.configPrice}>Cena: 450,000 zł</div>
              </div>
            </div>
            
            <div className={styles.configItem}>
              <Car size={20} />
              <div className={styles.configInfo}>
                <div className={styles.configName}>BMW M5 Sport</div>
                <div className={styles.configDate}>Zapisano: 12.01.2024</div>
                <div className={styles.configPrice}>Cena: 280,000 zł</div>
              </div>
            </div>
            
            <div className={styles.configItem}>
              <Car size={20} />
              <div className={styles.configInfo}>
                <div className={styles.configName}>BMW E46</div>
                <div className={styles.configDate}>Zapisano: 08.01.2024</div>
                <div className={styles.configPrice}>Cena: 320,000 zł</div>
              </div>
            </div>
          </div>
        </div>

        <div className={styles.sidebarFooter}>
          <button onClick={openPasswordModal} className={styles.settingsButton}>
            <Settings size={18} />
            <span>Ustawienia konta</span>
          </button>
          
          <button onClick={handleLogout} className={styles.logoutButton}>
            <LogOut size={18} />
            <span>Wyloguj się</span>
          </button>
        </div>
      </div>

      {/* Modal zmiany hasła */}
      {isPasswordModalOpen && (
        <div className={styles.passwordModal}>
          <div className={styles.modalContent}>
            <div className={styles.modalHeader}>
              <h2 className={styles.modalTitle}>
                <Lock size={20} />
                Zmiana hasła
              </h2>
              <button onClick={closePasswordModal} className={styles.closeButton}>
                <X size={20} />
              </button>
            </div>

            <form onSubmit={handlePasswordSubmit} className={styles.passwordForm}>
              {/* Obecne hasło */}
              <div className={styles.formGroup}>
                <label className={styles.label}>Obecne hasło</label>
                <div className={styles.inputWrapper}>
                  <input
                    type={showPasswords.current ? 'text' : 'password'}
                    value={passwordForm.currentPassword}
                    onChange={(e) => handlePasswordInputChange('currentPassword', e.target.value)}
                    className={`${styles.input} ${passwordErrors.currentPassword ? styles.errorInput : ''}`}
                    placeholder="Wprowadź obecne hasło"
                  />
                  <button
                    type="button"
                    className={styles.passwordToggle}
                    onClick={() => togglePasswordVisibility('current')}
                  >
                    {showPasswords.current ? <EyeOff size={18} /> : <Eye size={18} />}
                  </button>
                </div>
                {passwordErrors.currentPassword && (
                  <p className={styles.errorText}>{passwordErrors.currentPassword}</p>
                )}
              </div>

              {/* Nowe hasło */}
              <div className={styles.formGroup}>
                <label className={styles.label}>Nowe hasło</label>
                <div className={styles.inputWrapper}>
                  <input
                    type={showPasswords.new ? 'text' : 'password'}
                    value={passwordForm.newPassword}
                    onChange={(e) => handlePasswordInputChange('newPassword', e.target.value)}
                    className={`${styles.input} ${passwordErrors.newPassword ? styles.errorInput : ''}`}
                    placeholder="Wprowadź nowe hasło"
                  />
                  <button
                    type="button"
                    className={styles.passwordToggle}
                    onClick={() => togglePasswordVisibility('new')}
                  >
                    {showPasswords.new ? <EyeOff size={18} /> : <Eye size={18} />}
                  </button>
                </div>
                {passwordErrors.newPassword && (
                  <p className={styles.errorText}>{passwordErrors.newPassword}</p>
                )}
              </div>

              {/* Potwierdzenie nowego hasła */}
              <div className={styles.formGroup}>
                <label className={styles.label}>Potwierdź nowe hasło</label>
                <div className={styles.inputWrapper}>
                  <input
                    type={showPasswords.confirm ? 'text' : 'password'}
                    value={passwordForm.confirmPassword}
                    onChange={(e) => handlePasswordInputChange('confirmPassword', e.target.value)}
                    className={`${styles.input} ${passwordErrors.confirmPassword ? styles.errorInput : ''}`}
                    placeholder="Potwierdź nowe hasło"
                  />
                  <button
                    type="button"
                    className={styles.passwordToggle}
                    onClick={() => togglePasswordVisibility('confirm')}
                  >
                    {showPasswords.confirm ? <EyeOff size={18} /> : <Eye size={18} />}
                  </button>
                </div>
                {passwordErrors.confirmPassword && (
                  <p className={styles.errorText}>{passwordErrors.confirmPassword}</p>
                )}
              </div>

              <div className={styles.modalActions}>
                <button
                  type="button"
                  onClick={closePasswordModal}
                  className={styles.cancelButton}
                >
                  Anuluj
                </button>
                <button
                  type="submit"
                  disabled={isSubmitting}
                  className={`${styles.submitButton} ${isSubmitting ? styles.disabledButton : ''}`}
                >
                  {isSubmitting ? 'Zmieniam...' : 'Zmień hasło'}
                </button>
              </div>
            </form>
          </div>
        </div>
      )}
    </>
  );
};

export default Header;
