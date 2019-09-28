<template>
  <g>
    <line :x1="data.x" :y1="data.y" :x2="newX" :y2="newY" stroke="black" v-if="selected" />
    <rect
      :id="data.id"
      :x="(draggingX || data.x) - w/2"
      :y="(draggingY || data.y) - h/2"
      :width="w"
      :height="h"
      :fill="selected ? '#f00' : '#000'"
      @click="onClick"
      cursor="pointer"
    />
  </g>
</template>

<script>
const MIN_DRAGGING_DISTANCE = 50;

export default {
  name: 'Node',
  props: [
    'data',
    'selected',
  ],
  data() {
    return {
      w: 30,
      h: 30,
      draggingX: null,
      draggingY: null,
      down: false,
      dragging: false,
      newX: null,
      newY: null
    };
  },
  methods: {
    onClick(e) {
      e.preventDefault();
      e.stopPropagation();

      console.log('11 click', this.data.id);
      if (this.$store.state.depotEditor.mode === 'nodeSelected' && this.data.id !== this.$store.state.depotEditor.selectedNodeId) {
        this.$store.dispatch('depotEditor/createEdge', {
          from: this.$store.state.depotEditor.selectedNodeId,
          to: this.data.id
        });
        this.$store.commit('depotEditor/unselect');
      }
    },
    onMouseDown(e) {
      e.preventDefault();
      this.down = true;
    },
    onMouseMove(e) {
      this.newX = e.clientX;
      this.newY = e.clientY;

      if (this.down !== true) return;

      if (!this.dragging) {
        const distance = Math.max(
          Math.abs(this.data.x - this.newX),
          Math.abs(this.data.y - this.newY)
        );

        if (distance < MIN_DRAGGING_DISTANCE) {
          return;
        }
      }

      this.dragging = true;
      this.draggingX = this.newX;
      this.draggingY = this.newY;

      this.$store.commit('serverState/updateNode', {
        id: this.data.id,
        x: this.draggingX,
        y: this.draggingY
      });
      this.$store.commit('depotEditor/unselect');
    },
    onMouseUp(e) {
      const wasDown = this.down;
      this.down = false;

      if (wasDown && !this.dragging && !this.$store.state.depotEditor.selectedNodeId) {
        // we just selected
        this.$root.$emit('nodeSelected', this.data);
        return;
      }

      if (this.dragging) {
        console.log('update node');
        this.$root.$emit('nodeUpdated', {...this.data, x: this.draggingX, y: this.draggingY});
        this.dragging = false;
      }
    }
  },
  mounted() {
    this.$el.addEventListener('mousedown', this.onMouseDown);
    window.addEventListener('mousemove', this.onMouseMove);
    this.$el.addEventListener('mouseup', this.onMouseUp);
  },
  updated() {
    this.data.draggingX = null;
    this.data.draggingY = null;
  },
  destroyed() {
    this.$el.removeEventListener('mousedown', this.onMouseDown);
    window.removeEventListener('mousemove', this.onMouseMove);
    this.$el.removeEventListener('mouseup', this.onMouseUp);
  }
};
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped lang="scss">
h3 {
  margin: 40px 0 0;
}
ul {
  list-style-type: none;
  padding: 0;
}
li {
  display: inline-block;
  margin: 0 10px;
}
a {
  color: #42b983;
}
</style>
