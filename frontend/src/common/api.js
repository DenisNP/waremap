// const ENDPOINT = 'https://ice-break.herokuapp.com';
const ENDPOINT = 'https://waremap.justanother.app';
// const ENDPOINT = 'http://localhost:5000';

function postJson(url = '', data = {}) {
  // Default options are marked with *
  return fetch(url, {
    method: 'POST', // *GET, POST, PUT, DELETE, etc.
    mode: 'cors', // no-cors, cors, *same-origin
    cache: 'no-cache', // *default, no-cache, reload, force-cache, only-if-cached
    credentials: 'same-origin', // include, *same-origin, omit
    headers: {
      'Content-Type': 'application/json',
      // 'Content-Type': 'application/x-www-form-urlencoded',
    },
    redirect: 'follow', // manual, *follow, error
    referrer: 'no-referrer', // no-referrer, *client
    body: JSON.stringify(data), // body data type must match "Content-Type" header
  });
}

function postData(url = '', data = '') {
  // Default options are marked with *
  return fetch(url, {
    method: 'POST', // *GET, POST, PUT, DELETE, etc.
    mode: 'cors', // no-cors, cors, *same-origin
    cache: 'no-cache', // *default, no-cache, reload, force-cache, only-if-cached
    credentials: 'same-origin', // include, *same-origin, omit
    headers: {
      'Content-Type': 'text/plain',
    },
    redirect: 'follow', // manual, *follow, error
    referrer: 'no-referrer', // no-referrer, *client
    body: data, // body data type must match "Content-Type" header
  });
}

function getData(url = '') {
  // Default options are marked with *
  return fetch(url, {
    method: 'GET', // *GET, POST, PUT, DELETE, etc.
    mode: 'cors', // no-cors, cors, *same-origin
    cache: 'no-cache', // *default, no-cache, reload, force-cache, only-if-cached
    credentials: 'same-origin', // include, *same-origin, omit
    headers: {
      'Content-Type': 'application/json',
      // 'Content-Type': 'application/x-www-form-urlencoded',
    },
    redirect: 'follow', // manual, *follow, error
    referrer: 'no-referrer', // no-referrer, *client
  });
}

export default {
  getData,

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

  async updateNode({id, type, x, y, floor, depot}) {
    return await this.sendAction('addNode', {
      id, type, x, y, floor, depot
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

  async autoComputeEdges(depotId) {
    return await this.sendAction('computeEdges', {depotId});
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
