<template>

<v-btn class="primarySwitch" v-if="primarySwitch" icon="mdi-format-list-bulleted" @click="togglePrimary">
</v-btn>
<v-btn class="primarySwitch" v-else icon="mdi-lectern" @click="togglePrimary">
</v-btn>

<v-btn class="knowMore" v-if="!knowMore" icon="mdi-card-plus-outline" @click="toggleMore">
</v-btn>
<v-btn class="knowMore" v-else icon="mdi-card-remove-outline" @click="toggleMore">
</v-btn>
<v-btn class="toggleNotes" v-if="knowMore && !showNotes" icon="mdi-format-list-bulleted" @click="toggleNotes">
</v-btn>
<v-btn class="toggleNotes" v-if="knowMore && showNotes" icon="mdi-lectern" @click="toggleNotes">
</v-btn>

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

<style scoped>
.primarySwitch{
    position: absolute;
    left: 0px;
    height: 50px;
    top: 50%;
    padding: 0;
}
.knowMore {
    position: absolute;
    right: 0px;
    height: 50px;
    top: 50%;
    padding: 0;
}
.toggleNotes {
    position: absolute;
    right: 0px;
    height: 50px;
    top: 60%;
    padding: 0;
}
</style>