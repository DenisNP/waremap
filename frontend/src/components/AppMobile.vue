<template>
  <div class="container">
    <div v-show="!isScanning">
      <button @click="showScanner">Отметиться через QR код</button>
      <br>
      <div v-show="Boolean(code)">
        Вы отметились {{ checkpointDate }}!
        <br>
        <small>
          (Отсканированный QR код: {{ code }})
        </small>
      </div>
      <br>
      <div>Ваше местоположение: {{ position && position.current.node_name }}</div>
      <br>
      <div>Точка назначения: {{ position && position.next.node_name }}</div>
    </div>
    <div class="qr-scanner" v-show="isScanning">
      <div ref="loadingMessage"></div>
      <canvas ref="canvas" hidden></canvas>
      <div ref="output" hidden>
        <div ref="outputMessage">No QR code detected.</div>
        <div hidden><b>Data:</b> <span ref="outputData"></span></div>
      </div>
    </div>
  </div>
</template>

<script>
const jsQR = require('jsqr');
import {postJson, postData, getData} from '../common/helpers';
import config from '../common/config';
import API from '../common/api';

const ENDPOINT = config.backendUrl;

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
    };
  },
  async mounted() {
    setInterval(async () => {
      const res = await API.api('GET', 'position');
      this.position = res;
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
      this.code = code;
      this.checkpointDate = new Date().toLocaleString('ru');
      this.hideScanner();

      // const content = `${item.part_id}_${machineId}_${item.operation_id}`;
      const [part_id, machine_id, operation_id] = code.split('_');

      const res = await API.api('POST', 'position', {
        part_id: Number(part_id),
        machine_id: Number(machine_id),
        operation_id: Number(operation_id),
      });
      console.log('send checkpoint res', res);
    },
    async showScanner() {
      this.code = null;
      this.isScanning = true;

      var video = document.createElement('video');
      var canvasElement = this.$refs.canvas;
      var canvas = canvasElement.getContext('2d');
      var loadingMessage = this.$refs.loadingMessage;
      var outputContainer = this.$refs.output;
      var outputMessage = this.$refs.outputMessage;
      var outputData = this.$refs.outputData;

      // Use facingMode: environment to attemt to get the front camera on phones
      const stream = await navigator.mediaDevices.getUserMedia({
        video: {
          facingMode: 'environment',
          width: Math.max(screen.width, screen.height),
          height: Math.min(screen.width, screen.height),
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
        loadingMessage.innerText = 'Loading video...';
        if (video.readyState === video.HAVE_ENOUGH_DATA) {
          loadingMessage.hidden = true;
          canvasElement.hidden = false;
          outputContainer.hidden = false;

          canvasElement.height = video.videoHeight;
          canvasElement.width = video.videoWidth;
          canvas.drawImage(video, 0, 0, canvasElement.width, canvasElement.height);
          var imageData = canvas.getImageData(0, 0, canvasElement.width, canvasElement.height);
          var code = jsQR(imageData.data, imageData.width, imageData.height, {
            inversionAttempts: "dontInvert",
          });
          if (code) {
            this.sendCheckpoint(code.data);
          } else {
            outputMessage.hidden = false;
            outputData.parentElement.hidden = true;
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
  .container {
    color: #fff;
    font-size: 20px;
  }

  button {
    padding: 20px;
    margin: 20px;
    font-size: 20px;
  }
</style>
