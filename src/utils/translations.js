// Tłumaczenia typów akcesoriów i wyposażenia wnętrza

export const accessoryTypeTranslations = {
  'AlloyWheels': 'Felgi aluminiowe',
  'TintedWindows': 'Przyciemniane szyby',
  'DecorativeStrips': 'Listwy ozdobne',
  'Spoilers': 'Spoilery',
  'SillCovers': 'Osłony progów',
  'LicensePlateFrames': 'Ramki tablic rejestracyjnych',
  'FloorMats': 'Dywaniki',
  'PhoneTabletHolder': 'Uchwyt na telefon/tablet',
  'AmbientLighting': 'Oświetlenie nastrojowe',
  'SeatCovers': 'Pokrowce na fotele',
  'TrunkBags': 'Torby do bagażnika',
  'RoofRacks': 'Bagażniki dachowe',
  'RoofBoxes': 'Boksy dachowe',
  'BikeCarriers': 'Uchwyty rowerowe',
  'SkiCarriers': 'Uchwyty narciarskie',
  'WirelessChargers': 'Ładowarki bezprzewodowe',
  'PowerBanks': 'Power banki',
  'RearViewCameras': 'Kamery cofania',
  'AdditionalSensors': 'Dodatkowe czujniki',
  'SnowChains': 'Łańcuchy śniegowe',
  'USBCAdapters': 'Adaptery USB-C',
  'RearSeatChargers': 'Ładowarki tylne',
  'Navigation': 'Nawigacja',
  'AppleCarPlay': 'Apple CarPlay',
  'AndroidAuto': 'Android Auto',
  'WiFiModules': 'Moduły WiFi',
  'Hotspots': 'Hotspoty',
  'ChildSeats': 'Foteliki dziecięce',
  'SeatProtectiveMats': 'Maty ochronne foteli',
  'CargoNets': 'Siatki bagażnikowe'
};

export const interiorEquipmentTypeTranslations = {
  'SeatColor': 'Kolor foteli',
  'SeatHeating': 'Podgrzewanie foteli',
  'AdjustableHeadrests': 'Regulowane zagłówki',
  'MultifunctionSteeringWheel': 'Wielofunkcyjna kierownica',
  'RadioType': 'Typ radia',
  'AmbientLighting': 'Oświetlenie nastrojowe',
  'CruiseControl': 'Tempomat',
  'ElectricMirrors': 'Elektryczne lusterka'
};

export const accessoryCategoryTranslations = {
  'Wheels': 'Koła',
  'Exterior': 'Zewnętrzne',
  'Interior': 'Wnętrzne',
  'Electronics': 'Elektronika',
  'Transport': 'Transport',
  'Safety': 'Bezpieczeństwo',
  'Seasonal': 'Sezonowe',
  'Connectivity': 'Łączność',
  'Family': 'Rodzinne'
};

// Funkcje pomocnicze do tłumaczenia
export const translateAccessoryType = (type) => {
  return accessoryTypeTranslations[type] || type;
};

export const translateInteriorEquipmentType = (type) => {
  return interiorEquipmentTypeTranslations[type] || type;
};

export const translateAccessoryCategory = (category) => {
  return accessoryCategoryTranslations[category] || category;
};