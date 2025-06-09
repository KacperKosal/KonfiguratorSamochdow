import { jwtDecode } from 'jwt-decode';

export const initialState = {
  accessToken: localStorage.getItem("accessToken"),
  user: null
};

// Funkcja pomocnicza do dekodowania tokenu
const decodeToken = (token) => {
  try {
    const decoded = jwtDecode(token);
    console.log('Full JWT payload:', decoded); // Debug - pokażmy co jest w tokenie
    return {
      id: decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'] || decoded.nameid || decoded.UserId,
      email: decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'] || decoded.email,
      name: decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'] || decoded.name,
      role: decoded['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] || decoded.role,
      exp: decoded.exp
    };
  } catch (error) {
    console.error('Error decoding token:', error);
    return null;
  }
};

// Jeśli jest token w localStorage, dekoduj go przy starcie
if (initialState.accessToken) {
  const user = decodeToken(initialState.accessToken);
  if (user) {
    initialState.user = user;
    console.log('Store: Restored user from localStorage', { userId: user.id, role: user.role });
  } else {
    // Token w localStorage jest nieprawidłowy, usuń go
    console.warn('Store: Invalid token in localStorage, removing it');
    localStorage.removeItem('accessToken');
    initialState.accessToken = null;
    initialState.user = null;
  }
}

export function reducer(state, action) {
  switch (action.type) {
    case 'SET_TOKEN':
      const user = decodeToken(action.payload);
      if (user) {
        // Token jest prawidłowy
        localStorage.setItem('accessToken', action.payload);
        console.log('Store: Token set successfully', { userId: user.id, role: user.role });
        return { ...state, accessToken: action.payload, user };
      } else {
        // Token jest nieprawidłowy, wyloguj użytkownika
        console.warn('Store: Invalid token provided, logging out');
        localStorage.removeItem('accessToken');
        return { ...state, accessToken: null, user: null };
      }
    case 'LOGOUT':
      console.log('Store: Logging out user');
      localStorage.removeItem('accessToken');
      return { ...state, accessToken: null, user: null };
    default:
      return state;
  }
}
