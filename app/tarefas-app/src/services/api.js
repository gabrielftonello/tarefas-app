// src/services/api.js
import axios from 'axios';
import store from '@/store';

const api = axios.create({
    baseURL: 'http://localhost:5001/api/', 
    headers: {
        'Content-Type': 'application/json',
    },
});

api.interceptors.request.use(
    config => {
        const token = store.state.auth.token;
        if (token) {
            config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
    },
    error => {
        return Promise.reject(error);
    }
);

api.interceptors.response.use(
    response => response,
    error => {
        if (error.response && error.response.status === 401) {
            window.location.href = '/login';
        }
        return Promise.reject(error);
    }
);

export default api;
