module.exports = {
  geo: {
    depots: [{
      id: Number,
      floor: Number,
      w: Number,
      h: Number,
      x: Number,
      y: Number
    }],
    nodes: [{
      id: Number,
      type: String, // enum('Machine', 'Point'),
      depot: Number,
      floor: Number,
      x: Number,
      y: Number
    }],
    edges: [{
      from: Number,
      to: Number,
      type: String, // enum('Road', 'Elevator', 'Ladder')
      weight: Number
    }]
  },
  equipment: {
    cars: [{
      id: Number,
      type: String, // enum('Man', 'ManWithCar', 'Forklift')
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
