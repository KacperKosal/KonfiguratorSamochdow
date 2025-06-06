import React, { useContext, useState, useEffect } from 'react';
import { Mail, Lock, Eye, EyeOff, AlertCircle } from 'lucide-react';
import styles from './Login.module.css';
import { NavLink, useNavigate } from 'react-router-dom';
import { StoreContext } from '../../store/StoreContext';
import axiosInstance from '../../services/axiosConfig';

const LoginPage = () => {
  const { state, dispatch } = useContext(StoreContext);
  const navigate = useNavigate();
  const [showPassword, setShowPassword] = useState(false);
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [errors, setErrors] = useState({});
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [isFocused, setIsFocused] = useState({ email: false, password: false });
  const [apiError, setApiError] = useState('');

  // Przekieruj jeśli użytkownik jest już zalogowany
  useEffect(() => {
    if (state.accessToken) {
      navigate('/home');
    }
  }, [state.accessToken, navigate]);

  const handleLogin = async function(body) {
    try {
      const response = await axiosInstance.post('/login', body);
      
      // API zwraca token jako tekst
      const token = response.data;
      dispatch({ type: 'SET_TOKEN', payload: token });
      
      // Przekierowanie do strony głównej z modelami
      navigate('/home');
    } catch(error) {
      console.error('Login error:', error);
      
      if (error.response?.data) {
        const errorData = error.response.data;
        // API zwraca obiekt z errorCode i errorMessage
        const errorMessage = errorData.errorMessage || errorData.detail || errorData.message || 'Błąd logowania';
        setApiError(errorMessage);
      } else {
        setApiError('Wystąpił błąd podczas logowania');
      }
      
      setIsSubmitting(false);
    }
  };

  // Walidacja formularza
  const validateForm = () => {
    const newErrors = {};

    if (!email) {
      newErrors.email = 'Wymagane jest podanie adresu e-mail';
    } else if (!/\S+@\S+\.\S+/.test(email)) {
      newErrors.email = 'Niepoprawny format adresu e-mail';
    }

    if (!password) {
      newErrors.password = 'Wymagane jest podanie hasła';
    } else if (password.length < 6) {
      newErrors.password = 'Hasło musi zawierać co najmniej 6 znaków';
    }

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  // Wysyłanie formularza
  const handleSubmit = async (e) => {
    e.preventDefault();
    setApiError(''); // Czyszczenie poprzednich błędów API

    if (validateForm()) {
      setIsSubmitting(true);
      await handleLogin({
        email: email,
        password: password
      });
    }
  };

  // Obsługa focus dla pól
  const handleFocus = (field) => {
    setIsFocused(prev => ({ ...prev, [field]: true }));
  };

  const handleBlur = (field) => {
    setIsFocused(prev => ({ ...prev, [field]: false }));
  };

  return (
    <div className={styles.loginContainer}>
      {/* Główna zawartość strony */}
      <main className={styles.main}>
        <div className={styles.formContainer}>
          
          {/* Baner */}
          <div className={styles.banner}>
            <h1 className={styles.bannerTitle}>Witaj!</h1>
            <p className={styles.bannerSubtitle}>Zaloguj się do swojego konta</p>
          </div>

          {/* Wyświetlanie błędów z API */}
          {apiError && (
            <div className={styles.apiErrorContainer}>
              <AlertCircle size={18} />
              <p className={styles.apiErrorText}>{apiError}</p>
            </div>
          )}

          {/* Formularz logowania */}
          <form onSubmit={handleSubmit} className={styles.form}>
            
            {/* Pole e-mail */}
            <div className={styles.formGroup}>
              <label htmlFor="email" className={styles.label}>Adres e-mail</label>
              <div className={`${styles.inputWrapper} ${
                isFocused.email ? styles.inputWrapperFocused : ''
              } ${
                errors.email ? styles.inputWrapperError : ''
              }`}>
                <input
                  id="email"
                  type="email"
                  value={email}
                  onChange={(e) => setEmail(e.target.value)}
                  onFocus={() => handleFocus('email')}
                  onBlur={() => handleBlur('email')}
                  className={`${styles.input} ${errors.email ? styles.errorInput : ''}`}
                  placeholder="twoj@email.com"
                  autoComplete="email"
                />
                <Mail className={`${styles.inputIcon} ${
                  isFocused.email || email ? styles.inputIconActive : ''
                }`} size={18} />
              </div>
              {errors.email && (
                <p className={styles.errorText}>
                  <AlertCircle size={14} /> {errors.email}
                </p>
              )}
            </div>

            {/* Pole hasło */}
            <div className={styles.formGroup}>
              <div className={styles.labelWrapper}>
                <label htmlFor="password" className={styles.label}>Hasło</label>
              </div>
              <div className={`${styles.inputWrapper} ${
                isFocused.password ? styles.inputWrapperFocused : ''
              } ${
                errors.password ? styles.inputWrapperError : ''
              }`}>
                <input
                  id="password"
                  type={showPassword ? 'text' : 'password'}
                  value={password}
                  onChange={(e) => setPassword(e.target.value)}
                  onFocus={() => handleFocus('password')}
                  onBlur={() => handleBlur('password')}
                  className={`${styles.input} ${errors.password ? styles.errorInput : ''}`}
                  placeholder="Wprowadź hasło"
                  autoComplete="current-password"
                />
                <Lock className={`${styles.inputIcon} ${
                  isFocused.password || password ? styles.inputIconActive : ''
                }`} size={18} />
                <button
                  type="button"
                  className={styles.passwordToggle}
                  onClick={() => setShowPassword(!showPassword)}
                  aria-label={showPassword ? 'Ukryj hasło' : 'Pokaż hasło'}
                >
                  {showPassword ? <EyeOff size={18} /> : <Eye size={18} />}
                </button>
              </div>
              {errors.password && (
                <p className={styles.errorText}>
                  <AlertCircle size={14} /> {errors.password}
                </p>
              )}
            </div>

            {/* Przycisk logowania */}
            <div>
              <button
                type="submit"
                disabled={isSubmitting}
                className={`${styles.submitButton} ${isSubmitting ? styles.disabledButton : ''}`}
              >
                {isSubmitting ? 'Logowanie...' : 'Zaloguj się'}
              </button>
            </div>
          </form>

          {/* Link do rejestracji */}
          <div className={styles.registerSection}>
            <p className={styles.registerText}>
              Nie masz jeszcze konta?{' '}
              <NavLink className={styles.registerLink} to="/register">Zarejestruj się</NavLink>
            </p>
          </div>
        </div>
      </main>
    </div>
  );
};

export default LoginPage;
