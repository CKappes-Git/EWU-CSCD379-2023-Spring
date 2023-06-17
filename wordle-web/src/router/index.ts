import { createRouter, createWebHistory } from 'vue-router'
import AboutView from '../views/AboutView.vue'
import CivViewVue from '@/views/CivView.vue'
import CivEditorVue from '@/views/CivEditor.vue'
import Axios from 'axios'
import { SignInService } from '@/scripts/signInService'
import HomePageVue from '@/views/HomePage.vue'
import WarhammerEditorVue from '@/views/WarhammerEditor.vue'
import WarhammerViewVue from '@/views/WarhammerView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/about',
      name: 'about',
      component: AboutView
    },
    {
      path: '/civilization',
      name: 'civilization',
      component: CivViewVue
    },
    {
      path: '/warhammer',
      name: 'warhammer',
      component: WarhammerViewVue
    },
    {
      path: '/',
      name: 'home',
      component: HomePageVue
    },
    {
      path: '/civeditor',
      name: 'civEditor',
      component: CivEditorVue,
      beforeEnter: async (to, from, next) => {
        if (SignInService.instance.token.roles.includes('Admin')) next()
        else next({name: 'home'})
      }
    },
    {
      path: '/warhammereditor',
      name: 'warhammerEditor',
      component: WarhammerEditorVue,
      beforeEnter: async (to, from, next) => {
        if (SignInService.instance.token.roles.includes('Admin')) next()
        else next({name: 'home'})
      }
    }
  ]
})

export default router
