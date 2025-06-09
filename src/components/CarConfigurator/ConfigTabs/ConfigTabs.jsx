import React from 'react';
import styles from './ConfigTabs.module.css';

const ConfigTabs = ({ activeTab, setActiveTab }) => {
  return (
    <div className={styles.tabs}>
      <button
        onClick={() => setActiveTab('engine')}
        className={`${styles.tabButton} ${activeTab === 'engine' ? styles.active : ''}`}
      >
        🔧 Silnik
      </button>
      <button
        onClick={() => setActiveTab('exterior')}
        className={`${styles.tabButton} ${activeTab === 'exterior' ? styles.active : ''}`}
      >
        Nadwozie
      </button>
      <button
        onClick={() => setActiveTab('wheels')}
        className={`${styles.tabButton} ${activeTab === 'wheels' ? styles.active : ''}`}
      >
        Felgi
      </button>
      <button
        onClick={() => setActiveTab('interior')}
        className={`${styles.tabButton} ${activeTab === 'interior' ? styles.active : ''}`}
      >
        Wnętrze
      </button>
      <button
        onClick={() => setActiveTab('accessories')}
        className={`${styles.tabButton} ${activeTab === 'accessories' ? styles.active : ''}`}
      >
        Akcesoria
      </button>
    </div>
  );
};

export default ConfigTabs;
