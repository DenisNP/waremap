<template>
  <g class="CarNode">
    <foreignObject v-if="nodes && show"
      :x="x - $store.state.icons.node['Car'].w / 2"
      :y="y - $store.state.icons.node['Car'].w / 2"
      :width="$store.state.icons.node['Car'].w"
      :height="$store.state.icons.node['Car'].w"
    >
      <div class="car-icon">
        <img :src="$store.state.icons.node['Car'].i" />
      </div>
    </foreignObject>
  </g>
</template>

<script>

export default {
  name: 'Detail',
  props: [
    'data'
  ],
  computed: {
    nodes() {
      if (!this.data) return false;
      let position = this.data.path[this.data.position];
      if (!position) return false;
      return {
        from: this.$store.getters['serverState/nodeById'](position.from_node),
        to: this.$store.getters['serverState/nodeById'](position.to_node)
      };
    },
    x() {
      return this.nodes.from.x + (this.nodes.to.x - this.nodes.from.x) / 2;
    },
    y() {
      return this.nodes.from.y + (this.nodes.to.y - this.nodes.from.y) / 2;
    },
    floor() {
      return this.nodes.from ? this.nodes.from.floor : false;
    },
    show() {
      if (this.floor) {
        return (this.$store.state.editor.displayMode == 'floor' && this.floor == this.$store.state.editor.floor);
      }
    }
  }
}

</script>

<style>

.CarNode {
  pointer-events: none;
}

</style>