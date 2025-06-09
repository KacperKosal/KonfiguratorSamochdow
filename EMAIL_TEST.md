# Test funkcji email - Instrukcja

## Konfiguracja została zaktualizowana!

✅ **SMTP Gmail został skonfigurowany w `appsettings.json`**
✅ **UseMockService ustawione na `false`** - prawdziwe emaile
✅ **Docelowy email:** `konfiguratorsamochodwy@gmail.com`

## Jak przetestować:

### 1. Uruchom API
```bash
cd Api/KonfiguratorSamochodowy.Api/KonfiguratorSamochodowy.Api
dotnet run
```

### 2. Uruchom frontend (jeśli nie działa)
```bash
npm run dev
```

### 3. Przejdź na stronę kontakt
- Otwórz http://localhost:5173
- Kliknij na "Kontakt" w menu
- Sprawdź czy status pokazuje: "✅ API jest dostępne - emaile będą wysyłane na konfiguratorsamochodwy@gmail.com"

### 4. Wypełnij formularz
- Imię i nazwisko: np. "Jan Kowalski"
- Email: twój email (do Reply-To)
- Telefon: opcjonalne
- Temat: wybierz z listy
- Wiadomość: napisz test message

### 5. Wyślij formularz
- Kliknij "Wyślij wiadomość"
- Powinieneś zobaczyć: "Dziękujemy za wiadomość!"

### 6. Sprawdź email
**Email powinien przyjść na: `konfiguratorsamochodwy@gmail.com`**

### 7. Sprawdź logi API
W konsoli API powinieneś zobaczyć:
```
info: Attempting to send real email...
info: Sending email to konfiguratorsamochodwy@gmail.com with subject: [Konfigurator] ...
info: Contact email sent successfully from test@email.com to konfiguratorsamochodwy@gmail.com
```

## Co się zmienia:
- ❌ Nie ma już "MOCK EMAIL SERVICE" w logach
- ✅ Prawdziwe emaile HTML na Gmail
- ✅ Reply-To ustawione na email nadawcy
- ✅ Profesjonalny format wiadomości

## Jeśli email nie przychodzi:
1. Sprawdź spam/promocje w Gmail
2. Sprawdź logi API - czy są błędy SMTP
3. Email może być opóźniony (do 5 minut)

## Backup plan:
Jeśli SMTP Gmail nie działa, EmailService automatycznie przełączy się na tryb mock i zapisze wiadomość w logach (żeby nie stracić żadnej wiadomości).