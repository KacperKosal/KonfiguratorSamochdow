import React, { useContext, useEffect, useState } from 'react';
import { Navigate } from 'react-router-dom';
import { StoreContext } from '../../store/StoreContext';
import axiosInstance from '../../services/axiosConfig';

const ProtectedRoute = ({ children, requiredRole = null }) => {
  const { state, dispatch } = useContext(StoreContext);
  const [isAuthorized, setIsAuthorized] = useState(false);
  const [isLoading, setIsLoading] = useState(true);
  const [isRefreshing, setIsRefreshing] = useState(false);

  const refreshTokenAndRetry = async () => {
    if (isRefreshing) return;
    
    setIsRefreshing(true);
    
    try {
      const response = await axiosInstance.post('/refresh-jwt');
      const newToken = response.data;
      
      dispatch({ type: 'SET_TOKEN', payload: newToken });
      
      setIsRefreshing(false);
      return true;
    } catch (error) {
      dispatch({ type: 'LOGOUT' });
      setIsRefreshing(false);
      return false;
    }
  };

  useEffect(() => {
    const checkAuthorization = async () => {
      // Sprawdź czy jest token i zdekodowany użytkownik
      if (!state.accessToken || !state.user) {
        setIsAuthorized(false);
        setIsLoading(false);
        return;
      }

      // Sprawdź czy token nie wygasł
      const currentTime = Date.now() / 1000;
      
      // Jeśli token wygasł, spróbuj go odświeżyć
      if (state.user.exp && state.user.exp < currentTime) {
        const refreshSuccess = await refreshTokenAndRetry();
        if (!refreshSuccess) {
          setIsAuthorized(false);
          setIsLoading(false);
          return;
        }
        
        // Po odświeżeniu tokenu, sprawdź ponownie autoryzację
        // Nowy token będzie dostępny w następnym renderze przez state.accessToken
        return;
      }

      // Jeśli wymaga konkretnej roli, sprawdź ją
      if (requiredRole) {
        const hasRequiredRole = state.user.role === requiredRole;
        setIsAuthorized(hasRequiredRole);
      } else {
        // Jeśli nie wymaga roli, samo posiadanie tokenu i użytkownika wystarczy
        setIsAuthorized(true);
      }
      
      setIsLoading(false);
    };

    // Uruchom sprawdzenie autoryzacji od razu
    checkAuthorization();
  }, [state.accessToken, state.user, requiredRole]);

  if (isLoading || isRefreshing) {
    return (
      <div style={{ 
        display: 'flex', 
        justifyContent: 'center', 
        alignItems: 'center', 
        minHeight: '100vh' 
      }}>
        <div>{isRefreshing ? 'Odświeżanie sesji...' : 'Ładowanie...'}</div>
      </div>
    );
  }

  if (!isAuthorized) {
    return <Navigate to="/home" replace />;
  }

  return children;
};

export default ProtectedRoute;