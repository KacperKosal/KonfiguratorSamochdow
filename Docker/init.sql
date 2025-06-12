/* ===========================================================
   1.  TYPOWY ENUM
   ===========================================================*/
CREATE TYPE RolaTyp   AS ENUM ('Administrator', 'Uzytkownik');
CREATE TYPE StatusTyp AS ENUM ('Sukces', 'Niepowodzenie');


/* ===========================================================
   2.  TABELE BEZ ZALEŻNOŚCI / KORZENIE
   ===========================================================*/
-- Użytkownicy
CREATE TABLE Uzytkownik (
    ID SERIAL PRIMARY KEY,
    ImieNazwisko VARCHAR(100),
    Email VARCHAR(100),
    Haslo VARCHAR(255),
    RefreshToken VARCHAR(255),
    RefreshTokenExpires TIMESTAMP,
    Rola VARCHAR(255)
);

-- Pojazd – potrzebny zanim stworzymy silniki i powiązania
CREATE TABLE Pojazd (
    ID SERIAL PRIMARY KEY,
    Model VARCHAR(50),
    KolorNadwozia VARCHAR(30),
    TypNadwozia VARCHAR(50),
    WyposazenieWnetrza TEXT,
    Cena INT,
    Opis TEXT,
    Ma4x4 BOOLEAN,
    JestElektryczny BOOLEAN,
    Akcesoria TEXT,
    Zdjecie BYTEA,
    ImageUrl VARCHAR(255),
    isactive BOOLEAN DEFAULT true
);

-- Tabele bez FK-ów
CREATE TABLE user_configurations (
    id SERIAL PRIMARY KEY,
    user_id INTEGER NOT NULL,
    configuration_name VARCHAR(255) NOT NULL,
    car_model_id VARCHAR(255),
    car_model_name VARCHAR(255),
    engine_id VARCHAR(255),
    engine_name VARCHAR(255),
    exterior_color VARCHAR(50),
    exterior_color_name VARCHAR(100),
    selected_accessories TEXT,
    selected_interior_equipment TEXT,
    total_price DECIMAL(12, 2) NOT NULL DEFAULT 0,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE,
    is_active BOOLEAN DEFAULT true
);

CREATE TABLE car_model_colors (
    "Id"          VARCHAR(255) PRIMARY KEY,
    "CarModelId"  VARCHAR(255) NOT NULL,
    "ColorName"   VARCHAR(100) NOT NULL,
    "Price"       INTEGER NOT NULL DEFAULT 0 CHECK ("Price" >= 0 AND "Price" <= 60000),
    "CreatedAt"   TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    "UpdatedAt"   TIMESTAMP WITH TIME ZONE,
    CONSTRAINT unique_car_model_color UNIQUE ("CarModelId", "ColorName")
);

-- Pierwsza (główna) definicja zdjęć pojazdów
CREATE TABLE pojazd_zdjecie (
    "Id"           VARCHAR(255) PRIMARY KEY,
    "CarModelId"   VARCHAR(255) NOT NULL,
    "ImageUrl"     VARCHAR(500) NOT NULL,
    "Color"        VARCHAR(100) NOT NULL DEFAULT '',
    "DisplayOrder" INTEGER      NOT NULL DEFAULT 1,
    "IsMainImage"  BOOLEAN      NOT NULL DEFAULT FALSE,
    "CreatedAt"    TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    "UpdatedAt"    TIMESTAMP WITH TIME ZONE
);

-- Wyposażenie wnętrza (pierwsza definicja)
CREATE TABLE car_interior_equipment (
    id               VARCHAR(255) PRIMARY KEY,
    type             VARCHAR(50)  NOT NULL,
    value            VARCHAR(255) NOT NULL,
    description      TEXT,
    additional_price DECIMAL(10, 2) NOT NULL DEFAULT 0,
    has_navigation   BOOLEAN DEFAULT FALSE,
    has_premium_sound BOOLEAN DEFAULT FALSE,
    control_type     VARCHAR(50)
);

-- Akcesoria (bez FK – pola tekstowe przechowują identyfikatory)
CREATE TABLE car_accessories (
    id                  VARCHAR(255) PRIMARY KEY,
    car_id              VARCHAR(255) NOT NULL,
    car_model           VARCHAR(255) NOT NULL,
    category            VARCHAR(50)  NOT NULL,
    type                VARCHAR(50)  NOT NULL,
    name                VARCHAR(255) NOT NULL,
    description         TEXT,
    price               DECIMAL(10, 2) NOT NULL,
    manufacturer        VARCHAR(255),
    part_number         VARCHAR(100),
    is_original_bmw_part BOOLEAN NOT NULL,
    is_in_stock         BOOLEAN NOT NULL,
    stock_quantity      INT     NOT NULL,
    image_url           TEXT,
    size                VARCHAR(100),
    pattern             VARCHAR(100),
    color               VARCHAR(100),
    material            VARCHAR(100),
    capacity            INT,
    compatibility       VARCHAR(255),
    age_group           VARCHAR(50),
    max_load            INT,
    is_universal        BOOLEAN NOT NULL,
    installation_difficulty VARCHAR(50),
    warranty            VARCHAR(100)
);


