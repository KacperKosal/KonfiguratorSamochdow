import React, { useState, useEffect } from 'react';
import { Upload, Trash2, Star, ArrowUp, ArrowDown, Eye } from 'lucide-react';
import adminApiService from '../../services/adminApiService';
import styles from './CarAdminPanel.module.css';

const CarModelImageManager = ({ carModelId, onClose }) => {
  const [images, setImages] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');
  const [uploading, setUploading] = useState(false);

  useEffect(() => {
    console.log('CarModelImageManager: carModelId received:', carModelId, 'type:', typeof carModelId);
    if (carModelId) {
      loadImages();
    } else {
      console.error('CarModelImageManager: carModelId is empty or invalid');
      setError('Nieprawidłowy identyfikator modelu pojazdu');
    }
  }, [carModelId]);

  const loadImages = async () => {
    try {
      setLoading(true);
      const images = await adminApiService.getCarModelImages(carModelId);
      setImages(images.sort((a, b) => a.displayOrder - b.displayOrder));
    } catch (err) {
      setError('Nie udało się załadować zdjęć');
      console.error('Error loading images:', err);
      setImages([]);
    } finally {
      setLoading(false);
    }
  };

  const handleImageUpload = async (e) => {
    const file = e.target.files[0];
    if (!file) return;

    // Validate file
    if (!file.type.startsWith('image/')) {
      setError('Proszę wybrać plik obrazu (JPG, PNG, WebP)');
      return;
    }

    if (file.size > 5 * 1024 * 1024) {
      setError('Plik jest za duży. Maksymalny rozmiar to 5MB.');
      return;
    }

    if (images.length >= 8) {
      setError('Można dodać maksymalnie 8 zdjęć do modelu pojazdu');
      return;
    }

    try {
      setUploading(true);
      setError('');

      await adminApiService.uploadCarModelImage(carModelId, file);
      await loadImages();
      e.target.value = '';
    } catch (err) {
      setError(err.message || 'Nie udało się przesłać zdjęcia');
      console.error('Error uploading image:', err);
    } finally {
      setUploading(false);
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
            <label htmlFor="imageUpload" className={styles.uploadButton}>
              <Upload size={20} />
              {uploading ? 'Przesyłanie...' : 'Dodaj zdjęcie'}
            </label>
            <input
              id="imageUpload"
              type="file"
              accept="image/*"
              onChange={handleImageUpload}
              disabled={uploading || images.length >= 8}
              style={{ display: 'none' }}
            />
            <p className={styles.uploadInfo}>
              Maksymalnie 8 zdjęć, rozmiar do 5MB każde. Obsługiwane formaty: JPG, PNG, WebP
            </p>
            <p className={styles.uploadInfo}>
              Aktualnie: {images.length}/8 zdjęć
            </p>
          </div>

          {/* Images grid */}
          {loading ? (
            <div className={styles.loading}>Ładowanie zdjęć...</div>
          ) : (
            <div className={styles.imagesGrid}>
              {images.map((image, index) => (
                <div key={image.id} className={styles.imageCard}>
                  <div className={styles.imageWrapper}>
                    <img
                      src={adminApiService.getCarModelImageUrl(image.imageUrl)}
                      alt={`Zdjęcie ${index + 1}`}
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
                      <span className={styles.orderNumber}>#{image.displayOrder}</span>
                      <button
                        onClick={() => moveImage(image.id, 'up')}
                        disabled={image.displayOrder === 1}
                        className={styles.orderButton}
                        title="Przesuń w górę"
                      >
                        <ArrowUp size={16} />
                      </button>
                      <button
                        onClick={() => moveImage(image.id, 'down')}
                        disabled={image.displayOrder === images.length}
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
              ))}
            </div>
          )}

          {images.length === 0 && !loading && (
            <div className={styles.emptyState}>
              <p>Brak zdjęć dla tego modelu. Dodaj pierwsze zdjęcie używając przycisku powyżej.</p>
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