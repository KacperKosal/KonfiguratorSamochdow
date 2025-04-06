import React from 'react';

const ConfigTabs = ({ activeTab, setActiveTab }) => {
  return (
    <div className="tabs">
      <button
        onClick={() => setActiveTab('exterior')}
        className={`tab-button ${activeTab === 'exterior' ? 'active' : ''}`}
      >
        Nadwozie
      </button>
      <button
        onClick={() => setActiveTab('wheels')}
        className={`tab-button ${activeTab === 'wheels' ? 'active' : ''}`}
      >
        Felgi
      </button>
      <button
        onClick={() => setActiveTab('interior')}
        className={`tab-button ${activeTab === 'interior' ? 'active' : ''}`}
      >
        Wnętrze
      </button>
      <button
        onClick={() => setActiveTab('accessories')}
        className={`tab-button ${activeTab === 'accessories' ? 'active' : ''}`}
      >
        Akcesoria
      </button>
    </div>
  );
};

export default ConfigTabs;