/* ===========================================================
   3.  TABELE Z FK → POJAZD
   ===========================================================*/
-- Silnik z rozbudowanymi polami (z cytatami – nazwa pozostaje wielką literą)
CREATE TABLE "Silnik" (
    "ID" SERIAL PRIMARY KEY,
    "PojazdID" INT REFERENCES Pojazd(ID),
    "Pojemnosc" VARCHAR(10),
    "Typ" VARCHAR(20),
    "Moc" SMALLINT,
    "Nazwa" VARCHAR(255),
    "MomentObrotowy" INT,
    "RodzajPaliwa" VARCHAR(50),
    "Cylindry" INT,
    "Skrzynia" VARCHAR(50),
    "Biegi" INT,
    "NapedzType" VARCHAR(50),
    "ZuzyciePaliva" VARCHAR(10),
    "EmisjaCO2" INT,
    "Opis" TEXT,
    "JestAktywny" BOOLEAN DEFAULT TRUE
);

-- Druga (prostsza) wersja silnika – zapis bez cudzysłowów (=> nazwa „silnik”)
-- Usunięto duplikat tabeli Silnik - używamy tylko wersji z cytatami powyżej

-- Powiązanie model–silnik
CREATE TABLE ModelSilnik (
    ID SERIAL PRIMARY KEY,
    ModelID  INT REFERENCES Pojazd(ID),
    SilnikID INT REFERENCES "Silnik"("ID"),
    CenaDodatkowa DECIMAL(10,2) DEFAULT 0
);

-- Cechy pojazdu
CREATE TABLE CechyPojazdu (
    ID SERIAL PRIMARY KEY,
    IDPojazdu INT REFERENCES Pojazd(ID),
    Cecha VARCHAR(100)
);


/* ===========================================================
   4.  TABELE Z FK → UŻYTKOWNIK I INNE
   ===========================================================*/
-- Wyposażenie (ogólne)
CREATE TABLE ElementWyposazenia (
    ID SERIAL PRIMARY KEY,
    TypElementu VARCHAR(50),
    Wartosc VARCHAR(100),
    OpisElementu TEXT,
    CenaDodatkowa DECIMAL(8, 2)
);

-- Konfiguracje
CREATE TABLE Konfiguracja (
    ID SERIAL PRIMARY KEY,
    IDUzytkownika INT REFERENCES Uzytkownik(ID),
    IDPojazdu INT REFERENCES Pojazd(ID),
    ModelSilnikID INT REFERENCES ModelSilnik(ID),
    DataUtworzenia TIMESTAMP,
    NazwaKonfiguracji VARCHAR(100),
    OpisKonfiguracji TEXT
);

-- Udostępnienia konfiguracji
CREATE TABLE UdostepnienieKonfiguracji (
    ID SERIAL PRIMARY KEY,
    IDKonfiguracji INT REFERENCES Konfiguracja(ID),
    Link VARCHAR(255),
    Odbiorca VARCHAR(100),
    DataUdostepnienia TIMESTAMP
);

-- Logi
CREATE TABLE Logowanie (
    ID SERIAL PRIMARY KEY,
    IDUzytkownika INT REFERENCES Uzytkownik(ID),
    DataLogowania TIMESTAMP,
    AdresIP VARCHAR(45),
    Status StatusTyp
);

CREATE TABLE Rejestracja (
    ID SERIAL PRIMARY KEY,
    IDUzytkownika INT REFERENCES Uzytkownik(ID),
    DataRejestracji TIMESTAMP,
    AdresIP VARCHAR(45),
    Status StatusTyp
);

-- Administratorzy
CREATE TABLE Administrator (
    ID SERIAL PRIMARY KEY,
    IDUzytkownika INT REFERENCES Uzytkownik(ID),
    PoziomUprawnien VARCHAR(30),
    OstatnieLogowanie TIMESTAMP
);


/* ===========================================================
   5.  DRUGA KOPIA TABELE / ZDUPLIKOWANE DEFINICJE
   ===========================================================*/
