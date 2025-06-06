import React, { useState, useEffect } from 'react';
import { Plus, Edit, Trash2, Eye, Save, X, AlertCircle } from 'lucide-react';
import adminApiService from '../../services/adminApiService';
import styles from './CarAdminPanel.module.css';

const CarAdminPanel = () => {
  const [cars, setCars] = useState([]);
  const [loading, setLoading] = useState(true);
  const [apiError, setApiError] = useState('');
  const [showForm, setShowForm] = useState(false);
  const [isEditing, setIsEditing] = useState(false);
  const [editingId, setEditingId] = useState(null);
  const [selectedCar, setSelectedCar] = useState(null);
  const [showDetails, setShowDetails] = useState(false);

  // Stan formularza dopasowany do API
  const [formData, setFormData] = useState({
    name: '',
    productionYear: new Date().getFullYear(),
    bodyType: '',
    manufacturer: '',
    segment: '',
    basePrice: '',
    description: '',
    imageUrl: '',
    isActive: true
  });

  const [errors, setErrors] = useState({});
  const [uploadingImage, setUploadingImage] = useState(false);

  // Opcje dla select-ów
  const bodyTypes = ['Sedan', 'Hatchback', 'SUV', 'Combi', 'Coupe', 'Cabrio'];
  const segments = ['A', 'B', 'C', 'D', 'E', 'F'];
  const manufacturers = ['Toyota', 'BMW', 'Mercedes', 'Audi', 'Volkswagen', 'Ford', 'Opel', 'Peugeot'];

  // Ładowanie danych z API
  const loadCarModels = async () => {
    try {
      setLoading(true);
      setApiError('');
      const data = await adminApiService.getAllCarModels();
      setCars(data);
    } catch (error) {
      console.error('Error loading car models:', error);
      setApiError('Nie udało się załadować modeli samochodów');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadCarModels();
  }, []);

  // Resetowanie formularza
  const resetForm = () => {
    setFormData({
      name: '',
      productionYear: new Date().getFullYear(),
      bodyType: '',
      manufacturer: '',
      segment: '',
      basePrice: '',
      description: '',
      imageUrl: '',
      isActive: true
    });
    setErrors({});
    setIsEditing(false);
    setEditingId(null);
  };

  // Walidacja formularza
  const validateForm = () => {
    const newErrors = {};

    if (!formData.name.trim()) {
      newErrors.name = 'Nazwa modelu jest wymagana';
    }

    if (!formData.manufacturer) {
      newErrors.manufacturer = 'Producent jest wymagany';
    }

    if (!formData.bodyType) {
      newErrors.bodyType = 'Typ nadwozia jest wymagany';
    }

    if (!formData.segment) {
      newErrors.segment = 'Segment jest wymagany';
    }

    if (!formData.basePrice) {
      newErrors.basePrice = 'Cena bazowa jest wymagana';
    } else if (isNaN(formData.basePrice) || parseFloat(formData.basePrice) <= 0) {
      newErrors.basePrice = 'Cena musi być liczbą większą od 0';
    }

    if (!formData.description.trim()) {
      newErrors.description = 'Opis jest wymagany';
    } else if (formData.description.trim().length < 10) {
      newErrors.description = 'Opis musi mieć co najmniej 10 znaków';
    }

    const currentYear = new Date().getFullYear();
    if (formData.productionYear < 2000 || formData.productionYear > currentYear + 1) {
      newErrors.productionYear = `Rok produkcji musi być między 2000 a ${currentYear + 1}`;
    }

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  // Obsługa zmiany pól formularza
  const handleInputChange = (e) => {
    const { name, value, type, checked } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: type === 'checkbox' ? checked : value
    }));
  };

  // Obsługa uploadu zdjęcia
  const handleImageUpload = async (e) => {
    const file = e.target.files[0];
    if (!file) return;

    try {
      setUploadingImage(true);
      const result = await adminApiService.uploadImage(file);
      setFormData(prev => ({
        ...prev,
        imageUrl: result.fileName
      }));
    } catch (error) {
      console.error('Error uploading image:', error);
      setApiError('Nie udało się przesłać zdjęcia');
    } finally {
      setUploadingImage(false);
    }
  };

  // Usuwanie zdjęcia
  const handleRemoveImage = async () => {
    if (formData.imageUrl) {
      try {
        await adminApiService.deleteImage(formData.imageUrl);
        setFormData(prev => ({
          ...prev,
          imageUrl: ''
        }));
      } catch (error) {
        console.error('Error deleting image:', error);
        setApiError('Nie udało się usunąć zdjęcia');
      }
    }
  };

  // Dodawanie nowego modelu
  const handleAddCar = () => {
    resetForm();
    setShowForm(true);
    setShowDetails(false);
  };

  // Edycja modelu
  const handleEditCar = (car) => {
    setFormData({
      name: car.name,
      productionYear: car.productionYear,
      bodyType: car.bodyType,
      manufacturer: car.manufacturer,
      segment: car.segment,
      basePrice: car.basePrice.toString(),
      description: car.description,
      imageUrl: car.imageUrl || '',
      isActive: car.isActive
    });
    setIsEditing(true);
    setEditingId(car.id);
    setShowForm(true);
    setShowDetails(false);
  };

  // Zapisywanie formularza
  const handleSubmit = async (e) => {
    e.preventDefault();
    
    if (!validateForm()) {
      return;
    }

    try {
      setLoading(true);
      const carData = {
        ...formData,
        basePrice: parseFloat(formData.basePrice),
        productionYear: parseInt(formData.productionYear)
      };

      if (isEditing) {
        await adminApiService.updateCarModel(editingId, carData);
      } else {
        await adminApiService.createCarModel(carData);
      }

      await loadCarModels();
      setShowForm(false);
      resetForm();
    } catch (error) {
      console.error('Error saving car model:', error);
      setApiError(`Nie udało się ${isEditing ? 'zaktualizować' : 'dodać'} modelu`);
    }
  };

  // Usuwanie modelu
  const handleDeleteCar = async (id) => {
    if (!window.confirm('Czy na pewno chcesz usunąć ten model?')) {
      return;
    }

    try {
      setLoading(true);
      await adminApiService.deleteCarModel(id);
      await loadCarModels();
    } catch (error) {
      console.error('Error deleting car model:', error);
      setApiError('Nie udało się usunąć modelu');
    }
  };

  // Wyświetlanie szczegółów
  const handleViewDetails = (car) => {
    setSelectedCar(car);
    setShowDetails(true);
    setShowForm(false);
  };

  return (
    <div className={styles.adminContainer}>
      <header className={styles.header}>
        <h1>Panel Administratora - Modele Samochodów</h1>
        <button 
          onClick={handleAddCar}
          className={styles.addButton}
        >
          <Plus size={20} />
          Dodaj Model
        </button>
      </header>

      {apiError && (
        <div className={styles.errorMessage}>
          <AlertCircle size={18} />
          {apiError}
        </div>
      )}

      {/* Formularz */}
      {showForm && (
        <div className={styles.formContainer}>
          <div className={styles.formHeader}>
            <h2>{isEditing ? 'Edytuj Model' : 'Dodaj Nowy Model'}</h2>
            <button 
              onClick={() => setShowForm(false)}
              className={styles.closeButton}
            >
              <X size={20} />
            </button>
          </div>

          <form onSubmit={handleSubmit} className={styles.form}>
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

            <div className={styles.formGroup}>
              <label>Zdjęcie samochodu</label>
              <div className={styles.imageUploadContainer}>
                {formData.imageUrl ? (
                  <div className={styles.imagePreview}>
                    <img 
                      src={adminApiService.getImageUrl(formData.imageUrl)} 
                      alt="Podgląd" 
                      className={styles.previewImage}
                    />
                    <button 
                      type="button" 
                      onClick={handleRemoveImage}
                      className={styles.removeImageButton}
                    >
                      Usuń zdjęcie
                    </button>
                  </div>
                ) : (
                  <div className={styles.uploadArea}>
                    <input
                      type="file"
                      accept="image/*"
                      onChange={handleImageUpload}
                      className={styles.fileInput}
                      disabled={uploadingImage}
                    />
                    {uploadingImage ? (
                      <p>Przesyłanie...</p>
                    ) : (
                      <p>Kliknij aby wybrać zdjęcie lub przeciągnij je tutaj</p>
                    )}
                  </div>
                )}
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

            <div className={styles.formActions}>
              <button 
                type="submit" 
                className={styles.saveButton}
                disabled={loading}
              >
                <Save size={20} />
                {isEditing ? 'Zaktualizuj' : 'Dodaj'}
              </button>
              <button 
                type="button" 
                onClick={() => setShowForm(false)}
                className={styles.cancelButton}
              >
                Anuluj
              </button>
            </div>
          </form>
        </div>
      )}

      {/* Tabela */}
      <div className={styles.tableContainer}>
        <table className={styles.table}>
          <thead>
            <tr>
              <th>Nazwa</th>
              <th>Producent</th>
              <th>Typ</th>
              <th>Rok</th>
              <th>Cena</th>
              <th>Status</th>
              <th>Akcje</th>
            </tr>
          </thead>
          <tbody>
            {loading ? (
              <tr>
                <td colSpan="7" className={styles.loading}>Ładowanie...</td>
              </tr>
            ) : cars.length === 0 ? (
              <tr>
                <td colSpan="7" className={styles.noData}>Brak modeli</td>
              </tr>
            ) : (
              cars.map(car => (
                <tr key={car.id}>
                  <td>{car.name}</td>
                  <td>{car.manufacturer}</td>
                  <td>{car.bodyType}</td>
                  <td>{car.productionYear}</td>
                  <td>{car.basePrice?.toLocaleString('pl-PL')} PLN</td>
                  <td>
                    <span className={car.isActive ? styles.active : styles.inactive}>
                      {car.isActive ? 'Aktywny' : 'Nieaktywny'}
                    </span>
                  </td>
                  <td className={styles.actions}>
                    <button 
                      onClick={() => handleViewDetails(car)}
                      className={styles.viewButton}
                      title="Zobacz szczegóły"
                    >
                      <Eye size={16} />
                    </button>
                    <button 
                      onClick={() => handleEditCar(car)}
                      className={styles.editButton}
                      title="Edytuj"
                    >
                      <Edit size={16} />
                    </button>
                    <button 
                      onClick={() => handleDeleteCar(car.id)}
                      className={styles.deleteButton}
                      title="Usuń"
                    >
                      <Trash2 size={16} />
                    </button>
                  </td>
                </tr>
              ))
            )}
          </tbody>
        </table>
      </div>

      {/* Modal szczegółów */}
      {showDetails && selectedCar && (
        <div className={styles.modal}>
          <div className={styles.modalContent}>
            <div className={styles.modalHeader}>
              <h3>Szczegóły modelu: {selectedCar.name}</h3>
              <button 
                onClick={() => setShowDetails(false)}
                className={styles.closeButton}
              >
                <X size={20} />
              </button>
            </div>
            
            <div className={styles.modalBody}>
              <div className={styles.detailsGrid}>
                <div><strong>ID:</strong> {selectedCar.id}</div>
                <div><strong>Nazwa:</strong> {selectedCar.name}</div>
                <div><strong>Producent:</strong> {selectedCar.manufacturer}</div>
                <div><strong>Typ nadwozia:</strong> {selectedCar.bodyType}</div>
                <div><strong>Segment:</strong> {selectedCar.segment}</div>
                <div><strong>Rok produkcji:</strong> {selectedCar.productionYear}</div>
                <div><strong>Cena bazowa:</strong> {selectedCar.basePrice?.toLocaleString('pl-PL')} PLN</div>
                <div><strong>Status:</strong> {selectedCar.isActive ? 'Aktywny' : 'Nieaktywny'}</div>
              </div>
              
              {selectedCar.imageUrl && (
                <div className={styles.imageContainer}>
                  <img 
                    src={adminApiService.getImageUrl(selectedCar.imageUrl)} 
                    alt={selectedCar.name}
                    className={styles.carImage}
                  />
                </div>
              )}
              
              <div className={styles.description}>
                <strong>Opis:</strong>
                <p>{selectedCar.description}</p>
              </div>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default CarAdminPanel;