import React from 'react';

const Banner = () => {
  return (
    <div className="bg-gradient-to-r from-blue-800 to-blue-600 text-white">
      <div className="container mx-auto px-4 py-12">
        <h1 className="text-4xl font-bold mb-4">Odkryj nasze modele</h1>
        <p className="text-xl max-w-2xl">
          Wybierz samochód dopasowany do Twoich potrzeb i skonfiguruj go według własnych preferencji.
        </p>
      </div>
    </div>
  );
};

export default Banner;
