import React, { useState } from 'react';
import Header from '../Header';
import Banner from '../ModelsBrowser/Banner';
import SearchAndFilter from './SearchAndFilter';
import CategoryTabs from './CategoryTabs';
import ModelsGrid from './ModelsGrid';
import CTA from './CTA';
import Footer from '../Footer';

const ModelsBrowser = () => {
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
    <div className="min-h-screen flex flex-col">
      <Header />
      <Banner />
      <main className="container mx-auto px-4 py-8 flex-grow">
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
          <div className="bg-white rounded-lg shadow-lg p-12 text-center">
            <h2 className="text-xl font-bold mb-4">Brak modeli spełniających kryteria</h2>
            <p className="text-gray-600 mb-6">Spróbuj zmienić kryteria wyszukiwania lub filtry.</p>
            <button 
              onClick={() => {
                setActiveCategory('all');
                setPriceRange([100000, 500000]);
                setSearchQuery('');
              }}
              className="px-6 py-2 bg-blue-600 text-white rounded hover:bg-blue-700"
            >
              Resetuj filtry
            </button>
          </div>
        )}
      </main>
      <CTA />
      <Footer />
    </div>
  );
};

export default ModelsBrowser;
