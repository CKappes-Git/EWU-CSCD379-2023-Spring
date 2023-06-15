<template>
    <v-btn :class="[clicked? 'buttonGreen' : 'buttonRed']" @click="changeColor">{{ name }}</v-btn>
</template>

<script setup lang="ts">
import { ref } from 'vue';

const clicked = ref(false)
const props = defineProps<{
    name: string
}>()

function changeColor(){
    clicked.value = !clicked.value
}

</script>

<style scoped>
.buttonGreen{
    background-color: green;
}
.buttonRed{
    background-color: red;
}
</style>
