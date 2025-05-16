import React, { useReducer } from 'react';
import { StoreContext } from './StoreContext';
import { reducer, initialState } from './store';

export function StoreProvider({ children }) {
  const [state, dispatch] = useReducer(reducer, initialState);

  return (
    <StoreContext.Provider value={{ state, dispatch }}>
      {children}
    </StoreContext.Provider>
  );
}
