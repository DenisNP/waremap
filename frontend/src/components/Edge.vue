<template>
  <g
    cursor="pointer"
    @click="onClick"
    v-show="show"
  >
    <path
        :class="{
          edge: true,
          highlighted,
          selected,
          ['edge-'+data.type]: true
        }"
        :d="'M' + (fromNode.x - gap) + ',' + (fromNode.y)
          + 'L' + (toNode.x - gap) + ',' + (toNode.y)
          + 'L' + (toNode.x + gap) + ',' + (toNode.y)
          + 'L' + (fromNode.x + gap) + ',' + (fromNode.y)"
          />
    <line
      :x1="fromNode.x"
      :y1="fromNode.y"
      :x2="toNode.x"
      :y2="toNode.y"
      stroke-width="20"
      stroke="rgba(0,0,0,0.05)"
    />
    <g class="edge-weight">
      <text align="center" class="edge-weight-value"
        :x="fromNode.x + (toNode.x - fromNode.x) / 2 - 7"
        :y="fromNode.y + (toNode.y - fromNode.y) / 2 + 3"
      >
        {{ data.weight }}
      </text>
    </g>
  </g>
</template>

<script>
export default {
  name: 'Edge',
  props: [
    'data',
    'highlighted'
  ],
  data() {
    return {
      gap: 2
    }
  },
  methods: {
    onClick(e) {
      e.preventDefault();
      e.stopPropagation();
      this.$store.commit('editor/selectEdge', this.data);
    },
  },
  computed: {
    id() {
      return this.data.from + '_' + this.data.to;
    },
    show() {
      const from = this.$store.getters['serverState/nodeById'](this.data.from);
      const to = this.$store.getters['serverState/nodeById'](this.data.to);

      return (this.$store.state.editor.displayMode === 'floor' && from.floor === this.$store.state.editor.floor && to.floor === this.$store.state.editor.floor)
          || (this.$store.state.editor.displayMode === 'depot' && from.depot === this.$store.state.editor.depot.id);
    },
    selected() {
      const selected = this.$store.state.editor.selectedEdge;
      return selected && selected.from === this.data.from && selected.to === this.data.to;
    },
    fromNode() {
      return this.$store.getters['serverState/nodeById'](this.data.from);
    },
    toNode() {
      return this.$store.getters['serverState/nodeById'](this.data.to);
    },
  }
};
</script>

<style>

.edge-weight-value {
  font-size: 11px;
  fill: #FFF;
  stroke: #333;
  stroke-width: 1px;
  stroke-linecap: round;
  font-weight: 900;
}

</style>
