import React, { createContext } from 'react';

export const StoreContext = createContext({
  state: {},        
  dispatch: () => {},              
});
