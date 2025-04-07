import React from 'react';
import { ArrowRight, Info, Heart } from 'lucide-react';

const ModelCard = ({ model, formatPrice }) => {
  return (
    <div className="bg-white rounded-lg shadow-lg overflow-hidden hover:shadow-xl transition">
      <div className="relative">
        <img src={model.image} alt={model.name} className="w-full h-48 object-cover" />
        {model.new && (
          <div className="absolute top-2 left-2 bg-green-500 text-white text-sm font-bold px-2 py-1 rounded">
            NOWOŚĆ
          </div>
        )}
        <button className="absolute top-2 right-2 p-2 bg-white rounded-full shadow hover:bg-gray-100">
          <Heart size={18} className="text-gray-600" />
        </button>
      </div>
      <div className="p-6">
        <h2 className="text-xl font-bold mb-2">{model.name}</h2>
        <div className="flex items-center text-sm text-gray-500 mb-4">
          <span className="bg-gray-200 px-2 py-1 rounded mr-2">{model.category}</span>
          {model.electric && <span className="bg-blue-100 text-blue-800 px-2 py-1 rounded">Elektryczny</span>}
        </div>
        <p className="text-gray-600 mb-4 line-clamp-2">{model.description}</p>
        <div className="mb-4">
          <h3 className="font-medium mb-2">Dostępne silniki:</h3>
          <div className="flex flex-wrap gap-2">
            {model.engineOptions.map((engine, index) => (
              <span key={index} className="bg-gray-100 px-2 py-1 text-sm rounded">
                {engine}
              </span>
            ))}
          </div>
        </div>
        <div className="mb-4 text-gray-700">
          <h3 className="font-medium mb-2">Wybrane cechy:</h3>
          <ul className="text-sm space-y-1">
            {model.features.map((feature, index) => (
              <li key={index} className="flex items-center gap-1">
                <span className="w-1 h-1 bg-blue-600 rounded-full"></span>
                {feature}
              </li>
            ))}
          </ul>
        </div>
        <div className="border-t pt-4 mt-4">
          <div className="flex justify-between items-center mb-4">
            <div className="text-sm text-gray-500">Cena od:</div>
            <div className="text-xl font-bold">{formatPrice(model.basePrice)}</div>
          </div>
          <div className="flex gap-3">
            <button className="flex items-center justify-center gap-1 flex-grow px-4 py-2 bg-blue-600 text-white rounded hover:bg-blue-700">
              <span>Konfiguruj</span>
              <ArrowRight size={16} />
            </button>
            <button className="flex items-center justify-center px-4 py-2 border border-gray-300 rounded hover:bg-gray-50">
              <Info size={18} />
            </button>
          </div>
        </div>
      </div>
    </div>
  );
};

export default ModelCard;
