// CarTableRow.jsx
import styles from './CarTable.module.css';
import btnStyles from '../Form.module/Button.module.css';
import commonStyles from '../Form.module/Common.module.css';

const CarTableRow = ({ car, formatPrice, onShowDetails, onEdit, onDelete }) => (
  <tr>
    <td>
      <div className={styles.carInfo}>
        {car.zdjecie && (
          <img 
            src={car.zdjecie} 
            alt={`${car.marka} ${car.model}`}
            className={styles.carImage}
            onError={(e) => {
              e.target.style.display = 'none';
            }}
          />
        )}
        <div>
          <div className={styles.carName}>
            {car.marka} {car.model}
          </div>
          <div className={styles.carDetails}>
            {car.kolorNadwozia} • ID: {car.id}
          </div>
        </div>
      </div>
    </td>
    <td>
      <div>
        <span className={styles.carType}>
          {car.typ}
        </span>
        <span className={styles.carYear}>
          Rok {car.rok}
        </span>
      </div>
    </td>
    <td>
      <div>
        <span className={styles.engineInfo}>
          {car.pojemnosc}L
        </span>
        <span className={styles.enginePower}>
          {car.moc} KM
        </span>
      </div>
    </td>
    <td>
      <span className={styles.price}>
        {formatPrice(car.cena)}
      </span>
    </td>
    <td>
      <div className={styles.featureBadges}>
        {car.ma4x4 && (
          <span className={`${commonStyles.badge} ${commonStyles.badgeBlue}`}>
            4x4
          </span>
        )}
        {car.jestElektryczny && (
          <span className={`${commonStyles.badge} ${commonStyles.badgeGreen}`}>
            ⚡ Elektryczny
          </span>
        )}
        {car.cechy && car.cechy.slice(0, 2).map((cecha, index) => (
          <span
            key={index}
            className={`${commonStyles.badge} ${commonStyles.badgeGray}`}
          >
            {cecha}
          </span>
        ))}
        {car.cechy && car.cechy.length > 2 && (
          <span className={styles.moreFeatures}>
            +{car.cechy.length - 2} więcej
          </span>
        )}
      </div>
    </td>
    <td>
      <div className={btnStyles.actions}>
        <button
          onClick={() => onShowDetails(car)}
          className={`${btnStyles.btn} ${btnStyles.btnInfo}`}
        >
          Szczegóły
        </button>
        <button
          onClick={() => onEdit(car)}
          className={`${btnStyles.btn} ${btnStyles.btnWarning}`}
        >
          Edytuj
        </button>
        <button
          onClick={() => onDelete(car.id)}
          className={`${btnStyles.btn} ${btnStyles.btnDanger}`}
        >
          Usuń
        </button>
      </div>
    </td>
  </tr>
);

export default CarTableRow;