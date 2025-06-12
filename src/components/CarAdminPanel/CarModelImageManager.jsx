import React, { useState, useEffect } from 'react';
import { Upload, Trash2, Star, ArrowUp, ArrowDown, Eye } from 'lucide-react';
import adminApiService from '../../services/adminApiService';
import styles from './CarAdminPanel.module.css';

const CarModelImageManager = ({ carModelId, onClose }) => {
  const [images, setImages] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');
  const [uploading, setUploading] = useState(false);
  const [selectedColor, setSelectedColor] = useState('');
  const [activeTab, setActiveTab] = useState('');
  const [colorPrices, setColorPrices] = useState({});
  const [tempColorPrices, setTempColorPrices] = useState({});
  const [priceErrors, setPriceErrors] = useState({});
  const [hasUnsavedChanges, setHasUnsavedChanges] = useState(false);
  const [isSaving, setIsSaving] = useState(false);
  const [availableColors] = useState([
    { value: 'Biały', label: 'Biały' },
    { value: 'Czarny', label: 'Czarny' },
    { value: 'Srebrny', label: 'Srebrny' },
    { value: 'Szary', label: 'Szary' },
    { value: 'Czerwony', label: 'Czerwony' },
    { value: 'Niebieski', label: 'Niebieski' },
    { value: 'Zielony', label: 'Zielony' },
    { value: 'Żółty', label: 'Żółty' },
    { value: 'Pomarańczowy', label: 'Pomarańczowy' },
    { value: 'Brązowy', label: 'Brązowy' },
    { value: 'Beżowy', label: 'Beżowy' },
    { value: 'Złoty', label: 'Złoty' },
    { value: 'Granatowy', label: 'Granatowy' },
    { value: 'Fioletowy', label: 'Fioletowy' }
  ]);

  useEffect(() => {
    console.log('CarModelImageManager: carModelId received:', carModelId, 'type:', typeof carModelId);
    if (carModelId) {
      loadImages();
      loadColorPrices();
    } else {
      console.error('CarModelImageManager: carModelId is empty or invalid');
      setError('Nieprawidłowy identyfikator modelu pojazdu');
    }
  }, [carModelId]);

  // Check for unsaved changes
  useEffect(() => {
    const hasChanges = Object.keys(tempColorPrices).some(colorName => 
      tempColorPrices[colorName] !== (colorPrices[colorName] || '')
    );
    setHasUnsavedChanges(hasChanges);
  }, [tempColorPrices, colorPrices]);

  const loadImages = async () => {
    try {
      setLoading(true);
      const images = await adminApiService.getCarModelImages(carModelId);
      setImages(images.sort((a, b) => a.displayOrder - b.displayOrder));
      
      // Set initial active tab to first color with images
      const colorsWithImages = [...new Set(images.map(img => img.color))].filter(Boolean);
      if (colorsWithImages.length > 0 && !activeTab) {
        setActiveTab(colorsWithImages[0]);
      }
    } catch (err) {
      setError('Nie udało się załadować zdjęć');
      console.error('Error loading images:', err);
      setImages([]);
    } finally {
      setLoading(false);
    }
  };

  const loadColorPrices = async () => {
    try {
      const prices = await adminApiService.getColorPricesForModel(carModelId);
      setColorPrices(prices);
      setTempColorPrices(prices);
    } catch (err) {
      console.log('Błąd pobierania cen kolorów:', err);
    }
  };

  const handleImageUpload = async (e) => {
    const file = e.target.files[0];
    if (!file) return;

    // Validate color selection
    if (!selectedColor) {
      setError('Wybierz kolor, do którego chcesz dodać zdjęcia.');
      e.target.value = '';
      return;
    }

    // Validate file
    if (!file.type.startsWith('image/')) {
      setError('Proszę wybrać plik obrazu (JPG, PNG, WebP)');
      return;
    }

    if (file.size > 5 * 1024 * 1024) {
      setError('Plik jest za duży. Maksymalny rozmiar to 5MB.');
      return;
    }

    // Count images for the selected color
    const colorImages = images.filter(img => img.color === selectedColor);
    if (colorImages.length >= 8) {
      setError('Można dodać maksymalnie 8 zdjęć dla jednego koloru.');
      return;
    }

    try {
      setUploading(true);
      setError('');

      await adminApiService.uploadCarModelImage(carModelId, file, selectedColor);
      await loadImages();
      e.target.value = '';
      
      // Switch to the uploaded color tab
      if (!activeTab || activeTab !== selectedColor) {
        setActiveTab(selectedColor);
      }
    } catch (err) {
      setError(err.message || 'Nie udało się przesłać zdjęcia');
      console.error('Error uploading image:', err);
    } finally {
      setUploading(false);
    }
  };

  const handleColorChange = (color) => {
    setSelectedColor(color);
  };

  const handleTabColorPriceChange = (colorName, value) => {
    setTempColorPrices(prev => ({ ...prev, [colorName]: value }));
    
    // Clear error for this color
    setPriceErrors(prev => {
      const newErrors = { ...prev };
      delete newErrors[colorName];
      return newErrors;
    });
  };

  const validateColorPrice = (price) => {
    if (price === '' || price === null || price === undefined) {
      return 'Cena za kolor jest wymagana.';
    }
    
    const numPrice = parseInt(price);
    if (isNaN(numPrice) || numPrice < 0 || numPrice > 60000) {
      return 'Cena musi być liczbą całkowitą od 0 do 60 000.';
    }
    
    return null;
  };

  const saveAllChanges = async () => {
    setIsSaving(true);
    setPriceErrors({});
    
    try {
      // Validate all prices first
      const validationErrors = {};
      for (const [colorName, price] of Object.entries(tempColorPrices)) {
        if (price !== (colorPrices[colorName] || '')) {
          const error = validateColorPrice(price);
          if (error) {
            validationErrors[colorName] = error;
          }
        }
      }
      
      if (Object.keys(validationErrors).length > 0) {
        setPriceErrors(validationErrors);
        return;
      }
      
      // Save all changed prices
      const savePromises = [];
      for (const [colorName, price] of Object.entries(tempColorPrices)) {
        if (price !== (colorPrices[colorName] || '') && price !== '') {
          savePromises.push(
            adminApiService.setColorPrice(carModelId, colorName, parseInt(price))
          );
        }
      }
      
      await Promise.all(savePromises);
      
      // Update saved prices
      setColorPrices({ ...tempColorPrices });
      setHasUnsavedChanges(false);
      
      console.log('Wszystkie zmiany zostały zapisane');
    } catch (err) {
      setError('Nie udało się zapisać zmian: ' + (err.message || 'Nieznany błąd'));
      console.error('Error saving changes:', err);
    } finally {
      setIsSaving(false);
    }
  };

  const handleTabChange = (colorName) => {
    setActiveTab(colorName);
    
    // Load price for this color if not already loaded
    if (!(colorName in tempColorPrices)) {
      loadColorPriceForTab(colorName);
    }
  };

  const loadColorPriceForTab = async (colorName) => {
    try {
      const price = await adminApiService.getColorPrice(carModelId, colorName);
      setTempColorPrices(prev => ({ ...prev, [colorName]: price || '' }));
      setColorPrices(prev => ({ ...prev, [colorName]: price || 0 }));
    } catch (err) {
      console.log('Błąd pobierania ceny koloru:', err);
    }
  };

  const handleDeleteImage = async (imageId) => {
    if (!confirm('Czy na pewno chcesz usunąć to zdjęcie?')) return;

    try {
      await adminApiService.deleteCarModelImage(imageId);
      await loadImages();
    } catch (err) {
      setError('Nie udało się usunąć zdjęcia');
      console.error('Error deleting image:', err);
    }
  };

  const handleSetMainImage = async (imageId) => {
    try {
      await adminApiService.setMainImage(carModelId, imageId);
      await loadImages();
    } catch (err) {
      setError('Nie udało się ustawić zdjęcia głównego');
      console.error('Error setting main image:', err);
    }
  };

  const handleUpdateOrder = async (imageId, newOrder) => {
    try {
      await adminApiService.updateImageOrder(imageId, newOrder);
      await loadImages();
    } catch (err) {
      setError('Nie udało się zaktualizować kolejności');
      console.error('Error updating order:', err);
    }
  };

  const moveImage = (imageId, direction) => {
    const currentImage = images.find(img => img.id === imageId);
    if (!currentImage) return;

    const currentOrder = currentImage.displayOrder;
    const newOrder = direction === 'up' ? currentOrder - 1 : currentOrder + 1;

    // Check bounds
    if (newOrder < 1 || newOrder > images.length) return;

    handleUpdateOrder(imageId, newOrder);
  };

  // Get unique colors that have images
  const colorsWithImages = [...new Set(images.map(img => img.color))].filter(Boolean);
  
  // Get images for active tab
  const activeTabImages = activeTab ? images.filter(img => img.color === activeTab) : [];

  return (
    <div className={styles.modal}>
      <div className={`${styles.modalContent} ${styles.largeModal}`}>
        <div className={styles.modalHeader}>
          <h3>Zarządzanie zdjęciami modelu</h3>
          <button onClick={onClose} className={styles.closeButton}>×</button>
        </div>

        <div className={styles.modalBody}>
          {error && (
            <div className={styles.errorMessage}>
              {error}
            </div>
          )}

          {/* Upload section */}
          <div className={styles.uploadSection}>
            <div className={styles.colorSelection}>
              <label htmlFor="colorSelect">Wybierz kolor lakieru: *</label>
              <select
                id="colorSelect"
                value={selectedColor}
                onChange={(e) => {
                  handleColorChange(e.target.value);
                  if (e.target.value && !colorsWithImages.includes(e.target.value)) {
                    setActiveTab(e.target.value);
                  }
                }}
                className={styles.colorSelect}
              >
                <option value="">-- Wybierz kolor --</option>
                {availableColors.map(color => {
                  const count = images.filter(img => img.color === color.value).length;
                  return (
                    <option key={color.value} value={color.value}>
                      {color.label} ({count}/8 zdjęć)
                    </option>
                  );
                })}
              </select>
            </div>
            
            
            <label htmlFor="imageUpload" className={styles.uploadButton}>
              <Upload size={20} />
              {uploading ? 'Przesyłanie...' : 'Dodaj zdjęcie'}
            </label>
            <input
              id="imageUpload"
              type="file"
              accept="image/*"
              onChange={handleImageUpload}
              disabled={uploading}
              style={{ display: 'none' }}
            />
            {!selectedColor && (
              <p className={styles.warningInfo}>
                Wybierz kolor, do którego chcesz dodać zdjęcia.
              </p>
            )}
            <p className={styles.uploadInfo}>
              Maksymalnie 8 zdjęć na kolor, rozmiar do 5MB każde.
            </p>
          </div>
          
          {/* Price management section */}
          {availableColors.length > 0 && (
            <div className={styles.priceManagementSection}>
              <h4>Zarządzanie cenami kolorów:</h4>
              {hasUnsavedChanges && (
                <div className={styles.unsavedChangesWarning}>
                  ⚠️ Masz niezapisane zmiany w cenach kolorów
                </div>
              )}
              <div className={styles.colorPriceGrid}>
                {availableColors.map(colorOption => {
                  const hasImages = images.some(img => img.color === colorOption.value);
                  const currentPrice = tempColorPrices[colorOption.value] ?? colorPrices[colorOption.value] ?? '';
                  const hasError = priceErrors[colorOption.value];
                  
                  return (
                    <div key={colorOption.value} className={styles.colorPriceItem}>
                      <div className={styles.colorPriceHeader}>
                        <span className={styles.colorName}>{colorOption.label}</span>
                        {hasImages && <span className={styles.hasImagesIndicator}>📷</span>}
                      </div>
                      <div className={styles.priceInputGroup}>
                        <input
                          type="number"
                          min="0"
                          max="60000"
                          step="1"
                          value={currentPrice}
                          onChange={(e) => handleTabColorPriceChange(colorOption.value, e.target.value)}
                          placeholder="Cena (0-60000)"
                          className={`${styles.priceInput} ${hasError ? styles.inputError : ''}`}
                        />
                        <span className={styles.currency}>zł</span>
                      </div>
                      {hasError && (
                        <p className={styles.errorText}>{hasError}</p>
                      )}
                    </div>
                  );
                })}
              </div>
              <div className={styles.priceManagementActions}>
                <button
                  onClick={saveAllChanges}
                  disabled={!hasUnsavedChanges || isSaving}
                  className={styles.saveAllButton}
                >
                  {isSaving ? 'Zapisywanie...' : 'Zapisz zmiany'}
                </button>
              </div>
            </div>
          )}
          
          {/* Color tabs */}
          {colorsWithImages.length > 0 && (
            <div className={styles.colorTabs}>
              <h4>Zdjęcia według kolorów:</h4>
              <div className={styles.tabs}>
                {colorsWithImages.map((color, index) => {
                  const colorInfo = availableColors.find(c => c.value === color);
                  const count = images.filter(img => img.color === color).length;
                  return (
                    <button
                      key={`color-tab-${index}-${color}`}
                      onClick={() => handleTabChange(color)}
                      className={`${styles.tab} ${activeTab === color ? styles.activeTab : ''}`}
                    >
                      <div className={styles.tabContent}>
                        <span className={styles.tabLabel}>
                          {colorInfo?.label || color} ({count})
                        </span>
                      </div>
                    </button>
                  );
                })}
              </div>
            </div>
          )}


          {/* Images grid */}
          {loading ? (
            <div className={styles.loading}>Ładowanie zdjęć...</div>
          ) : activeTab && activeTabImages.length > 0 ? (
            <div className={styles.imagesGrid}>
              {activeTabImages.map((image, index) => {
                const orderInColor = activeTabImages.findIndex(img => img.id === image.id) + 1;
                return (
                  <div key={image.id} className={styles.imageCard}>
                    <div className={styles.imageWrapper}>
                      <img
                        src={adminApiService.getCarModelImageUrl(image.imageUrl)}
                        alt={`Zdjęcie ${orderInColor}`}
                        className={styles.thumbnailImage}
                      />
                      {image.isMainImage && (
                        <div className={styles.mainImageBadge}>
                          <Star size={16} fill="gold" />
                          Główne
                        </div>
                      )}
                    </div>

                    <div className={styles.imageControls}>
                      <div className={styles.orderControls}>
                        <span className={styles.orderNumber}>#{orderInColor}</span>
                        <button
                          onClick={() => moveImage(image.id, 'up')}
                          disabled={orderInColor === 1}
                          className={styles.orderButton}
                          title="Przesuń w górę"
                        >
                          <ArrowUp size={16} />
                        </button>
                        <button
                          onClick={() => moveImage(image.id, 'down')}
                          disabled={orderInColor === activeTabImages.length}
                          className={styles.orderButton}
                          title="Przesuń w dół"
                        >
                          <ArrowDown size={16} />
                        </button>
                      </div>

                      <div className={styles.actionButtons}>
                        {!image.isMainImage && (
                          <button
                            onClick={() => handleSetMainImage(image.id)}
                            className={styles.setMainButton}
                            title="Ustaw jako główne"
                          >
                            <Star size={16} />
                          </button>
                        )}
                        <button
                          onClick={() => window.open(adminApiService.getCarModelImageUrl(image.imageUrl), '_blank')}
                          className={styles.viewButton}
                          title="Zobacz w pełnym rozmiarze"
                        >
                          <Eye size={16} />
                        </button>
                        <button
                          onClick={() => handleDeleteImage(image.id)}
                          className={styles.deleteButton}
                          title="Usuń zdjęcie"
                        >
                          <Trash2 size={16} />
                        </button>
                      </div>
                    </div>
                  </div>
                );
              })}
            </div>
          ) : activeTab ? (
            <div className={styles.emptyState}>
              <p>Brak zdjęć dla koloru: {availableColors.find(c => c.value === activeTab)?.label || activeTab}</p>
              <p>Wybierz ten kolor z listy powyżej i dodaj pierwsze zdjęcie.</p>
            </div>
          ) : (
            <div className={styles.emptyState}>
              <p>Wybierz kolor z listy powyżej, aby zobaczyć zdjęcia.</p>
            </div>
          )}
        </div>

        <div className={styles.modalFooter}>
          <button onClick={onClose} className={styles.cancelButton}>
            Zamknij
          </button>
        </div>
      </div>
    </div>
  );
};

export default CarModelImageManager;