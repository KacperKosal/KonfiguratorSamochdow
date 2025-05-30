// CarForm.jsx
import ImageUpload from '../ImageUpload/ImageUpload';
import FeatureCheckboxes from '../FeatureCheckboxes/FeatureCheckboxes';
import CustomFeatures from '../CustomFeatures/CustomFeatures';
import formStyles from '../Form.module/Form.module.css';
import btnStyles from '../Form.module/Button.module.css';
import commonStyles from '../Form.module/Common.module.css';

const CarForm = ({ 
  formData, 
  errors, 
  isEditing, 
  dostepneCechy,
  nowaCecha,
  onInputChange, 
  onSubmit, 
  onCancel,
  onImageChange,
  onImageRemove,
  onFeatureChange,
  onNowaCechaChange,
  onAddCustomFeature,
  onRemoveCustomFeature
}) => (
  <div className={commonStyles.card}>
    <div className={commonStyles.cardHeader}>
      <h2 className={commonStyles.cardTitle}>
        {isEditing ? 'Edytuj Samochód' : 'Dodaj Nowy Samochód'}
      </h2>
      <button
        onClick={onCancel}
        className={commonStyles.closeBtn}
      >
        ×
      </button>
    </div>

    <div className={commonStyles.spaceY6}>
      <div className={formStyles.formGrid3}>
        {/* Marka */}
        <div className={formStyles.formGroup}>
          <label className={formStyles.formLabel}>
            Marka *
          </label>
          <input
            type="text"
            name="marka"
            value={formData.marka}
            onChange={onInputChange}
            className={`${formStyles.formInput} ${errors.marka ? formStyles.error : ''}`}
            placeholder="np. Toyota, BMW, Audi"
          />
          {errors.marka && (
            <p className={formStyles.errorMessage}>{errors.marka}</p>
          )}
        </div>

        {/* Model */}
        <div className={formStyles.formGroup}>
          <label className={formStyles.formLabel}>
            Model *
          </label>
          <input
            type="text"
            name="model"
            value={formData.model}
            onChange={onInputChange}
            className={`${formStyles.formInput} ${errors.model ? formStyles.error : ''}`}
            placeholder="np. Corolla, X5, A4"
          />
          {errors.model && (
            <p className={formStyles.errorMessage}>{errors.model}</p>
          )}
        </div>

        {/* Rok */}
        <div className={formStyles.formGroup}>
          <label className={formStyles.formLabel}>
            Rok produkcji *
          </label>
          <input
            type="number"
            name="rok"
            value={formData.rok}
            onChange={onInputChange}
            min="1900"
            max={new Date().getFullYear() + 1}
            className={`${formStyles.formInput} ${errors.rok ? formStyles.error : ''}`}
            placeholder="2023"
          />
          {errors.rok && (
            <p className={formStyles.errorMessage}>{errors.rok}</p>
          )}
        </div>

        {/* Cena */}
        <div className={formStyles.formGroup}>
          <label className={formStyles.formLabel}>
            Cena (PLN) *
          </label>
          <input
            type="number"
            name="cena"
            value={formData.cena}
            onChange={onInputChange}
            min="0"
            step="0.01"
            className={`${formStyles.formInput} ${errors.cena ? formStyles.error : ''}`}
            placeholder="85000"
          />
          {errors.cena && (
            <p className={formStyles.errorMessage}>{errors.cena}</p>
          )}
        </div>

        {/* Kolor Nadwozia */}
        <div className={formStyles.formGroup}>
          <label className={formStyles.formLabel}>
            Kolor nadwozia *
          </label>
          <input
            type="text"
            name="kolorNadwozia"
            value={formData.kolorNadwozia}
            onChange={onInputChange}
            className={`${formStyles.formInput} ${errors.kolorNadwozia ? formStyles.error : ''}`}
            placeholder="np. Czerwony, Niebieski metalik"
          />
          {errors.kolorNadwozia && (
            <p className={formStyles.errorMessage}>{errors.kolorNadwozia}</p>
          )}
        </div>

        {/* Typ nadwozia */}
        <div className={formStyles.formGroup}>
          <label className={formStyles.formLabel}>
            Typ nadwozia *
          </label>
          <select
            name="typ"
            value={formData.typ}
            onChange={onInputChange}
            className={`${formStyles.formSelect} ${errors.typ ? formStyles.error : ''}`}
          >
            <option value="">Wybierz typ</option>
            <option value="sedan">Sedan</option>
            <option value="hatchback">Hatchback</option>
            <option value="SUV">SUV</option>
            <option value="kombi">Kombi</option>
            <option value="coupe">Coupe</option>
            <option value="cabrio">Cabrio</option>
            <option value="pickup">Pickup</option>
            <option value="van">Van</option>
          </select>
          {errors.typ && (
            <p className={formStyles.errorMessage}>{errors.typ}</p>
          )}
        </div>

        {/* Pojemność */}
        <div className={formStyles.formGroup}>
          <label className={formStyles.formLabel}>
            Pojemność silnika (L) *
          </label>
          <input
            type="number"
            name="pojemnosc"
            value={formData.pojemnosc}
            onChange={onInputChange}
            min="0"
            step="0.1"
            className={`${formStyles.formInput} ${errors.pojemnosc ? formStyles.error : ''}`}
            placeholder="2.0"
          />
          {errors.pojemnosc && (
            <p className={formStyles.errorMessage}>{errors.pojemnosc}</p>
          )}
        </div>

        {/* Moc */}
        <div className={formStyles.formGroup}>
          <label className={formStyles.formLabel}>
            Moc silnika (KM) *
          </label>
          <input
            type="number"
            name="moc"
            value={formData.moc}
            onChange={onInputChange}
            min="0"
            className={`${formStyles.formInput} ${errors.moc ? formStyles.error : ''}`}
            placeholder="150"
          />
          {errors.moc && (
            <p className={formStyles.errorMessage}>{errors.moc}</p>
          )}
        </div>

        {/* Zdjęcie */}
        <ImageUpload 
          imageData={formData.zdjecie}
          onImageChange={onImageChange}
          onImageRemove={onImageRemove}
        />
      </div>

      {/* Wyposażenie wnętrza */}
      <div className={formStyles.formGroup}>
        <label className={formStyles.formLabel}>
          Wyposażenie wnętrza *
        </label>
        <textarea
          name="wyposazenieWnetrza"
          value={formData.wyposazenieWnetrza}
          onChange={onInputChange}
          rows={3}
          className={`${formStyles.formTextarea} ${errors.wyposazenieWnetrza ? formStyles.error : ''}`}
          placeholder="Skórzane fotele, klimatyzacja automatyczna, system multimedialny..."
        />
        {errors.wyposazenieWnetrza && (
          <p className={formStyles.errorMessage}>{errors.wyposazenieWnetrza}</p>
        )}
      </div>

      {/* Akcesoria */}
      <div className={formStyles.formGroup}>
        <label className={formStyles.formLabel}>
          Akcesoria
        </label>
        <textarea
          name="akcesoria"
          value={formData.akcesoria}
          onChange={onInputChange}
          rows={3}
          className={formStyles.formTextarea}
          placeholder="Mata bagażnika, komplet dywaników, hak holowniczy..."
        />
      </div>

      {/* Opis */}
      <div className={formStyles.formGroup}>
        <label className={formStyles.formLabel}>
          Opis *
        </label>
        <textarea
          name="opis"
          value={formData.opis}
          onChange={onInputChange}
          rows={4}
          className={`${formStyles.formTextarea} ${errors.opis ? formStyles.error : ''}`}
          placeholder="Szczegółowy opis samochodu, stan techniczny, historia..."
        />
        {errors.opis && (
          <p className={formStyles.errorMessage}>{errors.opis}</p>
        )}
      </div>

      {/* Checkboxy */}
      <FeatureCheckboxes 
        formData={formData}
        dostepneCechy={dostepneCechy}
        onFeatureChange={onFeatureChange}
        onBasicOptionChange={onInputChange}
      />

      {/* Własne cechy */}
      <CustomFeatures 
        formData={formData}
        nowaCecha={nowaCecha}
        onNowaCechaChange={onNowaCechaChange}
        onAddCustomFeature={onAddCustomFeature}
        onRemoveCustomFeature={onRemoveCustomFeature}
      />

      {/* Przyciski */}
      <div className={btnStyles.actions}>
        <button
          onClick={onSubmit}
          className={`${btnStyles.btn} ${btnStyles.btnPrimary}`}
        >
          {isEditing ? 'Zapisz Zmiany' : 'Dodaj Samochód'}
        </button>
        <button
          onClick={onCancel}
          className={`${btnStyles.btn} ${btnStyles.btnSecondary}`}
        >
          Anuluj
        </button>
      </div>
    </div>
  </div>
);

export default CarForm;