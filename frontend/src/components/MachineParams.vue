<template>
  <div class="controls">
    <div class="pallete tools">
    </div>

    <div class="pallete pallete-right floors">
      <div>Возможные операции</div>
      <div
        v-for="operation in operationTypes"
        :key="operation.id"
      >
        <span class="pallete-item-name">
          <label>
            <input type="checkbox" @input="onCheck" :value="operation.id" :checked="operation_ids.includes(operation.id)"/>
            {{ operation.name }}
          </label>
        </span>
      </div>
      <br>
      <button @click="save">Сохранить</button>
    </div>
  </div>
</template>

<script>
  const operationTypes = require('../../../shared/operations.json');

  export default {
    name: 'MachineParams',
    props: [
      'data',
    ],
    data() {
      return {
        operationTypes,
        operation_ids: this.data.operation_ids,
      };
    },
    computed: {

    },
    mounted() {
    },
    methods: {
      onCheck(e) {
        const id = Number(e.currentTarget.value);
        if (!this.operation_ids.includes(id)) {
          this.operation_ids.push(id);
        }
        console.log('new ids', this.operation_ids);
      },
      async save() {
        await this.$store.dispatch('editor/updateNode', {
          ...this.data,
          operation_ids: this.operation_ids
        });
        this.$store.commit('editor/unselect');
      }
    }
  }

</script>

<style>

</style>
