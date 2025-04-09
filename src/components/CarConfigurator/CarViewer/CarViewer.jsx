import React from 'react';
import { ChevronLeft, ChevronRight } from 'lucide-react';
import styles from './CarViewer.module.css';

const CarViewer = ({ currentRotation, rotateLeft, rotateRight }) => {
  return (
    <div className={styles.viewerContainer}>
      <img
        src="/api/placeholder/600/400"
        alt="Model samochodu"
        className={styles.viewerImage}
      />
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
