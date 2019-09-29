<template>
  <g class="FloorToFloorEdge" v-if="show">
    <line class="edge edge-Dashed"
      :x1="$store.state.editor.FloorToFloorEdge.floorPos.x"
      :y1="$store.state.editor.FloorToFloorEdge.floorPos.y"
      :x2="endX" :y2="endY" style="pointer-events: none;"/>
  </g>
</template>

<script>

export default {
  name: 'FloorToFloorEdge',
  props: [
    'data'
  ],
  data() {
    return {
      endX: null,
      endY: null
    }
  },
  computed: {
    show() {
      let floorToFloorEdge = this.$store.state.editor.FloorToFloorEdge;
      return floorToFloorEdge
          && floorToFloorEdge.fromNode
          && floorToFloorEdge.fromNode.floor != this.$store.state.editor.floor
          && (['Ladder', 'Elevator'].indexOf(floorToFloorEdge.fromNode.icon) > -1)
          && (this.$store.state.editor.mode == 'nodeSelected');
    }
  },
  methods: {
    onMouseMove(e) {
      this.endX = e.clientX;
      this.endY = e.clientY;
    }
  },
  mounted() {
    if (this.$store.state.editor.FloorToFloorEdge) {
      this.endX = this.$store.state.editor.FloorToFloorEdge.floorPos.x;
      this.endY = this.$store.state.editor.FloorToFloorEdge.floorPos.y;
    }

    window.addEventListener('mousemove', this.onMouseMove);
  },
  destroyed() {
    window.removeEventListener('mousemove', this.onMouseMove);
  }
}

</script>

<style>

</style>