import React, { useState, useEffect, useContext } from 'react';
import { 
  Mail, Lock, Eye, EyeOff, Facebook, Twitter, Linkedin, 
  AlertCircle, User, Check 
} from 'lucide-react';
import styles from './register.module.css';
import { NavLink, useNavigate } from 'react-router-dom';
import axiosInstance from '../../services/axiosConfig';
import { StoreContext } from '../../store/StoreContext';

const RegisterPage = () => {
  const { state } = useContext(StoreContext);
  const navigate = useNavigate();
  const [showPassword, setShowPassword] = useState(false);
  
  // Przekieruj jeśli użytkownik jest już zalogowany
  useEffect(() => {
    if (state.accessToken) {
      navigate('/home');
    }
  }, [state.accessToken, navigate]);
  const [showConfirmPassword, setShowConfirmPassword] = useState(false);
  const [formData, setFormData] = useState({
    firstName: '',
    lastName: '',
    email: '',
    password: '',
    confirmPassword: '',
    termsAccepted: false,
    marketingAccepted: false
  });
  const [errors, setErrors] = useState({});
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [registrationSuccess, setRegistrationSuccess] = useState(false);
  const [apiError, setApiError] = useState('');
  const [isFocused, setIsFocused] = useState({
    firstName: false,
    lastName: false,
    email: false,
    password: false,
    confirmPassword: false
  });

  const handleRegister = async function(body) {
    try {
      await axiosInstance.post('/register', body);
      
      setRegistrationSuccess(true);
      // Przekierowanie do strony logowania po 2 sekundach
      setTimeout(() => {
        navigate('/login');
      }, 2000);
    } catch(error) {
      setRegistrationSuccess(false);
      console.error('Registration error:', error);
      
      if (error.response?.data) {
        const errorData = error.response.data;
        // API zwraca obiekt z errorCode i errorMessage
        const errorMessage = errorData.errorMessage || errorData.detail || errorData.message || 'Błąd rejestracji';
        setApiError(errorMessage);
      } else {
        setApiError('Wystąpił błąd podczas rejestracji');
      }
      
      setIsSubmitting(false);
    }
  };

  // Obsługa zmian w formularzu
  const handleChange = (e) => {
    const { name, value, type, checked } = e.target;
    setFormData({
      ...formData,
      [name]: type === 'checkbox' ? checked : value
    });
  };

  // Obsługa focus dla pól
  const handleFocus = (field) => {
    setIsFocused(prev => ({ ...prev, [field]: true }));
  };

  const handleBlur = (field) => {
    setIsFocused(prev => ({ ...prev, [field]: false }));
  };

  // Walidacja hasła
  const validatePassword = (password) => {
    const errors = [];
    if (password.length < 6) {
      errors.push('Hasło musi zawierać co najmniej 6 znaków');
    }
    if (!/[A-Z]/.test(password)) {
      errors.push('Hasło musi zawierać co najmniej jedną wielką literę');
    }
    if (!/[a-z]/.test(password)) {
      errors.push('Hasło musi zawierać co najmniej jedną małą literę');
    }
    if (!/[0-9]/.test(password)) {
      errors.push('Hasło musi zawierać co najmniej jedną cyfrę');
    }
    return errors;
  };

  // Walidacja formularza
  const validateForm = () => {
    const newErrors = {};
    if (!formData.firstName.trim()) {
      newErrors.firstName = 'Imię jest wymagane';
    }
    if (!formData.lastName.trim()) {
      newErrors.lastName = 'Nazwisko jest wymagane';
    }
    if (!formData.email) {
      newErrors.email = 'Adres e-mail jest wymagany';
    } else if (!/\S+@\S+\.\S+/.test(formData.email)) {
      newErrors.email = 'Niepoprawny format adresu e-mail';
    }
    if (!formData.password) {
      newErrors.password = 'Hasło jest wymagane';
    } else {
      const passwordErrors = validatePassword(formData.password);
      if (passwordErrors.length > 0) {
        newErrors.password = passwordErrors;
      }
    }
    if (formData.password !== formData.confirmPassword) {
      newErrors.confirmPassword = 'Hasła nie są identyczne';
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
      await handleRegister({
        firstName: formData.firstName,
        lastName: formData.lastName,
        email: formData.email,
        password: formData.password,
        confirmPassword: formData.confirmPassword
      });
    }
  };

  // Sprawdzanie siły hasła
  const getPasswordStrength = (password) => {
    if (!password) return 0;
    let strength = 0;
    if (password.length >= 8) strength += 1;
    if (password.length >= 12) strength += 1;
    if (/[A-Z]/.test(password)) strength += 1;
    if (/[a-z]/.test(password)) strength += 1;
    if (/[0-9]/.test(password)) strength += 1;
    if (/[^A-Za-z0-9]/.test(password)) strength += 1;
    return Math.min(strength, 5);
  };

  const passwordStrength = getPasswordStrength(formData.password);
  
  const getPasswordStrengthText = () => {
    if (passwordStrength === 0) return 'Bardzo słabe';
    if (passwordStrength === 1) return 'Słabe';
    if (passwordStrength === 2) return 'Średnie';
    if (passwordStrength === 3) return 'Dobre';
    if (passwordStrength === 4) return 'Silne';
    return 'Bardzo silne';
  };
  
  const getPasswordStrengthColor = () => {
    if (passwordStrength <= 1) return '#ef4444';
    if (passwordStrength === 2) return '#f97316';
    if (passwordStrength === 3) return '#eab308';
    if (passwordStrength === 4) return '#22c55e';
    return '#16a34a';
  };

  return (
    <div className={styles.registerContainer}>
      {/* Główna zawartość */}
      <main className={styles.main}>
        {registrationSuccess ? (
          <div className={styles.successContainer}>
            <div className={styles.successIcon}>
              <Check size={48} />
            </div>
            <h2 className={styles.successTitle}>Rejestracja zakończona pomyślnie!</h2>
            <p className={styles.successMessage}>
              Dziękujemy za rejestrację w serwisie AutoKonfigurator. Na podany adres e-mail wysłaliśmy link aktywacyjny.
            </p>
            <NavLink to="/login" className={styles.successButton}>
              Przejdź do strony logowania
            </NavLink>
          </div>
        ) : (
          <div className={styles.formContainer}>
            {/* Baner */}
            <div className={styles.banner}>
              <h1 className={styles.bannerTitle}>Dołącz do nas</h1>
              <p className={styles.bannerSubtitle}>Stwórz swoje konto i rozpocznij personalizację</p>
            </div>
            
            {/* Wyświetlanie błędów z API */}
            {apiError && (
              <div className={styles.apiErrorContainer}>
                <AlertCircle size={18} />
                <p className={styles.apiErrorText}>{apiError}</p>
              </div>
            )}
            
            {/* Formularz rejestracji */}
            <form onSubmit={handleSubmit} className={styles.form}>
              <div className={styles.nameGrid}>
                <div className={styles.formGroup}>
                  <label htmlFor="firstName" className={styles.label}>Imię</label>
                  <div className={`${styles.inputWrapper} ${
                    isFocused.firstName ? styles.inputWrapperFocused : ''
                  } ${
                    errors.firstName ? styles.inputWrapperError : ''
                  }`}>
                    <input
                      id="firstName"
                      name="firstName"
                      type="text"
                      value={formData.firstName}
                      onChange={handleChange}
                      onFocus={() => handleFocus('firstName')}
                      onBlur={() => handleBlur('firstName')}
                      className={`${styles.input} ${errors.firstName ? styles.errorInput : ''}`}
                      placeholder="Jan"
                      autoComplete="given-name"
                    />
                    <User className={`${styles.inputIcon} ${
                      isFocused.firstName || formData.firstName ? styles.inputIconActive : ''
                    }`} size={18} />
                  </div>
                  {errors.firstName && (
                    <p className={styles.errorText}>
                      <AlertCircle size={14} /> {errors.firstName}
                    </p>
                  )}
                </div>
                
                <div className={styles.formGroup}>
                  <label htmlFor="lastName" className={styles.label}>Nazwisko</label>
                  <div className={`${styles.inputWrapper} ${
                    isFocused.lastName ? styles.inputWrapperFocused : ''
                  } ${
                    errors.lastName ? styles.inputWrapperError : ''
                  }`}>
                    <input
                      id="lastName"
                      name="lastName"
                      type="text"
                      value={formData.lastName}
                      onChange={handleChange}
                      onFocus={() => handleFocus('lastName')}
                      onBlur={() => handleBlur('lastName')}
                      className={`${styles.input} ${errors.lastName ? styles.errorInput : ''}`}
                      placeholder="Kowalski"
                      autoComplete="family-name"
                    />
                    <User className={`${styles.inputIcon} ${
                      isFocused.lastName || formData.lastName ? styles.inputIconActive : ''
                    }`} size={18} />
                  </div>
                  {errors.lastName && (
                    <p className={styles.errorText}>
                      <AlertCircle size={14} /> {errors.lastName}
                    </p>
                  )}
                </div>
              </div>

              {/* E-mail */}
              <div className={styles.formGroup}>
                <label htmlFor="email" className={styles.label}>Adres e-mail</label>
                <div className={`${styles.inputWrapper} ${
                  isFocused.email ? styles.inputWrapperFocused : ''
                } ${
                  errors.email ? styles.inputWrapperError : ''
                }`}>
                  <input
                    id="email"
                    name="email"
                    type="email"
                    value={formData.email}
                    onChange={handleChange}
                    onFocus={() => handleFocus('email')}
                    onBlur={() => handleBlur('email')}
                    className={`${styles.input} ${errors.email ? styles.errorInput : ''}`}
                    placeholder="twoj@email.com"
                    autoComplete="email"
                  />
                  <Mail className={`${styles.inputIcon} ${
                    isFocused.email || formData.email ? styles.inputIconActive : ''
                  }`} size={18} />
                </div>
                {errors.email && (
                  <p className={styles.errorText}>
                    <AlertCircle size={14} /> {errors.email}
                  </p>
                )}
              </div>

              {/* Hasło */}
              <div className={styles.formGroup}>
                <label htmlFor="password" className={styles.label}>Hasło</label>
                <div className={`${styles.inputWrapper} ${
                  isFocused.password ? styles.inputWrapperFocused : ''
                } ${
                  errors.password ? styles.inputWrapperError : ''
                }`}>
                  <input
                    id="password"
                    name="password"
                    type={showPassword ? 'text' : 'password'}
                    value={formData.password}
                    onChange={handleChange}
                    onFocus={() => handleFocus('password')}
                    onBlur={() => handleBlur('password')}
                    className={`${styles.input} ${errors.password ? styles.errorInput : ''}`}
                    placeholder="Wprowadź hasło"
                    autoComplete="new-password"
                  />
                  <Lock className={`${styles.inputIcon} ${
                    isFocused.password || formData.password ? styles.inputIconActive : ''
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
                
                {formData.password && (
                  <div className={styles.passwordStrength}>
                    <div className={styles.strengthLabel}>
                      <span>Siła hasła:</span>
                      <span style={{ color: getPasswordStrengthColor() }}>
                        {getPasswordStrengthText()}
                      </span>
                    </div>
                    <div className={styles.strengthBarContainer}>
                      <div
                        className={styles.strengthBar}
                        style={{ 
                          width: `${passwordStrength * 20}%`, 
                          backgroundColor: getPasswordStrengthColor() 
                        }}
                      ></div>
                    </div>
                  </div>
                )}
                
                {errors.password && Array.isArray(errors.password) ? (
                  <div className={styles.errorList}>
                    <div className={styles.errorLabel}>
                      <AlertCircle size={14} /> Twoje hasło nie spełnia wymagań:
                    </div>
                    <ul>
                      {errors.password.map((err, index) => (
                        <li key={`password-error-${index}-${err.substring(0, 10)}`}>{err}</li>
                      ))}
                    </ul>
                  </div>
                ) : errors.password ? (
                  <p className={styles.errorText}>
                    <AlertCircle size={14} /> {errors.password}
                  </p>
                ) : null}
              </div>

              {/* Potwierdzenie hasła */}
              <div className={styles.formGroup}>
                <label htmlFor="confirmPassword" className={styles.label}>Potwierdź hasło</label>
                <div className={`${styles.inputWrapper} ${
                  isFocused.confirmPassword ? styles.inputWrapperFocused : ''
                } ${
                  errors.confirmPassword ? styles.inputWrapperError : ''
                }`}>
                  <input
                    id="confirmPassword"
                    name="confirmPassword"
                    type={showConfirmPassword ? 'text' : 'password'}
                    value={formData.confirmPassword}
                    onChange={handleChange}
                    onFocus={() => handleFocus('confirmPassword')}
                    onBlur={() => handleBlur('confirmPassword')}
                    className={`${styles.input} ${errors.confirmPassword ? styles.errorInput : ''}`}
                    placeholder="Potwierdź hasło"
                    autoComplete="new-password"
                  />
                  <Lock className={`${styles.inputIcon} ${
                    isFocused.confirmPassword || formData.confirmPassword ? styles.inputIconActive : ''
                  }`} size={18} />
                  <button
                    type="button"
                    className={styles.passwordToggle}
                    onClick={() => setShowConfirmPassword(!showConfirmPassword)}
                    aria-label={showConfirmPassword ? 'Ukryj hasło' : 'Pokaż hasło'}
                  >
                    {showConfirmPassword ? <EyeOff size={18} /> : <Eye size={18} />}
                  </button>
                </div>
                {errors.confirmPassword && (
                  <p className={styles.errorText}>
                    <AlertCircle size={14} /> {errors.confirmPassword}
                  </p>
                )}
              </div>
              
              {/* Przycisk rejestracji */}
              <div className={styles.submitGroup}>
                <button
                  type="submit"
                  disabled={isSubmitting}
                  className={`${styles.submitButton} ${isSubmitting ? styles.disabledButton : ''}`}
                >
                  {isSubmitting ? 'Przetwarzanie...' : 'Zarejestruj się'}
                </button>
              </div>
            </form>

            {/* Link do logowania */}
            <div className={styles.loginLinkSection}>
              <p className={styles.loginLinkText}>
                Masz już konto? <NavLink className={styles.loginLink} to="/login">Zaloguj się</NavLink>
              </p>
            </div>
          </div>
        )}
      </main>
    </div>
  );
};

export default RegisterPage;
