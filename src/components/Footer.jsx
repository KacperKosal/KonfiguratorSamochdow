import React from 'react';

const Footer = () => {
  return (
    <footer className="bg-gray-800 text-white py-8">
      <div className="container mx-auto px-4">
        <div className="grid grid-cols-1 md:grid-cols-4 gap-8">
          <div>
            <h3 className="text-lg font-bold mb-4">AutoKonfigurator</h3>
            <p className="text-gray-300">Twój samochód, Twoje zasady. Konfiguruj i personalizuj swój wymarzony samochód online.</p>
          </div>
          <div>
            <h3 className="text-lg font-bold mb-4">Nawigacja</h3>
            <ul className="space-y-2 text-gray-300">
              <li className="hover:text-white cursor-pointer">Strona główna</li>
              <li className="hover:text-white cursor-pointer">Modele</li>
              <li className="hover:text-white cursor-pointer">Konfigurator</li>
              <li className="hover:text-white cursor-pointer">Akcesoria</li>
            </ul>
          </div>
          <div>
            <h3 className="text-lg font-bold mb-4">Kontakt</h3>
            <ul className="space-y-2 text-gray-300">
              <li>info@autokonfigurator.pl</li>
              <li>+48 123 456 789</li>
              <li>ul. Motoryzacyjna 10</li>
              <li>00-001 Warszawa</li>
            </ul>
          </div>
          <div>
            <h3 className="text-lg font-bold mb-4">Newsletter</h3>
            <p className="text-gray-300 mb-4">Zapisz się, aby otrzymywać najnowsze oferty i aktualizacje.</p>
            <div className="flex">
              <input type="email" placeholder="Twój e-mail" className="px-4 py-2 flex-grow rounded-l text-gray-800" />
              <button className="bg-blue-600 px-4 py-2 rounded-r hover:bg-blue-700">Zapisz</button>
            </div>
          </div>
        </div>
        <div className="border-t border-gray-700 mt-8 pt-6 text-center text-gray-300">
          <p>© 2025 AutoKonfigurator. Wszelkie prawa zastrzeżone.</p>
        </div>
      </div>
    </footer>
  );
};

export default Footer;
