<template>
  <div class="pallete edges">
    <div class="pallete-heading">Тип связи</div>
    <div class="inline-group">
      <div class="edge-type"
        v-for="item in types"
        :key="item.key"
        :class="{selected: item.key === type}"
        @click="setType(item.key)"
      >
        <img :src="item.icon.i" :width="item.icon.w" />
      </div>
    </div>

    <div class="inline-group">
      <div class="pallete-heading">Длина:</div>
      <input class="edge-weight-input" type="text" :value="data.weight" @input="setWeight">
    </div>
  </div>
</template>

<script>

  export default {
    name: 'EdgeParams',
    props: [
      'data',
    ],
    data() {
      return {
        types: [
          {key: 'Road',     name: 'Дорога для тележки',   icon: this.$store.state.icons.edgeParams.Road},
          {key: 'Footway',  name: 'Проход для человека',  icon: this.$store.state.icons.edgeParams.Footway},
          {key: 'Elevator', name: 'Лифт',                 icon: this.$store.state.icons.edgeParams.Elevator},
          {key: 'Ladder',   name: 'Лестница',             icon: this.$store.state.icons.edgeParams.Ladder}
        ],
        weight: this.data.weight,
        type: this.data.type,
      };
    },
    computed: {

    },
    methods: {
      setType(type) {
        this.type = type;
        this.save();
      },
      setWeight(e) {
        this.weight = e.target.value;
        this.save();
      },
      async save() {
        await this.$store.dispatch('editor/updateEdge', {
          ...this.data,
          weight: this.weight,
          type: this.type,
        });
      }
    }
  }

</script>

<style>

</style>
