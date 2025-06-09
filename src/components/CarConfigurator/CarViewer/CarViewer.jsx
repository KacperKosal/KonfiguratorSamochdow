import React from 'react';
import { ChevronLeft, ChevronRight } from 'lucide-react';
import SafeImage from '../../SafeImage/SafeImage';
import adminApiService from '../../../services/adminApiService';
import styles from './CarViewer.module.css';

const CarViewer = ({ 
  currentRotation, 
  rotateLeft, 
  rotateRight, 
  selectedCarModel, 
  carColor
}) => {
  // Map hex colors to color names for image file naming and display
  const colorMapping = {
    '#000000': { name: 'black', display: 'Czarny' },
    '#ffffff': { name: 'white', display: 'Biały' }, 
    '#c0c0c0': { name: 'silver', display: 'Srebrny' },
    '#0000ff': { name: 'blue', display: 'Niebieski' },
    '#ff0000': { name: 'red', display: 'Czerwony' }
  };

  const getCarImageUrl = () => {
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
    return `${baseImageUrl}?color=${colorName}&rotation=${currentRotation}`;
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
          </div>
        </div>
      )}
      
      <div className={styles.rotateInfo}>Rotacja: {currentRotation}°</div>

      <button onClick={rotateLeft} className={`${styles.rotateButton} ${styles.rotateLeft}`}>
        <ChevronLeft size={24} />
      </button>

      <button onClick={rotateRight} className={`${styles.rotateButton} ${styles.rotateRight}`}>
        <ChevronRight size={24} />
      </button>
    </div>
  );
};

export default CarViewer;
