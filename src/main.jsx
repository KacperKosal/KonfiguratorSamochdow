import './index.css';
import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import { BrowserRouter, Routes, Route } from 'react-router-dom'
import Home from './Views/Home/Home.jsx'
import CarConfigurator from './Views/CarConfigurator/CarConfigurator.jsx'
import Header from './components/Header/Header.jsx'
import Footer from './components/Footer/Footer.jsx'


createRoot(document.getElementById('root')).render(
  <StrictMode>
    <BrowserRouter>
      <Header />
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/car-configurator" element={<CarConfigurator />} />
      </Routes>
      <Footer />
    </BrowserRouter>
  </StrictMode>,
)
