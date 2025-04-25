-- Tworzenie typu ENUM dla roli użytkownika
CREATE TYPE RolaTyp AS ENUM ('Administrator', 'Uzytkownik');

-- Tworzenie typu ENUM dla statusu logowania/rejestracji
CREATE TYPE StatusTyp AS ENUM ('Sukces', 'Niepowodzenie');

-- Tworzenie tabeli Użytkownik
CREATE TABLE Uzytkownik (
    ID SERIAL PRIMARY KEY,
    ImieNazwisko VARCHAR(100),
    Email VARCHAR(100),
    Haslo VARCHAR(255),
    RefreshToken VARCHAR(255),
    RefreshTokenExpires TIMESTAMP,
    Rola VARCHAR(255)
);

CREATE TABLE Pojazd (
    ID SERIAL PRIMARY KEY,
    Model VARCHAR(50),
    KolorNadwozia VARCHAR(30),
    WyposazenieWnetrza TEXT,
    Cena INT,
    Opis TEXT,
    Ma4x4 BOOLEAN,
    JestElektryczny BOOLEAN,
    Akcesoria TEXT,
    Zdjecie BYTEA         
);

-- Silniki 
CREATE TABLE Silnik (
    ID SERIAL PRIMARY KEY,
    PojazdID INT REFERENCES Pojazd(ID),
    Pojemnosc VARCHAR(10),
    Typ VARCHAR(20),
    Moc SMALLINT
);

-- Cechy pojazdu
CREATE TABLE CechyPojazdu (
    ID SERIAL PRIMARY KEY,
    IDPojazdu INT REFERENCES Pojazd(ID),
    Cecha VARCHAR(100)
);

-- Tworzenie tabeli Element Wyposazenia
CREATE TABLE ElementWyposazenia (
    ID SERIAL PRIMARY KEY,
    Nazwa VARCHAR(100),
    Opis TEXT,
    Cena DECIMAL(10, 2),
    Kategoria VARCHAR(50),
    Zdjecie BYTEA
);

-- Tworzenie tabeli Opcja Wyposazenia
CREATE TABLE OpcjaWyposazenia (
    ID SERIAL PRIMARY KEY,
    IDPojazdu INT REFERENCES Pojazd(ID),
    IDElementuWyposazenia INT REFERENCES ElementWyposazenia(ID),
    Wartosc VARCHAR(100),
    CenaDodatkowa DECIMAL(10, 2)
);

-- Tworzenie tabeli Konfiguracja
CREATE TABLE Konfiguracja (
    ID SERIAL PRIMARY KEY,
    IDUzytkownika INT REFERENCES Uzytkownik(ID),
    IDPojazdu INT REFERENCES Pojazd(ID),
    DataUtworzenia TIMESTAMP,
    NazwaKonfiguracji VARCHAR(100),
    OpisKonfiguracji TEXT
);

-- Tworzenie tabeli Udostepnienie Konfiguracji
CREATE TABLE UdostepnienieKonfiguracji (
    ID SERIAL PRIMARY KEY,
    IDKonfiguracji INT REFERENCES Konfiguracja(ID),
    Link VARCHAR(255),
    Odbiorca VARCHAR(100),
    DataUdostepnienia TIMESTAMP
);

-- Tworzenie tabeli Logowanie
CREATE TABLE Logowanie (
    ID SERIAL PRIMARY KEY,
    IDUzytkownika INT REFERENCES Uzytkownik(ID),
    DataLogowania TIMESTAMP,
    AdresIP VARCHAR(45),
    Status StatusTyp
);

-- Tworzenie tabeli Rejestracja
CREATE TABLE Rejestracja (
    ID SERIAL PRIMARY KEY,
    IDUzytkownika INT REFERENCES Uzytkownik(ID),
    DataRejestracji TIMESTAMP,
    AdresIP VARCHAR(45),
    Status StatusTyp
);

-- Tworzenie tabeli Administrator
CREATE TABLE Administrator (
    ID SERIAL PRIMARY KEY,
    IDUzytkownika INT REFERENCES Uzytkownik(ID),
    PoziomUprawnien VARCHAR(30),
    OstatnieLogowanie TIMESTAMP
);

-- ---------------------------------------------------
-- Seed danych do bazy KonfiguratorSamochodowy
-- ---------------------------------------------------

-- 1. Użytkownicy
INSERT INTO Uzytkownik (ImieNazwisko, Email, Haslo, Rola) VALUES
  ('Jan Kowalski',    'jan.kowalski@example.com',    'hashedpwd1', 'Administrator'),
  ('Anna Nowak',      'anna.nowak@example.com',      'hashedpwd2', 'Uzytkownik'),
  ('Piotr Zieliński', 'piotr.zielinski@example.com', 'hashedpwd3', 'Uzytkownik');

