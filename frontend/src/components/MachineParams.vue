<template>
  <div class="pallete pallete-right MachineParams">
    <div class="pallete-heading-top">
      Участок
      <span v-if="data.id">№{{ data.id }}.</span>
      <span v-else>без номера.</span>
    </div>
    <div class="pallete-heading">Возможные операции</div>

    <div class="pallete-item"
      v-for="operation in $store.state.serverState.equipment.operations"
      :key="operation.id"
      :class="{selected: machineOperationsObject[operation.id]}"
      @click="setOperation(operation.id)"
    >
      <span class="pallete-item-icon"></span>
      <span class="pallete-item-name">
        {{ operation.name }}
      </span>
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
      return {
        operation_ids: this.data.operation_ids,
        machineOperationsObject: {} 
      };
    },
    computed: {
    },
    mounted() {
      let operations = {};
      this.operation_ids.map((id) => {
        operations[id] = true;
      });
      this.machineOperationsObject = operations;
    },
    methods: {
      setOperation(id) {
        this.machineOperationsObject[id] = !this.machineOperationsObject[id];
        this.operation_ids = Object.keys(this.machineOperationsObject).filter((id) => {
          return this.machineOperationsObject[id];
        });
        this.save();
      },
      async save() {
        await this.$store.dispatch('editor/updateNode', {
          ...this.data,
          operation_ids: this.operation_ids
        });
      }
    }
  }

</script>

<style>

</style>
