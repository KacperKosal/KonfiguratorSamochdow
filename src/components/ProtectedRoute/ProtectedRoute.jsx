import React, { useContext } from 'react';
import { Navigate } from 'react-router-dom';
import { StoreContext } from '../../store/StoreContext';

const ProtectedRoute = ({ children, requireAuth = true }) => {
  const { state } = useContext(StoreContext);
  const isAuthenticated = !!state.accessToken;

  // Jeśli trasa wymaga autoryzacji i użytkownik nie jest zalogowany
  if (requireAuth && !isAuthenticated) {
    return <Navigate to="/login" replace />;
  }

  // Jeśli trasa nie wymaga autoryzacji (login/register) i użytkownik jest zalogowany
  if (!requireAuth && isAuthenticated) {
    return <Navigate to="/home" replace />;
  }

  return children;
};

export default ProtectedRoute;