import React from 'react';

const CategoryTabs = ({ categories, activeCategory, setActiveCategory }) => {
  return (
    <div className="flex overflow-x-auto pb-4 mb-6 gap-2">
      {categories.map(category => (
        <button
          key={category.id}
          onClick={() => setActiveCategory(category.id)}
          className={`px-4 py-2 rounded-full whitespace-nowrap transition ${
            activeCategory === category.id 
              ? 'bg-blue-600 text-white' 
              : 'bg-white border border-gray-300 hover:bg-gray-50'
          }`}
        >
          {category.name}
        </button>
      ))}
    </div>
  );
};

export default CategoryTabs;
