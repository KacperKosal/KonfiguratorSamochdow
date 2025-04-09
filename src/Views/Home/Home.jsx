import React, { useState } from 'react';
import Banner from '../../components/ModelsBrowser/Banner/Banner';
import SearchAndFilter from '../../components/ModelsBrowser/SearchAndFilter/SearchAndFilter';
import CategoryTabs from '../../components/ModelsBrowser/CategoryTabs/CategoryTabs';
import ModelsGrid from '../../components/ModelsBrowser/ModelsGrid/ModelsGrid';
import styles from './Home.module.css';

export default function Home() {
  const [activeCategory, setActiveCategory] = useState('all');
  const [sortOption, setSortOption] = useState('popularity');
  const [priceRange, setPriceRange] = useState([100000, 500000]);
  const [filtersVisible, setFiltersVisible] = useState(false);
  const [searchQuery, setSearchQuery] = useState('');

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

  // Dane przykładowych modeli
  const carModels = [
    {
      id: 'xdrive-gt',
      name: 'X-Drive GT',
      category: 'sport',
      image: '/api/placeholder/300/180',
      basePrice: 120000,
      engineOptions: ['2.0 Turbo 250KM', '2.5 V6 320KM'],
      description: 'Sportowy sedan łączący dynamikę prowadzenia z eleganckimi liniami nadwozia.',
      features: ['Napęd na tylne koła', 'Sportowe zawieszenie', 'Skrzynia automatyczna 8-biegowa'],
      popularity: 5,
      new: false,
      electric: false
    },
    // … pozostałe modele
  ];

  const formatPrice = (price) => {
    return price.toLocaleString() + ' zł';
  };

  // Filtrowanie modeli
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

  return (
    <div className={styles.page}>
      <Banner />
      <main className={styles.main}>
        <div className={styles.container}>
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
          />
          <CategoryTabs 
            categories={categories} 
            activeCategory={activeCategory} 
            setActiveCategory={setActiveCategory} 
          />
          <ModelsGrid models={sortedModels} formatPrice={formatPrice} />
          {sortedModels.length === 0 && (
            <div className={styles.emptyState}>
              <h2 className={styles.emptyStateTitle}>Brak modeli spełniających kryteria</h2>
              <p className={styles.emptyStateDescription}>Spróbuj zmienić kryteria wyszukiwania lub filtry.</p>
              <button 
                onClick={() => {
                  setActiveCategory('all');
                  setPriceRange([100000, 500000]);
                  setSearchQuery('');
                }}
                className={styles.resetButton}
              >
                Resetuj filtry
              </button>
            </div>
          )}
        </div>
      </main>
    </div>
  );
}