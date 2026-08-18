import axios from 'axios';

const api = axios.create({
  baseURL: process.env.REACT_APP_API_URL || 'http://localhost:5000',
  headers: {
    'Content-Type': 'application/json',
  },
});

export const getMedicines = () => api.get('/api/Medicines');

export const addMedicine = (data) => api.post('/api/Medicines', data);

export const updateMedicine = (id, data) => api.put(`/api/Medicines/${id}`, data);

export const deleteMedicine = (id) => api.delete(`/api/Medicines/${id}`);

export const searchMedicines = (name) =>
  api.get('/api/Medicines/search', { params: { name } });

export default api;
