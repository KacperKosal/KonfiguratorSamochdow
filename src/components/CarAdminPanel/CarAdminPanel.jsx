import React, { useState, useEffect } from 'react';
import { Plus, Edit, Trash2, Eye, Save, X, AlertCircle, Car, Settings, Package, Palette, Link } from 'lucide-react';
import adminApiService from '../../services/adminApiService';
import SafeImage from '../SafeImage/SafeImage';
import EngineAssignment from './EngineAssignment';
import { translateAccessoryType, translateInteriorEquipmentType, translateAccessoryCategory } from '../../utils/translations';
import styles from './CarAdminPanel.module.css';

const CarAdminPanel = () => {
  const [activeTab, setActiveTab] = useState('models');
  const [cars, setCars] = useState([]);
  const [engines, setEngines] = useState([]);
  const [accessories, setAccessories] = useState([]);
  const [interiorEquipment, setInteriorEquipment] = useState([]);
  const [loading, setLoading] = useState(true);
  const [apiError, setApiError] = useState('');
  const [showForm, setShowForm] = useState(false);
  const [isEditing, setIsEditing] = useState(false);
  const [editingId, setEditingId] = useState(null);
  const [selectedCar, setSelectedCar] = useState(null);
  const [showDetails, setShowDetails] = useState(false);
  const [showEngineAssignment, setShowEngineAssignment] = useState(false);
  const [selectedModelForEngine, setSelectedModelForEngine] = useState(null);

  // Stan formularza dopasowany do API
  const getInitialFormData = (tab) => {
    switch (tab) {
      case 'models':
        return {
          name: '',
          productionYear: new Date().getFullYear(),
          bodyType: '',
          manufacturer: '',
          segment: '',
          basePrice: '',
          description: '',
          imageUrl: '',
          isActive: true
        };
      case 'engines':
        return {
          name: '',
          type: '',
          capacity: '',
          power: '',
          torque: '',
          fuelType: '',
          cylinders: '',
          transmission: '',
          gears: '',
          driveType: '',
          fuelConsumption: '',
          co2Emission: '',
          description: '',
          isActive: true
        };
      case 'accessories':
        return {
          carId: '',
          name: '',
          category: '',
          type: '',
          description: '',
          price: '',
          manufacturer: '',
          partNumber: '',
          isOriginalBmwPart: false,
          isInStock: true,
          stockQuantity: '',
          imageUrl: '',
          size: '',
          pattern: '',
          color: '',
          material: '',
          capacity: '',
          compatibility: '',
          ageGroup: '',
          maxLoad: '',
          isUniversal: false,
          installationDifficulty: '',
          warranty: ''
        };
      case 'interior':
        return {
          carId: '',
          type: '',
          value: '',
          description: '',
          additionalPrice: '',
          isDefault: false,
          colorCode: '',
          intensity: '',
          hasNavigation: false,
          hasPremiumSound: false,
          controlType: ''
        };
      default:
        return {
          name: '',
          productionYear: new Date().getFullYear(),
          bodyType: '',
          manufacturer: '',
          segment: '',
          basePrice: '',
          description: '',
          imageUrl: '',
          isActive: true
        };
    }
  };

  const [formData, setFormData] = useState(getInitialFormData('models'));

  const [errors, setErrors] = useState({});
  const [uploadingImage, setUploadingImage] = useState(false);
  const [pendingImageFile, setPendingImageFile] = useState(null);
  const [imagePreview, setImagePreview] = useState('');
  const [imageLoadedSuccessfully, setImageLoadedSuccessfully] = useState(false);

  // Opcje dla select-ów
  const bodyTypes = ['Sedan', 'Hatchback', 'SUV', 'Combi', 'Coupe', 'Cabrio'];
  const segments = ['A', 'B', 'C', 'D', 'E', 'F'];
  const manufacturers = ['Toyota', 'BMW', 'Mercedes', 'Audi', 'Volkswagen', 'Ford', 'Opel', 'Peugeot'];

  // Ładowanie danych z API
  const loadCarModels = async () => {
    try {
      setLoading(true);
      setApiError('');
      setCars([]); // Wyczyść przed załadowaniem
      const data = await adminApiService.getAllCarModels();
      setCars(data || []);
    } catch (error) {
      console.error('Error loading car models:', error);
      setApiError('Nie udało się załadować modeli samochodów');
      setCars([]);
    } finally {
      setLoading(false);
    }
  };

  const loadEngines = async () => {
    try {
      setLoading(true);
      setApiError('');
      setEngines([]); // Wyczyść przed załadowaniem
      const data = await adminApiService.getAllEngines();
      console.log('Loaded engines:', data);
      if (data && data.length > 0) {
        console.log('First engine ID:', data[0].id, 'type:', typeof data[0].id);
        console.log('First engine structure:', Object.keys(data[0]));
        console.log('First engine full object:', data[0]);
        
        // Sprawdź wszystkie silniki
        data.forEach((engine, index) => {
          console.log(`Engine ${index}: id="${engine.id}" (${typeof engine.id}), name="${engine.name}"`);
        });
      }
      setEngines(data || []);
    } catch (error) {
      console.error('Error loading engines:', error);
      setApiError('Nie udało się załadować silników');
      setEngines([]);
    } finally {
      setLoading(false);
    }
  };

  const loadAccessories = async () => {
    try {
      setLoading(true);
      setApiError('');
      setAccessories([]); // Wyczyść przed załadowaniem
      
      // Zawsze załaduj modele samochodów do dropdown
      const carModels = await adminApiService.getAllCarModels();
      setCars(carModels || []);
      
      const data = await adminApiService.getAllCarAccessories();
      setAccessories(data || []);
    } catch (error) {
      console.error('Error loading accessories:', error);
      setApiError('Nie udało się załadować akcesoriów');
      setAccessories([]);
    } finally {
      setLoading(false);
    }
  };

  const loadInteriorEquipment = async () => {
    try {
      setLoading(true);
      setApiError('');
      setInteriorEquipment([]); // Wyczyść przed załadowaniem
      
      // Zawsze załaduj modele samochodów do dropdown
      const carModels = await adminApiService.getAllCarModels();
      setCars(carModels || []);
      
      const data = await adminApiService.getAllCarInteriorEquipment();
      setInteriorEquipment(data || []);
    } catch (error) {
      console.error('Error loading interior equipment:', error);
      setApiError('Nie udało się załadować wyposażenia wnętrza');
      setInteriorEquipment([]);
    } finally {
      setLoading(false);
    }
  };

  const clearAllData = () => {
    setEngines([]);
    setAccessories([]);
    setInteriorEquipment([]);
    // Nie czyszczmy cars - potrzebne do dropdown w akcesoriach
  };

  const loadDataForActiveTab = () => {
    // Wyczyść dane przed załadowaniem nowych (oprócz cars)
    clearAllData();
    setApiError('');
    
    switch (activeTab) {
      case 'models':
        setCars([]); // Wyczyść cars tylko dla zakładki modeli
        loadCarModels();
        break;
      case 'engines':
        loadEngines();
        break;
      case 'accessories':
        loadAccessories();
        break;
      case 'interior':
        loadInteriorEquipment();
        break;
      default:
        setCars([]); // Wyczyść cars tylko dla domyślnej zakładki
        loadCarModels();
    }
  };

  useEffect(() => {
    // Zamknij wszystkie modalne przy zmianie zakładki
    setShowForm(false);
    setShowDetails(false);
    setShowEngineAssignment(false);
    setSelectedModelForEngine(null);
    setSelectedCar(null);
    
    // Załaduj dane dla nowej zakładki
    loadDataForActiveTab();
  }, [activeTab]);

  // Resetowanie formularza
  const resetForm = () => {
    setFormData(getInitialFormData(activeTab));
    setErrors({});
    setIsEditing(false);
    setEditingId(null);
    setPendingImageFile(null);
    setImagePreview('');
    setImageLoadedSuccessfully(false);
  };

  // Walidacja formularza
  const validateForm = () => {
    const newErrors = {};

    switch (activeTab) {
      case 'models':
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
        break;

      case 'engines':
        if (!formData.name.trim()) {
          newErrors.name = 'Nazwa silnika jest wymagana';
        }

        if (!formData.type) {
          newErrors.type = 'Typ silnika jest wymagany';
        }

        if (!formData.fuelType) {
          newErrors.fuelType = 'Rodzaj paliwa jest wymagany';
        }

        if (!formData.power || isNaN(formData.power) || parseInt(formData.power) <= 0) {
          newErrors.power = 'Moc musi być liczbą większą od 0';
        }

        if (formData.capacity && (isNaN(formData.capacity) || parseFloat(formData.capacity) < 0)) {
          newErrors.capacity = 'Pojemność musi być liczbą większą lub równą 0';
        }

        if (formData.cylinders && (isNaN(formData.cylinders) || parseInt(formData.cylinders) < 1)) {
          newErrors.cylinders = 'Liczba cylindrów musi być liczbą większą od 0';
        }
        break;

      case 'accessories':
        if (!formData.carId) {
          newErrors.carId = 'ID samochodu jest wymagane';
        }

        if (!formData.name.trim()) {
          newErrors.name = 'Nazwa akcesoria jest wymagana';
        }

        if (!formData.category) {
          newErrors.category = 'Kategoria jest wymagana';
        }

        if (!formData.type) {
          newErrors.type = 'Typ akcesoria jest wymagany';
        }

        if (!formData.price || isNaN(formData.price) || parseFloat(formData.price) < 0) {
          newErrors.price = 'Cena musi być liczbą większą lub równą 0';
        }

        if (!formData.stockQuantity || isNaN(formData.stockQuantity) || parseInt(formData.stockQuantity) < 0) {
          newErrors.stockQuantity = 'Ilość w magazynie musi być liczbą większą lub równą 0';
        }
        break;

      case 'interior':
        if (!formData.carId) {
          newErrors.carId = 'ID samochodu jest wymagane';
        }

        if (!formData.type) {
          newErrors.type = 'Typ wyposażenia jest wymagany';
        }

        if (!formData.value.trim()) {
          newErrors.value = 'Wartość jest wymagana';
        }

        if (!formData.additionalPrice || isNaN(formData.additionalPrice) || parseFloat(formData.additionalPrice) < 0) {
          newErrors.additionalPrice = 'Cena dodatkowa jest wymagana i musi być liczbą większą lub równą 0';
        }

        // Walidacja specyficzna dla typu
        if (formData.type === 'SeatColor' || formData.type === 'AmbientLighting') {
          if (!formData.colorCode || !/^#[0-9A-Fa-f]{6}$/.test(formData.colorCode)) {
            newErrors.colorCode = 'Kod koloru jest wymagany w formacie #RRGGBB';
          }
        }

        if (formData.type === 'AmbientLighting') {
          if (!formData.intensity || isNaN(formData.intensity) || parseInt(formData.intensity) < 1 || parseInt(formData.intensity) > 10) {
            newErrors.intensity = 'Intensywność musi być liczbą od 1 do 10';
          }
        }

        if (formData.type === 'CruiseControl') {
          if (!formData.controlType) {
            newErrors.controlType = 'Typ sterowania jest wymagany dla tempomatu';
          }
        }
        break;

      default:
        if (!formData.name.trim()) {
          newErrors.name = 'Nazwa jest wymagana';
        }
        break;
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

  // Obsługa uploadu zdjęcia - przygotowanie do późniejszego wysłania
  const handleImageUpload = (e) => {
    const file = e.target.files[0];
    if (!file) return;

    // Walidacja pliku
    if (!file.type.startsWith('image/')) {
      setApiError('Proszę wybrać plik obrazu (JPG, PNG, GIF, WebP)');
      e.target.value = '';
      return;
    }
    
    if (file.size > 10 * 1024 * 1024) {
      setApiError('Plik jest za duży. Maksymalny rozmiar to 10MB.');
      e.target.value = '';
      return;
    }

    // Zapisz plik do późniejszego wysłania
    setPendingImageFile(file);
    
    // Utwórz podgląd
    const reader = new FileReader();
    reader.onload = (event) => {
      setImagePreview(event.target.result);
      setImageLoadedSuccessfully(true); // Nowe zdjęcie zawsze jest dostępne
    };
    reader.readAsDataURL(file);
  };

  // Usuwanie zdjęcia
  const handleRemoveImage = () => {
    setPendingImageFile(null);
    setImagePreview('');
    setImageLoadedSuccessfully(false);
    setFormData(prev => ({
      ...prev,
      imageUrl: ''
    }));
  };

  // Dodawanie nowego elementu
  const handleAddItem = () => {
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
    setPendingImageFile(null);
    setImagePreview('');
    setImageLoadedSuccessfully(false);
  };

  // Zapisywanie formularza
  const handleSubmit = async (e) => {
    e.preventDefault();
    
    if (!validateForm()) {
      return;
    }

    try {
      setLoading(true);
      setUploadingImage(true);
      
      let imageUrl = formData.imageUrl;
      
      // Jeśli jest nowe zdjęcie do przesłania
      if (pendingImageFile) {
        try {
          const result = await adminApiService.uploadImage(pendingImageFile);
          imageUrl = result.fileName;
          
          // Usuń stare zdjęcie jeśli istnieje i jest inne niż nowe
          if (formData.imageUrl && formData.imageUrl !== imageUrl) {
            try {
              await adminApiService.deleteImage(formData.imageUrl);
            } catch (deleteError) {
              console.warn('Could not delete old image:', deleteError);
              // Nie przerywamy procesu jeśli nie udało się usunąć starego zdjęcia
            }
          }
        } catch (uploadError) {
          setApiError('Nie udało się przesłać zdjęcia');
          setUploadingImage(false);
          setLoading(false);
          return;
        }
      }

      const carData = {
        ...formData,
        imageUrl,
        basePrice: parseFloat(formData.basePrice),
        productionYear: parseInt(formData.productionYear)
      };

      let result;
      switch (activeTab) {
        case 'models':
          if (isEditing) {
            result = await adminApiService.updateCarModel(editingId, carData);
          } else {
            result = await adminApiService.createCarModel(carData);
          }
          await loadCarModels();
          break;
        case 'engines':
          const engineData = {
            ...formData,
            capacity: formData.capacity ? parseInt(formData.capacity) : null,
            power: parseInt(formData.power) || 0,
            torque: parseInt(formData.torque) || 0,
            cylinders: formData.cylinders ? parseInt(formData.cylinders) : null,
            gears: parseInt(formData.gears) || 0,
            fuelConsumption: formData.fuelConsumption ? parseFloat(formData.fuelConsumption) : 0,
            co2Emission: formData.co2Emission ? parseInt(formData.co2Emission) : 0
          };
          if (isEditing) {
            console.log('Updating engine with editingId:', editingId, 'and data:', engineData);
            result = await adminApiService.updateEngine(editingId, engineData);
          } else {
            console.log('Creating new engine with data:', engineData);
            result = await adminApiService.createEngine(engineData);
          }
          await loadEngines();
          break;
        case 'accessories':
          const accessoryData = {
            ...formData,
            imageUrl,
            price: parseFloat(formData.price) || 0,
            stockQuantity: parseInt(formData.stockQuantity) || 0,
            capacity: formData.capacity ? parseInt(formData.capacity) : null,
            maxLoad: formData.maxLoad ? parseInt(formData.maxLoad) : null
          };
          if (isEditing) {
            result = await adminApiService.updateCarAccessory(editingId, accessoryData);
          } else {
            result = await adminApiService.createCarAccessory(accessoryData);
          }
          await loadAccessories();
          break;
        case 'interior':
          const interiorData = {
            ...formData,
            additionalPrice: parseFloat(formData.additionalPrice) || 0
          };
          if (isEditing) {
            result = await adminApiService.updateCarInteriorEquipment(editingId, interiorData);
          } else {
            result = await adminApiService.createCarInteriorEquipment(interiorData);
          }
          await loadInteriorEquipment();
          break;
        default:
          if (isEditing) {
            result = await adminApiService.updateCarModel(editingId, carData);
          } else {
            result = await adminApiService.createCarModel(carData);
          }
          await loadCarModels();
      }
      setShowForm(false);
      resetForm();
    } catch (error) {
      console.error('Error saving car model:', error);
      setApiError(`Nie udało się ${isEditing ? 'zaktualizować' : 'dodać'} modelu`);
    } finally {
      setLoading(false);
      setUploadingImage(false);
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
  const handleViewDetails = (item) => {
    setSelectedCar(item);
    setShowDetails(true);
    setShowForm(false);
  };

  // Funkcje obsługi dla silników
  const handleEditEngine = (engine) => {
    console.log('handleEditEngine called with engine:', engine);
    console.log('Engine ID:', engine.id);
    setFormData({
      name: engine.name,
      type: engine.type,
      capacity: engine.capacity?.toString() || '',
      power: engine.power?.toString() || '',
      torque: engine.torque?.toString() || '',
      fuelType: engine.fuelType,
      cylinders: engine.cylinders?.toString() || '',
      transmission: engine.transmission,
      gears: engine.gears?.toString() || '',
      driveType: engine.driveType,
      fuelConsumption: engine.fuelConsumption?.toString() || '',
      co2Emission: engine.co2Emission?.toString() || '',
      description: engine.description,
      isActive: engine.isActive
    });
    setIsEditing(true);
    setEditingId(engine.id);
    console.log('Set editingId to:', engine.id);
    setShowForm(true);
    setShowDetails(false);
  };

  const handleDeleteEngine = async (id) => {
    console.log('handleDeleteEngine called with id:', id, 'type:', typeof id);
    if (!id) {
      console.error('ID is undefined or empty!');
      setApiError('Nie udało się usunąć silnika: Engine ID is required for deletion');
      return;
    }
    if (!window.confirm('Czy na pewno chcesz usunąć ten silnik?')) {
      return;
    }
    try {
      setLoading(true);
      console.log('About to call adminApiService.deleteEngine with id:', id);
      await adminApiService.deleteEngine(id);
      await loadEngines();
    } catch (error) {
      console.error('Error deleting engine:', error);
      setApiError('Nie udało się usunąć silnika: ' + (error.response?.data?.message || error.message));
    } finally {
      setLoading(false);
    }
  };

  // Funkcje obsługi dla akcesoriów
  const handleEditAccessory = (accessory) => {
    setFormData({
      carId: accessory.carId || '',
      name: accessory.name,
      category: accessory.category,
      type: accessory.type,
      description: accessory.description,
      price: accessory.price?.toString() || '',
      manufacturer: accessory.manufacturer,
      partNumber: accessory.partNumber,
      isOriginalBmwPart: accessory.isOriginalBmwPart,
      isInStock: accessory.isInStock,
      stockQuantity: accessory.stockQuantity?.toString() || '',
      imageUrl: accessory.imageUrl || '',
      size: accessory.size,
      pattern: accessory.pattern,
      color: accessory.color,
      material: accessory.material,
      capacity: accessory.capacity?.toString() || '',
      compatibility: accessory.compatibility,
      ageGroup: accessory.ageGroup,
      maxLoad: accessory.maxLoad?.toString() || '',
      isUniversal: accessory.isUniversal,
      installationDifficulty: accessory.installationDifficulty,
      warranty: accessory.warranty
    });
    setIsEditing(true);
    setEditingId(accessory.id);
    setShowForm(true);
    setShowDetails(false);
  };

  const handleDeleteAccessory = async (id) => {
    if (!window.confirm('Czy na pewno chcesz usunąć to akcesorium?')) {
      return;
    }
    try {
      setLoading(true);
      await adminApiService.deleteCarAccessory(id);
      await loadAccessories();
    } catch (error) {
      console.error('Error deleting accessory:', error);
      setApiError('Nie udało się usunąć akcesorium');
    }
  };

  // Funkcje obsługi dla wyposażenia wnętrza
  const handleEditInterior = (interior) => {
    setFormData({
      carId: interior.carId || '',
      type: interior.type,
      value: interior.value,
      description: interior.description,
      additionalPrice: interior.additionalPrice?.toString() || '',
      isDefault: interior.isDefault || false,
      colorCode: interior.colorCode || '',
      intensity: interior.intensity?.toString() || '',
      hasNavigation: interior.hasNavigation || false,
      hasPremiumSound: interior.hasPremiumSound || false,
      controlType: interior.controlType || ''
    });
    setIsEditing(true);
    setEditingId(interior.id);
    setShowForm(true);
    setShowDetails(false);
  };

  const handleDeleteInterior = async (id) => {
    if (!window.confirm('Czy na pewno chcesz usunąć to wyposażenie wnętrza?')) {
      return;
    }
    try {
      setLoading(true);
      await adminApiService.deleteCarInteriorEquipment(id);
      await loadInteriorEquipment();
    } catch (error) {
      console.error('Error deleting interior equipment:', error);
      setApiError('Nie udało się usunąć wyposażenia wnętrza');
    }
  };

  // Funkcja zarządzania silnikami dla modelu
  const handleManageEngines = (model) => {
    setSelectedModelForEngine(model);
    setShowEngineAssignment(true);
    setShowForm(false);
    setShowDetails(false);
  };

  const tabs = [
    { id: 'models', label: 'Modele Samochodów', icon: Car },
    { id: 'engines', label: 'Silniki', icon: Settings },
    { id: 'accessories', label: 'Akcesoria', icon: Package },
    { id: 'interior', label: 'Wyposażenie Wnętrza', icon: Palette }
  ];

  const renderDataTable = () => {
    let data, headers, emptyMessage;
    
    switch (activeTab) {
      case 'models':
        data = cars || [];
        headers = ['Nazwa', 'Producent', 'Typ', 'Rok', 'Cena', 'Status', 'Akcje'];
        emptyMessage = 'Brak modeli';
        break;
      case 'engines':
        data = engines || [];
        headers = ['Nazwa', 'Typ', 'Pojemność', 'Moc', 'Paliwo', 'Status', 'Akcje'];
        emptyMessage = 'Brak silników';
        break;
      case 'accessories':
        data = accessories || [];
        headers = ['Nazwa', 'Kategoria', 'Typ', 'Cena', 'Magazyn', 'Status', 'Akcje'];
        emptyMessage = 'Brak akcesoriów';
        break;
      case 'interior':
        data = interiorEquipment || [];
        headers = ['Typ', 'Wartość', 'Cena dodatkowa', 'Nawigacja', 'Premium Audio', 'Akcje'];
        emptyMessage = 'Brak wyposażenia wnętrza';
        break;
      default:
        data = cars || [];
        headers = ['Nazwa', 'Producent', 'Typ', 'Rok', 'Cena', 'Status', 'Akcje'];
        emptyMessage = 'Brak danych';
    }


    return (
      <div className={styles.tableContainer}>
        <table className={styles.table}>
          <thead>
            <tr>
              {headers.map(header => (
                <th key={header}>{header}</th>
              ))}
            </tr>
          </thead>
          <tbody>
            {loading ? (
              <tr>
                <td colSpan={headers.length} className={styles.loading}>Ładowanie...</td>
              </tr>
            ) : data.length === 0 ? (
              <tr>
                <td colSpan={headers.length} className={styles.noData}>{emptyMessage}</td>
              </tr>
            ) : (
              data.map(item => renderTableRow(item))
            )}
          </tbody>
        </table>
      </div>
    );
  };

  const renderTableRow = (item) => {
    const key = item.id || `${activeTab}-${Math.random()}`;
    
    switch (activeTab) {
      case 'models':
        return (
          <tr key={key}>
            <td>{item.name}</td>
            <td>{item.manufacturer}</td>
            <td>{item.bodyType}</td>
            <td>{item.productionYear}</td>
            <td>{item.basePrice?.toLocaleString('pl-PL')} PLN</td>
            <td>
              <span className={item.isActive ? styles.active : styles.inactive}>
                {item.isActive ? 'Aktywny' : 'Nieaktywny'}
              </span>
            </td>
            <td className={styles.actions}>
              <button onClick={() => handleViewDetails(item)} className={styles.viewButton} title="Zobacz szczegóły">
                <Eye size={16} />
              </button>
              <button onClick={() => handleEditCar(item)} className={styles.editButton} title="Edytuj">
                <Edit size={16} />
              </button>
              <button onClick={() => handleManageEngines(item)} className={styles.linkButton} title="Zarządzaj silnikami">
                <Link size={16} />
              </button>
              <button onClick={() => handleDeleteCar(item.id)} className={styles.deleteButton} title="Usuń">
                <Trash2 size={16} />
              </button>
            </td>
          </tr>
        );
      case 'engines':
        console.log('Rendering engine row, item:', item, 'item.id:', item.id);
        return (
          <tr key={key}>
            <td>{item.name}</td>
            <td>{item.type}</td>
            <td>{item.capacity ? `${item.capacity} cm³` : 'N/A'}</td>
            <td>{item.power} KM</td>
            <td>{item.fuelType}</td>
            <td>
              <span className={item.isActive ? styles.active : styles.inactive}>
                {item.isActive ? 'Aktywny' : 'Nieaktywny'}
              </span>
            </td>
            <td className={styles.actions}>
              <button onClick={() => handleViewDetails(item)} className={styles.viewButton} title="Zobacz szczegóły">
                <Eye size={16} />
              </button>
              <button onClick={() => handleEditEngine(item)} className={styles.editButton} title="Edytuj">
                <Edit size={16} />
              </button>
              <button onClick={() => {
                console.log('Delete button clicked, item.id:', item.id);
                handleDeleteEngine(item.id);
              }} className={styles.deleteButton} title="Usuń">
                <Trash2 size={16} />
              </button>
            </td>
          </tr>
        );
      case 'accessories':
        return (
          <tr key={key}>
            <td>{item.name}</td>
            <td>{translateAccessoryCategory(item.category)}</td>
            <td>{translateAccessoryType(item.type)}</td>
            <td>{item.price?.toLocaleString('pl-PL')} PLN</td>
            <td>
              <span className={item.isInStock ? styles.active : styles.inactive}>
                {item.isInStock ? `${item.stockQuantity} szt.` : 'Brak'}
              </span>
            </td>
            <td>
              <span className={item.isInStock ? styles.active : styles.inactive}>
                {item.isInStock ? 'Dostępne' : 'Niedostępne'}
              </span>
            </td>
            <td className={styles.actions}>
              <button onClick={() => handleViewDetails(item)} className={styles.viewButton} title="Zobacz szczegóły">
                <Eye size={16} />
              </button>
              <button onClick={() => handleEditAccessory(item)} className={styles.editButton} title="Edytuj">
                <Edit size={16} />
              </button>
              <button onClick={() => handleDeleteAccessory(item.id)} className={styles.deleteButton} title="Usuń">
                <Trash2 size={16} />
              </button>
            </td>
          </tr>
        );
      case 'interior':
        return (
          <tr key={key}>
            <td>{translateInteriorEquipmentType(item.type)}</td>
            <td>{item.value}</td>
            <td>{item.additionalPrice?.toLocaleString('pl-PL')} PLN</td>
            <td>
              <span className={item.hasNavigation ? styles.active : styles.inactive}>
                {item.hasNavigation ? 'Tak' : 'Nie'}
              </span>
            </td>
            <td>
              <span className={item.hasPremiumSound ? styles.active : styles.inactive}>
                {item.hasPremiumSound ? 'Tak' : 'Nie'}
              </span>
            </td>
            <td className={styles.actions}>
              <button onClick={() => handleViewDetails(item)} className={styles.viewButton} title="Zobacz szczegóły">
                <Eye size={16} />
              </button>
              <button onClick={() => handleEditInterior(item)} className={styles.editButton} title="Edytuj">
                <Edit size={16} />
              </button>
              <button onClick={() => handleDeleteInterior(item.id)} className={styles.deleteButton} title="Usuń">
                <Trash2 size={16} />
              </button>
            </td>
          </tr>
        );
      default:
        return null;
    }
  };

  // Funkcja renderowania formularzy
  const renderForm = () => {
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

      <div className={styles.formGroup}>
        <label>Zdjęcie samochodu</label>
        <div className={styles.imageUploadContainer}>
          {imagePreview ? (
            <div className={styles.imagePreview}>
              <img 
                src={imagePreview} 
                alt="Podgląd nowego zdjęcia" 
                className={styles.previewImage}
              />
              <button 
                type="button" 
                onClick={handleRemoveImage}
                className={styles.removeImageButton}
              >
                Usuń zdjęcie
              </button>
              <p className={styles.pendingUploadInfo}>
                Nowe zdjęcie zostanie przesłane po zapisaniu formularza
              </p>
            </div>
          ) : formData.imageUrl ? (
            <div className={styles.imagePreview}>
              <SafeImage 
                src={adminApiService.getImageUrl(formData.imageUrl)} 
                alt="Aktualne zdjęcie" 
                className={styles.previewImage}
                onImageStatusChange={setImageLoadedSuccessfully}
              />
              {imageLoadedSuccessfully && (
                <button 
                  type="button" 
                  onClick={handleRemoveImage}
                  className={styles.removeImageButton}
                >
                  Usuń zdjęcie
                </button>
              )}
            </div>
          ) : null}
          
          <div className={styles.uploadArea} style={{ 
            marginTop: (imagePreview || (formData.imageUrl && imageLoadedSuccessfully)) ? '1rem' : '0'
          }}>
            <input
              type="file"
              accept="image/*"
              onChange={handleImageUpload}
              className={styles.fileInput}
            />
            <p>
              {(imagePreview || (formData.imageUrl && imageLoadedSuccessfully)) 
                ? 'Dodaj nowe zdjęcie' 
                : 'Dodaj zdjęcie'
              }
            </p>
          </div>
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
            <option value="Petrol">Benzyna</option>
            <option value="Diesel">Diesel</option>
            <option value="Electric">Elektryczny</option>
            <option value="Hybrid">Hybryda</option>
            <option value="PlugInHybrid">Hybryda Plug-in</option>
          </select>
          {errors.type && <span className={styles.errorText}>{errors.type}</span>}
        </div>

        <div className={styles.formGroup}>
          <label>Pojemność (cm³)</label>
          <input
            type="number"
            name="capacity"
            value={formData.capacity}
            onChange={handleInputChange}
            min="0"
            step="100"
            placeholder="np. 2000 dla 2.0L"
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
            <option value="Petrol">Benzyna</option>
            <option value="Diesel">Diesel</option>
            <option value="Electric">Elektryczny</option>
            <option value="Hybrid">Hybryda</option>
            <option value="PlugInHybrid">Hybryda Plug-in</option>
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

  const renderAccessoriesForm = () => (
    <>
      <div className={styles.formGrid}>
        <div className={styles.formGroup}>
          <label>ID samochodu *</label>
          <select
            name="carId"
            value={formData.carId}
            onChange={handleInputChange}
            className={errors.carId ? styles.errorInput : ''}
          >
            <option value="">Wybierz model samochodu</option>
            {cars.map(car => (
              <option key={car.id} value={car.id}>{car.name} - {car.manufacturer}</option>
            ))}
          </select>
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
            {['Wheels', 'Exterior', 'Interior', 'Electronics', 'Transport', 'Safety', 'Seasonal', 'Connectivity', 'Family'].map(category => (
              <option key={category} value={category}>{translateAccessoryCategory(category)}</option>
            ))}
          </select>
          {errors.category && <span className={styles.errorText}>{errors.category}</span>}
        </div>

        <div className={styles.formGroup}>
          <label>Typ *</label>
          <select
            name="type"
            value={formData.type}
            onChange={handleInputChange}
            className={errors.type ? styles.errorInput : ''}
          >
            <option value="">Wybierz typ</option>
            {[
              'AlloyWheels', 'TintedWindows', 'DecorativeStrips', 'Spoilers', 'SillCovers',
              'LicensePlateFrames', 'FloorMats', 'PhoneTabletHolder', 'AmbientLighting',
              'SeatCovers', 'TrunkBags', 'RoofRacks', 'RoofBoxes', 'BikeCarriers', 'SkiCarriers',
              'WirelessChargers', 'PowerBanks', 'RearViewCameras', 'AdditionalSensors',
              'SnowChains', 'USBCAdapters', 'RearSeatChargers', 'Navigation', 'AppleCarPlay',
              'AndroidAuto', 'WiFiModules', 'Hotspots', 'ChildSeats', 'SeatProtectiveMats', 'CargoNets'
            ].map(type => (
              <option key={type} value={type}>{translateAccessoryType(type)}</option>
            ))}
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
            step="10"
            className={errors.price ? styles.errorInput : ''}
          />
          {errors.price && <span className={styles.errorText}>{errors.price}</span>}
        </div>

        <div className={styles.formGroup}>
          <label>Producent</label>
          <input
            type="text"
            name="manufacturer"
            value={formData.manufacturer}
            onChange={handleInputChange}
            className={errors.manufacturer ? styles.errorInput : ''}
          />
          {errors.manufacturer && <span className={styles.errorText}>{errors.manufacturer}</span>}
        </div>

        <div className={styles.formGroup}>
          <label>Numer części</label>
          <input
            type="text"
            name="partNumber"
            value={formData.partNumber}
            onChange={handleInputChange}
            className={errors.partNumber ? styles.errorInput : ''}
          />
          {errors.partNumber && <span className={styles.errorText}>{errors.partNumber}</span>}
        </div>

        <div className={styles.formGroup}>
          <label>Ilość w magazynie *</label>
          <input
            type="number"
            name="stockQuantity"
            value={formData.stockQuantity}
            onChange={handleInputChange}
            min="0"
            className={errors.stockQuantity ? styles.errorInput : ''}
          />
          {errors.stockQuantity && <span className={styles.errorText}>{errors.stockQuantity}</span>}
        </div>

        <div className={styles.formGroup}>
          <label>Kompatybilność</label>
          <input
            type="text"
            name="compatibility"
            value={formData.compatibility}
            onChange={handleInputChange}
            className={errors.compatibility ? styles.errorInput : ''}
          />
          {errors.compatibility && <span className={styles.errorText}>{errors.compatibility}</span>}
        </div>
      </div>

      {/* Conditional fields for AlloyWheels */}
      {formData.type === 'AlloyWheels' && (
        <>
          <div className={styles.formRow}>
            <div className={styles.formGroup}>
              <label>Rozmiar felg*</label>
              <select
                name="size"
                value={formData.size}
                onChange={handleInputChange}
                className={errors.size ? styles.errorInput : ''}
              >
                <option value="">Wybierz rozmiar</option>
                <option value="17">17"</option>
                <option value="18">18"</option>
                <option value="19">19"</option>
                <option value="20">20"</option>
              </select>
              {errors.size && <span className={styles.errorText}>{errors.size}</span>}
            </div>

            <div className={styles.formGroup}>
              <label>Wzór felg*</label>
              <input
                type="text"
                name="pattern"
                value={formData.pattern}
                onChange={handleInputChange}
                placeholder="np. M Double-spoke"
                className={errors.pattern ? styles.errorInput : ''}
              />
              {errors.pattern && <span className={styles.errorText}>{errors.pattern}</span>}
            </div>
          </div>
        </>
      )}

      {/* Conditional fields for FloorMats */}
      {formData.type === 'FloorMats' && (
        <div className={styles.formGroup}>
          <label>Materiał*</label>
          <select
            name="material"
            value={formData.material}
            onChange={handleInputChange}
            className={errors.material ? styles.errorInput : ''}
          >
            <option value="">Wybierz materiał</option>
            <option value="Welur">Welur</option>
            <option value="Guma">Guma</option>
            <option value="Tworzywo">Tworzywo</option>
          </select>
          {errors.material && <span className={styles.errorText}>{errors.material}</span>}
        </div>
      )}

      {/* Conditional fields for RoofBoxes */}
      {formData.type === 'RoofBoxes' && (
        <div className={styles.formRow}>
          <div className={styles.formGroup}>
            <label>Pojemność (L)*</label>
            <input
              type="number"
              name="capacity"
              value={formData.capacity}
              onChange={handleInputChange}
              placeholder="np. 420"
              className={errors.capacity ? styles.errorInput : ''}
            />
            {errors.capacity && <span className={styles.errorText}>{errors.capacity}</span>}
          </div>

          <div className={styles.formGroup}>
            <label>Maksymalne obciążenie (kg)*</label>
            <input
              type="number"
              name="maxLoad"
              value={formData.maxLoad}
              onChange={handleInputChange}
              placeholder="np. 75"
              className={errors.maxLoad ? styles.errorInput : ''}
            />
            {errors.maxLoad && <span className={styles.errorText}>{errors.maxLoad}</span>}
          </div>
        </div>
      )}

      {/* Conditional fields for ChildSeats */}
      {formData.type === 'ChildSeats' && (
        <div className={styles.formRow}>
          <div className={styles.formGroup}>
            <label>Grupa wiekowa*</label>
            <input
              type="text"
              name="ageGroup"
              value={formData.ageGroup}
              onChange={handleInputChange}
              placeholder="np. 0-13 kg, 9-36 kg"
              className={errors.ageGroup ? styles.errorInput : ''}
            />
            {errors.ageGroup && <span className={styles.errorText}>{errors.ageGroup}</span>}
          </div>

          <div className={styles.formGroup}>
            <label>Maksymalna waga dziecka (kg)*</label>
            <input
              type="number"
              name="maxLoad"
              value={formData.maxLoad}
              onChange={handleInputChange}
              placeholder="np. 36"
              className={errors.maxLoad ? styles.errorInput : ''}
            />
            {errors.maxLoad && <span className={styles.errorText}>{errors.maxLoad}</span>}
          </div>
        </div>
      )}

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
          W magazynie
        </label>
      </div>

      <div className={styles.formGroup}>
        <label className={styles.checkboxLabel}>
          <input
            type="checkbox"
            name="isUniversal"
            checked={formData.isUniversal}
            onChange={handleInputChange}
          />
          Uniwersalne
        </label>
      </div>
    </>
  );

  const renderInteriorForm = () => (
    <>
      <div className={styles.formGrid}>
        <div className={styles.formGroup}>
          <label>ID samochodu *</label>
          <select
            name="carId"
            value={formData.carId}
            onChange={handleInputChange}
            className={errors.carId ? styles.errorInput : ''}
          >
            <option value="">Wybierz model samochodu</option>
            {cars.map(car => (
              <option key={car.id} value={car.id}>{car.name} - {car.manufacturer}</option>
            ))}
          </select>
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
            {[
              'SeatColor', 'SeatHeating', 'AdjustableHeadrests', 'MultifunctionSteeringWheel',
              'RadioType', 'AmbientLighting', 'CruiseControl', 'ElectricMirrors'
            ].map(type => (
              <option key={type} value={type}>{translateInteriorEquipmentType(type)}</option>
            ))}
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
            step="100"
            className={errors.additionalPrice ? styles.errorInput : ''}
          />
          {errors.additionalPrice && <span className={styles.errorText}>{errors.additionalPrice}</span>}
        </div>

        {/* Pole koloru dla SeatColor i AmbientLighting */}
        {(formData.type === 'SeatColor' || formData.type === 'AmbientLighting') && (
          <div className={styles.formGroup}>
            <label>Kod koloru *</label>
            <input
              type="color"
              name="colorCode"
              value={formData.colorCode}
              onChange={handleInputChange}
              className={errors.colorCode ? styles.errorInput : ''}
            />
            {errors.colorCode && <span className={styles.errorText}>{errors.colorCode}</span>}
          </div>
        )}

        {/* Pole intensywności dla AmbientLighting */}
        {formData.type === 'AmbientLighting' && (
          <div className={styles.formGroup}>
            <label>Intensywność (1-10) *</label>
            <input
              type="number"
              name="intensity"
              value={formData.intensity}
              onChange={handleInputChange}
              min="1"
              max="10"
              className={errors.intensity ? styles.errorInput : ''}
            />
            {errors.intensity && <span className={styles.errorText}>{errors.intensity}</span>}
          </div>
        )}

        {/* Pola dla RadioType */}
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
                Zawiera nawigację
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
                Premium audio
              </label>
            </div>
          </>
        )}

        {/* Pole typu sterowania dla CruiseControl */}
        {formData.type === 'CruiseControl' && (
          <div className={styles.formGroup}>
            <label>Typ sterowania *</label>
            <select
              name="controlType"
              value={formData.controlType}
              onChange={handleInputChange}
              className={errors.controlType ? styles.errorInput : ''}
            >
              <option value="">Wybierz typ sterowania</option>
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

  return (
    <div className={styles.adminContainer}>
      <header className={styles.header}>
        <h1>Panel Administratora</h1>
        
        {/* Zakładki */}
        <div className={styles.tabsContainer}>
          {tabs.map(tab => {
            const Icon = tab.icon;
            return (
              <button
                key={tab.id}
                onClick={() => setActiveTab(tab.id)}
                className={`${styles.tab} ${activeTab === tab.id ? styles.activeTab : ''}`}
              >
                <Icon size={18} />
                {tab.label}
              </button>
            );
          })}
        </div>
        
        <button 
          onClick={handleAddItem}
          className={styles.addButton}
        >
          <Plus size={20} />
          {activeTab === 'models' && 'Dodaj Model'}
          {activeTab === 'engines' && 'Dodaj Silnik'}
          {activeTab === 'accessories' && 'Dodaj Akcesorium'}
          {activeTab === 'interior' && 'Dodaj Wyposażenie'}
        </button>
        
        {activeTab === 'engines' && (
          <button 
            onClick={async () => {
              console.log('=== DEBUG: Testing direct API call ===');
              try {
                const response = await fetch('https://localhost:7020/api/engines');
                const data = await response.json();
                console.log('Raw API response:', data);
                console.log('Response status:', response.status);
                if (data && data.length > 0) {
                  console.log('First engine from API:', data[0]);
                  console.log('Keys in first engine:', Object.keys(data[0]));
                }
              } catch (error) {
                console.error('Direct API call failed:', error);
              }
            }}
            className={styles.addButton}
            style={{marginLeft: '10px'}}
          >
            DEBUG API
          </button>
        )}
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
            <h2>
              {isEditing ? 'Edytuj ' : 'Dodaj '}
              {activeTab === 'models' && (isEditing ? 'Model' : 'Nowy Model')}
              {activeTab === 'engines' && (isEditing ? 'Silnik' : 'Nowy Silnik')}
              {activeTab === 'accessories' && (isEditing ? 'Akcesorium' : 'Nowe Akcesorium')}
              {activeTab === 'interior' && (isEditing ? 'Wyposażenie' : 'Nowe Wyposażenie')}
            </h2>
            <button 
              onClick={() => setShowForm(false)}
              className={styles.closeButton}
            >
              <X size={20} />
            </button>
          </div>

          <form onSubmit={handleSubmit} className={styles.form}>
            {renderForm()}

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
      {renderDataTable()}

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
              
              <div className={styles.imageContainer}>
                <SafeImage 
                  src={selectedCar.imageUrl ? adminApiService.getImageUrl(selectedCar.imageUrl) : null}
                  alt={selectedCar.name}
                  className={styles.carImage}
                  showFallback={true}
                />
              </div>
              
              <div className={styles.description}>
                <strong>Opis:</strong>
                <p>{selectedCar.description}</p>
              </div>
            </div>
          </div>
        </div>
      )}

      {/* Modal zarządzania silnikami */}
      {showEngineAssignment && selectedModelForEngine && (
        <EngineAssignment 
          model={selectedModelForEngine}
          onClose={() => {
            setShowEngineAssignment(false);
            setSelectedModelForEngine(null);
          }}
        />
      )}
    </div>
  );
};

export default CarAdminPanel;