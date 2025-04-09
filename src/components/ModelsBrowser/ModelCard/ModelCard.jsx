import React from 'react';
import { ArrowRight, Info, Heart } from 'lucide-react';
import styles from './ModelCard.module.css';

const ModelCard = ({ model, formatPrice }) => {
  return (
    <div className={styles.card}>
      <div className={styles.imageContainer}>
        <img src={model.image} alt={model.name} className={styles.image} />
        {model.new && (
          <div className={styles.newBadge}>
            NOWOŚĆ
          </div>
        )}
        <button className={styles.favoriteButton}>
          <Heart size={18} className="text-gray-600" />
        </button>
      </div>
      <div className={styles.content}>
        <h2 className={styles.title}>{model.name}</h2>
        <div className={styles.tags}>
          <span className={styles.categoryTag}>{model.category}</span>
          {model.electric && <span className={styles.electricTag}>Elektryczny</span>}
        </div>
        <p className={styles.description}>{model.description}</p>
        <div className={styles.engines}>
          <h3 className={styles.enginesTitle}>Dostępne silniki:</h3>
          <div className={styles.engineTags}>
            {model.engineOptions.map((engine, index) => (
              <span key={index} className={styles.engineTag}>
                {engine}
              </span>
            ))}
          </div>
        </div>
        <div className={styles.features}>
          <h3 className={styles.featuresTitle}>Wybrane cechy:</h3>
          <ul className={styles.featureList}>
            {model.features.map((feature, index) => (
              <li key={index} className={styles.featureItem}>
                <span className={styles.featureDot}></span>
                {feature}
              </li>
            ))}
          </ul>
        </div>
        <div className={styles.divider}>
          <div className={styles.priceContainer}>
            <div className={styles.priceLabel}>Cena od:</div>
            <div className={styles.price}>{formatPrice(model.basePrice)}</div>
          </div>
          <div className={styles.buttons}>
            <button className={styles.configureButton}>
              <span>Konfiguruj</span>
              <ArrowRight size={16} />
            </button>
            <button className={styles.infoButton}>
              <Info size={18} />
            </button>
          </div>
        </div>
      </div>
    </div>
  );
};

export default ModelCard;
