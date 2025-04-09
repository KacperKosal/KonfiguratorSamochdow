import React from 'react';
import ModelCard from '../ModelCard/ModelCard';
import styles from './ModelsGrid.module.css';

const ModelsGrid = ({ models, formatPrice }) => {
  return (
    <div className={styles.container}>
      {models.map(model => (
        <ModelCard key={model.id} model={model} formatPrice={formatPrice} />
      ))}
    </div>
  );
};

export default ModelsGrid;
