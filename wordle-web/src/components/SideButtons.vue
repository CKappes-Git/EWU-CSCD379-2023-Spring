<template>

    <v-btn class="primarySwitch" v-if="primarySwitch" icon="mdi-format-list-bulleted" @click="$emit('togglePrimary')">
    </v-btn>
    <v-btn class="primarySwitch" v-else icon="mdi-lectern" @click="$emit('togglePrimary')">
    </v-btn>
    
    <v-btn class="knowMore" v-if="!knowMore" icon="mdi-card-plus-outline" @click="$emit('toggleMore')">
    </v-btn>
    <v-btn class="knowMore" v-else icon="mdi-card-remove-outline" @click="$emit('toggleMore')">
    </v-btn>
    <v-btn class="toggleNotes" v-if="knowMore && !showNotes" icon="mdi-format-list-bulleted" @click="$emit('toggleNotes')">
    </v-btn>
    <v-btn class="toggleNotes" v-if="knowMore && showNotes" icon="mdi-lectern" @click="$emit('toggleNotes')">
    </v-btn>
    
    
    </template>
    
    <script lang="ts" setup>
    import { ref } from 'vue';
    
    const props = defineProps<{
        primarySwitch: boolean
        knowMore: boolean
        showNotes: boolean
    }>()
    
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