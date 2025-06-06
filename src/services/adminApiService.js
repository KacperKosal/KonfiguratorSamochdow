import axiosInstance from './axiosConfig';

// Serwis API dla panelu administratora
class AdminApiService {
  // CRUD dla modeli samochodów
  async getAllCarModels() {
    try {
      const response = await axiosInstance.get('/api/car-models');
      return response.data;
    } catch (error) {
      console.error('Error fetching car models:', error);
      throw error;
    }
  }

  async getCarModelById(id) {
    try {
      const response = await axiosInstance.get(`/api/car-models/${id}`);
      return response.data;
    } catch (error) {
      console.error('Error fetching car model:', error);
      throw error;
    }
  }

  async createCarModel(carModel) {
    try {
      const response = await axiosInstance.post('/api/car-models', carModel);
      return response.data;
    } catch (error) {
      console.error('Error creating car model:', error);
      throw error;
    }
  }

  async updateCarModel(id, carModel) {
    try {
      const response = await axiosInstance.put(`/api/car-models/${id}`, carModel);
      return response.data;
    } catch (error) {
      console.error('Error updating car model:', error);
      throw error;
    }
  }

  async deleteCarModel(id) {
    try {
      await axiosInstance.delete(`/api/car-models/${id}`);
    } catch (error) {
      console.error('Error deleting car model:', error);
      throw error;
    }
  }

  // CRUD dla silników
  async getAllEngines() {
    try {
      const response = await axiosInstance.get('/api/engines');
      return response.data;
    } catch (error) {
      console.error('Error fetching engines:', error);
      throw error;
    }
  }

  async getEngineById(id) {
    try {
      const response = await axiosInstance.get(`/api/engines/${id}`);
      return response.data;
    } catch (error) {
      console.error('Error fetching engine:', error);
      throw error;
    }
  }

  async createEngine(engine) {
    try {
      const response = await axiosInstance.post('/api/engines', engine);
      return response.data;
    } catch (error) {
      console.error('Error creating engine:', error);
      throw error;
    }
  }

  async updateEngine(id, engine) {
    try {
      const response = await axiosInstance.put(`/api/engines/${id}`, engine);
      return response.data;
    } catch (error) {
      console.error('Error updating engine:', error);
      throw error;
    }
  }

  async deleteEngine(id) {
    try {
      await axiosInstance.delete(`/api/engines/${id}`);
    } catch (error) {
      console.error('Error deleting engine:', error);
      throw error;
    }
  }

  // CRUD dla akcesoriów
  async getAllCarAccessories() {
    try {
      const response = await axiosInstance.get('/api/car-accessories');
      return response.data;
    } catch (error) {
      console.error('Error fetching car accessories:', error);
      throw error;
    }
  }

  async getCarAccessoryById(id) {
    try {
      const response = await axiosInstance.get(`/api/car-accessories/${id}`);
      return response.data;
    } catch (error) {
      console.error('Error fetching car accessory:', error);
      throw error;
    }
  }

  async createCarAccessory(accessory) {
    try {
      const response = await axiosInstance.post('/api/car-accessories', accessory);
      return response.data;
    } catch (error) {
      console.error('Error creating car accessory:', error);
      throw error;
    }
  }

  async updateCarAccessory(id, accessory) {
    try {
      const response = await axiosInstance.put(`/api/car-accessories/${id}`, accessory);
      return response.data;
    } catch (error) {
      console.error('Error updating car accessory:', error);
      throw error;
    }
  }

  async deleteCarAccessory(id) {
    try {
      await axiosInstance.delete(`/api/car-accessories/${id}`);
    } catch (error) {
      console.error('Error deleting car accessory:', error);
      throw error;
    }
  }

  // CRUD dla wyposażenia wnętrza
  async getAllCarInteriorEquipment() {
    try {
      const response = await axiosInstance.get('/api/car-interior-equipment');
      return response.data;
    } catch (error) {
      console.error('Error fetching car interior equipment:', error);
      throw error;
    }
  }

  async createCarInteriorEquipment(equipment) {
    try {
      const response = await axiosInstance.post('/api/car-interior-equipment', equipment);
      return response.data;
    } catch (error) {
      console.error('Error creating car interior equipment:', error);
      throw error;
    }
  }

  async updateCarInteriorEquipment(id, equipment) {
    try {
      const response = await axiosInstance.put(`/api/car-interior-equipment/${id}`, equipment);
      return response.data;
    } catch (error) {
      console.error('Error updating car interior equipment:', error);
      throw error;
    }
  }

  async deleteCarInteriorEquipment(id) {
    try {
      await axiosInstance.delete(`/api/car-interior-equipment/${id}`);
    } catch (error) {
      console.error('Error deleting car interior equipment:', error);
      throw error;
    }
  }

  // Obsługa zdjęć
  async uploadImage(file) {
    try {
      const formData = new FormData();
      formData.append('file', file);
      
      const response = await axiosInstance.post('/api/images/upload', formData, {
        headers: {
          'Content-Type': 'multipart/form-data'
        }
      });
      return response.data;
    } catch (error) {
      console.error('Error uploading image:', error);
      throw error;
    }
  }

  async deleteImage(fileName) {
    try {
      await axiosInstance.delete(`/api/images/${fileName}`);
    } catch (error) {
      console.error('Error deleting image:', error);
      throw error;
    }
  }

  // Funkcja pomocnicza do budowania URL zdjęcia
  getImageUrl(fileName) {
    if (!fileName) return null;
    const apiUrl = import.meta.env.VITE_API_URL || 'https://localhost:7001';
    return `${apiUrl}/api/images/${fileName}`;
  }
}

export default new AdminApiService();