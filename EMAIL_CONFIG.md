# Konfiguracja Email - Instrukcje

## Problem
Obecnie emaile nie są wysyłane fizycznie, tylko logowane do konsoli (tryb mock).

## Rozwiązania

### Opcja 1: Konfiguracja Gmail SMTP (Zalecana)

1. **Załóż konto Gmail dla aplikacji:**
   - Utwórz nowe konto Gmail: `konfiguratorsamochodowy.kontakt@gmail.com`
   
2. **Włącz weryfikację dwuetapową:**
   - Idź do ustawień konta Google
   - Bezpieczeństwo → Weryfikacja dwuetapowa
   
3. **Wygeneruj hasło aplikacji:**
   - Google Account → Bezpieczeństwo → Hasła aplikacji
   - Wybierz "Poczta" i "Komputer Windows"
   - Skopiuj wygenerowane hasło (16 znaków)

4. **Zaktualizuj appsettings.json:**
   ```json
   "Email": {
     "SmtpServer": "smtp.gmail.com",
     "SmtpPort": "587",
     "SenderEmail": "konfiguratorsamochodowy.kontakt@gmail.com",
     "SenderPassword": "TUTAJ_HASLO_APLIKACJI",
     "TargetEmail": "konfiguratorsamochodwy@gmail.com",
     "UseMockService": false
   }
   ```

### Opcja 2: Outlook/Hotmail SMTP

1. **Załóż konto Outlook:**
   - Utwórz konto: `konfiguratorsamochodowy@outlook.com`
   
2. **Zaktualizuj appsettings.json:**
   ```json
   "Email": {
     "SmtpServer": "smtp-mail.outlook.com",
     "SmtpPort": "587",
     "SenderEmail": "konfiguratorsamochodowy@outlook.com",
     "SenderPassword": "HASLO_DO_KONTA",
     "TargetEmail": "konfiguratorsamochodwy@gmail.com",
     "UseMockService": false
   }
   ```

### Opcja 3: Użyj EmailJS (Frontend)

To rozwiązanie nie wymaga konfiguracji SMTP i działa bezpośrednio z przeglądarki:

1. **Załóż konto na EmailJS.com**
2. **Skonfiguruj szablon email**
3. **Zaktualizuj frontend do używania EmailJS**

### Opcja 4: Brevo (dawniej SendinBlue) - BEZPŁATNE

1. **Załóż konto na Brevo.com**
2. **Pobierz klucz API**
3. **Zaktualizuj konfigurację**

## Aktualny status
- ✅ API endpoint działa
- ✅ Formularz wysyła dane
- ❌ Emaile są tylko logowane (tryb mock)
- ❌ Brak konfiguracji SMTP

## Szybki test
Po skonfigurowaniu SMTP, sprawdź logi API - zobaczysz:
- "Attempting to send real email..." - próba wysłania
- "Contact email sent successfully" - sukces
- Lub błędy SMTP z szczegółami

## Zalecenie
Użyj **Opcji 1 (Gmail)** - jest najprostsza i najbardziej niezawodna.