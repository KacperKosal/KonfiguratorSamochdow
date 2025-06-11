import React, { useState, useEffect } from 'react';
import Banner from '../../components/ModelsBrowser/Banner/Banner';
import SearchAndFilter from '../../components/ModelsBrowser/SearchAndFilter/SearchAndFilter';
import CategoryTabs from '../../components/ModelsBrowser/CategoryTabs/CategoryTabs';
import ModelsGrid from '../../components/ModelsBrowser/ModelsGrid/ModelsGrid';
import { AlertCircle, RefreshCw } from 'lucide-react';
import styles from './Home.module.css';
import axiosInstance from '../../services/axiosConfig';
import adminApiService from '../../services/adminApiService';

export default function Home() {
  const [activeCategory, setActiveCategory] = useState('all');
  const [sortOption, setSortOption] = useState('popularity');
  const [priceRange, setPriceRange] = useState([0, 1000000]);
  const [filtersVisible, setFiltersVisible] = useState(false);
  const [searchQuery, setSearchQuery] = useState('');
  const [carModels, setCarModels] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [manufacturers, setManufacturers] = useState([]);
  const [bodyTypes, setBodyTypes] = useState([]);
  const [autoRefresh, setAutoRefresh] = useState(false);
  const [lastRefresh, setLastRefresh] = useState(null);
  
  // Filter states
  const [selectedManufacturer, setSelectedManufacturer] = useState('');
  const [selectedBodyType, setSelectedBodyType] = useState('');
  const [onlyNew, setOnlyNew] = useState(false);
  const [onlyElectric, setOnlyElectric] = useState(false);
  const [only4x4, setOnly4x4] = useState(false);

  // Pobieranie wszystkich modeli z API
  const fetchCarModels = async (showLoading = true) => {
    try {
      if (showLoading) {
        setLoading(true);
      }
      setError(null);
      
      const response = await axiosInstance.get('/api/car-models');
      const data = response.data;
      
      // Mapowanie danych z API na format używany w komponencie
      const mappedModels = await Promise.all(data.map(async (model) => {
        // Pobierz silniki dla każdego modelu
        let engineNames = [];
        try {
          const engineResponse = await axiosInstance.get(`/api/car-models/${model.id}/engines`);
          if (engineResponse.data && Array.isArray(engineResponse.data)) {
            engineNames = engineResponse.data.map(engine => engine.engineName);
            console.log(`Pobrano ${engineNames.length} silników dla modelu ${model.name}: [${engineNames.join(', ')}]`);
          } else {
            console.log(`Brak silników dla modelu ${model.name}`);
          }
        } catch (err) {
          console.log(`Błąd pobierania silników dla modelu ${model.id}:`, err);
        }

        return {
          id: model.id,
          name: model.name,
          category: mapBodyTypeToCategory(model.bodyType),
          image: model.imageUrl ? adminApiService.getImageUrl(model.imageUrl) : '/api/placeholder/300/180',
          basePrice: model.basePrice,
          engineOptions: engineNames,
          description: model.description || 'Opis modelu',
          features: [], // Można rozszerzyć o dodatkowe cechy
          popularity: Math.floor(Math.random() * 5) + 1, // Tymczasowo losowa popularność
          new: new Date().getFullYear() - model.productionYear <= 1,
          electric: model.isElectric || false,
          has4x4: model.has4x4 || false,
          manufacturer: model.manufacturer,
          productionYear: model.productionYear,
          isActive: model.isActive
        };
      }));

      // Filtrowanie tylko aktywnych modeli
      const activeModels = mappedModels.filter(model => model.isActive);
      setCarModels(activeModels);
      
      // Wyciąganie unikalnych wartości dla filtrów
      const uniqueManufacturers = [...new Set(data.map(m => m.manufacturer).filter(Boolean))];
      const uniqueBodyTypes = [...new Set(data.map(m => m.bodyType).filter(Boolean))];
      
      setManufacturers(uniqueManufacturers);
      setBodyTypes(uniqueBodyTypes);
      setLastRefresh(new Date());
      
    } catch (err) {
      console.error('Error fetching car models:', err);
      if (err.response?.status === 401) {
        setError('Sesja wygasła. Zaloguj się ponownie.');
      } else {
        setError('Nie udało się pobrać modeli samochodów. Spróbuj ponownie.');
      }
    } finally {
      if (showLoading) {
        setLoading(false);
      }
    }
  };

  // Clear all filters
  const clearAllFilters = () => {
    setSelectedManufacturer('');
    setSelectedBodyType('');
    setOnlyNew(false);
    setOnlyElectric(false);
    setOnly4x4(false);
    setPriceRange([0, 1000000]);
  };

  // Filtrowanie modeli z użyciem API
  const fetchFilteredModels = async (filters) => {
    try {
      setLoading(true);
      setError(null);
      
      // Sprawdź czy są jakiekolwiek filtry
      const hasFilters = filters.manufacturer || filters.bodyType || 
                      filters.minPrice || filters.maxPrice || filters.minYear || filters.maxYear ||
                      filters.onlyElectric || filters.only4x4;
      
      let response;
      
      if (hasFilters) {
        // Użyj endpoint z filtrami
        const queryParams = new URLSearchParams();
        
        // API wymaga tych parametrów jako obowiązkowych
        queryParams.append('Manufacturer', filters.manufacturer || '');
        queryParams.append('BodyType', filters.bodyType || '');
        queryParams.append('Segment', '');
        
        // Opcjonalne parametry
        if (filters.minPrice) queryParams.append('MinPrice', filters.minPrice);
        if (filters.maxPrice) queryParams.append('MaxPrice', filters.maxPrice);
        if (filters.minYear) queryParams.append('MinProductionYear', filters.minYear);
        if (filters.maxYear) queryParams.append('MaxProductionYear', filters.maxYear);
        if (filters.onlyElectric) queryParams.append('IsElectric', 'true');
        if (filters.only4x4) queryParams.append('Has4x4', 'true');
        queryParams.append('IsActive', 'true');

        response = await axiosInstance.get(`/api/car-models/filter?${queryParams}`);
      } else {
        // Użyj podstawowy endpoint bez filtrów
        response = await axiosInstance.get('/api/car-models');
      }

      const data = response.data;
      
      let mappedModels = await Promise.all(data.map(async (model) => {
        // Pobierz silniki dla każdego modelu
        let engineNames = [];
        try {
          const engineResponse = await axiosInstance.get(`/api/car-models/${model.id}/engines`);
          if (engineResponse.data && Array.isArray(engineResponse.data)) {
            engineNames = engineResponse.data.map(engine => engine.engineName);
            console.log(`Pobrano ${engineNames.length} silników dla modelu ${model.name}: [${engineNames.join(', ')}]`);
          } else {
            console.log(`Brak silników dla modelu ${model.name}`);
          }
        } catch (err) {
          console.log(`Błąd pobierania silników dla modelu ${model.id}:`, err);
        }

        return {
          id: model.id,
          name: model.name,
          category: mapBodyTypeToCategory(model.bodyType),
          image: model.imageUrl ? adminApiService.getImageUrl(model.imageUrl) : '/api/placeholder/300/180',
          basePrice: model.basePrice,
          engineOptions: engineNames,
          description: model.description || 'Opis modelu',
          features: [],
          popularity: Math.floor(Math.random() * 5) + 1,
          new: new Date().getFullYear() - model.productionYear <= 1,
          // ✅ Używamy prawdziwych pól z bazy danych
          electric: model.isElectric || false,
          has4x4: model.has4x4 || false,
          manufacturer: model.manufacturer,
          productionYear: model.productionYear,
          isActive: model.isActive
        };
      }));

      // Filtrowanie tylko aktywnych modeli
      mappedModels = mappedModels.filter(model => model.isActive);

      // Filtrowanie only po stronie klienta (jeśli API nie obsługuje tych filtrów)
      // API już filtruje po IsElectric i Has4x4, więc te filtry nie są potrzebne tutaj

      // ✅ Dodaj filtr "Tylko nowe modele"
      if (filters.onlyNew) {
        mappedModels = mappedModels.filter(model => model.new);
      }
      
      setCarModels(mappedModels);
      setLastRefresh(new Date());
      
    } catch (err) {
      console.error('Error filtering car models:', err);
      if (err.response?.status === 401) {
        setError('Sesja wygasła. Zaloguj się ponownie.');
      } else {
        setError('Nie udało się przefiltrować modeli. Spróbuj ponownie.');
      }
    } finally {
      setLoading(false);
    }
  };

  // Mapowanie typów nadwozia na kategorie
  const mapBodyTypeToCategory = (bodyType) => {
    if (!bodyType) return 'all';
    return bodyType.toLowerCase();
  };

  // Pobieranie danych przy pierwszym załadowaniu
  useEffect(() => {
    fetchCarModels();
  }, []);

  // Automatyczne odświeżanie co 5 sekund
  useEffect(() => {
    if (!autoRefresh) return;

    const interval = setInterval(() => {
      fetchCarModels(false); // false = nie pokazuj loading spinner podczas auto-refresh
    }, 5000); // 5 sekund

    return () => clearInterval(interval);
  }, [autoRefresh]);

  // Kategorie modeli
  const categories = [
    { id: 'all', name: 'Wszystkie modele' },
    { id: 'sedan', name: 'Sedany' },
    { id: 'suv', name: 'SUV' },
    { id: 'electric', name: 'Elektryczne' },
    { id: 'sport', name: 'Sportowe' },
    { id: 'hybrid', name: 'Hybrydy' },
    { id: 'compact', name: 'Kompaktowe' }
  ];

  const formatPrice = (price) => {
    return price.toLocaleString() + ' zł';
  };

  // Filtrowanie modeli lokalnie
  const filteredModels = carModels.filter(model => {
    if (activeCategory !== 'all' && model.category !== activeCategory) return false;
    if (model.basePrice < priceRange[0] || model.basePrice > priceRange[1]) return false;
    if (searchQuery && !model.name.toLowerCase().includes(searchQuery.toLowerCase()) && 
        !model.description.toLowerCase().includes(searchQuery.toLowerCase())) return false;
    return true;
  });

  // Sortowanie modeli
  const sortedModels = [...filteredModels].sort((a, b) => {
    if (sortOption === 'price-asc') return a.basePrice - b.basePrice;
    if (sortOption === 'price-desc') return b.basePrice - a.basePrice;
    if (sortOption === 'popularity') return b.popularity - a.popularity;
    if (sortOption === 'name') return a.name.localeCompare(b.name);
    return 0;
  });

  // Funkcja odświeżania danych
  const handleRefresh = () => {
    fetchCarModels();
  };

  // Funkcja resetowania filtrów
  const handleResetFilters = () => {
    setActiveCategory('all');
    setPriceRange([100000, 500000]);
    setSearchQuery('');
    fetchCarModels();
  };

  // Funkcja przełączania auto-refresh
  const toggleAutoRefresh = () => {
    setAutoRefresh(!autoRefresh);
  };

  // Formatowanie czasu ostatniego odświeżenia
  const formatLastRefresh = (date) => {
    if (!date) return '';
    return date.toLocaleTimeString('pl-PL', { 
      hour: '2-digit', 
      minute: '2-digit', 
      second: '2-digit' 
    });
  };

  return (
    <div className={styles.page}>
      <Banner />
      <main className={styles.main}>
        <div className={styles.container}>
          {/* Auto-refresh controls */}
          <div className={styles.refreshControls}>
            <div className={styles.refreshInfo}>
              {lastRefresh && (
                <span className={styles.lastRefreshTime}>
                  Ostatnie odświeżenie: {formatLastRefresh(lastRefresh)}
                </span>
              )}
            </div>
            <div className={styles.refreshButtons}>
              <button 
                onClick={toggleAutoRefresh} 
                className={`${styles.autoRefreshButton} ${autoRefresh ? styles.active : ''}`}
              >
                <RefreshCw 
                  size={16} 
                  className={autoRefresh ? styles.spinning : ''} 
                />
                Auto-odświeżanie {autoRefresh ? 'ON' : 'OFF'}
              </button>
              <button onClick={handleRefresh} className={styles.manualRefreshButton}>
                <RefreshCw size={16} />
                Odśwież teraz
              </button>
            </div>
          </div>

          {/* Loading state */}
          {loading && (
            <div className={styles.loadingContainer}>
              <RefreshCw className={styles.loadingIcon} size={32} />
              <p className={styles.loadingText}>Ładowanie modeli...</p>
            </div>
          )}

          {/* Error state */}
          {error && (
            <div className={styles.errorContainer}>
              <AlertCircle className={styles.errorIcon} size={24} />
              <p className={styles.errorText}>{error}</p>
              <button onClick={handleRefresh} className={styles.retryButton}>
                <RefreshCw size={16} />
                Spróbuj ponownie
              </button>
            </div>
          )}

          {/* Main content */}
          {!loading && !error && (
            <>
              <SearchAndFilter 
                searchQuery={searchQuery}
                setSearchQuery={setSearchQuery}
                sortOption={sortOption}
                setSortOption={setSortOption}
                filtersVisible={filtersVisible}
                setFiltersVisible={setFiltersVisible}
                priceRange={priceRange}
                setPriceRange={setPriceRange}
                formatPrice={formatPrice}
                manufacturers={manufacturers}
                bodyTypes={bodyTypes}
                selectedManufacturer={selectedManufacturer}
                setSelectedManufacturer={setSelectedManufacturer}
                selectedBodyType={selectedBodyType}
                setSelectedBodyType={setSelectedBodyType}
                onlyNew={onlyNew}
                setOnlyNew={setOnlyNew}
                onlyElectric={onlyElectric}
                setOnlyElectric={setOnlyElectric}
                only4x4={only4x4}
                setOnly4x4={setOnly4x4}
                onFilter={fetchFilteredModels}
                onClearFilters={clearAllFilters}
              />
              <CategoryTabs 
                categories={categories} 
                activeCategory={activeCategory} 
                setActiveCategory={setActiveCategory} 
              />
              <ModelsGrid models={sortedModels} formatPrice={formatPrice} />
              
              {/* Empty state */}
              {sortedModels.length === 0 && (
                <div className={styles.emptyState}>
                  <h2 className={styles.emptyStateTitle}>Brak modeli spełniających kryteria</h2>
                  <p className={styles.emptyStateDescription}>
                    Spróbuj zmienić kryteria wyszukiwania lub filtry.
                  </p>
                  <button onClick={handleResetFilters} className={styles.resetButton}>
                    Resetuj filtry
                  </button>
                </div>
              )}
            </>
          )}
        </div>
      </main>
    </div>
  );
}