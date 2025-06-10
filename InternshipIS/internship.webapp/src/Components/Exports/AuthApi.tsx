import axios from 'axios';

const api = axios.create({
    baseURL: 'https://praxeosu.cz:5001',
    withCredentials: true,
    headers: {
        'Content-Type': 'application/json',
    },
});

export default api;