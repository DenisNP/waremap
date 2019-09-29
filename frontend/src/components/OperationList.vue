<template>
  <div class="operationsList">
    <div class="operationsList-item" v-for="(item, index) in list" :key="index" v-if="item.operation && item.operation.name">{{ item.operation.name }}</div>
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

<style>

.pallete.PartsList .pallete-item {
  flex-wrap: wrap;
}
.operationsList {
  display: block;
}
.operationsList-item {
  padding: 5px 15px 0;
  font-size: .8em;
  line-height: 16px;
}

</style>
