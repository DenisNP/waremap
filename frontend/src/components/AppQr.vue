<template>
  <div>
    <div class="container">
      <div
        v-for="(data, index) in qrs"
        :key="index"
        :src="data.base64"
        class="qr-wrapper"
      >
        <img :src="data.base64" />
        <div>{{ data.part_name }}</div>
        <div>{{ data.operation_name }}</div>
      </div>
    </div>
  </div>
</template>

<script>
import {postJson, postData, getData} from '../common/helpers';
import config from '../common/config';
const QRCode = require('qrcode');

const ENDPOINT = config.backendUrl;

export default {
  name: 'app',
  components: {
  },
  data() {
    return {
      qrs: []
    };
  },
  async mounted() {
    var urlParams = new URLSearchParams(window.location.search);

    const machineId = urlParams.get('id');

    //const data = await getData(ENDPOINT + '/machine?id=');
    const data = [{
      part_id: 999,
      part_name: 'Тапок',
      operation_name: 'Одевание'
    }, {
      part_id: 666,
      part_name: 'Сосиска в тесте',
      operation_name: 'Поднимание'
    }];

    data.forEach(item => {
      const content = `${item.part_id}_${machineId}`;
      QRCode.toDataURL(content, {
        errorCorrectionLevel: 'H',
        width: 400,
        height: 400,
        margin: 2
      }, (err, base64) => {
        if (err) {
          return console.error(err);
        }
        this.qrs.push({
          ...item,
          base64,
        });
      });
    });
  },
  computed: {
  }
};
</script>

<style lang="scss" scoped>
  img {
    margin: 40px;
  }

  .container {
    display: flex;
    justify-content: center;
  }

  .qr-wrapper {
    flex-direction: column;
    text-align: center;
    color: #fff;
    font-size: 20px;
  }
</style>
