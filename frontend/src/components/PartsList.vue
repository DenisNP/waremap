<template>
  <div class="pallete pallete-right PartsList">
    <div class="scrollable">
      <template v-for="group in assemblies">
        <div
          class="pallete-heading"
          :class="{bold: selectedAssemblyId === group.id}"
          @click="showAssemblyNodes(group.id)">{{ group.name }}
        </div>
        <div class="pallete-item"
          v-for="detail in group.details"
          :key="detail.id"
          :class="{bold: selectedDetailId === detail.id}"
          @click="showDetailNodes(detail)"
        >
          <span class="pallete-item-icon">—</span>
          <span class="pallete-item-name">
            {{ detail.name }}
          </span>
        </div>
      </template>
    </div>
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
        selectedDetailId: null,
        selectedAssemblyId: null,
      };
    },
    computed: {
      assemblies() {
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
          if (assembly && assembly.name) {
            assemblies[id].name = assembly.name;
          } else {
            assemblies[id].name = 'Детали без сборки';
          }
        });
        return assemblies;
      }
    },
    mounted() {
    },
    methods: {
      showAssemblyNodes(id) {
        this.selectedDetailId = null;
        this.selectedAssemblyId = id;

        let details = this.assemblies[id].details.map(detail => detail);
        // console.log(details);
        this.$store.commit('editor/highlightNodes', details.map(detail => detail.roadmap.position));
      },
      showDetailNodes(detail) {
        this.selectedAssemblyId = null;
        this.selectedDetailId = detail.id;
        // console.log(detail);
        this.$store.commit('editor/highlightNodes', [detail.roadmap.position]);

        /*
        detail.roadmap.path = [
          {from: 10, to: 12},
          {from: 12, to: 17},
          {from: 17, to: 11},
          {from: 11, to: 14},
          {from: 14, to: 15}
        ];
        */
        this.$store.commit('editor/highlightedEdges', detail.roadmap.path);
      }
    }
  }

</script>

<style>
.PartsList {
}
.PartsList .scrollable {
  overflow-y: auto;
  height: calc(100% - 70px);
}
.bold {
  font-weight: bold;
}
</style>
