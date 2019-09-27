module.exports = {
  geo: {
    depots: [{
      id: Number,
      floor: Number,
      name: String,
      width: Number,
    }],
    nodes: [{
      id: Number,
      type: String, // enum('machine', 'point'),
      depot_id: Number,
      x: Number,
      y: Number
    }],
    edges: [{
      from: Number,
      to: Number,
      type: String, // enum(...)
      weight: Number
    }]
  },
  equipment: {
    cars: [{
      id: Number,
      type: String, // enum(...)
      total_capacity: Number,
      free_capacity: Number,
      from_node_id: Number,
      to_node_id: Number,
      progress: Number // ?
    }],
    parts: [{
      id: Number,
      car_id: Number,
      name: String,
      weight: Number,
      path: [{
        node_id: Number,
        processing_time: Number // sec
      }]
    }]
  }
};
