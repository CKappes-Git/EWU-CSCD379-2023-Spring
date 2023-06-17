<template>

    <SideButtons :knowMore="knowMore" :primarySwitch="primarySwitch" :showNotes="showNotes" @togglePrimary="togglePrimary" @toggleMore="toggleMore" @toggleNotes="toggleNotes"></SideButtons>

<v-row style="width: 90%; margin-left: 5%; height: 100%;">
    <v-col>
        <CivLeader v-if="primarySwitch"></CivLeader>
        <NoteViewer v-else></NoteViewer>
    </v-col>
    <v-col v-if="knowMore == true">
        <CivLeader v-if="!showNotes"></CivLeader>
        <NoteViewer v-if="showNotes"></NoteViewer>
    </v-col>
</v-row>


</template>

<script lang="ts" setup>
import CivLeader from '@/components/CivLeader.vue';
import NoteViewer from '@/components/NoteViewer.vue'
import SideButtons from '@/components/SideButtons.vue';
import { ref } from 'vue';

const primarySwitch = ref(true)
const curIconLeft = ref('mdi-format-list-bulleted')
const knowMore = ref(false)
const showNotes = ref(false)

function togglePrimary(){
    primarySwitch.value = !primarySwitch.value
}

function toggleMore(){
    knowMore.value = !knowMore.value
}

function toggleNotes(){
    showNotes.value = !showNotes.value
}

</script>
