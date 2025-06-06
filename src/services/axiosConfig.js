import axios from 'axios';

const API_URL = import.meta.env.VITE_API_URL || 'https://localhost:7001';

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
  console.log('setAuthToken called (deprecated):', !!token);
};

// Funkcja do odświeżenia tokenu
const refreshAccessToken = async () => {
  console.log('🔄 RefreshToken: STARTING token refresh process');
  
  try {
    // Sprawdź czy mamy dostęp do store i obecny token
    const store = window.__store;
    console.log('🔄 RefreshToken: Store check', {
      storeExists: !!store,
      hasCurrentToken: store ? !!store.getState()?.accessToken : false,
      currentTokenPreview: store?.getState()?.accessToken ? 
        store.getState().accessToken.substring(0, 20) + '...' : 'no token'
    });
    
    console.log('🔄 RefreshToken: Attempting refresh request to /refresh-jwt');
    const response = await axiosInstance.post('/refresh-jwt');
    
    console.log('🔄 RefreshToken: Refresh request completed', {
      status: response.status,
      hasData: !!response.data,
      dataType: typeof response.data
    });
    
    const newToken = response.data;
    
    console.log('🔄 RefreshToken: Got new token', { 
      hasToken: !!newToken,
      tokenPreview: newToken ? newToken.substring(0, 20) + '...' : null,
      tokenLength: newToken ? newToken.length : 0
    });
    
    // Zapisz nowy token w store
    if (store) {
      console.log('🔄 RefreshToken: Dispatching SET_TOKEN to store');
      store.dispatch({ type: 'SET_TOKEN', payload: newToken });
      
      // Sprawdź czy token został zapisany
      const updatedState = store.getState();
      console.log('🔄 RefreshToken: Store updated', {
        hasAccessToken: !!updatedState.accessToken,
        hasUser: !!updatedState.user,
        userRole: updatedState.user?.role,
        tokenMatches: updatedState.accessToken === newToken
      });
    } else {
      console.error('🔄 RefreshToken: No store available to save token!');
    }
    
    console.log('🔄 RefreshToken: SUCCESS - returning new token');
    return newToken;
  } catch (error) {
    console.error('🔄 RefreshToken: FAILED to refresh token', {
      message: error.message,
      status: error.response?.status,
      statusText: error.response?.statusText,
      responseData: error.response?.data
    });
    
    // Jeśli odświeżenie się nie powiedzie, wyloguj użytkownika
    const store = window.__store;
    if (store) {
      console.log('🔄 RefreshToken: Logging out user due to refresh failure');
      store.dispatch({ type: 'LOGOUT' });
    }
    throw error;
  }
};

// Request interceptor - dodaje token do każdego żądania
axiosInstance.interceptors.request.use(
  (config) => {
    console.log('Request interceptor: Processing request', {
      url: config.url,
      existingAuthHeader: config.headers?.Authorization,
      allHeaders: config.headers
    });

    const store = window.__store;
    if (store) {
      const token = store.getState().accessToken;
      if (token) {
        const authHeader = `Bearer ${token}`;
        config.headers = config.headers || {};
        config.headers['Authorization'] = authHeader;
        console.log('Request interceptor: Token added successfully', { 
          url: config.url, 
          hasToken: !!token,
          tokenPreview: token ? token.substring(0, 20) + '...' : null,
          authHeaderPreview: authHeader.substring(0, 30) + '...',
          finalHeaders: config.headers
        });
      } else {
        console.log('Request interceptor: No token found for request', { 
          url: config.url,
          storeExists: !!store,
          stateExists: !!store?.getState(),
          accessToken: store?.getState()?.accessToken
        });
      }
    } else {
      console.log('Request interceptor: No store found');
    }
    return config;
  },
  (error) => {
    console.error('Request interceptor error:', error);
    return Promise.reject(error);
  }
);

// Response interceptor - obsługuje błędy 401 i automatycznie odświeża token
axiosInstance.interceptors.response.use(
  (response) => {
    console.log('Axios interceptor: Successful response', { 
      status: response.status, 
      url: response.config?.url 
    });
    return response;
  },
  async (error) => {
    console.log('Axios interceptor: ERROR CAUGHT', {
      hasResponse: !!error.response,
      status: error.response?.status,
      statusText: error.response?.statusText,
      url: error.config?.url,
      message: error.message,
      code: error.code
    });

    const originalRequest = error.config;

    console.log('Axios interceptor: Response error details', { 
      status: error.response?.status, 
      url: originalRequest?.url,
      retry: originalRequest?._retry,
      hasOriginalRequest: !!originalRequest
    });

    // Nie próbuj odświeżać tokenu dla żądania refresh-jwt
    if (originalRequest?.url && originalRequest.url.includes('/refresh-jwt')) {
      console.log('Axios interceptor: Skipping token refresh for refresh-jwt error');
      return Promise.reject(error);
    }

    // Jeśli błąd 401 i nie próbowaliśmy jeszcze odświeżyć tokenu
    if (error.response?.status === 401 && !originalRequest._retry) {
      console.log('🚨 Axios interceptor: 401 DETECTED - starting automatic token refresh');
      originalRequest._retry = true;

      try {
        console.log('🚨 Axios interceptor: Calling refreshAccessToken()...');
        const newToken = await refreshAccessToken();
        console.log('🚨 Axios interceptor: refreshAccessToken() completed successfully');
        
        // Upewnij się, że headers istnieją
        originalRequest.headers = originalRequest.headers || {};
        originalRequest.headers['Authorization'] = `Bearer ${newToken}`;
        
        console.log('🚨 Axios interceptor: Retrying original request with new token', {
          originalUrl: originalRequest.url,
          newTokenPreview: newToken ? newToken.substring(0, 20) + '...' : null,
          authHeaderSet: !!originalRequest.headers['Authorization']
        });
        
        console.log('🚨 Axios interceptor: Executing retry request...');
        const retryResponse = await axiosInstance(originalRequest);
        console.log('🚨 Axios interceptor: Retry request SUCCESS', {
          status: retryResponse.status,
          url: retryResponse.config?.url
        });
        
        return retryResponse;
      } catch (refreshError) {
        console.error('🚨 Axios interceptor: Token refresh FAILED', {
          error: refreshError.message,
          status: refreshError.response?.status,
          responseData: refreshError.response?.data
        });
        
        // Wyloguj użytkownika zamiast przekierowywać
        const store = window.__store;
        if (store) {
          console.log('🚨 Axios interceptor: Logging out user due to refresh failure');
          store.dispatch({ type: 'LOGOUT' });
        }
        return Promise.reject(refreshError);
      }
    } else {
      console.log('🚨 Axios interceptor: NOT attempting refresh', {
        status: error.response?.status,
        alreadyRetried: originalRequest?._retry,
        reason: error.response?.status !== 401 ? 'not 401 error' : 'already retried once'
      });
    }

    return Promise.reject(error);
  }
);

export default axiosInstance;