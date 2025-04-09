import React from 'react';
import { Search, Sliders, ChevronDown } from 'lucide-react';
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
  formatPrice
}) => {
  return (
    <div className={styles.container}>
      <div className={styles.searchContainer}>
        <div className={styles.searchInput}>
          <input 
            type="text" 
            placeholder="Szukaj modelu..." 
            className={styles.input}
            value={searchQuery}
            onChange={(e) => setSearchQuery(e.target.value)}
          />
          <Search className={styles.searchIcon} size={18} />
        </div>
        <div className={styles.controls}>
          <select 
            className={styles.select}
            value={sortOption}
            onChange={(e) => setSortOption(e.target.value)}
          >
            <option value="popularity">Sortuj po: Popularności</option>
            <option value="price-asc">Sortuj po: Cena (rosnąco)</option>
            <option value="price-desc">Sortuj po: Cena (malejąco)</option>
            <option value="name">Sortuj po: Nazwie</option>
          </select>
          <button 
            className={styles.filterButton}
            onClick={() => setFiltersVisible(!filtersVisible)}
          >
            <Sliders size={18} />
            <span>Filtry</span>
            <ChevronDown size={14} className={`${styles.chevronIcon} ${filtersVisible ? styles.rotated : ''}`} />
          </button>
        </div>
      </div>
      
      {filtersVisible && (
        <div className={styles.filters}>
          <div className={styles.filtersGrid}>
            <div className={styles.filterGroup}>
              <h3 className={styles.filterTitle}>Zakres cenowy</h3>
              <div className={styles.rangeContainer}>
                <input 
                  type="range" 
                  min="50000" 
                  max="500000" 
                  step="10000"
                  value={priceRange[0]}
                  onChange={(e) => setPriceRange([parseInt(e.target.value), priceRange[1]])}
                  className={styles.rangeInput}
                />
                <span className={styles.priceLabel}>{formatPrice(priceRange[0])}</span>
              </div>
              <div className={styles.rangeContainer}>
                <input 
                  type="range" 
                  min="50000" 
                  max="500000" 
                  step="10000"
                  value={priceRange[1]}
                  onChange={(e) => setPriceRange([priceRange[0], parseInt(e.target.value)])}
                  className={styles.rangeInput}
                />
                <span className={styles.priceLabel}>{formatPrice(priceRange[1])}</span>
              </div>
            </div>
            <div className={styles.filterGroup}>
              <h3 className={styles.filterTitle}>Dodatkowe filtry</h3>
              <div className={styles.checkboxGroup}>
                <label className={styles.checkboxLabel}>
                  <input type="checkbox" className={styles.checkbox} />
                  <span>Tylko nowe modele</span>
                </label>
                <label className={styles.checkboxLabel}>
                  <input type="checkbox" className={styles.checkbox} />
                  <span>Tylko elektryczne</span>
                </label>
                <label className={styles.checkboxLabel}>
                  <input type="checkbox" className={styles.checkbox} />
                  <span>Napęd 4x4</span>
                </label>
              </div>
            </div>
          </div>
          <div className={styles.applyButton}>
            <button className={styles.button}>
              Zastosuj filtry
            </button>
          </div>
        </div>
      )}
    </div>
  );
};

export default SearchAndFilter;
