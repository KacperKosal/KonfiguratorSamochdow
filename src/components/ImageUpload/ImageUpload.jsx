// ImageUpload.jsx
import { useRef } from 'react';
import styles from './ImageUpload.module.css';

const ImageUpload = ({ imageData, onImageChange, onImageRemove }) => {
  const fileInputRef = useRef(null);

  const handleFileChange = (e) => {
    const file = e.target.files[0];
    if (file) {
      if (!file.type.startsWith('image/')) {
        alert('Proszę wybrać plik obrazu (JPG, PNG, GIF, WebP)');
        e.target.value = '';
        return;
      }
      
      if (file.size > 10 * 1024 * 1024) {
        alert('Plik jest za duży. Maksymalny rozmiar to 10MB.');
        e.target.value = '';
        return;
      }
      
      const reader = new FileReader();
      reader.onload = (event) => {
        onImageChange(event.target.result);
      };
      reader.onerror = () => {
        alert('Wystąpił błąd podczas wczytywania pliku.');
        e.target.value = '';
      };
      reader.readAsDataURL(file);
    }
  };

  const handleRemoveImage = () => {
    onImageRemove();
    if (fileInputRef.current) {
      fileInputRef.current.value = '';
    }
  };

  return (
    <div className={styles.fileUpload}>
      <label className={styles.formLabel}>
        Zdjęcie samochodu
      </label>
      <div className={styles.spaceY3}>
        <div>
          <input
            ref={fileInputRef}
            type="file"
            accept="image/*"
            onChange={handleFileChange}
            className={styles.fileInput}
          />
          <p className={styles.fileHint}>
            Obsługiwane formaty: JPG, PNG, GIF, WebP (max. 10MB)
          </p>
        </div>
        {imageData && (
          <div className={styles.imagePreview}>
            <img 
              src={imageData} 
              alt="Podgląd zdjęcia samochodu"
              className={styles.previewImage}
            />
            <button
              type="button"
              onClick={handleRemoveImage}
              className={styles.removeImage}
              title="Usuń zdjęcie"
            >
              ×
            </button>
          </div>
        )}
      </div>
    </div>
  );
};

export default ImageUpload;