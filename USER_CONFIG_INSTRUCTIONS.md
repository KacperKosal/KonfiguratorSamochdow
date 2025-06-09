# 📋 Instrukcja: Zapisywanie konfiguracji w "Moje Konto"

## ✅ Co zostało zaimplementowane:

### 🔧 Backend (API):
1. **Model UserConfiguration** - szczegółowa konfiguracja użytkownika
2. **UserConfigurationRepository** - obsługa bazy danych
3. **UserConfigurationService** - logika biznesowa  
4. **UserConfigurationEndpoints** - API endpointy
5. **Automatyczne tworzenie tabeli** `user_configurations`

### 🎨 Frontend:
1. **Zaktualizowany konfigurator** - zapisuje do konta użytkownika
2. **Nowa strona "Moje Konto"** - `/my-account`
3. **Lista konfiguracji** - wyświetlanie, usuwanie
4. **Nawigacja** - link "Moje konto" w headerze

## 🚀 Jak przetestować:

### Krok 1: Uruchom aplikacje
```bash
# Terminal 1 - API
cd Api/KonfiguratorSamochodowy.Api/KonfiguratorSamochodowy.Api
dotnet run

# Terminal 2 - Frontend  
npm run dev
```

### Krok 2: Zaloguj się
1. Otwórz http://localhost:5173
2. Kliknij **"Zaloguj się"**
3. Zaloguj się na swoje konto

### Krok 3: Stwórz konfigurację
1. Przejdź do **"Konfigurator"** 
2. Wybierz model samochodu
3. Wybierz silnik
4. Wybierz kolor, akcesoria, wyposażenie
5. Kliknij **"Zapisz konfigurację"**
6. Podaj nazwę (np. "Moja BMW X5")
7. Kliknij OK

### Krok 4: Sprawdź "Moje Konto"
1. Kliknij **"Moje konto"** w menu
2. Zobaczysz swoją zapisaną konfigurację
3. Sprawdź szczegóły: model, silnik, kolor, cena
4. Możesz usunąć konfigurację przyciskiem "Usuń"

## 📊 API Endpointy:

### POST `/api/user-configurations`
Zapisz nową konfigurację użytkownika
```json
{
  "configurationName": "Moja BMW X5",
  "carModelId": "1", 
  "engineId": "2",
  "exteriorColor": "#000000",
  "exteriorColorName": "Czarny",
  "accessoryIds": ["1", "2"],
  "interiorEquipmentIds": ["3"],
  "totalPrice": 450000
}
```

### GET `/api/user-configurations`
Pobierz wszystkie konfiguracje użytkownika

### GET `/api/user-configurations/{id}`
Pobierz konkretną konfigurację

### DELETE `/api/user-configurations/{id}`` 
Usuń konfigurację

## 🔐 Uwierzytelnianie:
Wszystkie endpointy wymagają:
```
Authorization: Bearer {accessToken}
```

## 📁 Struktura bazy danych:
Tabela `user_configurations`:
- `id` - identyfikator konfiguracji
- `user_id` - identyfikator użytkownika  
- `configuration_name` - nazwa nadana przez użytkownika
- `car_model_id`, `car_model_name` - dane modelu
- `engine_id`, `engine_name` - dane silnika
- `exterior_color`, `exterior_color_name` - kolor
- `selected_accessories` - JSON z akcesoriami
- `selected_interior_equipment` - JSON z wyposażeniem
- `total_price` - całkowita cena
- `created_at`, `updated_at` - daty
- `is_active` - czy aktywna

## 🎯 Funkcjonalności:
✅ Zapisywanie konfiguracji w koncie użytkownika  
✅ Wyświetlanie listy konfiguracji
✅ Usuwanie konfiguracji
✅ Walidacja autoryzacji
✅ Responsywny design
✅ Formatowanie cen i dat
✅ Automatyczne tworzenie tabel

**KONFIGURACJE SĄ TERAZ ZAPISYWANE W "MOJE KONTO"!**