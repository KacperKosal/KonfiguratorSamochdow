import React from 'react';
import styles from './CategoryTabs.module.css';

const CategoryTabs = ({ categories, activeCategory, setActiveCategory }) => {
  return (
    <div className={styles.container}>
      {categories.map(category => (
        <button
          key={category.id}
          onClick={() => setActiveCategory(category.id)}
          className={`${styles.tab} ${
            activeCategory === category.id 
              ? styles.tabActive 
              : styles.tabInactive
          }`}
        >
          {category.name}
        </button>
      ))}
    </div>
  );
};

export default CategoryTabs;