-- 2. Pojazdy
INSERT INTO Pojazd (Model, KolorNadwozia, WyposazenieWnetrza, Cena, Opis, Ma4x4, JestElektryczny, Akcesoria) VALUES
  ('Toyota Corolla', 'Czarny', 'Skórzana tapicerka, Klimatyzacja',  80000, 'Ekonomiczny i niezawodny kompakt',          FALSE, FALSE, 'Nawigacja, Hak'),
  ('BMW X5',         'Biały',  'Welur, Podgrzewane fotele',         250000,'Komfortowy SUV z napędem 4×4',            TRUE,  FALSE, 'Hak, Dachowy bagażnik'),
  ('Tesla Model 3',  'Czerwony','Skórzana tapicerka, Autopilot',     200000,'Bezpieczeństwo i zero emisji',            FALSE, TRUE,  'Ładowarka, Dywaniki gumowe');

-- 3. Silniki
INSERT INTO Silnik (PojazdID, Pojemnosc, Typ, Moc) VALUES
  (1, '1.6L',      'benzyna',     132),
  (2, '3.0L',      'diesel',      265),
  (3, '—',         'elektryczny', 283);

-- 4. Cechy pojazdu
INSERT INTO CechyPojazdu (IDPojazdu, Cecha) VALUES
  (1, 'Eco'),
  (1, 'Automatyczna skrzynia'),
  (2, 'Napęd 4×4'),
  (3, 'Zero emisji');

-- 5. Elementy wyposażenia
INSERT INTO ElementWyposazenia (Nazwa, Opis, Cena, Kategoria, Zdjecie) VALUES
  ('Hak holowniczy',     'Hak do holowania przyczepy',                       1200.00, 'Zewnętrzne', NULL),
  ('System nagłośnienia','Premium audio z 12 głośnikami',                  3500.00, 'Wnętrze',    NULL),
  ('Alufelgi 19\"',      'Felgi aluminiowe 19-calowe',                      2200.00, 'Koła',       NULL),
  ('Pakiet zimowy',      'Podgrzewane fotele + kierownica + dysze spryskiwaczy', 1500.00, 'Wnętrze', NULL);

-- 6. Opcje wyposażenia
INSERT INTO OpcjaWyposazenia (IDPojazdu, IDElementuWyposazenia, Wartosc, CenaDodatkowa) VALUES
  (1, 1, 'Standardowy', 0.00),
  (1, 2, 'High-End',    1500.00),
  (2, 3, 'Sportowe',    0.00),
  (3, 4, 'Zimowy',      0.00);

-- 7. Konfiguracje
INSERT INTO Konfiguracja (IDUzytkownika, IDPojazdu, DataUtworzenia, NazwaKonfiguracji, OpisKonfiguracji) VALUES
  (2, 1, '2025-04-20 10:15:00', 'Moja Corolla',    'Podstawowa konfiguracja z pakietem zimowym'),
  (3, 3, '2025-04-22 14:30:00', 'Elektro Tesla',    'Maksymalny zasięg + Premium audio'),
  (1, 2, '2025-04-23 09:45:00', 'Biznesowy X5',     'Wersja luksusowa z felgami sportowymi');

-- 8. Udostępnienia konfiguracji
INSERT INTO UdostepnienieKonfiguracji (IDKonfiguracji, Link, Odbiorca, DataUdostepnienia) VALUES
  (1, 'https://app.example.com/config/abc123', 'adam@example.com', '2025-04-21 09:00:00'),
  (2, 'https://app.example.com/config/def456', 'ewa@example.com',  '2025-04-22 15:00:00');

-- 9. Logowania
INSERT INTO Logowanie (IDUzytkownika, DataLogowania, AdresIP, Status) VALUES
  (1, '2025-04-22 08:30:00', '192.168.1.10', 'Sukces'),
  (2, '2025-04-22 12:05:00', '203.0.113.45', 'Niepowodzenie'),
  (3, '2025-04-24 17:20:00', '198.51.100.23','Sukces');

-- 10. Rejestracje
INSERT INTO Rejestracja (IDUzytkownika, DataRejestracji, AdresIP, Status) VALUES
  (1, '2025-01-15 11:00:00', '192.0.2.10',   'Sukces'),
  (2, '2025-02-10 14:30:00', '198.51.100.50','Sukces'),
  (3, '2025-03-05 09:15:00', '203.0.113.99','Sukces');

-- 11. Administratorzy
INSERT INTO Administrator (IDUzytkownika, PoziomUprawnien, OstatnieLogowanie) VALUES
  (1, 'SuperAdmin', '2025-04-22 08:30:00'),
  (3, 'Moderator',  '2025-04-24 17:20:00');
