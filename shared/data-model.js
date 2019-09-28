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
      y: Number,
      operation_ids: [], // array of ints 
    }],
    edges: [{
      from: Number,
      to: Number,
      type: String, // enum('Road', 'Elevator', 'Ladder', 'Footway)
      weight: Number
    }]
  },
  equipment: {
    parts: [{
      id: Number,
      car_id: Number,
      name: String,
      weight: Number,
      assembly_id: Number,
      path: [{
        node_id: Number,
        processing_time: Number // sec
      }]
    }],
    operations: [{
        id: Number,
        name: String,
        processing_time: Number
    }]
  }
};
