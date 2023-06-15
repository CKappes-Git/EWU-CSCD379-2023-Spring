<template>
    <v-container :class="[theme.global.name.value === 'dark' ? 'containerD' : 'containerL']" style="margin-top: 8px;">
        <div v-if="SignInService.instance.isSignedIn">
            <v-btn class="editButton" @click="toggleEdit">
                <div v-if="editMode">Close</div>
                <div v-else>Edit</div>
            </v-btn>
            <v-text-field :class="[theme.global.name.value === 'dark' ? 'textInputD' : 'textInputL']" label="Leader" type="text" v-model="leaderName" @click="search" @input="search"></v-text-field>
            <div class="searchBox">
                <div v-for="(leader, index) in searchResults" :key="index">
                    <v-btn style="margin-bottom: 1px; margin-bottom: 1px; width: 100%;" @click="setData(leader.name)">{{ leader.name }}</v-btn>
                </div>
            </div>
            <div style="margin-top: 5px;" v-if="curLeader">
                <v-row>
                     <v-card-title>
                        {{ curLeader.name }}
                     </v-card-title>
                </v-row>
                <div v-if="notes.length || editMode">
                <v-row>
                    
                    <div v-for="(note, index) in notes" :key="index">
                        <v-col>
                            <v-btn @click="setNote(index)">{{ note.noteName }}</v-btn>
                        </v-col>
                    </div>
                    <div v-if="editMode">
                        <v-col>
                            <v-btn @click="newNote">+</v-btn>
                        </v-col>
                    </div>
                    
                </v-row>
                </div>
                <div v-else>
                    <v-card-title>
                        You have no notes, add them in edit mode
                    </v-card-title>
                </div>
                <div v-if="curNote">
                    <v-row v-if="editMode">
                        <v-text-field v-model="curNote.noteName"></v-text-field>
                        <v-btn style="width: 5%; background-color: red" @click="deleteNote()">-</v-btn>
                    </v-row>

                    <NoteNodes v-if="curNote" :treeType="'Science:'" :editMode="editMode" :tree="curNote.scienceTree"></NoteNodes>
                    <NoteNodes v-if="curNote" :treeType="'Culture:'" :editMode="editMode" :tree="curNote.cultureTree"></NoteNodes>
                    <NoteNodes v-if="curNote" :treeType="'Production:'" :editMode="editMode" :tree="curNote.production"></NoteNodes>
                    
                    <div style="margin-top: 25px;" v-if="editMode">
                        <v-card-title>
                            Notes:
                        </v-card-title>
                        <v-textarea label="Notes:" type="text" v-model="curNote.notes"></v-textarea>
                        <v-btn style="margin-top: 15px;" @click="submitNote">Submit</v-btn>
                    </div>
                    <div style="margin-top: 25px;" v-else>
                        <v-card-title>
                            Notes:
                        </v-card-title>
                        <v-card-text>
                            <span style="white-space: pre-wrap;">{{ curNote.notes }}</span>
                        </v-card-text>
                    </div>
                </div>
                <div v-else>
                    <v-card-title v-if="notes.length">
                        You have not selected a note
                    </v-card-title>
                </div>
                
                
            </div>
            <div v-else>
                <v-card-title>
                    You have not selected a leader
                </v-card-title>
            </div>
            
        </div>
    </v-container>
</template>

<script lang="ts" setup>
import type { Leader } from '@/scripts/leader';
import { LeaderNote } from '@/scripts/leaderNote';
import { SignInService } from '@/scripts/signInService'
import Axios from 'axios';
import { ref } from 'vue';
import { useTheme } from 'vuetify/lib/framework.mjs'
import NoteNodes from './NoteNodes.vue';
import { LeaderNoteDto } from '@/scripts/LeaderNoteDto';

