<template>
  <rect :x="draggingX || data.x" :y="draggingY || data.y" :width="w" :height="h" :fill="selected ? '#f00' : '#000'"/>
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
    };
  },
  methods: {
    onMouseDown(e) {
      e.preventDefault();
      this.down = true;
    },
    onMouseMove(e) {
      if (this.down !== true) return;

      const newX = e.clientX - this.w / 2;
      const newY = e.clientY - this.h / 2;

      if (!this.dragging) {
        const distance = Math.max(
          Math.abs(this.data.x - newX),
          Math.abs(this.data.y - newY)
        );

        if (distance < MIN_DRAGGING_DISTANCE) {
          return;
        }
      }

      this.dragging = true;
      this.draggingX = newX;
      this.draggingY = newY;
    },
    onMouseUp(e) {
      if (this.down && !this.dragging) {
        // we just selected
        this.$root.$emit('nodeSelected', this.data);
      }

      this.down = false;
      this.dragging = false;
      this.$root.$emit('nodeUpdated', {...this.data, x: this.draggingX, y: this.draggingY});
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
