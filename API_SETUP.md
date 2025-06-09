# Instrukcja uruchomienia API do testowania funkcji email

## Aby uruchomić API i przetestować funkcję wysyłania emaili:

### Opcja 1: Uruchomienie za pomocą .NET (zalecane)
1. Zainstaluj .NET 8 SDK na swoim systemie
2. Otwórz terminal/CMD w folderze głównym projektu
3. Przejdź do folderu API:
   ```
   cd Api/KonfiguratorSamochodowy.Api/KonfiguratorSamochodowy.Api
   ```
4. Uruchom API:
   ```
   dotnet run
   ```
5. API będzie dostępne na https://localhost:7020

### Opcja 2: Uruchomienie w Visual Studio
1. Otwórz plik KonfiguratorSamochdow.sln w Visual Studio
2. Ustaw KonfiguratorSamochodowy.Api jako projekt startowy
3. Naciśnij F5 lub kliknij "Start"

### Opcja 3: Docker (jeśli masz Docker Desktop)
1. W folderze Api/KonfiguratorSamochodowy.Api/KonfiguratorSamochodowy.Api:
   ```
   docker build -t konfigurator-api .
   docker run -p 7020:8080 konfigurator-api
   ```

### Konfiguracja email (opcjonalna)
Aby faktycznie wysyłać emaile (nie tylko w trybie mock):

1. Otwórz `Api/KonfiguratorSamochodowy.Api/KonfiguratorSamochodowy.Api/appsettings.json`
2. Wypełnij sekcję Email:
   ```json
   "Email": {
     "SmtpServer": "smtp.gmail.com",
     "SmtpPort": "587",
     "SenderEmail": "twoj-email@gmail.com",
     "SenderPassword": "haslo-aplikacji-gmail",
     "TargetEmail": "konfiguratorsamochodwy@gmail.com"
   }
   ```

**Uwaga:** Bez konfiguracji SMTP, API będzie działać w trybie mock - wiadomości będą logowane do konsoli, ale nie będą rzeczywiście wysyłane.

### Sprawdzenie czy API działa
Otwórz https://localhost:7020/swagger w przeglądarce - powinieneś zobaczyć dokumentację API.

### Testowanie endpoint contact
Po uruchomieniu API, formularz kontaktowy na stronie będzie działać poprawnie.