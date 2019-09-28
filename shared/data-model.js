module.exports = {
  geo: {
    depots: [{
      id: Number,
      name: String,
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
      from_node: Number, // из какого узла прямо сейчас едет деталь
      to_node: Number, // в какой узел прямо сейчас едет деталь, если from_node = to_node, то она сидит в конкретном узлу
      time_left: Number, // сколько ещё времени будет продолжаться процесс над деталью
      time_total: Number, // сколько всего времени занимает текущий процесс над деталью
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
