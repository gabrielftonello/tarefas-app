
import api from '@/services/api';

const state = {
    token: localStorage.getItem('token') || '',
    userEmail: localStorage.getItem('userEmail') || '',
};

const getters = {
    isAuthenticated: state => !!state.token,
    userEmail: state => state.userEmail,
};

const actions = {
    async register({ commit }, payload) {
        const response = await api.post('/Conta/registro', payload);
        if (response.data.token) {
            commit('SET_AUTH', response.data);
        }
        return response;
    },
    async login({ commit }, payload) {
        const response = await api.post('/Conta/login', payload);
        if (response.data.token) {
            commit('SET_AUTH', response.data);
        }
        return response;
    },

};

const mutations = {
    SET_AUTH(state, payload) {
        state.token = payload.token;
        state.userEmail = payload.email;
        localStorage.setItem('token', payload.token);
        localStorage.setItem('userEmail', payload.email);
    },
    CLEAR_AUTH(state) {
        state.token = '';
        state.userEmail = '';
        localStorage.removeItem('token');
        localStorage.removeItem('userEmail');
    },
};

export default {
    namespaced: true,
    state,
    getters,
    actions,
    mutations,
};
