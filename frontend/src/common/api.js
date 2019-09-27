// const ENDPOINT = 'https://ice-break.herokuapp.com';
const ENDPOINT = 'https://icebreak.tech';
// const ENDPOINT = 'http://localhost:5000';

function postData(url = '', data = {}) {
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

export default {
  async api(method, data = {}) {
    if (this.sessionId) {
      data.id = this.sessionId;
    }

    const endpoint = window.endpoint || ENDPOINT;

    const res = await postData(`${endpoint}/${method}`, data);

    if (!res.ok || res.status !== 200) {
      throw new Error('fetch fail', res);
    }

    return res.json();
  },

  async sendAction(actionName, data) {
    return this.api('stage', {
      action: actionName,
      ...data
    });
  },
};
