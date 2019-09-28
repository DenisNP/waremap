<template>
  <div class="container">
    <div v-if="!isScanning">
      <button @click="showScanner">Отметиться через QR код</button>
      <br>
      <div v-show="Boolean(code)">Отсканированный QR код: {{ code }}</div>
      <br>
      <div>Ваше местоположение: {{ myPosition }}</div>
      <br>
      <div>Точка назначения: {{ targetPosition }}</div>
    </div>
    <div class="qr-scanner" v-show="isScanning">
      <div ref="loadingMessage">Unable to access video stream (please make sure you have a webcam enabled)</div>
      <canvas ref="canvas" hidden></canvas>
      <div ref="output" hidden>
        <div ref="outputMessage">No QR code detected.</div>
        <div hidden><b>Data:</b> <span ref="outputData"></span></div>
      </div>
    </div>
  </div>
</template>

<script>
import {postJson, postData, getData} from '../common/helpers';
import config from '../common/config';
const jsQR = require('jsqr');

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
      code: null
    };
  },
  async mounted() {
    var urlParams = new URLSearchParams(window.location.search);

    // const carId = urlParams.get('id');
    // const data = await getData(ENDPOINT + '/position');
  },
  computed: {
  },
  methods: {
    hideScanner() {
      this.isScanning = false;
      cancelAnimationFrame(this.animationFrame);
      video.stop();

    },
    async showScanner() {
      this.code = null;
      this.isScanning = true;

      var video = document.createElement("video");
      var canvasElement = this.$refs.canvas;
      var canvas = canvasElement.getContext("2d");
      var loadingMessage = this.$refs.loadingMessage;
      var outputContainer = this.$refs.output;
      var outputMessage = this.$refs.outputMessage;
      var outputData = this.$refs.outputData;

      function drawLine(begin, end, color) {
        canvas.beginPath();
        canvas.moveTo(begin.x, begin.y);
        canvas.lineTo(end.x, end.y);
        canvas.lineWidth = 4;
        canvas.strokeStyle = color;
        canvas.stroke();
      }

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
            this.code = code.chunks[0].text;
            this.hideScanner();
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
