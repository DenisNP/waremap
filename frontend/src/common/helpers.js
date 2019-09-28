module.exports = {
	distance(from, to) {
		return Math.round(Math.sqrt(
			Math.pow(from.x - to.x, 2) +
			Math.pow(from.y - to.y, 2)
		));
	},

  toBase64(file) {
    return new Promise((resolve, reject) => {
      const reader = new FileReader();
      reader.readAsDataURL(file);
      reader.onload = () => resolve(reader.result);
      reader.onerror = error => reject(error);
    });
  }
};
