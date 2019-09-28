<template>
  <g class="node" :class="{show}">
    <line :x1="data.x" :y1="data.y" :x2="newX" :y2="newY" stroke="black" v-if="selected" style="pointer-events: none;"/>

    <foreignObject
      @click="onClick"
      :id="data.id"
      :x="(draggingX || data.x) - w/2"
      :y="(draggingY || data.y) - h/2"
      :width="w"
      :height="h"
    >
      <div
        class="walls"
        :class="{selected}"
      >
        <div class="wall top"></div>
        <div class="wall right"></div>
        <div class="wall bottom"></div>
        <div class="wall left"></div>

        <div class="corner top"></div>
        <div class="corner right"></div>
        <div class="corner bottom"></div>
        <div class="corner left"></div>
      </div>
    </foreignObject>
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
      w: 50,
      h: 50,
      draggingX: null,
      draggingY: null,
      down: false,
      dragging: false,
      newX: null,
      newY: null
    };
  },
  computed: {
    show() {
      return (this.$store.state.depotEditor.displayMode == 'floor' && this.data.floor == this.$store.state.depotEditor.floor)
        ||   (this.$store.state.depotEditor.displayMode == 'depot' && this.data.depot == this.$store.state.depotEditor.depot.id);
    }
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
<style lang="scss">

.node {
  display: none;
}
.node.show {
  display: block;
}

.walls {
    position: relative;
    display: block;
    width: 100%;
    height: 100%;
    background-color: #FFF;
    cursor: pointer;
}
.walls.selected {
  background-color: #f00;
}
.wall, .corner {
    position: absolute;
    background-color: rgba(0,0,0,.5);
}
.wall.left,
.wall.right,
.corner { width: 5px; }
.wall.left,
.wall.right { height: 100%; }
.wall.top,
.wall.bottom,
.corner { height: 5px; }
.wall.top,
.wall.bottom { width: 100%; }
.walls .top { top: 0; bottom: auto; }
.walls .bottom { top: auto; bottom: 0; }
.walls .left { left: 0; right: auto; }
.walls .right { left: auto; right: 0; }

</style>
