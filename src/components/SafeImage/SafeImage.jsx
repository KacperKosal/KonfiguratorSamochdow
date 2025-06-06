import React, { useState, useEffect } from 'react';

const SafeImage = ({ src, alt, className, fallbackSrc = null, showFallback = false, onImageStatusChange, ...props }) => {
  const [hasError, setHasError] = useState(false);
  const [isLoading, setIsLoading] = useState(true);

  const handleError = () => {
    setHasError(true);
    setIsLoading(false);
    // Powiadom rodzica że obraz się nie załadował
    if (onImageStatusChange) {
      onImageStatusChange(false);
    }
  };

  const handleLoad = () => {
    setIsLoading(false);
    setHasError(false);
    // Powiadom rodzica że obraz się załadował
    if (onImageStatusChange) {
      onImageStatusChange(true);
    }
  };

  // Resetuj stan gdy src się zmienia
  useEffect(() => {
    setHasError(false);
    setIsLoading(true);
  }, [src]);

  // Jeśli nie ma src lub wystąpił błąd i nie ma fallback, nie renderuj nic
  if (!src || (hasError && !fallbackSrc && !showFallback)) {
    return null;
  }

  // Jeśli wystąpił błąd i mamy fallback
  if (hasError && fallbackSrc) {
    return (
      <img
        src={fallbackSrc}
        alt={alt}
        className={className}
        {...props}
      />
    );
  }

  // Jeśli wystąpił błąd i chcemy pokazać placeholder
  if (hasError && showFallback) {
    return (
      <div 
        className={className}
        style={{ 
          display: 'flex', 
          alignItems: 'center', 
          justifyContent: 'center',
          backgroundColor: '#f3f4f6',
          color: '#6b7280',
          fontSize: '0.875rem',
          border: '2px dashed #d1d5db',
          borderRadius: '0.5rem',
          minHeight: '100px'
        }}
      >
        Brak zdjęcia
      </div>
    );
  }

  return (
    <img
      src={src}
      alt={alt}
      className={className}
      onError={handleError}
      onLoad={handleLoad}
      {...props}
    />
  );
};

export default SafeImage;