import React from 'react';
import { ShoppingCart } from 'lucide-react';

const SummaryPanel = ({
  exteriorColors,
  wheelTypes,
  interiorColors,
  carColor,
  wheelType,
  interiorColor,
  totalPrice,
}) => {
  const selectedExterior = exteriorColors.find(c => c.value === carColor);
  const selectedWheels = wheelTypes.find(w => w.value === wheelType);
  const selectedInterior = interiorColors.find(i => i.value === interiorColor);

  return (
    <div>
      <h2 className="summary-title">Twoja konfiguracja</h2>
      <div className="summary-section">
        <h3>Model bazowy</h3>
        <p>X-Drive GT 2.0 Turbo 250KM</p>
        <p style={{ color: '#666' }}>120 000 zł</p>
      </div>
      <div className="summary-section">
        <h3>Kolor nadwozia</h3>
        <p>{selectedExterior?.name}</p>
        <p style={{ color: '#666' }}>+{selectedExterior?.price.toLocaleString()} zł</p>
      </div>
      <div className="summary-section">
        <h3>Felgi</h3>
        <p>{selectedWheels?.name}</p>
        <p style={{ color: '#666' }}>+{selectedWheels?.price.toLocaleString()} zł</p>
      </div>
      <div className="summary-section">
        <h3>Wnętrze</h3>
        <p>{selectedInterior?.name}</p>
        <p style={{ color: '#666' }}>+{selectedInterior?.price.toLocaleString()} zł</p>
      </div>
      <div style={{ borderTop: '1px solid #ccc', paddingTop: '16px', marginBottom: '16px' }}>
        <div className="total-price">
          <span>Cena łączna</span>
          <span>{totalPrice.toLocaleString()} zł</span>
        </div>
        <div style={{ fontSize: '14px', color: '#666', marginBottom: '16px' }}>
          Cena zawiera VAT (23%)
        </div>
      </div>
      <div className="button-group">
        <button className="action-button save">
          <ShoppingCart size={18} />
          <span>Zapisz konfigurację</span>
        </button>
        <button className="action-button test-drive">
          Umów jazdę próbną
        </button>
        <button className="action-button availability">
          Sprawdź dostępność
        </button>
      </div>
    </div>
  );
};

export default SummaryPanel;
