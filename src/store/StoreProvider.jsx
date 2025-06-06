import React, { useReducer, useEffect } from 'react';
import { StoreContext } from './StoreContext';
import { reducer, initialState } from './store';

export function StoreProvider({ children }) {
  const [state, dispatch] = useReducer(reducer, initialState);

  // Udostępnienie store globalnie dla axios interceptorów
  useEffect(() => {
    window.__store = {
      getState: () => state,
      dispatch
    };
  }, [state, dispatch]);

  return (
    <StoreContext.Provider value={{ state, dispatch }}>
      {children}
    </StoreContext.Provider>
  );
}
