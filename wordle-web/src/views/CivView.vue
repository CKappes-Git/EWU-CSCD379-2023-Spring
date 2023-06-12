<template>

<v-btn class="knowMore" @click="toggleMore">
    <Text v-if="knowMore == false">+</Text>
    <Text v-else>-</Text>
</v-btn>

<v-row style="width: 95%; margin-left: 8px; height: 100%;">
    <v-col>
        <CivLeader></CivLeader>
    </v-col>
    <v-col v-if="knowMore == true">
        <CivLeader></CivLeader>
    </v-col>
</v-row>


</template>

<script lang="ts" setup>
import CivLeader from '@/components/CivLeader.vue';
import { ref } from 'vue';

const knowMore = ref(false)

function toggleMore(){
    knowMore.value = !knowMore.value
}

</script>

<style scoped>
.knowMore {
    position: absolute;
    right: 0px;
    height: 500px;
    top: 50%;
}
</style>