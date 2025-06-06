// CarAdminPanel.jsx
import { useState, useEffect } from 'react';
import CarHeader from '../CarHeader/CarHeader';
import CarForm from '../CarFrom/CarForm';
import CarTable from '../CarTable/CarTable';
import CarDetailsModal from '../CarDetailModal/CarDetailsModal';
import adminApiService from '../../services/adminApiService';
import styles from './CarAdminPanel.module.css';

const CarAdminPanel = () => {
  // Stan dla listy samochodów
  const [cars, setCars] = useState([]);
  const [loading, setLoading] = useState(true);
  const [apiError, setApiError] = useState('');

  // Stan dla formularza - dopasowany do struktury API
  const [formData, setFormData] = useState({
    name: '',
    productionYear: '',
    bodyType: '',
    manufacturer: '',
    segment: '',
    basePrice: '',
    description: '',
    imageUrl: '',
    isActive: true
  });

  // Lista dostępnych cech
  const dostepneCechy = [
    'Eco',
    'Automatyczna skrzynia',
    'Napęd 4×4',
    'Zero emisji',
    'Klimatyzacja',
    'Podgrzewane fotele',
    'System nawigacji',
    'Kamera cofania'
  ];

  // Stan dla dodawania własnych cech
  const [nowaCecha, setNowaCecha] = useState('');

  // Stan dla trybu edycji
  const [isEditing, setIsEditing] = useState(false);
  const [editingId, setEditingId] = useState(null);
  const [showForm, setShowForm] = useState(false);
  const [showDetails, setShowDetails] = useState(false);
  const [selectedCar, setSelectedCar] = useState(null);

  // Stan dla błędów walidacji
  const [errors, setErrors] = useState({});

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

  // Ładowanie danych przy starcie komponentu
  useEffect(() => {
    loadCarModels();
  }, []);

  // Resetowanie formularza
  const resetForm = () => {
    setFormData({
      name: '',
      model: '',
      rok: '',
      cena: '',
      kolorNadwozia: '',
      wyposazenieWnetrza: '',
      opis: '',
      ma4x4: false,
      jestElektryczny: false,
      akcesoria: '',
      zdjecie: '',
      pojemnosc: '',
      typ: '',
      moc: '',
      cechy: [],
      wlasneCechy: []
    });
    setNowaCecha('');
    setErrors({});
    setIsEditing(false);
    setEditingId(null);
    setShowForm(false);
    setShowDetails(false);
    setSelectedCar(null);
  };

  // Walidacja formularza
  const validateForm = () => {
    const newErrors = {};

    if (!formData.marka.trim()) {
      newErrors.marka = 'Marka jest wymagana';
    }

    if (!formData.model.trim()) {
      newErrors.model = 'Model jest wymagany';
    }

    if (!formData.rok) {
      newErrors.rok = 'Rok jest wymagany';
    } else if (isNaN(formData.rok) || formData.rok < 1900 || formData.rok > new Date().getFullYear() + 1) {
      newErrors.rok = 'Podaj prawidłowy rok (1900-' + (new Date().getFullYear() + 1) + ')';
    }

    if (!formData.cena) {
      newErrors.cena = 'Cena jest wymagana';
    } else if (isNaN(formData.cena) || parseFloat(formData.cena) <= 0) {
      newErrors.cena = 'Cena musi być liczbą większą od 0';
    }

    if (!formData.kolorNadwozia.trim()) {
      newErrors.kolorNadwozia = 'Kolor nadwozia jest wymagany';
    }

    if (!formData.wyposazenieWnetrza.trim()) {
      newErrors.wyposazenieWnetrza = 'Wyposażenie wnętrza jest wymagane';
    }

    if (!formData.opis.trim()) {
      newErrors.opis = 'Opis jest wymagany';
    } else if (formData.opis.trim().length < 10) {
      newErrors.opis = 'Opis musi mieć co najmniej 10 znaków';
    }

    if (!formData.pojemnosc) {
      newErrors.pojemnosc = 'Pojemność silnika jest wymagana';
    } else if (isNaN(formData.pojemnosc) || parseFloat(formData.pojemnosc) <= 0) {
      newErrors.pojemnosc = 'Pojemność musi być liczbą większą od 0';
    }

    if (!formData.typ) {
      newErrors.typ = 'Typ nadwozia jest wymagany';
    }

    if (!formData.moc) {
      newErrors.moc = 'Moc silnika jest wymagana';
    } else if (isNaN(formData.moc) || parseInt(formData.moc) <= 0) {
      newErrors.moc = 'Moc musi być liczbą większą od 0';
    }

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  // Obsługa dodawania samochodu
  const handleAddCar = () => {
    setShowForm(true);
    setIsEditing(false);
    setShowDetails(false);
    resetForm();
  };

  // Obsługa edycji samochodu
  const handleEditCar = (car) => {
    setFormData({
      marka: car.marka,
      model: car.model,
      rok: car.rok.toString(),
      cena: car.cena.toString(),
      kolorNadwozia: car.kolorNadwozia,
      wyposazenieWnetrza: car.wyposazenieWnetrza,
      opis: car.opis,
      ma4x4: car.ma4x4,
      jestElektryczny: car.jestElektryczny,
      akcesoria: car.akcesoria,
      zdjecie: car.zdjecie || '',
      pojemnosc: car.pojemnosc.toString(),
      typ: car.typ,
      moc: car.moc.toString(),
      cechy: car.cechy || [],
      wlasneCechy: car.wlasneCechy || []
    });
    setIsEditing(true);
    setEditingId(car.id);
    setShowForm(true);
    setShowDetails(false);
    setErrors({});
  };

  // Obsługa usuwania samochodu
  const handleDeleteCar = (id) => {
    if (window.confirm('Czy na pewno chcesz usunąć ten samochód?')) {
      setCars(cars.filter(car => car.id !== id));
    }
  };

  // Obsługa zapisywania formularza
  const handleSubmit = () => {
    if (!validateForm()) {
      return;
    }

    const carData = {
      marka: formData.marka.trim(),
      model: formData.model.trim(),
      rok: parseInt(formData.rok),
      cena: parseFloat(formData.cena),
      kolorNadwozia: formData.kolorNadwozia.trim(),
      wyposazenieWnetrza: formData.wyposazenieWnetrza.trim(),
      opis: formData.opis.trim(),
      ma4x4: formData.ma4x4,
      jestElektryczny: formData.jestElektryczny,
      akcesoria: formData.akcesoria.trim(),
      zdjecie: formData.zdjecie.trim(),
      pojemnosc: parseFloat(formData.pojemnosc),
      typ: formData.typ,
      moc: parseInt(formData.moc),
      cechy: formData.cechy,
      wlasneCechy: formData.wlasneCechy
    };

    if (isEditing) {
      setCars(cars.map(car => 
        car.id === editingId 
          ? { ...carData, id: editingId }
          : car
      ));
    } else {
      const newId = Math.max(...cars.map(car => car.id), 0) + 1;
      setCars([...cars, { ...carData, id: newId }]);
    }

    resetForm();
  };

  // Obsługa zmian w polach formularza
  const handleInputChange = (e) => {
    const { name, value, type, checked } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: type === 'checkbox' ? checked : value
    }));

    if (errors[name]) {
      setErrors(prev => ({
        ...prev,
        [name]: ''
      }));
    }
  };

  // Obsługa checkboxów cech
  const handleCechaChange = (cecha) => {
    setFormData(prev => ({
      ...prev,
      cechy: prev.cechy.includes(cecha)
        ? prev.cechy.filter(c => c !== cecha)
        : [...prev.cechy, cecha]
    }));
  };

  // Dodawanie własnej cechy
  const handleDodajWlasnaCeche = () => {
    if (nowaCecha.trim() && !formData.wlasneCechy.includes(nowaCecha.trim())) {
      setFormData(prev => ({
        ...prev,
        wlasneCechy: [...prev.wlasneCechy, nowaCecha.trim()]
      }));
      setNowaCecha('');
    }
  };

  // Usuwanie własnej cechy
  const handleUsunWlasnaCeche = (cecha) => {
    setFormData(prev => ({
      ...prev,
      wlasneCechy: prev.wlasneCechy.filter(c => c !== cecha)
    }));
  };

  // Obsługa zmiany zdjęcia
  const handleImageChange = (imageData) => {
    setFormData(prev => ({
      ...prev,
      zdjecie: imageData
    }));
  };

  // Obsługa usuwania zdjęcia
  const handleImageRemove = () => {
    setFormData(prev => ({
      ...prev,
      zdjecie: ''
    }));
  };

  // Obsługa wyświetlania szczegółów
  const handleShowDetails = (car) => {
    setSelectedCar(car);
    setShowDetails(true);
    setShowForm(false);
  };

  // Formatowanie ceny
  const formatPrice = (price) => {
    return new Intl.NumberFormat('pl-PL', {
      style: 'currency',
      currency: 'PLN'
    }).format(price);
  };

  return (
    <div className={styles.carAdminPanel}>
      <div className={styles.container}>
        {/* Header */}
        <CarHeader carsCount={cars.length} />

        {/* Panel szczegółów samochodu */}
        {showDetails && selectedCar && (
          <CarDetailsModal 
            car={selectedCar}
            onClose={() => setShowDetails(false)}
            onEdit={handleEditCar}
            formatPrice={formatPrice}
          />
        )}

        {/* Przycisk dodawania */}
        <div className={styles.card}>
          {!showForm && !showDetails ? (
            <div className={styles.center}>
              <button
                onClick={handleAddCar}
                className={styles.btnSuccess}
              >
                <span style={{marginRight: '0.5rem'}}>+</span>
                Dodaj Nowy Samochód
              </button>
            </div>
          ) : null}
        </div>

        {/* Formularz */}
        {showForm && (
          <CarForm 
            formData={formData}
            errors={errors}
            isEditing={isEditing}
            dostepneCechy={dostepneCechy}
            nowaCecha={nowaCecha}
            onInputChange={handleInputChange}
            onSubmit={handleSubmit}
            onCancel={resetForm}
            onImageChange={handleImageChange}
            onImageRemove={handleImageRemove}
            onFeatureChange={handleCechaChange}
            onNowaCechaChange={setNowaCecha}
            onAddCustomFeature={handleDodajWlasnaCeche}
            onRemoveCustomFeature={handleUsunWlasnaCeche}
          />
        )}

        {/* Lista samochodów */}
        <CarTable 
          cars={cars}
          formatPrice={formatPrice}
          onShowDetails={handleShowDetails}
          onEdit={handleEditCar}
          onDelete={handleDeleteCar}
        />
      </div>
    </div>
  );
};

export default CarAdminPanel;