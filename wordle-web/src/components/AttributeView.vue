<template>
  <v-card :class="[theme.global.name.value === 'dark' ? 'attributeD' : 'attributeL']">
    <v-card-title> {{ attributeType }}: {{ abilityName }} </v-card-title>
    <v-card-text>
      <span style="white-space: pre-wrap">{{ description }}</span>
    </v-card-text>
  </v-card>
</template>

<script lang="ts" setup>
import { useTheme } from 'vuetify/lib/framework.mjs'

const theme = useTheme()
const props = defineProps<{
  attributeType: string
  abilityName: string
  description: string
}>()
</script>
