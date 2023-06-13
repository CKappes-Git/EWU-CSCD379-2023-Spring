<template>
    <v-container :class="[theme.global.name.value === 'dark' ? 'containerD' : 'containerL']">
        <div>
            <v-row>
                <v-text-field style="width: 75%;" label="Civ" type="text" v-model="civName" @click="searchCiv" @input="searchCiv"></v-text-field>
                <v-btn style="margin-top: 8px; background-color: green;" @click="addCiv">
                    +
                </v-btn>
            </v-row>
            <div class="searchBox">
                <div v-for="(civ, index) in civResults" :key="index">
                    <v-btn style="margin-bottom: 1px; margin-bottom: 1px; width: 100%;" @click="setCiv(civ.civName)">{{ civ.civName }}</v-btn>
                </div>
            </div>
        </div>
        <div v-if="curCiv != null">
            <v-row>
                <v-text-field style="width: 75%;" label="Leader" type="text" v-model="leaderName" @click="search" @input="search"></v-text-field>
                <v-btn style="margin-top: 8px; background-color: green;" @click="addLeader">
                    +
                </v-btn>
            </v-row>
            <div class="searchBox">
                <div v-for="(leader, index) in searchResults" :key="index">
                    <v-btn style="margin-bottom: 1px; margin-bottom: 1px; width: 100%;" @click="setLeader(leader.name)">{{ leader.name }}</v-btn>
                </div>
            </div>
        </div>
        <v-row v-if="curLeader != null">
            <v-col style="width: 100%;" v-if="curLeader.civAttributes != null">
                <v-card-title>{{ curLeader.civName }}:</v-card-title>
                <div style="width: 100%;" v-for="(attribute, index) in curLeader.civAttributes">
                    <v-card style="margin-bottom: 15px;">
                        <v-text-field label="Type" type="text" v-model="attribute.attributeType"></v-text-field>
                        <v-text-field label="Name" type="text" v-model="attribute.abilityName"></v-text-field>
                        <v-text-field label="Description" type="text" v-model="attribute.description"></v-text-field>
                    </v-card>
                </div>
                <v-btn @click="addCivAttribute">
                    +
                </v-btn>
            </v-col>
            <v-col v-if="curLeader.leaderAttributes != null">
                <v-card-title>{{ curLeader.leaderName }}:</v-card-title>
                <div style="width: 100%;" v-for="(attribute, index) in curLeader.leaderAttributes">
                    <v-card style="margin-bottom: 15px;">
                        <v-text-field label="Type" type="text" v-model="attribute.attributeType"></v-text-field>
                        <v-text-field label="Name" type="text" v-model="attribute.abilityName"></v-text-field>
                        <v-text-field label="Description" type="text" v-model="attribute.description"></v-text-field>
                    </v-card>
                </div>
                <v-btn @click="addLeaderAttribute">
                    +
                </v-btn>
            </v-col>
        </v-row>
        <v-btn  v-if="curLeader != null" @click="submit" style="margin: 8px; width: 75%;">
            Submit Changes
        </v-btn>
        <div  v-if="backgroundUrl != null">
            <v-row>
                <v-text-field style="width: 75%;" label="Background" type="text" v-model="backgroundUrl"></v-text-field>
                <v-btn @click="setBackground">Set</v-btn>
            </v-row>
        </div>
    </v-container>
</template>

<script lang="ts" setup>

import { useTheme } from 'vuetify/lib/framework.mjs'
import type { Leader } from '@/scripts/leader'
import Axios from 'axios'
import { inject, ref } from 'vue'
import type { Civ } from '@/scripts/civ'
import type { LeaderInfoDto } from '@/scripts/leaderInfoDto'
import { CivAttribute } from '@/scripts/civAttribute'
import { LeaderAttribute } from '@/scripts/leaderAttribute'


const theme = useTheme()


const civName = ref('')
const curCiv = ref<Civ>()
const civResults = ref<Civ[]>([])
const leaderName = ref('')
const searchResults = ref<Leader[]>([])
const curLeader = ref<LeaderInfoDto>()
const backgroundUrl = ref('')

async function searchCiv(){
    let apiPath = `civilization/GetCivs?start=${civName.value}`
    Axios.get(apiPath).then((result) => {
        console.log(result.data)
        civResults.value = result.data
    })
}

async function setCiv(name = ''){
    civName.value = name
    civResults.value = []
    let apiPath  = `civilization/GetCivs?count=1&start=${civName.value}`
    Axios.get(apiPath).then((result) => {
        console.log(result.data)
        curCiv.value = result.data[0]
    })
}

async function search() {
  console.log(leaderName.value)
  let apiPath = `civilization/GetLeaders?civName=${curCiv.value?.civName}&start=${leaderName.value}`
  Axios.get(apiPath).then((result) => {
    console.log(result.data)
    searchResults.value = result.data
  })
}

async function setBackground(){
    let apiPath = `civilization/SetBackgroundUrl?civName=${curLeader.value?.civName}&url=${backgroundUrl.value}`
    Axios.post(apiPath).then((result) => {
        console.log(result.data)
        backgroundUrl.value = result.data
    }).catch((error) => {
        console.log(error)
    })
}

async function getUrl(){
    let apiPath = `civilization/GetBackgroundUrl?civName=${curLeader.value?.civName}`
    Axios.get(apiPath).then((result) => {
        console.log(result.data)
        backgroundUrl.value = result.data
    })
}

async function setLeader(name = ''){
    leaderName.value = name
    searchResults.value = []
    console.log(name)
    let apiPath = `civilization/AllLeaderData?leaderName=${name}`
    Axios.get(apiPath).then((result) => {
        console.log(result.data)
        curLeader.value = result.data
        getUrl()
    })
}

async function addLeader(){
    let apiPath = `civilization/AddLeader?civName=${curCiv.value?.civName}&leaderName=${leaderName.value}`
    Axios.post(apiPath).then((result) => {
        setLeader(result.data.name)
    })
}

async function addCiv(){
    let apiPath = `civilization/AddCiv?civName=${civName.value}`
    Axios.post(apiPath).then((result) => {
        setCiv(result.data.civName)
    })
}

function addCivAttribute(){
    if(curLeader.value != undefined){
        curLeader.value.civAttributes.push(new CivAttribute)
    }
}
function addLeaderAttribute(){
    if(curLeader.value != undefined){
        curLeader.value.leaderAttributes.push(new LeaderAttribute)
    }
}

async function submit(){
    console.log(curLeader.value)
    Axios.post('/civilization/AddAttributes', curLeader.value).then((result) => {
        curLeader.value = result.data
    }).catch((error) => {
        console.log(error)
    })
}

</script>

<style scoped>
    .containerD{
        border-radius: 20px;
        margin-top: 5px;
        background-color:rgba(0, 0, 0, 0.8);
    }
    .containerL{
        border-radius: 20px;
        margin-top: 5px;
        background-color:rgba(255, 255, 255, 0.8);
    }
    .searchBox{
    overflow-y: scroll;
    max-height: 150px;
}
</style>