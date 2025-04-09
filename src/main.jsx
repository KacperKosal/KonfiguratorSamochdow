import './index.css';
import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import { BrowserRouter, Routes, Route } from 'react-router-dom'
import Home from './Views/Home/Home.jsx'
import CarConfigurator from './Views/CarConfigurator/CarConfigurator.jsx'
import Header from './components/Header/Header.jsx'
import Footer from './components/Footer/Footer.jsx'
import Login from './Views/Login/Login.jsx'

createRoot(document.getElementById('root')).render(
  <StrictMode>
    <BrowserRouter>
      <Header />
      <Routes>
        <Route path="/home" element={<Home />} />
        <Route path="/car-configurator" element={<CarConfigurator />} />
        <Route path="/login" element={<Login/>} />
      </Routes>
      <Footer />
    </BrowserRouter>
  </StrictMode>,
)
