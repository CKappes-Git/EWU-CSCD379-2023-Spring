<template>
  <v-container
    class="leaderContainer"
    :style="{ 'background-image': `url(${backgroundImageUrl})` }"
    style="height: 100%; margin-top: 8px"
  >
    <v-text-field
      :class="[theme.global.name.value === 'dark' ? 'textInputD' : 'textInputL']"
      clearable
      @click:clear="clearName(game)"
      label="Leader"
      type="text"
      v-model="leaderName"
      @click="search(game)"
      @input="search(game)"
    ></v-text-field>
    <div class="searchBox">
      <div v-for="(leader, index) in searchResults" :key="index">
        <v-btn
          style="margin-bottom: 1px; margin-bottom: 1px; width: 100%"
          @click="setLeader(game, leader.name)"
          >{{ leader.name }}</v-btn
        >
      </div>
    </div>
    <v-card
      :class="[theme.global.name.value === 'dark' ? 'opacityDropD' : 'opacityDropL']"
      style="border-radius: 20px; padding: 10px; margin-top: 5px"
      v-if="curLeader != null"
    >
      <v-card-title> {{ curLeader.leaderName }} of {{ curLeader.civName }} </v-card-title>
      <v-row>
        <v-col style="width: 100%" v-if="curLeader.civAttributes != null">
          <v-card-title>{{ curLeader.civName }}:</v-card-title>
          <div style="width: 100%" v-for="(attribute, index) in curLeader.civAttributes">
            <attribute-view :attribute="attribute"></attribute-view>
          </div>
        </v-col>
        <v-col v-if="curLeader.leaderAttributes != null">
          <v-card-title>{{ curLeader.leaderName }}:</v-card-title>
          <div style="width: 100%" v-for="(attribute, index) in curLeader.leaderAttributes">
            <attribute-view :attribute="attribute"></attribute-view>
          </div>
        </v-col>
      </v-row>
    </v-card>
  </v-container>
</template>

<script lang="ts" setup>
import { useTheme } from 'vuetify/lib/framework.mjs'
import type { Leader } from '@/scripts/leader'
import type { LeaderInfoDto } from '@/scripts/leaderInfoDto'
import AttributeView from './AttributeView.vue'
import Axios from 'axios'
import { inject, ref } from 'vue'

const theme = useTheme()

const leaderName = ref('')
const searchResults = ref<Leader[]>([])
const curLeader = ref<LeaderInfoDto>()
const backgroundImageUrl = ref(``)

const props = defineProps<{
  game: string
}>()

async function clearName(game = '') {
  leaderName.value = ''
  search(game)
}

async function search(game = '') {
  console.log(game)
  console.log(leaderName.value)
  let apiPath = `civilization/GetLeaders?game=${game}&start=${leaderName.value}`
  Axios.get(apiPath).then((result) => {
    console.log(result.data)
    searchResults.value = result.data
  })
}

async function setLeader(game = '', name = '') {
  leaderName.value = name
  searchResults.value = []
  console.log(name)
  let apiPath = `civilization/AllLeaderData?game=${game}&leaderName=${name}`
  Axios.get(apiPath)
    .then((result) => {
      console.log(result.data)
      curLeader.value = result.data
    })
    .then(() => {
      apiPath = `civilization/GetBackgroundUrl?game=${game}&civName=${curLeader.value?.civName}`
      Axios.get(apiPath).then((result) => {
        backgroundImageUrl.value = result.data
      })
    })
}
</script>

<style scoped>
@import './style.css';
</style>
