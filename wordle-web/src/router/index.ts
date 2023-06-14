import { createRouter, createWebHistory } from 'vue-router'
import AboutView from '../views/AboutView.vue'
import CivViewVue from '@/views/CivView.vue'
import CivEditorVue from '@/views/CivEditor.vue'
import Axios from 'axios'
import { SignInService } from '@/scripts/signInService'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/about',
      name: 'about',
      component: AboutView
    },
    {
      path: '/',
      name: 'civ',
      component: CivViewVue
    },
    {
      path: '/civeditor',
      name: 'civEditor',
      component: CivEditorVue,
      beforeEnter: async (to, from, next) => {
        if (SignInService.instance.token.roles.includes('Admin')) next()
        else next({name: 'civ'})
      }
    }
  ]
})

export default router
