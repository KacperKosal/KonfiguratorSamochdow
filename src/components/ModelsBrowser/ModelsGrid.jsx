import React from 'react';
import ModelCard from './ModelCard';

const ModelsGrid = ({ models, formatPrice }) => {
  return (
    <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
      {models.map(model => (
        <ModelCard key={model.id} model={model} formatPrice={formatPrice} />
      ))}
    </div>
  );
};

export default ModelsGrid;
