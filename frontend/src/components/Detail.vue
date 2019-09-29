<template>
  <g class="detail">
    <foreignObject v-if="nodes && show"
      :id="data.id"
      :x="x"
      :y="y - 5"
      :width="$store.state.icons.node['Detail'].w"
      :height="$store.state.icons.node['Detail'].w"
    >
      <div class="detail-icon">
        <img :src="$store.state.icons.node['Detail'].i" />
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
      let position = this.data.roadmap.path[this.data.roadmap.position];
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

.detail {
  pointer-events: none;
}

</style>