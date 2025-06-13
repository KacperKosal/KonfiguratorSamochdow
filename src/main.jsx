import './index.css';
import React from 'react';
import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';
import { BrowserRouter, Routes, Route } from 'react-router-dom';

import Home from './Views/Home/Home.jsx';
import CarConfigurator from './Views/CarConfigurator/CarConfigurator.jsx';
import SavedCarConfigurator from './Views/CarConfigurator/SavedCarConfigurator.jsx';
import NewCarConfigurator from './Views/CarConfigurator/NewCarConfigurator.jsx';
import Header from './components/Header/Header.jsx';
import Footer from './components/Footer/Footer.jsx';
import Login from './Views/Login/Login.jsx';
import Register from './Views/Register/Register.jsx';
import Contact from './Views/Contact/Contact.jsx';
import MyAccount from './Views/MyAccount/MyAccount.jsx';
import CarAdminPanel from './components/CarAdminPanel/CarAdminPanel.jsx';
import ProtectedRoute from './components/ProtectedRoute/ProtectedRoute.jsx';
import { StoreProvider } from './store/StoreProvider.jsx';


createRoot(document.getElementById('root')).render(
  <StrictMode>
    <StoreProvider>
      <BrowserRouter>
        <Routes>
          {/* Trasa dla panelu administracyjnego bez Header i Footer - tylko dla administratorów */}
          <Route 
            path="/admin" 
            element={
              <ProtectedRoute requiredRole="Administrator">
                <CarAdminPanel />
              </ProtectedRoute>
            } 
          />
          
          {/* Pozostałe trasy z Header i Footer */}
          <Route path="/*" element={
            <>
              <Header />
              <Routes>
                <Route path="/" element={<Home />} />
                <Route path="/home" element={<Home />} />
                {/* Stary routing dla kompatybilności wstecznej */}
                <Route path="/car-configurator" element={<CarConfigurator />} />
                {/* Nowe routingi */}
                <Route path="/car-configurator/new" element={<NewCarConfigurator />} />
                <Route path="/car-configurator/saved/:configurationId" element={<SavedCarConfigurator />} />
                <Route path="/login" element={<Login />} />
                <Route path="/register" element={<Register />} />
                <Route path="/contact" element={<Contact />} />
                <Route path="/my-account" element={<MyAccount />} />
              </Routes>
              <Footer />
            </>
          } />
        </Routes>
      </BrowserRouter>
    </StoreProvider>
  </StrictMode>
);