-- Zdjęcia pojazdu – duplikat (bez IF NOT EXISTS)
CREATE TABLE pojazd_zdjecie (
    "Id"           VARCHAR(255) PRIMARY KEY,
    "CarModelId"   VARCHAR(255) NOT NULL,
    "ImageUrl"     VARCHAR(500) NOT NULL,
    "Color"        VARCHAR(100) NOT NULL DEFAULT '',
    "DisplayOrder" INTEGER      NOT NULL DEFAULT 1,
    "IsMainImage"  BOOLEAN      NOT NULL DEFAULT FALSE,
    "CreatedAt"    TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    "UpdatedAt"    TIMESTAMP WITH TIME ZONE
);

-- Wyposażenie wnętrza – duplikat z IF NOT EXISTS
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


/* ===========================================================
   6.  INDEKSY (po wszystkich CREATE TABLE)
   ===========================================================*/
-- user_configurations
CREATE INDEX idx_user_configurations_user_id      ON user_configurations(user_id);
CREATE INDEX idx_user_configurations_created_at   ON user_configurations(created_at);

-- car_model_colors
CREATE INDEX idx_car_model_colors_carmodelid      ON car_model_colors("CarModelId");
CREATE INDEX idx_car_model_colors_colorname       ON car_model_colors("ColorName");

-- pojazd_zdjecie (tylko pierwsza wersja tabeli)
CREATE INDEX idx_pojazd_zdjecie_carmodelid        ON pojazd_zdjecie("CarModelId");
CREATE INDEX idx_pojazd_zdjecie_displayorder      ON pojazd_zdjecie("DisplayOrder");
CREATE INDEX idx_pojazd_zdjecie_color             ON pojazd_zdjecie("Color");

-- car_accessories
CREATE INDEX idx_car_accessories_car_id           ON car_accessories (car_id);
CREATE INDEX idx_car_accessories_car_model        ON car_accessories (car_model);
CREATE INDEX idx_car_accessories_category         ON car_accessories (category);
CREATE INDEX idx_car_accessories_type             ON car_accessories (type);
CREATE INDEX idx_car_accessories_price            ON car_accessories (price);
CREATE INDEX idx_car_accessories_is_original_bmw_part ON car_accessories (is_original_bmw_part);
CREATE INDEX idx_car_accessories_is_in_stock      ON car_accessories (is_in_stock);


/* ===========================================================
   7.  BLOKI MIGRACYJNE (ALTER TABLE itd.) – muszą być po CREATE
   ===========================================================*/
-- Dodanie kolumn (ImageUrl, IsActive, TypNadwozia) do Pojazd
DO $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM information_schema.columns
                   WHERE table_name = 'pojazd' AND column_name = 'imageurl') THEN
        ALTER TABLE pojazd ADD COLUMN imageurl VARCHAR(255);
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM information_schema.columns
                   WHERE table_name = 'pojazd' AND column_name = 'isactive') THEN
        ALTER TABLE pojazd ADD COLUMN isactive BOOLEAN DEFAULT true;
        UPDATE pojazd SET isactive = true WHERE isactive IS NULL;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM information_schema.columns
                   WHERE table_name = 'pojazd' AND column_name = 'typnadwozia') THEN
        ALTER TABLE pojazd ADD COLUMN typnadwozia VARCHAR(50);
        UPDATE pojazd SET typnadwozia = 'Sedan' WHERE typnadwozia IS NULL;
    END IF;
END $$;


/* ===========================================================
   8.  “KOPIE” TABEL MAŁYMI LITERAMI (zgodność z EF)
   ===========================================================*/
CREATE TABLE IF NOT EXISTS pojazd AS
    SELECT id, model, kolornadwozia, typnadwozia,
           wyposazeniewetrza, cena, opis, ma4x4,
           jestelektryczny, akcesoria, zdjecie, imageurl
    FROM Pojazd;

-- Tabela "Silnik" (z cytatami) już istnieje w PostgreSQL - nie tworzymy kopii lowercase

-- Najpierw utwórz strukturę tabeli modelsilnik z właściwymi kluczami obcymi
CREATE TABLE IF NOT EXISTS modelsilnik (
    id SERIAL PRIMARY KEY,
    modelid INT REFERENCES pojazd(id),
    silnikid INT REFERENCES "Silnik"("ID"),
    cenadodatkowa DECIMAL(10,2) DEFAULT 0
);

-- Następnie wstaw dane z tabeli ModelSilnik (jeśli istnieją)
INSERT INTO modelsilnik (id, modelid, silnikid, cenadodatkowa)
SELECT id, modelid, silnikid, cenadodatkowa
FROM ModelSilnik
ON CONFLICT (id) DO NOTHING;

CREATE TABLE IF NOT EXISTS konfiguracja AS
    SELECT id, iduzytkownika, idpojazdu, modelsilnikid,
           datautworzenia, nazwakonfiguracji, opiskonfiguracji
    FROM Konfiguracja;

CREATE TABLE IF NOT EXISTS cechypojazdu AS
    SELECT id, idpojazdu, cecha
    FROM CechyPojazdu;
