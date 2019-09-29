<template>
  <ul>
    <li v-for="item in data">{{ item.name }}</li>
  </ul>
</template>

<script>

export default {
  name: 'OperationsList',
  props: [
    'data'
  ],
  data() {
  },
  methods: {
  },
  mounted() {
  },
}

</script>

<style>

</style>
