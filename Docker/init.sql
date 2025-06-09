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
    Zdjecie BYTEA,
    ImageUrl VARCHAR(255)         
);

-- Silniki 
CREATE TABLE Silnik (
    ID SERIAL PRIMARY KEY,
    PojazdID INT REFERENCES Pojazd(ID),
    Pojemnosc VARCHAR(10),
    Typ VARCHAR(20),
    Moc SMALLINT
);

-- Tabela powiązań model-silnik
CREATE TABLE ModelSilnik (
    ID SERIAL PRIMARY KEY,
    ModelID INT REFERENCES Pojazd(ID),
    SilnikID INT REFERENCES Silnik(ID),
    CenaDodatkowa DECIMAL(10,2) DEFAULT 0,
    UNIQUE(ModelID, SilnikID)
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
    TypElementu VARCHAR(50),
    Wartosc VARCHAR(100),
    OpisElementu TEXT,
    CenaDodatkowa DECIMAL(8, 2)
);

-- Tworzenie tabeli Konfiguracja
CREATE TABLE Konfiguracja (
    ID SERIAL PRIMARY KEY,
    IDUzytkownika INT REFERENCES Uzytkownik(ID),
    IDPojazdu INT REFERENCES Pojazd(ID),
    ModelSilnikID INT REFERENCES ModelSilnik(ID),
    DataUtworzenia TIMESTAMP,
    NazwaKonfiguracji VARCHAR(100),
    OpisKonfiguracji TEXT
);

-- Tworzenie tabeli Udostępnienie Konfiguracji
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
  (1, '1.6L',  'benzyna',     132),  -- Corolla - silnik benzynowy
  (1, '1.8L',  'benzyna',     140),  -- Corolla - drugi silnik benzynowy
  (2, '3.0L',  'diesel',      265),  -- BMW X5 - silnik diesla
  (2, '4.4L',  'benzyna',     530),  -- BMW X5 - silnik benzynowy V8
  (3, '—',     'elektryczny', 283);  -- Tesla Model 3 - silnik elektryczny

-- 4. Powiązania model-silnik
INSERT INTO ModelSilnik (ModelID, SilnikID, CenaDodatkowa) VALUES
  (1, 1, 0),      -- Toyota Corolla z silnikiem 1.6L (podstawowy)
  (1, 2, 5000),   -- Toyota Corolla z silnikiem 1.8L (+5000 zł)
  (2, 3, 0),      -- BMW X5 z silnikiem diesel (podstawowy)
  (2, 4, 25000),  -- BMW X5 z silnikiem V8 (+25000 zł)
  (3, 5, 0);      -- Tesla Model 3 z silnikiem elektrycznym

-- 5. Cechy pojazdów
INSERT INTO CechyPojazdu (IDPojazdu, Cecha) VALUES
  (1, 'System multimedialny'),
  (1, 'Tempomat'),
  (1, 'Czujniki parkowania'),
  (2, 'Napęd 4×4'),
  (2, 'Skórzana tapicerka'),
  (2, 'System nawigacji'),
  (2, 'Panoramiczny dach'),
  (3, 'Autopilot'),
  (3, 'Supercharging'),
  (3, 'Wyświetlacz 15"');

-- 6. Elementy wyposażenia
INSERT INTO ElementWyposazenia (TypElementu, Wartosc, OpisElementu, CenaDodatkowa) VALUES
  (1, 1, 'Standardowy', 0.00),
  (1, 2, 'High-End',    1500.00),
  (2, 3, 'Sportowe',    0.00),
  (3, 4, 'Zimowy',      0.00);

-- 7. Konfiguracje
INSERT INTO Konfiguracja (IDUzytkownika, IDPojazdu, ModelSilnikID, DataUtworzenia, NazwaKonfiguracji, OpisKonfiguracji) VALUES
  (2, 1, 1, '2025-04-20 10:15:00', 'Moja Corolla',    'Podstawowa konfiguracja z pakietem zimowym'),
  (3, 3, 5, '2025-04-22 14:30:00', 'Elektro Tesla',    'Maksymalny zasięg + Premium audio'),
  (1, 2, 3, '2025-04-23 09:45:00', 'Biznesowy X5',     'Wersja luksusowa z felgami sportowymi');

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

  CREATE TABLE car_accessories (
    id VARCHAR(255) PRIMARY KEY,
    car_id VARCHAR(255) NOT NULL,
    car_model VARCHAR(255) NOT NULL,
    category VARCHAR(50) NOT NULL,
    type VARCHAR(50) NOT NULL,
    name VARCHAR(255) NOT NULL,
    description TEXT,
    price DECIMAL(10, 2) NOT NULL,
    manufacturer VARCHAR(255),
    part_number VARCHAR(100),
    is_original_bmw_part BOOLEAN NOT NULL,
    is_in_stock BOOLEAN NOT NULL,
    stock_quantity INT NOT NULL,
    image_url TEXT,
    size VARCHAR(100),
    pattern VARCHAR(100),
    color VARCHAR(100),
    material VARCHAR(100),
    capacity INT,
    compatibility VARCHAR(255),
    age_group VARCHAR(50),
    max_load INT,
    is_universal BOOLEAN NOT NULL,
    installation_difficulty VARCHAR(50),
    warranty VARCHAR(100)
);

