import React, { useState, useEffect } from 'react';
import { Plus, Edit, Trash2, X, Save } from 'lucide-react';
import adminApiService from '../../services/adminApiService';
import styles from './CarAdminPanel.module.css';

const EngineAssignment = ({ model, onClose }) => {
  const [assignedEngines, setAssignedEngines] = useState([]);
  const [availableEngines, setAvailableEngines] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [showAddForm, setShowAddForm] = useState(false);
  const [selectedEngine, setSelectedEngine] = useState('');
  const [engineData, setEngineData] = useState({
    additionalPrice: 0,
    isDefault: false,
    topSpeed: 200,
    acceleration0To100: 8.0,
    isAvailable: true
  });

  useEffect(() => {
    if (model) {
      loadData();
    }
  }, [model]);

  const loadData = async () => {
    try {
      setLoading(true);
      setError('');
      
      // Załaduj wszystkie dostępne silniki
      const allRaw = await adminApiService.getAllEngines();
      const all = (allRaw || []).filter(engine => engine && engine.id);
      
      // Załaduj przypisane silniki
      let assigned = [];
      try {
        assigned = await adminApiService.getEnginesForModel(model.id);
      } catch (assignedError) {
        console.warn('Could not load assigned engines, assuming none assigned:', assignedError);
        // Jeśli nie udało się załadować przypisanych silników, ustaw pustą tablicę
        assigned = [];
      }
      
      setAssignedEngines(assigned);
      
      // Filtruj silniki które nie są już przypisane
      const assignedIds = assigned.map(ae => ae.engineId || ae.id).filter(id => id); // Usuń puste ID
      const available = all.filter(engine => {
        const engineId = engine.id || engine.engineId;
        return engineId && !assignedIds.includes(engineId);
      });
      console.log('Assigned IDs:', assignedIds);
      console.log('Available engines after filtering:', available);
      setAvailableEngines(available);
      
    } catch (error) {
      console.error('Error loading data:', error);
      setError('Nie udało się załadować danych: ' + error.message);
      // W przypadku błędu, spróbuj załadować przynajmniej wszystkie silniki
      try {
        const all = await adminApiService.getAllEngines();
        setAvailableEngines(all);
        setAssignedEngines([]);
      } catch (fallbackError) {
        console.error('Fallback loading also failed:', fallbackError);
      }
    } finally {
      setLoading(false);
    }
  };

  const handleAddEngine = async () => {
    if (!selectedEngine) {
      setError('Wybierz silnik do dodania');
      return;
    }

    try {
      const addData = {
        engineId: selectedEngine,
        additionalPrice: parseFloat(engineData.additionalPrice) || 0,
        isDefault: engineData.isDefault,
        topSpeed: parseInt(engineData.topSpeed) || 200,
        acceleration0To100: parseFloat(engineData.acceleration0To100) || 8.0,
        isAvailable: engineData.isAvailable
      };

      await adminApiService.addEngineToModel(model.id, addData);
      setShowAddForm(false);
      setSelectedEngine('');
      setEngineData({
        additionalPrice: 0,
        isDefault: false,
        topSpeed: 200,
        acceleration0To100: 8.0,
        isAvailable: true
      });
      await loadData();
    } catch (error) {
      console.error('Error adding engine:', error);
      setError('Nie udało się dodać silnika');
    }
  };

  const handleRemoveEngine = async (engineId) => {
    if (!window.confirm('Czy na pewno chcesz usunąć ten silnik z modelu?')) {
      return;
    }

    try {
      await adminApiService.removeEngineFromModel(model.id, engineId);
      await loadData();
    } catch (error) {
      console.error('Error removing engine:', error);
      setError('Nie udało się usunąć silnika');
    }
  };

  const handleInputChange = (e) => {
    const { name, value, type, checked } = e.target;
    setEngineData(prev => ({
      ...prev,
      [name]: type === 'checkbox' ? checked : value
    }));
  };

  if (loading) {
    return (
      <div className={styles.modal}>
        <div className={styles.modalContent}>
          <div className={styles.loading}>Ładowanie...</div>
        </div>
      </div>
    );
  }

  return (
    <div className={styles.modal}>
      <div className={styles.modalContent} style={{ maxWidth: '800px' }}>
        <div className={styles.modalHeader}>
          <h3>Zarządzanie silnikami - {model.name}</h3>
          <button onClick={onClose} className={styles.closeButton}>
            <X size={20} />
          </button>
        </div>

        <div className={styles.modalBody}>
          {error && (
            <div className={styles.errorMessage}>
              {error}
            </div>
          )}

          {/* Lista przypisanych silników */}
          <div className={styles.section}>
            <div className={styles.sectionHeader}>
              <h4>Przypisane silniki</h4>
              <button 
                onClick={() => setShowAddForm(true)}
                className={styles.addButton}
                style={{ padding: '0.5rem 1rem', fontSize: '0.875rem' }}
              >
                <Plus size={16} />
                Dodaj silnik
              </button>
            </div>

            {assignedEngines.length === 0 ? (
              <p className={styles.noData}>Brak przypisanych silników</p>
            ) : (
              <div className={styles.tableContainer}>
                <table className={styles.table}>
                  <thead>
                    <tr>
                      <th>Silnik</th>
                      <th>Typ</th>
                      <th>Moc</th>
                      <th>Cena dodatkowa</th>
                      <th>Domyślny</th>
                      <th>Akcje</th>
                    </tr>
                  </thead>
                  <tbody>
                    {assignedEngines.map(engine => (
                      <tr key={engine.engineId}>
                        <td>{engine.engineName}</td>
                        <td>{engine.type}</td>
                        <td>{engine.power} KM</td>
                        <td>{engine.additionalPrice?.toLocaleString('pl-PL')} PLN</td>
                        <td>
                          <span className={engine.isDefault ? styles.active : styles.inactive}>
                            {engine.isDefault ? 'Tak' : 'Nie'}
                          </span>
                        </td>
                        <td className={styles.actions}>
                          <button 
                            onClick={() => handleRemoveEngine(engine.engineId)}
                            className={styles.deleteButton}
                            title="Usuń"
                          >
                            <Trash2 size={16} />
                          </button>
                        </td>
                      </tr>
                    ))}
                  </tbody>
                </table>
              </div>
            )}
          </div>

          {/* Formularz dodawania silnika */}
          {showAddForm && (
            <div className={styles.section}>
              <div className={styles.sectionHeader}>
                <h4>Dodaj silnik</h4>
                <button 
                  onClick={() => setShowAddForm(false)}
                  className={styles.closeButton}
                >
                  <X size={16} />
                </button>
              </div>

              <div className={styles.formGrid}>
                <div className={styles.formGroup}>
                  <label>Silnik *</label>
                  <select
                    value={selectedEngine}
                    onChange={(e) => setSelectedEngine(e.target.value)}
                    className={styles.formInput}
                  >
                    <option value="">Wybierz silnik ({availableEngines.length} dostępnych)</option>
                    {availableEngines.length > 0 ? (
                      availableEngines.map((engine, index) => {
                        // Debugging - sprawdź strukturę danych
                        console.log('Engine data:', engine);
                        const engineId = engine.id || engine.engineId || `engine-${index}`;
                        const engineName = engine.name || 'Nieznany silnik';
                        const engineType = engine.type || 'Nieznany typ';
                        const enginePower = engine.power || 'N/A';
                        
                        return (
                          <option key={`available-engine-${engineId}`} value={engineId}>
                            {engineName} - {engineType} - {enginePower} KM
                          </option>
                        );
                      })
                    ) : (
                      <option value="" disabled>Brak dostępnych silników</option>
                    )}
                  </select>
                  {availableEngines.length === 0 && !loading && (
                    <div style={{ fontSize: '0.875rem', color: '#6b7280', marginTop: '0.5rem' }}>
                      Wszystkie silniki są już przypisane do tego modelu lub wystąpił błąd podczas ładowania.
                    </div>
                  )}
                </div>

                <div className={styles.formGroup}>
                  <label>Cena dodatkowa (PLN)</label>
                  <input
                    type="number"
                    name="additionalPrice"
                    value={engineData.additionalPrice}
                    onChange={handleInputChange}
                    min="0"
                    step="1000"
                    className={styles.formInput}
                  />
                </div>

                <div className={styles.formGroup}>
                  <label>Prędkość maksymalna (km/h)</label>
                  <input
                    type="number"
                    name="topSpeed"
                    value={engineData.topSpeed}
                    onChange={handleInputChange}
                    min="100"
                    max="400"
                    className={styles.formInput}
                  />
                </div>

                <div className={styles.formGroup}>
                  <label>Przyspieszenie 0-100 km/h (s)</label>
                  <input
                    type="number"
                    name="acceleration0To100"
                    value={engineData.acceleration0To100}
                    onChange={handleInputChange}
                    min="2"
                    max="20"
                    step="0.1"
                    className={styles.formInput}
                  />
                </div>

                <div className={styles.formGroup}>
                  <label className={styles.checkboxLabel}>
                    <input
                      type="checkbox"
                      name="isDefault"
                      checked={engineData.isDefault}
                      onChange={handleInputChange}
                    />
                    Silnik domyślny
                  </label>
                </div>

                <div className={styles.formGroup}>
                  <label className={styles.checkboxLabel}>
                    <input
                      type="checkbox"
                      name="isAvailable"
                      checked={engineData.isAvailable}
                      onChange={handleInputChange}
                    />
                    Dostępny
                  </label>
                </div>
              </div>

              <div className={styles.formActions}>
                <button onClick={handleAddEngine} className={styles.saveButton}>
                  <Save size={16} />
                  Dodaj silnik
                </button>
                <button onClick={() => setShowAddForm(false)} className={styles.cancelButton}>
                  Anuluj
                </button>
              </div>
            </div>
          )}
        </div>
      </div>
    </div>
  );
};

export default EngineAssignment;