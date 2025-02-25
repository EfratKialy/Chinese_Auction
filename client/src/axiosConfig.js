// axiosConfig.js
import axios from 'axios';

axios.defaults.baseURL = 'http://localhost:5068';

// פונקציה לשחזור ה-token מה-localStorage והוספתו ל-headers
const addTokenToRequest = (config) => {
  const token = localStorage.getItem('token');
  if (token) {
    config.headers['Authorization'] = `Bearer ${token}`;
  }
  return config;
};

// הוספת interceptor ל-Axios כדי להוסיף את ה-token לכל בקשה
axios.interceptors.request.use(addTokenToRequest);

export default axios;
