import axios from 'axios';

const API_URL = import.meta.env.VITE_API_URL || 'https://localhost:7020';

// Utworzenie instancji axios
const axiosInstance = axios.create({
  baseURL: API_URL,
  headers: {
    'Content-Type': 'application/json',
  },
  withCredentials: true // Ważne dla ciasteczek z refresh tokenem
});

// Funkcja do ustawienia tokenu (deprecated - używamy request interceptora)
export const setAuthToken = (token) => {
  // Nie robimy nic - token jest dodawany przez request interceptor
};

// Funkcja do odświeżenia tokenu
const refreshAccessToken = async () => {
  try {
    console.log('🔄 Próba odświeżenia tokenu...');
    const store = window.__store;
    const response = await axiosInstance.post('/refresh-jwt');
    const newToken = response.data;
    
    console.log('✅ Otrzymano nowy token:', newToken ? 'Yes' : 'No');
    
    // Zapisz nowy token w store
    if (store) {
      store.dispatch({ type: 'SET_TOKEN', payload: newToken });
      console.log('✅ Nowy token zapisany w store');
    }
    
    return newToken;
  } catch (error) {
    console.error('❌ Błąd odświeżania tokenu:', error.response?.status, error.response?.data);
    // Jeśli odświeżenie się nie powiedzie, wyloguj użytkownika
    const store = window.__store;
    if (store) {
      store.dispatch({ type: 'LOGOUT' });
    }
    throw error;
  }
};

// Request interceptor - dodaje token do każdego żądania
axiosInstance.interceptors.request.use(
  (config) => {
    const store = window.__store;
    if (store) {
      const token = store.getState().accessToken;
      if (token) {
        config.headers = config.headers || {};
        config.headers['Authorization'] = `Bearer ${token}`;
      }
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

// Response interceptor - obsługuje błędy 401 i automatycznie odświeża token
axiosInstance.interceptors.response.use(
  (response) => {
    return response;
  },
  async (error) => {
    const originalRequest = error.config;

    // Nie próbuj odświeżać tokenu dla żądania refresh-jwt
    if (originalRequest?.url && originalRequest.url.includes('/refresh-jwt')) {
      return Promise.reject(error);
    }

    // Jeśli błąd 401 i nie próbowaliśmy jeszcze odświeżyć tokenu
    if (error.response?.status === 401 && !originalRequest._retry) {
      console.log('🔒 Otrzymano błąd 401, próba odświeżenia tokenu...');
      originalRequest._retry = true;

      try {
        const newToken = await refreshAccessToken();
        
        // Upewnij się, że headers istnieją
        originalRequest.headers = originalRequest.headers || {};
        originalRequest.headers['Authorization'] = `Bearer ${newToken}`;
        
        console.log('🔄 Ponowienie żądania z nowym tokenem...');
        const retryResponse = await axiosInstance(originalRequest);
        console.log('✅ Żądanie ponowione pomyślnie');
        return retryResponse;
      } catch (refreshError) {
        console.error('❌ Nie udało się odświeżyć tokenu, wylogowanie...');
        // Wyloguj użytkownika zamiast przekierowywać
        const store = window.__store;
        if (store) {
          store.dispatch({ type: 'LOGOUT' });
        }
        return Promise.reject(refreshError);
      }
    }

    return Promise.reject(error);
  }
);

export { axiosInstance };
export default axiosInstance;