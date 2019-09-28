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
      icon: String, // Machine, Point, Ladder, Elevator, Door
      depot: Number,
      floor: Number,
      x: Number,
      y: Number,
      operation_ids: [], // array of ints
    }],
    edges: [{
      from: Number,
      to: Number,
      type: String, // enum('Road', 'Elevator', 'Ladder', 'Footway')
      weight: Number
    }]
  },
  equipment: {
    parts: [{
      id: Number,
      name: String,
      weight: Number,
      assembly_id: Number,
      path: [{
        id: Number,
        order: Number,
        operation_id: Number,
        start_time: Number,
        end_time: Number
      }]
    }],
    operations: [{
        id: Number,
        name: String,
        processing_time: Number
    }],
    assemblies: [{
        id: Number,
        name: String
    }],
  }
};
