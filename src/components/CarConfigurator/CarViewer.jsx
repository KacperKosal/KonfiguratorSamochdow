import React from 'react';
import { ChevronLeft, ChevronRight } from 'lucide-react';

export default function CarViewer({ currentRotation, rotateLeft, rotateRight }){
  return (
    <div className="relative bg-gray-200 rounded h-96 flex items-center justify-center mb-4">
      <div className="w-full h-full flex items-center justify-center">
        <img 
          src="/api/placeholder/600/400" 
          alt="Model samochodu" 
          className="max-h-80"
        />
        <div className="absolute bottom-4 text-sm text-gray-600">
          Rotacja: {currentRotation}°
        </div>
      </div>
      <div className="absolute left-4 top-1/2 transform -translate-y-1/2">
        <button 
          onClick={rotateLeft}
          className="bg-white rounded-full p-2 shadow hover:bg-gray-100"
        >
          <ChevronLeft size={24} />
        </button>
      </div>
      <div className="absolute right-4 top-1/2 transform -translate-y-1/2">
        <button 
          onClick={rotateRight}
          className="bg-white rounded-full p-2 shadow hover:bg-gray-100"
        >
          <ChevronRight size={24} />
        </button>
      </div>
    </div>
  );
};

