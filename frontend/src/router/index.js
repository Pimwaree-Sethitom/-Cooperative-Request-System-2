import { createRouter, createWebHistory } from 'vue-router'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    { path: '/', redirect: '/login' },
    {
      path: '/login',
      name: 'login',
      component: () => import('@/views/auth/LoginView.vue'),
    },
    {
      path: '/register',
      name: 'register',
      component: () => import('@/views/auth/RegisterView.vue'),
    },
    {
      path: '/',
      component: () => import('@/layouts/AppLayout.vue'),
      meta: { requiresAuth: true },
      children: [
        {
          path: 'dashboard',
          name: 'dashboard',
          component: () => import('@/views/public/DashboardView.vue'),
          meta: { role: 'public' },
        },
        {
          path: 'my-requests',
          name: 'my-requests',
          component: () => import('@/views/public/MyRequestsView.vue'),
          meta: { role: 'public' },
        },
        {
          path: 'new-request',
          name: 'new-request',
          component: () => import('@/views/public/NewRequestView.vue'),
          meta: { role: 'public' },
        },
        {
          path: 'staff/dashboard',
          name: 'staff-dashboard',
          component: () => import('@/views/staff/StaffDashboardView.vue'),
          meta: { role: 'staff' },
        },
        {
          path: 'staff/requests',
          name: 'staff-requests',
          component: () => import('@/views/staff/AllRequestsView.vue'),
          meta: { role: 'staff' },
        },
      ],
    },
  ],
})

router.beforeEach((to) => {
  const token    = localStorage.getItem('token')
  const role     = localStorage.getItem('role')
  const isLoggedIn = !!token
  const isStaff    = role === 'staff'

  if (to.meta.requiresAuth && !isLoggedIn) return { name: 'login' }

  if ((to.name === 'login' || to.name === 'register') && isLoggedIn) {
    return isStaff ? { name: 'staff-dashboard' } : { name: 'dashboard' }
  }

  if (to.meta.role === 'staff'  && !isStaff) return { name: 'dashboard' }
  if (to.meta.role === 'public' &&  isStaff) return { name: 'staff-dashboard' }
})

export default router
