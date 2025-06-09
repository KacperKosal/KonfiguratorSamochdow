import React, { useState, useEffect } from 'react';
import { ChevronLeft, ChevronRight } from 'lucide-react';
import SafeImage from '../../SafeImage/SafeImage';
import adminApiService from '../../../services/adminApiService';
import styles from './CarViewer.module.css';

const CarViewer = ({ 
  selectedCarModel, 
  carColor
}) => {
  const [carImages, setCarImages] = useState([]);
  const [currentImageIndex, setCurrentImageIndex] = useState(0);
  const [loadingImages, setLoadingImages] = useState(false);

  // Load car images when car model changes
  useEffect(() => {
    if (selectedCarModel?.id) {
      loadCarImages();
    } else {
      setCarImages([]);
      setCurrentImageIndex(0);
    }
  }, [selectedCarModel?.id]);

  const loadCarImages = async () => {
    try {
      setLoadingImages(true);
      console.log(`Loading images for car model: ${selectedCarModel.id}`);
      const images = await adminApiService.getCarModelImages(selectedCarModel.id);
      console.log(`Loaded ${images.length} images for car model ${selectedCarModel.id}:`, images);
      setCarImages(images.sort((a, b) => a.displayOrder - b.displayOrder));
      setCurrentImageIndex(0);
    } catch (error) {
      console.error('Error loading car images:', error);
      setCarImages([]);
    } finally {
      setLoadingImages(false);
    }
  };

  // Map hex colors to color names for image file naming and display
  const colorMapping = {
    '#000000': { name: 'black', display: 'Czarny' },
    '#ffffff': { name: 'white', display: 'Biały' }, 
    '#c0c0c0': { name: 'silver', display: 'Srebrny' },
    '#0000ff': { name: 'blue', display: 'Niebieski' },
    '#ff0000': { name: 'red', display: 'Czerwony' }
  };

  const getCarImageUrl = () => {
    // If we have multiple images, use the current one
    if (carImages.length > 0 && carImages[currentImageIndex]) {
      return adminApiService.getCarModelImageUrl(carImages[currentImageIndex].imageUrl);
    }
    
    // Fallback to the original imageUrl from car model
    if (!selectedCarModel?.imageUrl) {
      return "/api/placeholder/600/400";
    }
    
    // Use the same image URL generation as in the main panel
    const baseImageUrl = adminApiService.getImageUrl(selectedCarModel.imageUrl);
    
    if (!baseImageUrl) {
      return "/api/placeholder/600/400";
    }
    
    // Create color-based image URL variations
    const colorInfo = colorMapping[carColor];
    const colorName = colorInfo?.name || 'default';
    
    // Try to generate color-specific image URL
    // Method 1: Replace file extension with color variant
    if (baseImageUrl.includes('.')) {
      const urlParts = baseImageUrl.split('.');
      const extension = urlParts.pop();
      const basePath = urlParts.join('.');
      
      // Try color-specific image first: model_red.jpg, model_blue.jpg, etc.
      const colorSpecificUrl = `${basePath}_${colorName}.${extension}`;
      return colorSpecificUrl;
    }
    
    // Method 2: Append color as query parameter (fallback)
    return `${baseImageUrl}?color=${colorName}`;
  };

  // Navigation functions for images
  const nextImage = () => {
    if (carImages.length > 1) {
      setCurrentImageIndex((prev) => (prev + 1) % carImages.length);
    }
  };

  const prevImage = () => {
    if (carImages.length > 1) {
      setCurrentImageIndex((prev) => (prev - 1 + carImages.length) % carImages.length);
    }
  };

  return (
    <div className={styles.viewerContainer}>
      <SafeImage
        src={getCarImageUrl()}
        alt={selectedCarModel?.name || "Model samochodu"}
        className={styles.viewerImage}
        fallbackSrc={selectedCarModel?.imageUrl ? adminApiService.getImageUrl(selectedCarModel.imageUrl) : "/api/placeholder/600/400"}
      />
      
      {selectedCarModel && (
        <div className={styles.carInfo}>
          <h3 className={styles.carName}>{selectedCarModel.name}</h3>
          <div className={styles.carDetails}>
            <span>Kolor: {colorMapping[carColor]?.display || carColor}</span>
            {carImages.length > 0 && (
              <span>Zdjęcie: {currentImageIndex + 1} z {carImages.length}</span>
            )}
          </div>
        </div>
      )}

      {/* Image navigation buttons - only show if there are multiple images */}
      {carImages.length > 1 && (
        <>
          <button onClick={prevImage} className={`${styles.imageNavButton} ${styles.imageNavLeft}`}>
            <ChevronLeft size={20} />
          </button>
          <button onClick={nextImage} className={`${styles.imageNavButton} ${styles.imageNavRight}`}>
            <ChevronRight size={20} />
          </button>
        </>
      )}

      {/* Image thumbnails */}
      {carImages.length > 1 && (
        <div className={styles.thumbnailStrip}>
          {carImages.map((image, index) => (
            <button
              key={image.id}
              onClick={() => setCurrentImageIndex(index)}
              className={`${styles.thumbnail} ${index === currentImageIndex ? styles.activeThumbnail : ''}`}
            >
              <img
                src={adminApiService.getCarModelImageUrl(image.imageUrl)}
                alt={`Zdjęcie ${index + 1}`}
                className={styles.thumbnailImage}
              />
              {image.isMainImage && <div className={styles.mainImageIndicator}>●</div>}
            </button>
          ))}
        </div>
      )}
    </div>
  );
};

export default CarViewer;
