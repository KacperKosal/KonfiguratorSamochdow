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
            maxLength={255}
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
            {manufacturers.map((manufacturer, index) => (
              <option key={`manufacturer-${index}-${manufacturer}`} value={manufacturer}>{manufacturer}</option>
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
            {bodyTypes.map((type, index) => (
              <option key={`bodytype-${index}-${type}`} value={type}>{type}</option>
            ))}
          </select>
          {errors.bodyType && <span className={styles.errorText}>{errors.bodyType}</span>}
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
            max="1000000"
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
          maxLength={800}
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

      <div className={styles.formGroup}>
        <label className={styles.checkboxLabel}>
          <input
            type="checkbox"
            name="has4x4"
            checked={formData.has4x4}
            onChange={handleInputChange}
          />
          Napęd 4x4
        </label>
      </div>

      <div className={styles.formGroup}>
        <label className={styles.checkboxLabel}>
          <input
            type="checkbox"
            name="isElectric"
            checked={formData.isElectric}
            onChange={handleInputChange}
          />
          Samochód elektryczny
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
            maxLength={255}
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
          <label>Pojemność (L) *</label>
          <select
            name="capacity"
            value={formData.capacity}
            onChange={handleInputChange}
            className={errors.capacity ? styles.errorInput : ''}
          >
            <option value="">Wybierz pojemność</option>
            <option value="1000">1.0L</option>
            <option value="1600">1.6L</option>
            <option value="1800">1.8L</option>
            <option value="1900">1.9L</option>
            <option value="2000">2.0L</option>
            <option value="2500">2.5L</option>
            <option value="3000">3.0L</option>
            <option value="4000">4.0L</option>
            <option value="5000">5.0L</option>
          </select>
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
            max="1000000"
            className={errors.power ? styles.errorInput : ''}
          />
          {errors.power && <span className={styles.errorText}>{errors.power}</span>}
        </div>

        <div className={styles.formGroup}>
          <label>Moment obrotowy (Nm) *</label>
          <input
            type="number"
            name="torque"
            value={formData.torque}
            onChange={handleInputChange}
            min="1"
            max="1000000"
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
          <label>Liczba cylindrów *</label>
          <input
            type="number"
            name="cylinders"
            value={formData.cylinders}
            onChange={handleInputChange}
            min="1"
            max="1000000"
            className={errors.cylinders ? styles.errorInput : ''}
          />
          {errors.cylinders && <span className={styles.errorText}>{errors.cylinders}</span>}
        </div>

        <div className={styles.formGroup}>
          <label>Skrzynia biegów *</label>
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
          <label>Liczba biegów *</label>
          <input
            type="number"
            name="gears"
            value={formData.gears}
            onChange={handleInputChange}
            min="1"
            max="1000000"
            className={errors.gears ? styles.errorInput : ''}
          />
          {errors.gears && <span className={styles.errorText}>{errors.gears}</span>}
        </div>

        <div className={styles.formGroup}>
          <label>Napęd *</label>
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
          <label>Zużycie paliwa (l/100km) *</label>
          <input
            type="number"
            name="fuelConsumption"
            value={formData.fuelConsumption}
            onChange={handleInputChange}
            min="1"
            max="1000000"
            step="0.1"
            className={errors.fuelConsumption ? styles.errorInput : ''}
          />
          {errors.fuelConsumption && <span className={styles.errorText}>{errors.fuelConsumption}</span>}
        </div>

        <div className={styles.formGroup}>
          <label>Emisja CO2 (g/km) *</label>
          <input
            type="number"
            name="co2Emission"
            value={formData.co2Emission}
            onChange={handleInputChange}
            min="1"
            max="1000000"
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
          maxLength={800}
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

  const renderAccessoriesForm = () => (
    <>
      <div className={styles.formGrid}>
        <div className={styles.formGroup}>
          <label>ID samochodu *</label>
          <input
            type="text"
            name="carId"
            value={formData.carId}
            onChange={handleInputChange}
            className={errors.carId ? styles.errorInput : ''}
            maxLength={255}
          />
          {errors.carId && <span className={styles.errorText}>{errors.carId}</span>}
        </div>

        <div className={styles.formGroup}>
          <label>Nazwa akcesoria *</label>
          <input
            type="text"
            name="name"
            value={formData.name}
            onChange={handleInputChange}
            className={errors.name ? styles.errorInput : ''}
            maxLength={255}
          />
          {errors.name && <span className={styles.errorText}>{errors.name}</span>}
        </div>

        <div className={styles.formGroup}>
          <label>Kategoria *</label>
          <select
            name="category"
            value={formData.category}
            onChange={handleInputChange}
            className={errors.category ? styles.errorInput : ''}
          >
            <option value="">Wybierz kategorię</option>
            <option value="Wnętrze">Wnętrze</option>
            <option value="Zewnętrze">Zewnętrze</option>
            <option value="Bezpieczeństwo">Bezpieczeństwo</option>
            <option value="Komfort">Komfort</option>
            <option value="Transport">Transport</option>
          </select>
          {errors.category && <span className={styles.errorText}>{errors.category}</span>}
        </div>

        <div className={styles.formGroup}>
          <label>Typ akcesoria *</label>
          <select
            name="type"
            value={formData.type}
            onChange={handleInputChange}
            className={errors.type ? styles.errorInput : ''}
          >
            <option value="">Wybierz typ</option>
            <option value="AlloyWheels">Felgi aluminiowe</option>
            <option value="FloorMats">Dywaniki</option>
            <option value="RoofBoxes">Bagażniki dachowe</option>
            <option value="ChildSeats">Foteliki dziecięce</option>
            <option value="Other">Inne</option>
          </select>
          {errors.type && <span className={styles.errorText}>{errors.type}</span>}
        </div>

        <div className={styles.formGroup}>
          <label>Cena (PLN) *</label>
          <input
            type="number"
            name="price"
            value={formData.price}
            onChange={handleInputChange}
            min="0"
            max="1000000"
            step="0.01"
            className={errors.price ? styles.errorInput : ''}
          />
          {errors.price && <span className={styles.errorText}>{errors.price}</span>}
        </div>

        <div className={styles.formGroup}>
          <label>Ilość w magazynie *</label>
          <input
            type="number"
            name="stockQuantity"
            value={formData.stockQuantity}
            onChange={handleInputChange}
            min="0"
            max="1000000"
            className={errors.stockQuantity ? styles.errorInput : ''}
          />
          {errors.stockQuantity && <span className={styles.errorText}>{errors.stockQuantity}</span>}
        </div>

        <div className={styles.formGroup}>
          <label>Numer części *</label>
          <input
            type="text"
            name="partNumber"
            value={formData.partNumber}
            onChange={handleInputChange}
            className={errors.partNumber ? styles.errorInput : ''}
            maxLength={255}
          />
          {errors.partNumber && <span className={styles.errorText}>{errors.partNumber}</span>}
        </div>

        <div className={styles.formGroup}>
          <label>Wzór (dla felg)</label>
          <input
            type="text"
            name="pattern"
            value={formData.pattern}
            onChange={handleInputChange}
            className={errors.pattern ? styles.errorInput : ''}
            maxLength={255}
          />
          {errors.pattern && <span className={styles.errorText}>{errors.pattern}</span>}
        </div>

        <div className={styles.formGroup}>
          <label>Pojemność (dla bagażników)</label>
          <input
            type="number"
            name="capacity"
            value={formData.capacity}
            onChange={handleInputChange}
            min="0"
            max="1000000"
            className={errors.capacity ? styles.errorInput : ''}
          />
          {errors.capacity && <span className={styles.errorText}>{errors.capacity}</span>}
        </div>

        <div className={styles.formGroup}>
          <label>Max. obciążenie (kg)</label>
          <input
            type="number"
            name="maxLoad"
            value={formData.maxLoad}
            onChange={handleInputChange}
            min="0"
            max="1000000"
            className={errors.maxLoad ? styles.errorInput : ''}
          />
          {errors.maxLoad && <span className={styles.errorText}>{errors.maxLoad}</span>}
        </div>

        <div className={styles.formGroup}>
          <label>Grupa wiekowa (dla fotelika)</label>
          <input
            type="text"
            name="ageGroup"
            value={formData.ageGroup}
            onChange={handleInputChange}
            className={errors.ageGroup ? styles.errorInput : ''}
            maxLength={255}
          />
          {errors.ageGroup && <span className={styles.errorText}>{errors.ageGroup}</span>}
        </div>

        <div className={styles.formGroup}>
          <label>Producent *</label>
          <input
            type="text"
            name="manufacturer"
            value={formData.manufacturer}
            onChange={handleInputChange}
            className={errors.manufacturer ? styles.errorInput : ''}
            maxLength={255}
          />
          {errors.manufacturer && <span className={styles.errorText}>{errors.manufacturer}</span>}
        </div>

        <div className={styles.formGroup}>
          <label>Rozmiar</label>
          <input
            type="text"
            name="size"
            value={formData.size}
            onChange={handleInputChange}
            className={errors.size ? styles.errorInput : ''}
            maxLength={255}
          />
          {errors.size && <span className={styles.errorText}>{errors.size}</span>}
        </div>

        <div className={styles.formGroup}>
          <label>Kolor</label>
          <input
            type="text"
            name="color"
            value={formData.color}
            onChange={handleInputChange}
            className={errors.color ? styles.errorInput : ''}
            maxLength={255}
          />
          {errors.color && <span className={styles.errorText}>{errors.color}</span>}
        </div>

        <div className={styles.formGroup}>
          <label>Materiał</label>
          <input
            type="text"
            name="material"
            value={formData.material}
            onChange={handleInputChange}
            className={errors.material ? styles.errorInput : ''}
            maxLength={255}
          />
          {errors.material && <span className={styles.errorText}>{errors.material}</span>}
        </div>

        <div className={styles.formGroup}>
          <label>Kompatybilność *</label>
          <input
            type="text"
            name="compatibility"
            value={formData.compatibility}
            onChange={handleInputChange}
            className={errors.compatibility ? styles.errorInput : ''}
            maxLength={255}
          />
          {errors.compatibility && <span className={styles.errorText}>{errors.compatibility}</span>}
        </div>

        <div className={styles.formGroup}>
          <label>Trudność instalacji</label>
          <select
            name="installationDifficulty"
            value={formData.installationDifficulty}
            onChange={handleInputChange}
            className={errors.installationDifficulty ? styles.errorInput : ''}
          >
            <option value="">Wybierz poziom</option>
            <option value="Easy">Łatwy</option>
            <option value="Medium">Średni</option>
            <option value="Professional">Profesjonalny</option>
          </select>
          {errors.installationDifficulty && <span className={styles.errorText}>{errors.installationDifficulty}</span>}
        </div>

        <div className={styles.formGroup}>
          <label>Gwarancja</label>
          <input
            type="text"
            name="warranty"
            value={formData.warranty}
            onChange={handleInputChange}
            className={errors.warranty ? styles.errorInput : ''}
            maxLength={255}
          />
          {errors.warranty && <span className={styles.errorText}>{errors.warranty}</span>}
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
          maxLength={800}
        />
        {errors.description && <span className={styles.errorText}>{errors.description}</span>}
      </div>

      <div className={styles.formGroup}>
        <label className={styles.checkboxLabel}>
          <input
            type="checkbox"
            name="isOriginalBmwPart"
            checked={formData.isOriginalBmwPart}
            onChange={handleInputChange}
          />
          Oryginalna część BMW
        </label>
      </div>

      <div className={styles.formGroup}>
        <label className={styles.checkboxLabel}>
          <input
            type="checkbox"
            name="isInStock"
            checked={formData.isInStock}
            onChange={handleInputChange}
          />
          Dostępny w magazynie
        </label>
      </div>
    </>
  );

  const renderInteriorForm = () => (
    <>
      <div className={styles.formGrid}>
        <div className={styles.formGroup}>
          <label>ID samochodu *</label>
          <input
            type="text"
            name="carId"
            value={formData.carId}
            onChange={handleInputChange}
            className={errors.carId ? styles.errorInput : ''}
          />
          {errors.carId && <span className={styles.errorText}>{errors.carId}</span>}
        </div>

        <div className={styles.formGroup}>
          <label>Typ wyposażenia *</label>
          <select
            name="type"
            value={formData.type}
            onChange={handleInputChange}
            className={errors.type ? styles.errorInput : ''}
          >
            <option value="">Wybierz typ</option>
            <option value="DashboardMaterial">Materiał deski rozdzielczej</option>
            <option value="InteriorTrim">Wykładzina wnętrza</option>
            <option value="SteeringWheel">Kierownica</option>
            <option value="RadioType">System audio</option>
            <option value="CruiseControl">Tempomat</option>
            <option value="Other">Inne</option>
          </select>
          {errors.type && <span className={styles.errorText}>{errors.type}</span>}
        </div>

        <div className={styles.formGroup}>
          <label>Wartość *</label>
          <input
            type="text"
            name="value"
            value={formData.value}
            onChange={handleInputChange}
            className={errors.value ? styles.errorInput : ''}
            maxLength={255}
          />
          {errors.value && <span className={styles.errorText}>{errors.value}</span>}
        </div>

        <div className={styles.formGroup}>
          <label>Cena dodatkowa (PLN) *</label>
          <input
            type="number"
            name="additionalPrice"
            value={formData.additionalPrice}
            onChange={handleInputChange}
            min="0"
            max="1000000"
            step="0.01"
            className={errors.additionalPrice ? styles.errorInput : ''}
          />
          {errors.additionalPrice && <span className={styles.errorText}>{errors.additionalPrice}</span>}
        </div>


        {formData.type === 'RadioType' && (
          <>
            <div className={styles.formGroup}>
              <label className={styles.checkboxLabel}>
                <input
                  type="checkbox"
                  name="hasNavigation"
                  checked={formData.hasNavigation}
                  onChange={handleInputChange}
                />
                Z nawigacją
              </label>
            </div>
            <div className={styles.formGroup}>
              <label className={styles.checkboxLabel}>
                <input
                  type="checkbox"
                  name="hasPremiumSound"
                  checked={formData.hasPremiumSound}
                  onChange={handleInputChange}
                />
                Premium Sound System
              </label>
            </div>
          </>
        )}

        {formData.type === 'CruiseControl' && (
          <div className={styles.formGroup}>
            <label>Typ sterowania</label>
            <select
              name="controlType"
              value={formData.controlType}
              onChange={handleInputChange}
              className={errors.controlType ? styles.errorInput : ''}
            >
              <option value="">Wybierz typ</option>
              <option value="Standard">Standardowy</option>
              <option value="Adaptive">Adaptacyjny</option>
              <option value="None">Brak</option>
            </select>
            {errors.controlType && <span className={styles.errorText}>{errors.controlType}</span>}
          </div>
        )}
      </div>

      <div className={styles.formGroup}>
        <label>Opis</label>
        <textarea
          name="description"
          value={formData.description}
          onChange={handleInputChange}
          rows="4"
          className={errors.description ? styles.errorInput : ''}
          maxLength={800}
        />
        {errors.description && <span className={styles.errorText}>{errors.description}</span>}
      </div>

      <div className={styles.formGroup}>
        <label className={styles.checkboxLabel}>
          <input
            type="checkbox"
            name="isDefault"
            checked={formData.isDefault}
            onChange={handleInputChange}
          />
          Domyślne wyposażenie
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
      return renderAccessoriesForm();
    case 'interior':
      return renderInteriorForm();
    default:
      return renderModelsForm();
  }
};

export default FormRenderer;