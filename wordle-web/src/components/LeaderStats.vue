<template>
    <v-text-field style="width: 75%;" label="Wordle" type="text" v-model="searchName" @input="search" maxlength="5"></v-text-field>
    <div v-for="(leader, index) in searchList" :key="index" class="border my-3">
        <v-card-text class="d-flex flex-column">
        <v-row dense>
            <v-col cols="3">{{ leader }}</v-col>
        </v-row>
        </v-card-text>
    </div>
    <v-btn style="width: 20%" @click="getLeaderData"></v-btn>
    <header>{{ leaderName }}</header>
    <div v-if="searchComplete">
        <v-card>
            <v-card-title>
                Civilization Ability:
            </v-card-title>
            <v-card-text>
                {{ civAbility }}
            </v-card-text>
        </v-card>
        <v-card>
            <v-card-title>
                Leader Ability:
            </v-card-title>
            <v-card-text>
                {{ leaderAbility }}
            </v-card-text>
        </v-card>
        <v-card>
            <v-card-title>
                Unique Unit:
            </v-card-title>
            <v-card-text>
                {{ unit }}
            </v-card-text>
        </v-card>
        <v-card>
            <v-card-title>
                Unique Building:
            </v-card-title>
            <v-card-text>
                {{ district }}
            </v-card-text>
        </v-card>
    </div>
</template>

<script setup lang="ts">
import Axios from 'axios'
import { inject, ref } from 'vue'

const searchComplete = ref(false)

const searchList = ref<String[]>([])
const searchName = ref('')
const leaderName = ref('')
const civAbility = ref('')
const leaderAbility = ref('')
const unit = ref('')
const district = ref('')

async function search() {
  console.log(searchName.value)
  let apiPath = `civ/leadersearch?start=${searchName.value}`
  Axios.get(apiPath).then((result) => {
    console.log(result.data)
    searchList.value = result.data as String[]
  })
}

async function getLeaderData(){
    let apiPath = `civ/leadersearch?start=${searchName.value}`
    Axios.get(apiPath).then((result) => {
        console.log(result.data)
        leaderName.value = searchName.value
        civAbility.value = result.data.civAbility
        leaderAbility.value = result.data.leaderAbility
        unit.value = result.data.uniqueUnit
        district.value = result.data.ability
        searchComplete.value = true
        searchList.value = []
    })
}

</script>