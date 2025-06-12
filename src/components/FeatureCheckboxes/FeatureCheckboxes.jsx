// FeatureCheckboxes.jsx
import styles from './FeatureCheckboxes.module.css';

const FeatureCheckboxes = ({ formData, dostepneCechy, onFeatureChange, onBasicOptionChange }) => (
  <div className={styles.checkboxSection}>
    <div className={styles.checkboxGroup}>
      <h3 className={styles.checkboxTitle}>Opcje podstawowe</h3>
      <div className={styles.checkboxList}>
        <div className={styles.checkboxItem}>
          <input
            type="checkbox"
            name="ma4x4"
            checked={formData.ma4x4}
            onChange={onBasicOptionChange}
            className={styles.checkbox}
          />
          <label className={styles.checkboxLabel}>
            Napęd 4x4
          </label>
        </div>
        <div className={styles.checkboxItem}>
          <input
            type="checkbox"
            name="jestElektryczny"
            checked={formData.jestElektryczny}
            onChange={onBasicOptionChange}
            className={styles.checkbox}
          />
          <label className={styles.checkboxLabel}>
            Samochód elektryczny
          </label>
        </div>
      </div>
    </div>

    <div className={styles.checkboxGroup}>
      <h3 className={styles.checkboxTitle}>Cechy dodatkowe</h3>
      <div className={styles.checkboxList}>
        {dostepneCechy.map((cecha, index) => (
          <div key={`feature-${index}-${cecha}`} className={styles.checkboxItem}>
            <input
              type="checkbox"
              checked={formData.cechy.includes(cecha)}
              onChange={() => onFeatureChange(cecha)}
              className={styles.checkbox}
            />
            <label className={styles.checkboxLabel}>
              {cecha}
            </label>
          </div>
        ))}
      </div>
    </div>
  </div>
);

export default FeatureCheckboxes;