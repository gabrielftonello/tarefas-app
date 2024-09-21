
import { createRouter, createWebHistory } from 'vue-router';
import Home from '@/views/Home.vue';
import Login from '@/views/Login.vue';
import Register from '@/views/Register.vue';
import NotFound from '@/views/NotFound.vue';
import store from '@/store';

const routes = [
    { path: '/', name: 'Home', component: Home },
    { path: '/login', name: 'Login', component: Login },
    { path: '/register', name: 'Register', component: Register },
    { path: '/:pathMatch(.*)*', name: 'NotFound', component: NotFound },
];

const router = createRouter({
    history: createWebHistory(),
    routes,
});

router.beforeEach((to, from, next) => {
    const isAuthenticated = store.getters['auth/isAuthenticated'];
    if (to.meta.requiresAuth && !isAuthenticated) {
        next({ name: 'Login' });
    } else {
        next();
    }
});

export default router;
