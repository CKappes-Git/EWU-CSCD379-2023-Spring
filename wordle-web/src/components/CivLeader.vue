<template>
    <v-container class="leaderContainer" :style="{'background-image': `url(${backgroundImageUrl})`}" style="height: 100%; margin-top: 8px;">
        <v-text-field :class="[theme.global.name.value === 'dark' ? 'textInputD' : 'textInputL']" label="Leader" type="text" v-model="leaderName" @click="search" @input="search"></v-text-field>
        <div class="searchBox">
            <div v-for="(leader, index) in searchResults" :key="index">
                <v-btn style="margin-bottom: 1px; margin-bottom: 1px; width: 100%;" @click="setLeader(leader.name)">{{ leader.name }}</v-btn>
            </div>
        </div>
        <v-card :class="[theme.global.name.value === 'dark' ? 'opacityDropD' : 'opacityDropL']" style="border-radius: 20px; padding: 10px; margin-top: 5px;" v-if="curLeader != null">
            <v-card-title>
                {{ curLeader.leaderName }} of {{ curLeader.civName }}
            </v-card-title>
            <v-row>
                <v-col style="width: 100%;" v-if="curLeader.civAttributes != null">
                    <v-card-title>{{ curLeader.civName }}:</v-card-title>
                    <div style="width: 100%;" v-for="(attribute, index) in curLeader.civAttributes">
                        <v-card :class="[theme.global.name.value === 'dark' ? 'attributeD' : 'attributeL']">
                            <v-card-title>
                                {{ attribute.attributeType }}: {{ attribute.abilityName }}
                            </v-card-title>
                            <v-card-text>
                                {{ attribute.description }}
                            </v-card-text>
                        </v-card>
                    </div>
                </v-col>
                <v-col v-if="curLeader.leaderAttributes != null">
                    <v-card-title>{{ curLeader.leaderName }}:</v-card-title>
                    <div style="width: 100%;" v-for="(attribute, index) in curLeader.leaderAttributes">
                        <v-card :class="[theme.global.name.value === 'dark' ? 'attributeD' : 'attributeL']">
                            <v-card-title>
                                {{ attribute.attributeType }}: {{ attribute.abilityName }}
                            </v-card-title>
                            <v-card-text>
                                {{ attribute.description }}
                            </v-card-text>
                        </v-card>
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
import Axios from 'axios'
import { inject, ref } from 'vue'

const theme = useTheme()

const leaderName = ref('')
const searchResults = ref<Leader[]>([])
const curLeader = ref<LeaderInfoDto>()
const backgroundImageUrl = ref('')

async function search() {
  console.log(leaderName.value)
  let apiPath = `civilization/GetLeaders?start=${leaderName.value}`
  Axios.get(apiPath).then((result) => {
    console.log(result.data)
    searchResults.value = result.data
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
    }).then(() => {
        apiPath = `civilization/GetBackgroundUrl?civName=${curLeader.value?.civName}`
        Axios.get(apiPath).then((result) => {
            backgroundImageUrl.value = result.data
        })

    })
    
}

</script>

<style scoped>
.leaderContainer{
    background-size: cover;
    border-radius: 20px;
}
.opacityDropD{
    background-color:rgba(0, 0, 0, 0.8);
}
.attributeD{
    border-radius: 10px;
    margin-bottom: 15px;
    background-color:rgba(0, 0, 0, 0.8);
}
.textInputD{
    background-color: black;
    border-radius: 20px;
}
.opacityDropL{
    background-color:rgba(255, 255, 255, 0.8);
}
.attributeL{
    border-radius: 10px;
    margin-bottom: 15px;
    background-color:rgba(255, 255, 255, 0.8);
}
.textInputL{
    background-color: white;
    border-radius: 20px;
}
.searchBox{
    overflow-y: scroll;
    max-height: 25%;
}
</style>