<template>
    <v-card :class="[theme.global.name.value === 'dark' ? 'attributeD' : 'attributeL']">
        <v-card-title>
            {{ attribute.attributeType }}: {{ attribute.abilityName }}
        </v-card-title>
        <v-card-text>
            <span style="white-space: pre-wrap;">{{ attribute.description }}</span>
        </v-card-text>
    </v-card>
</template>

<script lang="ts" setup>
import type { CivAttribute } from '@/scripts/civAttribute';
import { useTheme } from 'vuetify/lib/framework.mjs'

const theme = useTheme()
const props = defineProps<{
    attribute: CivAttribute
}>()

</script>