-- Tworzenie indeksów dla poprawy wydajności zapytań
CREATE INDEX idx_car_accessories_car_id ON car_accessories (car_id);
CREATE INDEX idx_car_accessories_car_model ON car_accessories (car_model);
CREATE INDEX idx_car_accessories_category ON car_accessories (category);
CREATE INDEX idx_car_accessories_type ON car_accessories (type);
CREATE INDEX idx_car_accessories_price ON car_accessories (price);
CREATE INDEX idx_car_accessories_is_original_bmw_part ON car_accessories (is_original_bmw_part);
CREATE INDEX idx_car_accessories_is_in_stock ON car_accessories (is_in_stock);

-- Migracja: Dodanie kolumny ImageUrl do istniejącej tabeli Pojazd (jeśli nie istnieje)
DO $$ 
BEGIN 
    IF NOT EXISTS (SELECT 1 FROM information_schema.columns 
                   WHERE table_name = 'pojazd' AND column_name = 'imageurl') THEN
        ALTER TABLE pojazd ADD COLUMN imageurl VARCHAR(255);
    END IF;
END $$;

-- Tworzenie tabel z małymi literami dla zgodności z Entity Framework
CREATE TABLE IF NOT EXISTS pojazd AS SELECT 
    id, model, kolornadwozia, wyposazeniewetrza, cena, opis, ma4x4, jestelektryczny, akcesoria, zdjecie, imageurl 
    FROM Pojazd;
    
CREATE TABLE IF NOT EXISTS silnik AS SELECT 
    id, pojazdid, pojemnosc, typ, moc 
    FROM Silnik;
    
CREATE TABLE IF NOT EXISTS modelsilnik AS SELECT 
    id, modelid, silnikid, cenadodatkowa 
    FROM ModelSilnik;
    
CREATE TABLE IF NOT EXISTS konfiguracja AS SELECT 
    id, iduzytkownika, idpojazdu, modelsilnikid, datautworzenia, nazwakonfiguracji, opiskonfiguracji 
    FROM Konfiguracja;
    
CREATE TABLE IF NOT EXISTS cechypojazdu AS SELECT 
    id, idpojazdu, cecha 
    FROM CechyPojazdu;

-- Dodanie przykładowych danych do tabeli wyposażenia wnętrza
CREATE TABLE IF NOT EXISTS car_interior_equipment (
    id VARCHAR(255) PRIMARY KEY,
    type VARCHAR(50) NOT NULL,
    value VARCHAR(255) NOT NULL,
    description TEXT,
    additional_price DECIMAL(10, 2) NOT NULL DEFAULT 0,
    has_navigation BOOLEAN DEFAULT FALSE,
    has_premium_sound BOOLEAN DEFAULT FALSE,
    control_type VARCHAR(50)
);

INSERT INTO car_interior_equipment (id, type, value, description, additional_price, has_navigation, has_premium_sound, control_type) VALUES
('1', 'Audio', 'System audio podstawowy', 'Standardowy system audio z radiem FM/AM', 0, FALSE, FALSE, 'Przyciski'),
('2', 'Audio', 'System audio Premium', 'System audio wysokiej jakości z 12 głośnikami', 2500, FALSE, TRUE, 'Dotykowy'),
('3', 'Nawigacja', 'Nawigacja podstawowa', 'System nawigacji z mapami Europy', 1500, TRUE, FALSE, 'Dotykowy'),
('4', 'Nawigacja', 'Nawigacja Premium', 'Zaawansowany system nawigacji z Traffic Live', 3000, TRUE, TRUE, 'Głosowy'),
('5', 'Klimatyzacja', 'Klimatyzacja automatyczna', 'Dwustrefowa automatyczna klimatyzacja', 2000, FALSE, FALSE, 'Automatyczny'),
('6', 'Oświetlenie', 'Oświetlenie ambientowe', 'Kolorowe oświetlenie wnętrza z regulacją', 800, FALSE, FALSE, 'Dotykowy');