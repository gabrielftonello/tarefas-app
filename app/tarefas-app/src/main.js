// src/main.js
import { createApp } from 'vue';
import App from '../App.vue';
import router from './router';
import store from './store';
import 'vuetify/styles';
import { createVuetify } from 'vuetify';
import '@mdi/font/css/materialdesignicons.css'; // Optional: for icons
import './assets/styles/main.scss';

const vuetify = createVuetify({
    theme: {
        themes: {
            light: {
                primary: '#1976D2',
                secondary: '#424242',
                accent: '#82B1FF',
                error: '#FF5252',
                info: '#2196F3',
                success: '#4CAF50',
                warning: '#FFC107',
            },
        },
    },
});

const app = createApp(App);

app.use(router);
app.use(store);
app.use(vuetify);

app.mount('#app');
