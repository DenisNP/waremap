<template>
  <g class="FloorToFloorEdge" v-if="$store.state.editor.FloorToFloorEdge">
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