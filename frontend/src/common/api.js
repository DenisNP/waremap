import {postJson, postData, getData} from './helpers';
import config from './config';
const ENDPOINT = config.backendUrl;

export default {
  async api(method, action, data = {}) {
    const endpoint = window.endpoint || ENDPOINT;

    const res = method === 'GET' ?
      await getData(`${endpoint}/${action}`, data) :
      await postJson(`${endpoint}/${action}`, data);


    if (!res.ok || res.status !== 200) {
      throw new Error('fetch fail', res);
    }

    return res.json();
  },

  async getState() {
    return this.api('GET', 'state');
  },

  async sendAction(actionName, data) {
    return this.api('POST', 'state?event=' + actionName, {
      ...data
    });
  },

  async sendBackground(base64, floor) {
    const endpoint = window.endpoint || ENDPOINT;

    const res = await postData(`${endpoint}/background?floor=${floor}`, base64);

    if (!res.ok || res.status !== 200) {
      throw new Error('fetch fail', res);
    }

    return res.text();
  },

  async getBackground(floor) {
    const endpoint = window.endpoint || ENDPOINT;

    const res = await getData(`${endpoint}/background?floor=${floor}`);

    if (!res.ok || res.status !== 200) {
      throw new Error('fetch fail', res);
    }

    return res.text();
  },

  async addNode({type, x, y, floor, depot, icon}) {
    return await this.sendAction('addNode', {
      type, x, y, floor, depot, icon
    });
  },

  async updateNode({id, type, x, y, floor, depot, operation_ids}) {
    return await this.sendAction('addNode', {
      id, type, x, y, floor, depot, operation_ids
    });
  },

  async removeNode(id) {
    return await this.sendAction('removeNode', {id});
  },

  async addOrUpdateEdge({type, weight, from, to}) {
    return await this.sendAction('addEdge', {
      type, weight, from, to
    });
  },

  async removeEdge({from, to}) {
    return await this.sendAction('removeEdge', {from, to});
  },

  async autoComputeEdges(depotId, floorId) {
    return await this.sendAction('computeEdges', {depotId, floor});
  },

  async addDepot({x, y, w, h, floor, name}) {
    return await this.sendAction('addDepot', {
      x, y, w, h, floor, name
    });
  },

  async updateDepot({id, x, y, w, h, floor, name}) {
    return await this.sendAction('addDepot', {
      id, x, y, w, h, floor, name
    });
  },

  async removeDepot(id) {
    return await this.sendAction('removeDepot', {id});
  },
};