const theme = useTheme()
const leaderName = ref('')
const curLeader = ref<Leader>()
const searchResults = ref<Leader[]>([])
const notes = ref<LeaderNote[]>([])
const resultNodes = ref<LeaderNoteDto[]>([])
const tempNoteDto = ref<LeaderNoteDto>()
const curNote = ref<LeaderNote>()
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

function setNote(index: number = 0){
    curNote.value = notes.value[index]
}

async function setCurLeader(name: string = ''){
    if(name != ''){
        let apiPath = `civilization/GetLeaders?start=${name}`
        Axios.get(apiPath).then((result) => {
            console.log(result.data)
            curLeader.value = result.data[0]
        })
    }
}

async function setNotes(name: string = ''){
    if(name != ''){
        notes.value = []
        let apiPath = `civilization/GetLeaderNotes?leaderName=${name}&appUserId=${SignInService.instance.token.userId}`
        Axios.get(apiPath).then((result) => {
            console.log(result.data)
            searchResults.value = []
            resultNodes.value = result.data
            for(var i = 0; i < resultNodes.value.length; i++){
                var tempNote = new LeaderNote
                tempNote.leaderNoteID = resultNodes.value[i].leaderNoteID
                tempNote.leaderID = resultNodes.value[i].leaderID
                tempNote.appUserID = resultNodes.value[i].appUserID
                tempNote.noteName = resultNodes.value[i].noteName
                if(resultNodes.value[i].scienceTree != ""){
                    tempNote.scienceTree = resultNodes.value[i].scienceTree.split(',')
                } else {
                    tempNote.scienceTree = []
                }
                if(resultNodes.value[i].cultureTree != ""){
                    tempNote.cultureTree = resultNodes.value[i].cultureTree.split(',')
                } else {
                    tempNote.cultureTree = []
                }
                if(resultNodes.value[i].production != ""){
                    tempNote.production = resultNodes.value[i].production.split(',')
                } else {
                    tempNote.production = []
                }
                tempNote.notes = resultNodes.value[i].notes

                notes.value.push(tempNote)
            }
        })
    }
}

async function setData(name: string = ''){
    curNote.value = undefined
    setCurLeader(name)
    setNotes(name)
}

function newNote(){
    if(curLeader.value != undefined){
        var newNote = new LeaderNote
        newNote.noteName = "New"
        newNote.leaderID = curLeader.value.leaderID
        newNote.appUserID = SignInService.instance.token.userId
        notes.value.push(newNote)
    }
}

async function submitNote(){
    if(curNote != undefined && curNote.value?.leaderID){
        var sendNote = new LeaderNoteDto
        sendNote.leaderNoteID = curNote.value.leaderNoteID
        sendNote.leaderID = curNote.value.leaderID
        sendNote.appUserID = SignInService.instance.token.userId
        sendNote.noteName = curNote.value.noteName
        sendNote.scienceTree = curNote.value.scienceTree.toString()
        sendNote.cultureTree = curNote.value.cultureTree.toString()
        sendNote.production = curNote.value.production.toString()
        sendNote.notes = curNote.value.notes
        console.log(sendNote)

        Axios.post('/civilization/SetLeaderNote', sendNote).then((result) => {
            if(curNote.value?.leaderNoteID != undefined){
                console.log("Updated, ID: ")
                curNote.value.leaderNoteID = result.data
                console.log(curNote.value.leaderNoteID)
            }
        }).catch((error) => {
            console.log(error)
        })
    }
}

async function deleteNote(){
    const answer = window.confirm("You are about to delete a civ, all leaders of that civ, and all attributes of both.  Are you sure you want to do this?")
    if(answer){
        console.log(curNote.value?.leaderNoteID)
        Axios.post(`/civilization/DeleteLeaderNote?leaderNoteId=${curNote.value?.leaderNoteID}`).then(() => {
            setNotes(curLeader.value?.name)
            curNote.value = undefined
        }).catch((error) => {
            console.log(error)
        })
    }
}


</script>

<style scoped>
@import './style.css';
</style>