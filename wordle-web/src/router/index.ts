import { createRouter, createWebHistory } from 'vue-router'
import WordleView from '../views/WordleView.vue'
import AboutView from '../views/AboutView.vue'
import LeaderboardView from '@/views/LeaderboardView.vue'
import WordOfTheDayView from '@/views/WordOfTheDayView.vue'
import WordleEditorView from '@/views/WordleEditor.vue'
import CivViewVue from '@/views/CivView.vue'
import CivEditorVue from '@/views/CivEditor.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'wordle',
      component: WordleView
    },
    {
      path: '/about',
      name: 'about',
      component: AboutView
    },
    {
      path: '/leaderboard',
      name: 'leaderboard',
      component: LeaderboardView
    },
    {
      path: '/wordoftheday',
      name: 'wordOfTheDay',
      component: WordOfTheDayView
    },
    {
      path: '/wordleeditor',
      name: 'wordleEditor',
      component: WordleEditorView
    },
    {
      path: '/civ',
      name: 'civ',
      component: CivViewVue
    },
    {
      path: '/civeditor',
      name: 'civEditor',
      component: CivEditorVue
    }
  ]
})

export default router
