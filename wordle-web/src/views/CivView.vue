<template>

<v-btn class="knowMore" @click="toggleMore">
    <p v-if="knowMore == false">+</p>
    <p v-else>-</p>
</v-btn>
<v-btn class="toggleNotes" v-if="knowMore" @click="toggleNotes">
    <p>
        S
    </p>
</v-btn>

<v-row style="width: 95%; margin-left: 8px; height: 100%;">
    <v-col>
        <CivLeader></CivLeader>
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
import { ref } from 'vue';

const knowMore = ref(false)
const showNotes = ref(false)

function toggleMore(){
    knowMore.value = !knowMore.value
}

function toggleNotes(){
    showNotes.value = !showNotes.value
}

</script>

<style scoped>
.knowMore {
    position: absolute;
    right: 0px;
    height: 50px;
    top: 50%;
}
.toggleNotes {
    position: absolute;
    right: 0px;
    height: 50px;
    top: 60%;
}
</style>