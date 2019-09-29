<template>
  <div class="root">
    <div v-for="item in list" :key="">{{ item.operation && item.operation.name || '?' }}</div>
  </div>
</template>

<script>

export default {
  name: 'OperationsList',
  props: [
    'detail'
  ],
  computed: {
    list() {
      return this.detail.roadmap.path.map(item => {
        return {
          ...item,
          operation: this.$store.state.serverState.equipment.operations.find(op => op.id === item.operation_id)
        };
      });
    }
  },
  methods: {
  },
  mounted() {
  },
}

</script>

<style scoped>
.root {
}
</style>
