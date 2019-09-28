<template>
  <div class="pallete pallete-right PartsList">
    <template v-for="group in assemblies">
      <div class="pallete-heading" @click="showAssemblyNodes(group.id)">{{ group.name }}</div>
      <div class="pallete-item"
        v-for="detail in group.details"
        :key="detail.id"
        @click="showDetailNodes(detail)"
      >
        <span class="pallete-item-icon">&laquo;</span>
        <span class="pallete-item-name">
          {{ detail.name }}
        </span>
      </div>
    </template>
  </div>
</template>

<script>
  export default {
    name: 'PartsList',
    props: [
      'data',
    ],
    data() {
      return {
        
      };
    },
    computed: {
      assemblies() {
        console.log(this.$store.state.serverState.equipment)
        let assemblies = {};
        this.$store.state.serverState.equipment.parts.map((detail) => {
          if (!(detail.assembly_id in assemblies)) {
            assemblies[detail.assembly_id] = {
              id: detail.assembly_id,
              name: '',
              details: [detail]
            };
          } else {
            assemblies[detail.assembly_id].details.push(detail);
          }
        });
        Object.keys(assemblies).map((id) => {
          let assembly = this.$store.state.serverState.equipment.assemblies.find(r => r.id == id);
          if (assembly) {
            assemblies[id].name = assembly.name;
          }
        });
        return assemblies;
      }
    },
    mounted() {      
    },
    methods: {
      showAssemblyNodes(id) {
        let details = this.assemblies[id].details.map(detail => detail);
        // console.log(details);
        this.$store.commit('editor/highlightNodes', ['5', '3']);
      },
      showDetailNodes(detail) {
        // console.log(detail);
        this.$store.commit('editor/highlightNodes', ['6']);
      }
    }
  }

</script>

<style>

</style>
