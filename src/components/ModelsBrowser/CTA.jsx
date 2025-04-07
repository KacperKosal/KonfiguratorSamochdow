import React from 'react';

const CTA = () => {
  return (
    <div className="bg-gray-900 text-white py-12">
      <div className="container mx-auto px-4 text-center">
        <h2 className="text-3xl font-bold mb-4">Nie możesz się zdecydować?</h2>
        <p className="text-xl mb-8 max-w-2xl mx-auto">
          Skorzystaj z naszego asystenta doboru, który pomoże Ci wybrać idealny model na podstawie Twoich potrzeb.
        </p>
        <button className="px-8 py-3 bg-blue-600 text-white text-lg font-medium rounded-lg hover:bg-blue-700">
          Uruchom asystenta doboru
        </button>
      </div>
    </div>
  );
};

export default CTA;
