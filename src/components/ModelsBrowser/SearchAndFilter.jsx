import React from 'react';
import { Search, Sliders, ChevronDown } from 'lucide-react';

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
    <div className="bg-white rounded-lg shadow-lg p-6 mb-8">
      <div className="flex flex-col md:flex-row md:items-center gap-4 mb-4">
        <div className="relative flex-grow">
          <input 
            type="text" 
            placeholder="Szukaj modelu..." 
            className="w-full pl-10 pr-4 py-2 border border-gray-300 rounded"
            value={searchQuery}
            onChange={(e) => setSearchQuery(e.target.value)}
          />
          <Search className="absolute left-3 top-2.5 text-gray-400" size={18} />
        </div>
        <div className="flex gap-2">
          <select 
            className="px-4 py-2 border border-gray-300 rounded"
            value={sortOption}
            onChange={(e) => setSortOption(e.target.value)}
          >
            <option value="popularity">Sortuj po: Popularności</option>
            <option value="price-asc">Sortuj po: Cena (rosnąco)</option>
            <option value="price-desc">Sortuj po: Cena (malejąco)</option>
            <option value="name">Sortuj po: Nazwie</option>
          </select>
          <button 
            className="flex items-center gap-1 px-4 py-2 border border-gray-300 rounded hover:bg-gray-50"
            onClick={() => setFiltersVisible(!filtersVisible)}
          >
            <Sliders size={18} />
            <span>Filtry</span>
            <ChevronDown size={14} className={`transition-transform ${filtersVisible ? 'rotate-180' : ''}`} />
          </button>
        </div>
      </div>
      
      {filtersVisible && (
        <div className="border-t pt-4 mt-4">
          <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
            <div>
              <h3 className="font-medium mb-3">Zakres cenowy</h3>
              <div className="flex items-center gap-4">
                <input 
                  type="range" 
                  min="50000" 
                  max="500000" 
                  step="10000"
                  value={priceRange[0]}
                  onChange={(e) => setPriceRange([parseInt(e.target.value), priceRange[1]])}
                  className="w-full"
                />
                <span className="whitespace-nowrap">{formatPrice(priceRange[0])}</span>
              </div>
              <div className="flex items-center gap-4 mt-2">
                <input 
                  type="range" 
                  min="50000" 
                  max="500000" 
                  step="10000"
                  value={priceRange[1]}
                  onChange={(e) => setPriceRange([priceRange[0], parseInt(e.target.value)])}
                  className="w-full"
                />
                <span className="whitespace-nowrap">{formatPrice(priceRange[1])}</span>
              </div>
            </div>
            <div>
              <h3 className="font-medium mb-3">Dodatkowe filtry</h3>
              <div className="flex flex-wrap gap-3">
                <label className="flex items-center gap-2">
                  <input type="checkbox" className="w-4 h-4" />
                  <span>Tylko nowe modele</span>
                </label>
                <label className="flex items-center gap-2">
                  <input type="checkbox" className="w-4 h-4" />
                  <span>Tylko elektryczne</span>
                </label>
                <label className="flex items-center gap-2">
                  <input type="checkbox" className="w-4 h-4" />
                  <span>Napęd 4x4</span>
                </label>
              </div>
            </div>
          </div>
          <div className="flex justify-end mt-4">
            <button className="px-4 py-2 bg-blue-600 text-white rounded hover:bg-blue-700">
              Zastosuj filtry
            </button>
          </div>
        </div>
      )}
    </div>
  );
};

export default SearchAndFilter;
