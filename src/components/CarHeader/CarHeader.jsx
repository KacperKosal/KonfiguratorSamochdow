// CarHeader.jsx
import styles from './CarHeader.module.css';

const CarHeader = ({ carsCount }) => (
  <div className={styles.header}>
    <div className={styles.headerContent}>
      <div>
        <h1 className={styles.headerTitle}>
          Panel Administratora
        </h1>
        <p className={styles.headerSubtitle}>
          Zarządzanie flotą samochodów
        </p>
      </div>
      <div className={styles.stats}>
        <div className={styles.statsNumber}>
          {carsCount}
        </div>
        <div className={styles.statsLabel}>
          Samochodów w bazie
        </div>
      </div>
    </div>
  </div>
);

export default CarHeader;