// CarTable.jsx
import CarTableRow from './CarTableRow';
import styles from './CarTable.module.css';
import commonStyles from '../Form.module/Common.module.css';

const CarTable = ({ cars, formatPrice, onShowDetails, onEdit, onDelete }) => (
  <div className={styles.tableContainer}>
    <div className={styles.tableHeader}>
      <h2 className={styles.tableTitle}>Lista Samochodów</h2>
      <p className={styles.tableSubtitle}>Zarządzaj swoją flotą samochodów</p>
    </div>

    {cars.length === 0 ? (
      <div className={commonStyles.emptyState}>
        <div className={commonStyles.emptyIcon}>🚗</div>
        <h3 className={commonStyles.emptyTitle}>
          Brak samochodów w bazie
        </h3>
        <p className={commonStyles.emptySubtitle}>
          Dodaj pierwszy samochód, aby rozpocząć zarządzanie flotą
        </p>
      </div>
    ) : (
      <div className={styles.tableWrapper}>
        <table className={styles.table}>
          <thead className={styles.tableHead}>
            <tr>
              <th>Samochód</th>
              <th>Typ/Rok</th>
              <th>Silnik</th>
              <th>Cena</th>
              <th>Cechy</th>
              <th>Akcje</th>
            </tr>
          </thead>
          <tbody className={styles.tableBody}>
            {cars.map((car) => (
              <CarTableRow
                key={car.id}
                car={car}
                formatPrice={formatPrice}
                onShowDetails={onShowDetails}
                onEdit={onEdit}
                onDelete={onDelete}
              />
            ))}
          </tbody>
        </table>
      </div>
    )}
  </div>
);

export default CarTable;