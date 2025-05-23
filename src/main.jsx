import './index.css';
import React from 'react'
import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import { BrowserRouter, Routes, Route } from 'react-router-dom'
import Home from './Views/Home/Home.jsx'
import CarConfigurator from './Views/CarConfigurator/CarConfigurator.jsx'
import Header from './components/Header/Header.jsx'
import Footer from './components/Footer/Footer.jsx'
import Login from './Views/Login/Login.jsx'
import Register from './Views/Register/Register.jsx'
import Contact from './Views/Contact/Contact.jsx'
import { StoreProvider } from '../src/store/StoreProvider.jsx';
import ProtectedRoute from './components/ProtectedRoute/ProtectedRoute.jsx';

createRoot(document.getElementById('root')).render(
  <StrictMode>
    <StoreProvider>
    <BrowserRouter>
      <Header />
      <Routes>
        <Route path="/home" element={<Home />} />
        <Route path="/car-configurator" element={
          <ProtectedRoute>
            <CarConfigurator />
          </ProtectedRoute>
        } />
        <Route path="/login" element={
          <ProtectedRoute requireAuth={false}>
            <Login/>
          </ProtectedRoute>
        } />
        <Route path='/register' element={
          <ProtectedRoute requireAuth={false}>
            <Register />
          </ProtectedRoute>
        } />
        <Route path='/contact' element={<Contact />} />
      </Routes>
      <Footer />
    </BrowserRouter>
    </StoreProvider>
  </StrictMode>,
)
