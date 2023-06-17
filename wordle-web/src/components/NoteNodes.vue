<template>
  <div style="width: 100%; margin-top: 10px">
    <v-card-title v-if="tree.length != 0 || editMode">
      {{ treeType }}
    </v-card-title>
    <v-row style="margin-top: 5px">
      <div style="margin-left: 5%" v-if="!editMode">
        <v-row v-if="tree.length != 0">
          <v-col v-for="(node, index) in tree">
            <NodeButton :name="node" />
          </v-col>
        </v-row>
      </div>
      <div style="width: 100%" v-else>
        <div v-if="tree.length == 0">
          <v-btn @click="tree.push('')" style="width: 2%; background-color: green">+</v-btn>
        </div>
        <div v-else style="width: 100%" v-for="(node, index) in tree">
          <div style="width: 100%; margin-bottom: 20px">
            <v-row>
              <v-text-field v-model="tree[index]"></v-text-field>
              <v-btn style="width: 2%; background-color: red" @click="tree.splice(index, 1)"
                >-</v-btn
              >
            </v-row>
            <v-btn style="width: 2%; background-color: green" @click="tree.splice(index + 1, 0, '')"
              >+</v-btn
            >
          </div>
        </div>
      </div>
    </v-row>
  </div>
</template>

<script setup lang="ts">
import NodeButton from './NodeButton.vue'

const props = defineProps<{
  treeType: string
  editMode: boolean
  tree: string[]
}>()
</script>

<style scoped>
@import './style.css';
</style>
