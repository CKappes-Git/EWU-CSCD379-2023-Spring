<template>
  <v-app>
    <span class="bg"></span>
    <v-app-bar :elevation="3">
      <template v-slot>
        <v-app-bar-title>
          <RouterLink style="color: #42A5F5; max-width: 25%; margin: 0;" to="/">
            <v-row style="width: 150px;" dense>
              <v-col style="max-width: 40px;"><v-img :width="50" src="/Campus.ico"></v-img></v-col>
              <v-col style="max-width: 50px;"><p>Campus</p></v-col>
            </v-row>
          </RouterLink>
        </v-app-bar-title>
        <v-spacer></v-spacer>

        <AppUser></AppUser>

        <v-btn icon="mdi-brightness-7" @click="switchTheme"></v-btn>

        <v-menu>
          <template v-slot:activator="{ props }">
            <v-btn icon="mdi-menu" v-bind="props"></v-btn>
          </template>

          <v-list width="200">
            <v-list-item>
              <v-list-item-title>
                <RouterLink :to="{ name: 'home' }"> Home </RouterLink>
              </v-list-item-title>
            </v-list-item>
            <v-list-item>
              <v-list-item-title>
                <RouterLink :to="{ name: 'civilization' }"> Civilization </RouterLink>
              </v-list-item-title>
            </v-list-item>
            <v-list-item>
              <v-list-item-title>
                <RouterLink :to="{ name: 'warhammer' }"> Warhammer </RouterLink>
              </v-list-item-title>
            </v-list-item>
            <div v-if="signInService.token.roles.includes('Admin')">
              <v-list-item>
                <v-list-item-title>
                  <RouterLink :to="{ name: 'civEditor' }"> Civ Editor </RouterLink>
                </v-list-item-title>
              </v-list-item>
              <v-list-item>
                <v-list-item-title>
                  <RouterLink :to="{ name: 'warhammerEditor' }"> War Editor </RouterLink>
                </v-list-item-title>
              </v-list-item>
            </div>
            <v-list-item v-if="signInService.isSignedIn">
              <v-list-item-title>
                <v-btn @click="signInService.signOut()">Sign Out</v-btn>
              </v-list-item-title>
            </v-list-item>
          </v-list>
        </v-menu>
      </template>
    </v-app-bar>

    <v-main>
      <RouterView />
    </v-main>
  </v-app>
</template>

<script setup lang="ts">
import { useTheme } from 'vuetify/lib/framework.mjs'
import { inject, reactive } from 'vue'
import { useDisplay } from 'vuetify'
import { provide } from 'vue'
import { Services } from './scripts/services'
import type { SignInService } from './scripts/signInService'
import AppUser from './components/AppUser.vue'

// Provide the useDisplay to other components so that it can be used in testing.
const display = reactive(useDisplay())
provide(Services.Display, display)
const signInService = inject(Services.SignInService) as SignInService

const theme = useTheme()

function switchTheme() {
  if (theme.global.name.value === 'light') {
    setDarkTheme()
  } else {
    setLightTheme()
  }
}

function setLightTheme() {
  theme.global.name.value = 'light'
}

function setDarkTheme() {
  theme.global.name.value = 'dark'
}
setTimeout(() => {
  // This is terrible, nasty, and should be removed.
  signInService.signIn('admin@intellitect.com', 'S3cur3P@ssw0rd!')
}, 1000)
</script>
