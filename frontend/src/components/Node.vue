<template>
  <rect :x="data.x" :y=" data.y" :width="w" :height="h"/>
</template>

<script>
export default {
  name: 'Node',
  props: [
    'data',
  ],
  data() {
    return {
      w: 30,
      h: 30,
      down: false
    };
  },
  methods: {
    onMouseDown(e) {
      this.down = true;
      this.data.x = e.clientX - this.w / 2;
      this.data.y = e.clientY - this.h / 2;
    },
    onMouseMove(e) {
      if (this.down !== true) return;

      this.data.x = e.clientX - this.w / 2;
      this.data.y = e.clientY - this.h / 2;
    },
    onMouseUp(e) {
      this.down = false;
      this.$root.$emit('nodeUpdated', this.data);
    }
  },
  mounted() {
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
