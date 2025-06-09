import React from 'react';
import styles from './CarAdminPanel.module.css';

// Komponent do renderowania formularzy dla różnych typów obiektów
const FormRenderer = ({
  activeTab,
  formData,
  handleInputChange,
  errors,
  manufacturers,
  bodyTypes,
  segments,
  imagePreview,
  formData: { imageUrl },
  imageLoadedSuccessfully,
  handleRemoveImage,
  handleImageUpload,
  adminApiService
}) => {

  const renderModelsForm = () => (
    <>
      <div className={styles.formGrid}>
        <div className={styles.formGroup}>
          <label>Nazwa modelu *</label>
          <input
            type="text"
            name="name"
            value={formData.name}
            onChange={handleInputChange}
            className={errors.name ? styles.errorInput : ''}
          />
          {errors.name && <span className={styles.errorText}>{errors.name}</span>}
        </div>

        <div className={styles.formGroup}>
          <label>Producent *</label>
          <select
            name="manufacturer"
            value={formData.manufacturer}
            onChange={handleInputChange}
            className={errors.manufacturer ? styles.errorInput : ''}
          >
            <option value="">Wybierz producenta</option>
            {manufacturers.map(manufacturer => (
              <option key={manufacturer} value={manufacturer}>{manufacturer}</option>
            ))}
          </select>
          {errors.manufacturer && <span className={styles.errorText}>{errors.manufacturer}</span>}
        </div>

        <div className={styles.formGroup}>
          <label>Typ nadwozia *</label>
          <select
            name="bodyType"
            value={formData.bodyType}
            onChange={handleInputChange}
            className={errors.bodyType ? styles.errorInput : ''}
          >
            <option value="">Wybierz typ</option>
            {bodyTypes.map(type => (
              <option key={type} value={type}>{type}</option>
            ))}
          </select>
          {errors.bodyType && <span className={styles.errorText}>{errors.bodyType}</span>}
        </div>

        <div className={styles.formGroup}>
          <label>Segment *</label>
          <select
            name="segment"
            value={formData.segment}
            onChange={handleInputChange}
            className={errors.segment ? styles.errorInput : ''}
          >
            <option value="">Wybierz segment</option>
            {segments.map(segment => (
              <option key={segment} value={segment}>{segment}</option>
            ))}
          </select>
          {errors.segment && <span className={styles.errorText}>{errors.segment}</span>}
        </div>

        <div className={styles.formGroup}>
          <label>Rok produkcji *</label>
          <input
            type="number"
            name="productionYear"
            value={formData.productionYear}
            onChange={handleInputChange}
            min="2000"
            max={new Date().getFullYear() + 1}
            className={errors.productionYear ? styles.errorInput : ''}
          />
          {errors.productionYear && <span className={styles.errorText}>{errors.productionYear}</span>}
        </div>

        <div className={styles.formGroup}>
          <label>Cena bazowa (PLN) *</label>
          <input
            type="number"
            name="basePrice"
            value={formData.basePrice}
            onChange={handleInputChange}
            min="0"
            step="1000"
            className={errors.basePrice ? styles.errorInput : ''}
          />
          {errors.basePrice && <span className={styles.errorText}>{errors.basePrice}</span>}
        </div>
      </div>

      {/* Image upload section */}
      <div className={styles.formGroup}>
        <label>Zdjęcie samochodu</label>
        <div className={styles.imageUploadContainer}>
          {/* Image preview logic */}
        </div>
      </div>

      <div className={styles.formGroup}>
        <label>Opis *</label>
        <textarea
          name="description"
          value={formData.description}
          onChange={handleInputChange}
          rows="4"
          className={errors.description ? styles.errorInput : ''}
        />
        {errors.description && <span className={styles.errorText}>{errors.description}</span>}
      </div>

      <div className={styles.formGroup}>
        <label className={styles.checkboxLabel}>
          <input
            type="checkbox"
            name="isActive"
            checked={formData.isActive}
            onChange={handleInputChange}
          />
          Model aktywny
        </label>
      </div>
    </>
  );

  const renderEnginesForm = () => (
    <>
      <div className={styles.formGrid}>
        <div className={styles.formGroup}>
          <label>Nazwa silnika *</label>
          <input
            type="text"
            name="name"
            value={formData.name}
            onChange={handleInputChange}
            className={errors.name ? styles.errorInput : ''}
          />
          {errors.name && <span className={styles.errorText}>{errors.name}</span>}
        </div>

        <div className={styles.formGroup}>
          <label>Typ silnika *</label>
          <select
            name="type"
            value={formData.type}
            onChange={handleInputChange}
            className={errors.type ? styles.errorInput : ''}
          >
            <option value="">Wybierz typ</option>
            <option value="benzyna">Benzyna</option>
            <option value="diesel">Diesel</option>
            <option value="elektryczny">Elektryczny</option>
            <option value="hybryda">Hybryda</option>
          </select>
          {errors.type && <span className={styles.errorText}>{errors.type}</span>}
        </div>

        <div className={styles.formGroup}>
          <label>Pojemność (L)</label>
          <input
            type="number"
            name="capacity"
            value={formData.capacity}
            onChange={handleInputChange}
            min="0"
            step="0.1"
            className={errors.capacity ? styles.errorInput : ''}
          />
          {errors.capacity && <span className={styles.errorText}>{errors.capacity}</span>}
        </div>

        <div className={styles.formGroup}>
          <label>Moc (KM) *</label>
          <input
            type="number"
            name="power"
            value={formData.power}
            onChange={handleInputChange}
            min="0"
            className={errors.power ? styles.errorInput : ''}
          />
          {errors.power && <span className={styles.errorText}>{errors.power}</span>}
        </div>

        <div className={styles.formGroup}>
          <label>Moment obrotowy (Nm)</label>
          <input
            type="number"
            name="torque"
            value={formData.torque}
            onChange={handleInputChange}
            min="0"
            className={errors.torque ? styles.errorInput : ''}
          />
          {errors.torque && <span className={styles.errorText}>{errors.torque}</span>}
        </div>

        <div className={styles.formGroup}>
          <label>Rodzaj paliwa *</label>
          <select
            name="fuelType"
            value={formData.fuelType}
            onChange={handleInputChange}
            className={errors.fuelType ? styles.errorInput : ''}
          >
            <option value="">Wybierz rodzaj paliwa</option>
            <option value="benzyna">Benzyna</option>
            <option value="diesel">Diesel</option>
            <option value="elektryczny">Elektryczny</option>
            <option value="hybryda">Hybryda</option>
            <option value="CNG">CNG</option>
            <option value="LPG">LPG</option>
          </select>
          {errors.fuelType && <span className={styles.errorText}>{errors.fuelType}</span>}
        </div>

        <div className={styles.formGroup}>
          <label>Liczba cylindrów</label>
          <input
            type="number"
            name="cylinders"
            value={formData.cylinders}
            onChange={handleInputChange}
            min="1"
            max="16"
            className={errors.cylinders ? styles.errorInput : ''}
          />
          {errors.cylinders && <span className={styles.errorText}>{errors.cylinders}</span>}
        </div>

        <div className={styles.formGroup}>
          <label>Skrzynia biegów</label>
          <select
            name="transmission"
            value={formData.transmission}
            onChange={handleInputChange}
            className={errors.transmission ? styles.errorInput : ''}
          >
            <option value="">Wybierz skrzynię</option>
            <option value="Manualna">Manualna</option>
            <option value="Automatyczna">Automatyczna</option>
            <option value="CVT">CVT</option>
            <option value="DSG">DSG</option>
          </select>
          {errors.transmission && <span className={styles.errorText}>{errors.transmission}</span>}
        </div>

        <div className={styles.formGroup}>
          <label>Liczba biegów</label>
          <input
            type="number"
            name="gears"
            value={formData.gears}
            onChange={handleInputChange}
            min="1"
            max="10"
            className={errors.gears ? styles.errorInput : ''}
          />
          {errors.gears && <span className={styles.errorText}>{errors.gears}</span>}
        </div>

        <div className={styles.formGroup}>
          <label>Napęd</label>
          <select
            name="driveType"
            value={formData.driveType}
            onChange={handleInputChange}
            className={errors.driveType ? styles.errorInput : ''}
          >
            <option value="">Wybierz napęd</option>
            <option value="FWD">Przedni (FWD)</option>
            <option value="RWD">Tylni (RWD)</option>
            <option value="AWD">4x4 (AWD)</option>
          </select>
          {errors.driveType && <span className={styles.errorText}>{errors.driveType}</span>}
        </div>

        <div className={styles.formGroup}>
          <label>Zużycie paliwa (l/100km)</label>
          <input
            type="number"
            name="fuelConsumption"
            value={formData.fuelConsumption}
            onChange={handleInputChange}
            min="0"
            step="0.1"
            className={errors.fuelConsumption ? styles.errorInput : ''}
          />
          {errors.fuelConsumption && <span className={styles.errorText}>{errors.fuelConsumption}</span>}
        </div>

        <div className={styles.formGroup}>
          <label>Emisja CO2 (g/km)</label>
          <input
            type="number"
            name="co2Emission"
            value={formData.co2Emission}
            onChange={handleInputChange}
            min="0"
            className={errors.co2Emission ? styles.errorInput : ''}
          />
          {errors.co2Emission && <span className={styles.errorText}>{errors.co2Emission}</span>}
        </div>
      </div>

      <div className={styles.formGroup}>
        <label>Opis</label>
        <textarea
          name="description"
          value={formData.description}
          onChange={handleInputChange}
          rows="4"
          className={errors.description ? styles.errorInput : ''}
        />
        {errors.description && <span className={styles.errorText}>{errors.description}</span>}
      </div>

      <div className={styles.formGroup}>
        <label className={styles.checkboxLabel}>
          <input
            type="checkbox"
            name="isActive"
            checked={formData.isActive}
            onChange={handleInputChange}
          />
          Silnik aktywny
        </label>
      </div>
    </>
  );

  switch (activeTab) {
    case 'models':
      return renderModelsForm();
    case 'engines':
      return renderEnginesForm();
    case 'accessories':
      // TODO: Implement accessories form
      return <div>Formularz akcesoriów w przygotowaniu...</div>;
    case 'interior':
      // TODO: Implement interior form  
      return <div>Formularz wyposażenia wnętrza w przygotowaniu...</div>;
    default:
      return renderModelsForm();
  }
};

export default FormRenderer;