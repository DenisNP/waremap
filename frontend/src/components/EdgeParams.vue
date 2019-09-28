<template>
  <div class="controls">
    <div class="pallete tools">
    </div>

    <div class="pallete pallete-right floors">
      <div>Тип связи</div>
      <select :value="data.type">
        <option :value="'Road'">Дорога для тележки</option>
        <option :value="'Footway'">Проход для человека</option>
        <option :value="'Elevator'">Лифт</option>
        <option :value="'Ladder'">Лестница</option>
      </select>
      <div>Вес</div>
      <input type="text" :value="data.weight" @input="setWeight">
      <br>
      <button @click="save">Сохранить</button>
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
        weight: this.data.weight,
        type: this.data.type,
      };
    },
    computed: {

    },
    methods: {
      setWeight(e) {
        this.weight = e.target.value;
      },
      setType(e) {
        this.type = e.target.value;
      },
      async save() {
        await this.$store.dispatch('depotEditor/updateEdge', {
          ...this.data,
          weight: this.weight
        });
        this.$store.commit('depotEditor/unselect');
      }
    }
  }

</script>

<style>

</style>
