<template>
  <div class="controls">
    <div class="pallete tools">
    </div>

    <div class="pallete pallete-right floors">
      <div>Возможные операции</div>
      <div
           v-for="operation in data.operations"
           :key="operation.id"
      >
        <span class="pallete-item-name">
            {{ operation.title }}
        </span>
      </div>
      <select>
        <option value="">Сверление</option>
        <option>Точение</option>
        <option>Фрезерование</option>
      </select>
      <div class="pallete-item" :class="{selected: selectedFloor === 'new'}" @click="selectedFloor = 'new'">+</div>
    </div>
  </div>
</template>

<script>

  export default {
    name: 'MachineParams',
    props: [
      'data',
    ],
    data() {
    },
    computed: {

    },
    mounted() {
    },
    methods: {
    }
  }

</script>

<style>

</style>
