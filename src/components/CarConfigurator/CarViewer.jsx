import React from 'react';
import { ChevronLeft, ChevronRight } from 'lucide-react';

const CarViewer = ({ currentRotation, rotateLeft, rotateRight }) => {
  return (
    <div className="viewer-container">
      <img
        src="/api/placeholder/600/400"
        alt="Model samochodu"
        className="viewer-image"
      />
      <div className="rotate-info">Rotacja: {currentRotation}°</div>

      <button onClick={rotateLeft} className="rotate-button rotate-left">
        <ChevronLeft size={24} />
      </button>

      <button onClick={rotateRight} className="rotate-button rotate-right">
        <ChevronRight size={24} />
      </button>
    </div>
  );
};

export default CarViewer;
