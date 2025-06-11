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
  const [filteredImages, setFilteredImages] = useState([]);
  const [currentImageIndex, setCurrentImageIndex] = useState(0);
  const [loadingImages, setLoadingImages] = useState(false);
  const [imageTransition, setImageTransition] = useState(false);

  // Load car images when car model changes
  useEffect(() => {
    if (selectedCarModel?.id) {
      loadCarImages();
    } else {
      setCarImages([]);
      setFilteredImages([]);
      setCurrentImageIndex(0);
    }
  }, [selectedCarModel?.id]);

  // Filter images by color when color changes
  useEffect(() => {
    if (carImages.length > 0) {
      const colorInfo = colorMapping[carColor];
      const colorName = colorInfo?.name || 'default';
      
      // Start transition
      setImageTransition(true);
      
      setTimeout(() => {
        // Filter images that match the selected color
        const colorImages = carImages.filter(img => 
          img.color === colorName || 
          img.color === carColor ||
          (img.color === '' && colorName === 'default')
        );
        
        // If no images match the color, set empty array to show message
        setFilteredImages(colorImages);
        setCurrentImageIndex(0);
        
        // End transition
        setImageTransition(false);
      }, 150);
    }
  }, [carColor, carImages]);

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
    '#000000': { name: 'Czarny', display: 'Czarny' },
    '#ffffff': { name: 'Biały', display: 'Biały' }, 
    '#c0c0c0': { name: 'Srebrny', display: 'Srebrny' },
    '#808080': { name: 'Szary', display: 'Szary' },
    '#0000ff': { name: 'Niebieski', display: 'Niebieski' },
    '#ff0000': { name: 'Czerwony', display: 'Czerwony' },
    '#008000': { name: 'Zielony', display: 'Zielony' },
    '#ffff00': { name: 'Żółty', display: 'Żółty' },
    '#ffa500': { name: 'Pomarańczowy', display: 'Pomarańczowy' },
    '#8b4513': { name: 'Brązowy', display: 'Brązowy' },
    '#f5f5dc': { name: 'Beżowy', display: 'Beżowy' },
    '#ffd700': { name: 'Złoty', display: 'Złoty' },
    '#000080': { name: 'Granatowy', display: 'Granatowy' },
    '#800080': { name: 'Fioletowy', display: 'Fioletowy' }
  };

  const getCarImageUrl = () => {
    // If we have filtered images, use the current one
    if (filteredImages.length > 0 && filteredImages[currentImageIndex]) {
      return adminApiService.getCarModelImageUrl(filteredImages[currentImageIndex].imageUrl);
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
    if (filteredImages.length > 1) {
      setCurrentImageIndex((prev) => (prev + 1) % filteredImages.length);
    }
  };

  const prevImage = () => {
    if (filteredImages.length > 1) {
      setCurrentImageIndex((prev) => (prev - 1 + filteredImages.length) % filteredImages.length);
    }
  };

  return (
    <div className={styles.viewerContainer}>
      {filteredImages.length > 0 ? (
        <div className={`${styles.imageWrapper} ${imageTransition ? styles.transitioning : ''}`}>
          <SafeImage
            src={getCarImageUrl()}
            alt={selectedCarModel?.name || "Model samochodu"}
            className={styles.viewerImage}
            fallbackSrc={selectedCarModel?.imageUrl ? adminApiService.getImageUrl(selectedCarModel.imageUrl) : "/api/placeholder/600/400"}
          />
        </div>
      ) : loadingImages ? (
        <div className={styles.loadingWrapper}>
          <div className={styles.loading}>Ładowanie zdjęć...</div>
        </div>
      ) : (
        <div className={styles.noImagesWrapper}>
          <div className={styles.noImagesMessage}>
            <p>Brak zdjęć dla wybranego koloru.</p>
            <p className={styles.colorInfo}>Kolor: {colorMapping[carColor]?.display || 'Nieznany'}</p>
          </div>
        </div>
      )}
      
      {selectedCarModel && (
        <div className={styles.carInfo}>
          <h3 className={styles.carName}>{selectedCarModel.name}</h3>
          <div className={styles.carDetails}>
            <span>Kolor: {colorMapping[carColor]?.display || carColor}</span>
            {filteredImages.length > 0 && (
              <span>Zdjęcie: {currentImageIndex + 1} z {filteredImages.length}</span>
            )}
          </div>
        </div>
      )}

      {/* Image navigation buttons - only show if there are multiple images */}
      {filteredImages.length > 1 && (
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
      {filteredImages.length > 1 && (
        <div className={styles.thumbnailStrip}>
          {filteredImages.map((image, index) => (
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
