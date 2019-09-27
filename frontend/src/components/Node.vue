<template>
  <rect :x="down ? x : data.x" :y="down ? y : data.y" width="10" height="10"/>
</template>

<script>
export default {
  name: 'Node',
  props: [
    'data',
  ],
  data() {
    return {
      down: false,
      x: null,
      y: null
    };
  },
  methods: {
    onMouseDown(e) {
      this.down = true;
      this.x = e.clientX;
      this.y = e.clientY;
    },
    onMouseMove(e) {
      if (this.down !== true) return;

      this.x = e.clientX;
      this.y = e.clientY;
    },
    onMouseUp(e) {
      this.down = false;
    }
  },
  mounted() {
    this.x = this.data.x;
    this.y = this.data.y;

    this.$el.addEventListener('mousedown', this.onMouseDown);
    window.addEventListener('mousemove', this.onMouseMove);
    this.$el.addEventListener('mouseup', this.onMouseUp);
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
