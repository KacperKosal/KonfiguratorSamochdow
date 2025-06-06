import { useContext, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { StoreContext } from '../store/StoreContext';

export const useAuth = (requireAuth = true) => {
  const { state } = useContext(StoreContext);
  const navigate = useNavigate();

  useEffect(() => {
    if (requireAuth && !state.accessToken) {
      navigate('/login');
    }
  }, [state.accessToken, requireAuth, navigate]);

  return {
    isAuthenticated: !!state.accessToken,
    token: state.accessToken
  };
};