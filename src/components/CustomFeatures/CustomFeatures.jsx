// CustomFeatures.jsx
import styles from './CustomFeatures.module.css';

const CustomFeatures = ({ formData, nowaCecha, onNowaCechaChange, onAddCustomFeature, onRemoveCustomFeature }) => (
  <div className={styles.customFeatures}>
    <h3 className={styles.checkboxTitle}>Własne cechy</h3>
    <div className={styles.featureInputGroup}>
      <input
        type="text"
        value={nowaCecha}
        onChange={(e) => onNowaCechaChange(e.target.value)}
        placeholder="Dodaj własną cechę..."
        className={styles.featureInput}
        onKeyPress={(e) => e.key === 'Enter' && onAddCustomFeature()}
      />
      <button
        onClick={onAddCustomFeature}
        className={styles.featureAddBtn}
      >
        Dodaj
      </button>
    </div>
    {formData.wlasneCechy.length > 0 && (
      <div className={styles.featureTags}>
        {formData.wlasneCechy.map((cecha, index) => (
          <span
            key={`custom-feature-${index}-${cecha}`}
            className={styles.featureTag}
          >
            {cecha}
            <button
              onClick={() => onRemoveCustomFeature(cecha)}
              className={styles.featureTagRemove}
            >
              ×
            </button>
          </span>
        ))}
      </div>
    )}
  </div>
);

export default CustomFeatures;