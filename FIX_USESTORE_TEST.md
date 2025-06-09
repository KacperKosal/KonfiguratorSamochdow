# ✅ NAPRAWIONO: Błąd useStore export

## 🔧 Co zostało naprawione:

### Problem:
```
Uncaught SyntaxError: The requested module '/src/store/useStore.js' does not provide an export named 'useStore'
```

### Rozwiązanie:
1. **Stworzono hook useStore** w `/src/store/useStore.js`:
   ```javascript
   export const useStore = () => {
     const context = useContext(StoreContext);
     return {
       ...context.state,
       dispatch: context.dispatch
     };
   };
   ```

2. **Rozszerzono dekodowanie JWT** o więcej pól:
   - email, name, role, id
   - Dodano debug log dla JWT payload

3. **Zaktualizowano MyAccount.jsx**:
   - Używa poprawnych pól z user obiektu
   - Graceful handling gdy brak danych

## 🚀 Test:

### Sprawdź czy działa:
1. **Uruchom dev server**: `npm run dev`
2. **Otwórz**: http://localhost:5173
3. **Sprawdź konsolę** - nie powinno być błędów useStore
4. **Zaloguj się** i idź do "Moje konto"

### Sprawdź JWT debug:
1. **Zaloguj się**
2. **Otwórz konsolę przeglądarki**
3. **Szukaj**: "Full JWT payload:" - zobaczysz co jest w tokenie

## 📊 Struktura store:

### State:
- `accessToken` - JWT token
- `user` - obiekt z:
  - `id` - ID użytkownika
  - `email` - email użytkownika  
  - `name` - imię/nazwa
  - `role` - rola (User/Administrator)
  - `exp` - expiration

### useStore() zwraca:
- Wszystkie pola ze state + dispatch
- Można używać: `const { accessToken, user, dispatch } = useStore()`

**BŁĄD USESTORE ZOSTAŁ NAPRAWIONY!**