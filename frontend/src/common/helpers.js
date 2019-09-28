module.exports = {
	distance(from, to) {
		return Math.round(Math.sqrt(
			Math.pow(from.x - to.x, 2) +
			Math.pow(from.y - to.y, 2)
		));
	}
};
