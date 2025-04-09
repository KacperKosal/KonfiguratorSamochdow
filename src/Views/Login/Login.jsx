import React, { useState } from 'react';
import { Mail, Lock, Eye, EyeOff,} from 'lucide-react';
import styles from './Login.module.css';

const LoginPage = () => {
  const [showPassword, setShowPassword] = useState(false);
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [rememberMe, setRememberMe] = useState(false);
  const [errors, setErrors] = useState({});
  const [isSubmitting, setIsSubmitting] = useState(false);

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
  const handleSubmit = (e) => {
    e.preventDefault();

    if (validateForm()) {
      setIsSubmitting(true);
      // Symulacja wysyłania danych
      setTimeout(() => {
        setIsSubmitting(false);
        alert('Zalogowano pomyślnie!');
      }, 1500);
    }
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

          {/* Formularz logowania */}
          <form onSubmit={handleSubmit} className={styles.form}>
            
            {/* Pole e-mail */}
            <div className={styles.formGroup}>
              <label htmlFor="email" className={styles.label}>Adres e-mail</label>
              <div className={styles.inputWrapper}>
                <input
                  id="email"
                  type="email"
                  value={email}
                  onChange={(e) => setEmail(e.target.value)}
                  className={`${styles.input} ${errors.email ? styles.errorInput : ''}`}
                  placeholder="twoj@email.com"
                />
                <Mail className={styles.inputIcon} size={18} />
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
                <a href="#" className={styles.forgotPassword}>Nie pamiętasz hasła?</a>
              </div>
              <div className={styles.inputWrapper}>
                <input
                  id="password"
                  type={showPassword ? 'text' : 'password'}
                  value={password}
                  onChange={(e) => setPassword(e.target.value)}
                  className={`${styles.input} ${errors.password ? styles.errorInput : ''}`}
                  placeholder="Wprowadź hasło"
                />
                <Lock className={styles.inputIcon} size={18} />
                <button
                  type="button"
                  className={styles.passwordToggle}
                  onClick={() => setShowPassword(!showPassword)}
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

            {/* Zapamiętaj mnie */}
            <div className={styles.rememberMe}>
              <input
                id="remember-me"
                type="checkbox"
                checked={rememberMe}
                onChange={() => setRememberMe(!rememberMe)}
                className={styles.checkbox}
              />
              <label htmlFor="remember-me" className={styles.label}>Zapamiętaj mnie</label>
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
              <a href="#" className={styles.registerLink}>Zarejestruj się</a>
            </p>
          </div>
        </div>
      </main>

    </div>
  );
};

export default LoginPage;
