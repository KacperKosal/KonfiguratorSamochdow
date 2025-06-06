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
    console.log('🔄 ProtectedRoute: Starting token refresh...');
    
    try {
      const response = await axiosInstance.post('/refresh-jwt');
      const newToken = response.data;
      
      console.log('🔄 ProtectedRoute: Token refreshed successfully');
      dispatch({ type: 'SET_TOKEN', payload: newToken });
      
      setIsRefreshing(false);
      return true;
    } catch (error) {
      console.error('🔄 ProtectedRoute: Token refresh failed', error);
      dispatch({ type: 'LOGOUT' });
      setIsRefreshing(false);
      return false;
    }
  };

  useEffect(() => {
    const checkAuthorization = async () => {
      console.log('🛡️ ProtectedRoute: STARTING authorization check', { 
        hasToken: !!state.accessToken,
        hasUser: !!state.user,
        userRole: state.user?.role,
        requiredRole,
        tokenPreview: state.accessToken ? state.accessToken.substring(0, 20) + '...' : 'no token'
      });

      // Sprawdź czy jest token i zdekodowany użytkownik
      if (!state.accessToken || !state.user) {
        console.log('🛡️ ProtectedRoute: FAILED - No access token or user data found', {
          hasToken: !!state.accessToken,
          hasUser: !!state.user,
          tokenLength: state.accessToken ? state.accessToken.length : 0
        });
        setIsAuthorized(false);
        setIsLoading(false);
        return;
      }

      // Sprawdź czy token nie wygasł
      const currentTime = Date.now() / 1000;
      console.log('🛡️ ProtectedRoute: Token expiration check', {
        userExp: state.user.exp,
        currentTime: currentTime,
        timeUntilExp: state.user.exp ? state.user.exp - currentTime : 'no exp',
        isExpired: state.user.exp && state.user.exp < currentTime,
        expDate: state.user.exp ? new Date(state.user.exp * 1000).toISOString() : 'no exp',
        currentDate: new Date().toISOString()
      });
      
      // Jeśli token wygasł, spróbuj go odświeżyć
      if (state.user.exp && state.user.exp < currentTime) {
        console.log('🛡️ ProtectedRoute: Token expired, attempting refresh...', {
          expiredBy: currentTime - state.user.exp,
          expiredBySecs: Math.floor(currentTime - state.user.exp)
        });
        
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
        console.log('🛡️ ProtectedRoute: Role check', { 
          userRole: state.user.role, 
          requiredRole, 
          authorized: hasRequiredRole,
          roleMatches: state.user.role === requiredRole
        });
        setIsAuthorized(hasRequiredRole);
        
        if (hasRequiredRole) {
          console.log('🛡️ ProtectedRoute: SUCCESS - User authorized with correct role');
        } else {
          console.log('🛡️ ProtectedRoute: FAILED - User does not have required role');
        }
      } else {
        // Jeśli nie wymaga roli, samo posiadanie tokenu i użytkownika wystarczy
        console.log('🛡️ ProtectedRoute: SUCCESS - No role required, user authorized');
        setIsAuthorized(true);
      }
      
      setIsLoading(false);
    };

    console.log('🛡️ ProtectedRoute: useEffect triggered', {
      accessTokenChanged: true,
      userChanged: true,
      requiredRoleChanged: true
    });

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
    console.log('🛡️ ProtectedRoute: NOT AUTHORIZED - redirecting to /home', {
      isLoading,
      isAuthorized,
      hasToken: !!state.accessToken,
      hasUser: !!state.user,
      requiredRole
    });
    return <Navigate to="/home" replace />;
  }

  console.log('🛡️ ProtectedRoute: AUTHORIZED - rendering protected content');

  return children;
};

export default ProtectedRoute;