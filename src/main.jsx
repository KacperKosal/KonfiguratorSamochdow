import './index.css';
import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import { BrowserRouter, Routes, Route } from 'react-router-dom'
import Home from './Views/Home.jsx'
import CarConfigurator from './Views/CarConfigurator.jsx'


createRoot(document.getElementById('root')).render(
  <StrictMode>
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/car-configurator" element={<CarConfigurator />} />
      </Routes>
    </BrowserRouter>
  </StrictMode>,
)
