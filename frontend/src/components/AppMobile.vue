<template>
  <div class="root">
    <div class="main-page" v-show="!isScanning">
      <button @click="showScanner">Отметиться через QR код</button>
      <br>
      <div v-show="wrongQrCode" class="error msg">
        Ошибочный QR-код
      </div>
      <div v-show="Boolean(code)">
        <div class="success msg">
          Вы отметились {{ checkpointDate }}!<br>
          <span class="small">QR код: {{ code }}</span>
        </div>
      </div>
      <br>
      <div class="baloon">
        <span class="small">Ваше местоположение:</span>
        <br>
        <span>{{ position && position.current.node_name || '...' }}</span>
      </div>
      <div class="arrow">↓</div>
      <div class="baloon">
        <span class="small">Точка назначения:</span>
        <br>
        <span>{{ position && position.next.node_name || '...' }}</span>
      </div>
    </div>
    <div class="qr-scanner" v-show="isScanning">
      <button @click="hideScanner">Назад</button>
      <div ref="loadingMessage"></div>
      <div>
        <canvas ref="canvas" hidden></canvas>
      </div>
    </div>
  </div>
</template>

<script>
const jsQR = require('jsqr');
import {postJson, postData, getData} from '../common/helpers';
import config from '../common/config';
import API from '../common/api';

export default {
  name: 'app',
  components: {
  },
  data() {
    return {
      myPosition: '-',
      targetPosition: '-',
      isScanning: false,
      animationFrame: null,
      code: null,
      stream: null,
      position: null,
      checkpointDate: null,
      wrongQrCode: false,
    };
  },
  async mounted() {
    this.position = await API.api('GET', 'position');

    setInterval(async () => {
      this.position = await API.api('GET', 'position');
    }, 5000);
  },
  computed: {
  },
  methods: {
    hideScanner() {
      this.isScanning = false;

      let tracks = this.stream.getTracks();
      tracks.forEach(function(track) {
        track.stop();
      });
      this.stream = null;
      cancelAnimationFrame(this.animationFrame);
    },
    async sendCheckpoint(code) {
      this.hideScanner();

      // const content = `${item.part_id}_${machineId}_${item.operation_id}`;
      const arr = code.split('_').map(Number);

      if (arr.length !== 3 || arr.some(a => !Number.isInteger(a))) {
        // wrong QR code
        this.wrongQrCode = true;
        return;
      }

      this.code = code;
      this.checkpointDate = new Date().toLocaleString('ru');

      const [part_id, machine_id, operation_id] = arr;

      const res = await API.api('POST', 'position', {
        part_id: Number(part_id),
        machine_id: Number(machine_id),
        operation_id: Number(operation_id),
      });
      console.log('send checkpoint res', res);
    },
    async showScanner() {
      this.code = null;
      this.wrongQrCode = null;
      this.isScanning = true;

      var video = document.createElement('video');
      var canvasElement = this.$refs.canvas;
      var canvas = canvasElement.getContext('2d');
      var loadingMessage = this.$refs.loadingMessage;

      // Use facingMode: environment to attemt to get the front camera on phones
      const stream = await navigator.mediaDevices.getUserMedia({
        video: {
          facingMode: 'environment',
          width: 320,
          height: 320,
          frameRate: {
            ideal: 60,
            max: 60
          }
        }
      });
      this.stream = stream;
      video.srcObject = stream;
      video.setAttribute('playsinline', true); // required to tell iOS safari we don't want fullscreen
      video.play();

      const tick = () => {
        loadingMessage.innerText = 'Инициализируем камеру...';
        if (video.readyState === video.HAVE_ENOUGH_DATA) {
          loadingMessage.hidden = true;
          canvasElement.hidden = false;

          canvasElement.height = video.videoHeight;
          canvasElement.width = video.videoWidth;
          canvas.drawImage(video, 0, 0, canvasElement.width, canvasElement.height);
          var imageData = canvas.getImageData(0, 0, canvasElement.width, canvasElement.height);
          var code = jsQR(imageData.data, imageData.width, imageData.height, {
            inversionAttempts: "dontInvert",
          });
          if (code) {
            this.sendCheckpoint(code.data);
          }
        }
        this.animationFrame = window.requestAnimationFrame(tick);
      };

      this.animationFrame = window.requestAnimationFrame(tick);
    }
  }
};
</script>

<style lang="scss" scoped>
  .root {
    position: absolute;
    left: 0;
    top: 0;
    width: 100%;
    min-height: 100%;
    background: rgb(56, 120, 255);
    color: #fff;
    font-size: 1.6em;
  }

  button {
  }

  .root .main-page {
    display: flex;
    flex-direction: column;
    justify-content: center;
  }

  .root .qr-scanner {
    display: flex;
    flex-direction: column;
    justify-content: flex-start;
  }

  .root div * {
    text-align: center;
  }

  button {
    padding: 20px;
    margin: 20px;
    font-size: 20px;
  }

  .msg {
    padding: 14px;
  }

  .msg.error {
    background: darkred;
  }

  .msg.success {
    background: green;
  }

  .small {
    font-size: 16px;
    opacity: 0.5;
  }

  .baloon {
    background: #ffffff;
    border-radius: 40px;
    padding: 10px 10px 20px 10px;
    margin: 10px 20px;
    color: #000;
  }

  button {
    cursor: pointer;
  }

  .arrow {
    font-size: 40px;
    font-weight: bold;
  }
</style>
