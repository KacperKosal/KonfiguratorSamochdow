import React from 'react';
import { ArrowRight, Info, Heart } from 'lucide-react';
import SafeImage from '../../SafeImage/SafeImage';
import styles from './ModelCard.module.css';
import { useNavigate } from 'react-router-dom';

const ModelCard = ({ model, formatPrice }) => 
  {
   const navigate = useNavigate()
   const goToConfigurator = () => {
    navigate(`/car-configurator/new?modelId=${model.id}`)
  }
  
  return (
    <div className={styles.card}>
      <div className={styles.imageContainer}>
        <SafeImage 
          src={model.image} 
          alt={model.name} 
          className={styles.image}
          fallbackSrc="/api/placeholder/300/180"
        />
        
      </div>
      <div className={styles.content}>
        <h2 className={styles.title}>{model.name}</h2>
        <div className={styles.tags}>
          <span className={styles.categoryTag}>{model.category}</span>
          {model.electric && <span className={styles.electricTag}>Elektryczny</span>}
        </div>
        <p className={styles.description}>{model.description}</p>
        {model.engineOptions && model.engineOptions.length > 0 && (
          <div className={styles.engines}>
            <h3 className={styles.enginesTitle}>Dostępne silniki:</h3>
            <div className={styles.engineTags}>
              {model.engineOptions.map((engine, index) => (
                <span key={`${model.id}-engine-${index}-${engine}`} className={styles.engineTag}>
                  {engine}
                </span>
              ))}
            </div>
          </div>
        )}
        <div className={styles.divider}>
          <div className={styles.priceContainer}>
            <div className={styles.priceLabel}>Cena od:</div>
            <div className={styles.price}>{formatPrice(model.basePrice)}</div>
          </div>
          <div className={styles.buttons}>
            
            
            <button className={styles.configureButton} 
            onClick={goToConfigurator}>

              <span>Konfiguruj</span>
              <ArrowRight size={16} />
              
            </button>
            
          </div>
        </div>
      </div>
    </div>
  );
};

export default ModelCard;
