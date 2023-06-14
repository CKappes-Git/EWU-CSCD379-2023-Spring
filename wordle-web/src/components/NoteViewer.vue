<template>
    <v-container :class="[theme.global.name.value === 'dark' ? 'containerD' : 'containerL']" style="height: 100%; margin-top: 8px;">
        <div v-if="SignInService.instance.isSignedIn">
            <v-btn class="editButton" @click="toggleEdit">
                <div v-if="editMode">Close</div>
                <div v-else>Edit</div>
            </v-btn>
            <v-text-field :class="[theme.global.name.value === 'dark' ? 'textInputD' : 'textInputL']" label="Leader" type="text" v-model="leaderName" @click="search" @input="search"></v-text-field>
            <div class="searchBox">
                <div v-for="(leader, index) in searchResults" :key="index">
                    <v-btn style="margin-bottom: 1px; margin-bottom: 1px; width: 100%;" @click="setNotes(leader.name)">{{ leader.name }}</v-btn>
                </div>
            </div>
            <div v-if="!editMode">
                <v-row>
                    <div v-for="(note, index) in notes" :key="index">
                        <v-col>
                            <v-btn>{{ note.noteName }}</v-btn>
                        </v-col>
                    </div>
                </v-row>
            </div>
            <div v-else>

            </div>
        </div>
    </v-container>
</template>

<script lang="ts" setup>
import type { Leader } from '@/scripts/leader';
import type { LeaderNote } from '@/scripts/leaderNote';
import { SignInService } from '@/scripts/signInService'
import Axios from 'axios';
import { ref } from 'vue';
import { useTheme } from 'vuetify/lib/framework.mjs'

const theme = useTheme()
const leaderName = ref('')
const searchResults = ref<Leader[]>([])
const notes = ref<LeaderNote[]>([])
const editMode = ref(false)

async function search() {
  console.log(leaderName.value)
  let apiPath = `civilization/GetLeaders?start=${leaderName.value}`
  Axios.get(apiPath).then((result) => {
    console.log(result.data)
    searchResults.value = result.data
  })
}

function toggleEdit(){
    editMode.value = !editMode.value
}

async function setNotes(name: string = ''){
    if(name != ''){
        let apiPath = `civilization/GetLeaderNotes?leaderName=${leaderName.value}&appUserId=${SignInService.instance.token.sub}`
        Axios.get(apiPath).then((result) => {
            console.log(result.data)
            notes.value = result.data
        })
    }
}


</script>
<style scoped>
.containerD{
    background-size: cover;
    border-radius: 20px;
    background-color:rgba(0, 0, 0, 0.8);
}
.containerL{
    background-size: cover;
    border-radius: 20px;
    background-color:rgba(255, 255, 255, 0.8);
}
.editButton{
    margin-left: 90%;
    width: 10%;
}
</style>