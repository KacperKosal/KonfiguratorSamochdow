import React, { useState, useEffect } from 'react';
import { Search, Sliders, ChevronDown, Filter, X, Check } from 'lucide-react';
import styles from './SearchAndFilter.module.css';

const SearchAndFilter = ({
  searchQuery,
  setSearchQuery,
  sortOption,
  setSortOption,
  filtersVisible,
  setFiltersVisible,
  priceRange,
  setPriceRange,
  formatPrice,
  manufacturers = [],
  bodyTypes = [],
  selectedManufacturer,
  setSelectedManufacturer,
  selectedBodyType,
  setSelectedBodyType,
  onlyNew,
  setOnlyNew,
  onlyElectric,
  setOnlyElectric,
  only4x4,
  setOnly4x4,
  onFilter,
  onClearFilters
}) => {
  const [activeFiltersCount, setActiveFiltersCount] = useState(0);

  // Update active filters count when filter values change
  useEffect(() => {
    let count = 0;
    if (selectedManufacturer) count++;
    if (selectedBodyType) count++;
    if (onlyNew) count++;
    if (onlyElectric) count++;
    if (only4x4) count++;
    if (priceRange[0] > 0 || priceRange[1] < 1000000) count++;
    
    setActiveFiltersCount(count);
  }, [selectedManufacturer, selectedBodyType, onlyNew, onlyElectric, only4x4, priceRange]);

  const sortOptions = [
    { value: 'popularity', label: 'Popularność', icon: '🔥' },
    { value: 'price-asc', label: 'Cena: od najniższej', icon: '💰' },
    { value: 'price-desc', label: 'Cena: od najwyższej', icon: '💎' },
    { value: 'name', label: 'Nazwa A-Z', icon: '🔤' },
    { value: 'year', label: 'Rok produkcji', icon: '📅' }
  ];

  const handleApplyFilters = (e) => {
    // Zapobieganie domyślnemu zachowaniu formularza
    if (e) {
      e.preventDefault();
    }
    
    const filters = {
      manufacturer: selectedManufacturer,
      bodyType: selectedBodyType,
      minPrice: priceRange[0],
      maxPrice: priceRange[1],
      // Dodanie obsługi dodatkowych filtrów
      minYear: onlyNew ? new Date().getFullYear() : null,
      maxYear: onlyNew ? new Date().getFullYear() : null,
      onlyNew,
      onlyElectric,
      only4x4
    };
    
    // Sprawdzenie czy funkcja onFilter istnieje przed wywołaniem
    if (onFilter && typeof onFilter === 'function') {
      try {
        onFilter(filters);
      } catch (error) {
        console.error('Błąd podczas stosowania filtrów:', error);
      }
    } else {
      console.warn('Funkcja onFilter nie została przekazana lub nie jest funkcją');
    }
  };

  const handleClearFilters = (e) => {
    // Zapobieganie domyślnemu zachowaniu formularza
    if (e) {
      e.preventDefault();
    }
    
    if (onClearFilters && typeof onClearFilters === 'function') {
      onClearFilters();
    }
  };

  const handleSearchClear = () => {
    setSearchQuery('');
  };

  return (
    <div className={styles.container}>
      <div className={styles.searchContainer}>
        <div className={styles.searchInput}>
          <div className={styles.searchWrapper}>
            <Search className={styles.searchIcon} size={20} />
            <input 
              type="text" 
              placeholder="Wyszukaj model, markę lub opis..." 
              className={styles.input}
              value={searchQuery}
              onChange={(e) => setSearchQuery(e.target.value)}
            />
            {searchQuery && (
              <button 
                className={styles.clearButton}
                onClick={handleSearchClear}
                aria-label="Wyczyść wyszukiwanie"
              >
                <X size={16} />
              </button>
            )}
          </div>
        </div>
        
        <div className={styles.controls}>
          <div className={styles.sortContainer}>
            <label className={styles.sortLabel}>Sortuj:</label>
            <select 
              className={styles.select}
              value={sortOption}
              onChange={(e) => setSortOption(e.target.value)}
            >
              {sortOptions.map(option => (
                <option key={option.value} value={option.value}>
                  {option.icon} {option.label}
                </option>
              ))}
            </select>
          </div>
          
          <button 
            className={`${styles.filterButton} ${filtersVisible ? styles.active : ''}`}
            onClick={() => setFiltersVisible(!filtersVisible)}
          >
            <Filter size={18} />
            <span>Filtry</span>
            {activeFiltersCount > 0 && (
              <span className={styles.filterBadge}>{activeFiltersCount}</span>
            )}
            <ChevronDown 
              size={14} 
              className={`${styles.chevronIcon} ${filtersVisible ? styles.rotated : ''}`} 
            />
          </button>
        </div>
      </div>
      
      {filtersVisible && (
        <div className={styles.filters}>
          <div className={styles.filtersHeader}>
            <h3 className={styles.filtersTitle}>
              <Filter size={20} />
              Zaawansowane filtry
            </h3>
            <button 
              className={styles.clearFiltersButton}
              onClick={handleClearFilters}
            >
              <X size={16} />
              Wyczyść wszystkie
            </button>
          </div>
          
          <div className={styles.filtersGrid}>
            {/* Zakres cenowy */}
            <div className={styles.filterGroup}>
              <h4 className={styles.filterTitle}>
                💰 Zakres cenowy
              </h4>
              <div className={styles.priceRangeContainer}>
                <div className={styles.priceInputs}>
                  <div className={styles.priceInput}>
                    <label>Od:</label>
                    <input 
                      type="number" 
                      min="0"
                      max="1000000"
                      value={priceRange[0]}
                      onChange={(e) => {
                        const value = parseInt(e.target.value) || 0;
                        const clampedValue = Math.max(0, Math.min(1000000, value));
                        if (clampedValue <= priceRange[1]) {
                          setPriceRange([clampedValue, priceRange[1]]);
                        }
                      }}
                      className={styles.numberInput}
                    />
                  </div>
                  <div className={styles.priceInput}>
                    <label>Do:</label>
                    <input 
                      type="number" 
                      min="0"
                      max="1000000"
                      value={priceRange[1]}
                      onChange={(e) => {
                        const value = parseInt(e.target.value) || 1000000;
                        const clampedValue = Math.max(0, Math.min(1000000, value));
                        if (clampedValue >= priceRange[0]) {
                          setPriceRange([priceRange[0], clampedValue]);
                        }
                      }}
                      className={styles.numberInput}
                    />
                  </div>
                </div>
                <div className={styles.rangeSlider}>
                  <input 
                    type="range" 
                    min="0" 
                    max="1000000" 
                    step="10000"
                    value={priceRange[0]}
                    onChange={(e) => {
                      const value = parseInt(e.target.value);
                      if (value <= priceRange[1]) {
                        setPriceRange([value, priceRange[1]]);
                      }
                    }}
                    className={styles.rangeInput}
                  />
                  <input 
                    type="range" 
                    min="0" 
                    max="1000000" 
                    step="10000"
                    value={priceRange[1]}
                    onChange={(e) => {
                      const value = parseInt(e.target.value);
                      if (value >= priceRange[0]) {
                        setPriceRange([priceRange[0], value]);
                      }
                    }}
                    className={styles.rangeInput}
                  />
                </div>
                <div className={styles.priceLabels}>
                  <span>{formatPrice(priceRange[0])}</span>
                  <span>{formatPrice(priceRange[1])}</span>
                </div>
              </div>
            </div>

            {/* Marka */}
            {manufacturers.length > 0 && (
              <div className={styles.filterGroup}>
                <h4 className={styles.filterTitle}>
                  🏭 Marka
                </h4>
                <select 
                  className={styles.filterSelect}
                  value={selectedManufacturer}
                  onChange={(e) => setSelectedManufacturer(e.target.value)}
                >
                  <option value="">Wszystkie marki</option>
                  {manufacturers.map(manufacturer => (
                    <option key={manufacturer} value={manufacturer}>
                      {manufacturer}
                    </option>
                  ))}
                </select>
              </div>
            )}

            {/* Typ nadwozia */}
            {bodyTypes.length > 0 && (
              <div className={styles.filterGroup}>
                <h4 className={styles.filterTitle}>
                  🚗 Typ nadwozia
                </h4>
                <select 
                  className={styles.filterSelect}
                  value={selectedBodyType}
                  onChange={(e) => setSelectedBodyType(e.target.value)}
                >
                  <option value="">Wszystkie typy</option>
                  {bodyTypes.map(bodyType => (
                    <option key={bodyType} value={bodyType}>
                      {bodyType}
                    </option>
                  ))}
                </select>
              </div>
            )}


            {/* Dodatkowe opcje */}
            <div className={styles.filterGroup}>
              <h4 className={styles.filterTitle}>
                ⚡ Dodatkowe opcje
              </h4>
              <div className={styles.checkboxGroup}>
                <label className={styles.checkboxLabel}>
                  <input 
                    type="checkbox" 
                    className={styles.checkbox}
                    checked={onlyNew}
                    onChange={(e) => setOnlyNew(e.target.checked)}
                  />
                  <span className={styles.checkboxText}>
                    <Check size={14} className={styles.checkIcon} />
                    Tylko nowe modele
                  </span>
                </label>
                <label className={styles.checkboxLabel}>
                  <input 
                    type="checkbox" 
                    className={styles.checkbox}
                    checked={onlyElectric}
                    onChange={(e) => setOnlyElectric(e.target.checked)}
                  />
                  <span className={styles.checkboxText}>
                    <Check size={14} className={styles.checkIcon} />
                    Tylko elektryczne
                  </span>
                </label>
                <label className={styles.checkboxLabel}>
                  <input 
                    type="checkbox" 
                    className={styles.checkbox}
                    checked={only4x4}
                    onChange={(e) => setOnly4x4(e.target.checked)}
                  />
                  <span className={styles.checkboxText}>
                    <Check size={14} className={styles.checkIcon} />
                    Napęd 4x4
                  </span>
                </label>
              </div>
            </div>
          </div>
          
          <div className={styles.filtersActions}>
            <button 
              type="button"
              className={styles.applyButton}
              onClick={handleApplyFilters}
            >
              <Check size={16} />
              Zastosuj filtry
            </button>
          </div>
        </div>
      )}
    </div>
  );
};

export default SearchAndFilter;
