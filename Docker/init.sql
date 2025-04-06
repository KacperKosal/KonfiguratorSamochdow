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
    Rola RolaTyp,
    Status2FA BOOLEAN
);

-- Tworzenie tabeli Pojazd
CREATE TABLE Pojazd (
    ID SERIAL PRIMARY KEY,
    Marka VARCHAR(50),
    Model VARCHAR(50),
    RokProdukcji INT,
    VIN VARCHAR(17),
    TypSilnika VARCHAR(50),
    KolorNadwozia VARCHAR(30),
    WyposazenieWnetrza TEXT,
    Akcesoria TEXT
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
