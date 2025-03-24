import React from 'react';

const Header = () => {
  return (
    <header className="bg-white shadow-md">
      <div className="container mx-auto px-4 py-4 flex justify-between items-center">
        <div className="text-2xl font-bold text-blue-700">AutoKonfigurator</div>
        <nav>
          <ul className="flex space-x-6">
            <li className="hover:text-blue-700 cursor-pointer">Modele</li>
            <li className="hover:text-blue-700 cursor-pointer">Salony</li>
            <li className="hover:text-blue-700 cursor-pointer">Finansowanie</li>
            <li className="hover:text-blue-700 cursor-pointer">Kontakt</li>
          </ul>
        </nav>
      </div>
    </header>
  );
};

export default Header;
