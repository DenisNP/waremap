using System;
using System.Collections.Generic;
using System.Linq;
using Waremap.Models;

namespace Waremap
{
    public class AntColony
    {
        private const double InitialFood = 1.0;
        private readonly double[,] _food;

        public AntColony(Graph graph)
        {
            _food = new double[graph.Nodes.Count, graph.Nodes.Count];
            for (var i = 0; i < graph.Nodes.Count; i++)
            {
                for (var j = 0; j < graph.Nodes.Count; j++)
                {
                    _food[i, j] = InitialFood;
                }
            }
        }
        
        public void RunAnt(Route route)
        {
            var random = new Random();
            double currentFood = 0.0;
            
            var carNode = route.CarWaypoints.Last().FromNode;
            var potential = GraphUtils.FindNeighbours(route.Graph, carNode, new List<int>());
            var probs = new List<double>();
            foreach (var neighbour in potential)
            {
                var edgeFood = _food[carNode, neighbour];
                probs.Add(edgeFood * route.Graph.DistFromWeight(neighbour, carNode));
            }

            var sumProb = probs.Sum();
            var index = GraphUtils.SelectFromRoulette(probs.Select(p => p / sumProb).ToArray(), random);
            if (index == -1)
            {
                index = random.Next(0, probs.Count);
            }

            var nextNode = potential[index];
            route.Time += AddTime(carNode, nextNode);
            currentFood += TakeInPart(route, nextNode);
            currentFood += TakeOutPart(route, nextNode);
            
            route.CarWaypoints.Add(new Waypoint
            {
                FromNode = carNode,
                ToNode = nextNode,
            });
        }

        private double TakeOutPart(Route route, int node)
        {
            throw new NotImplementedException();
        }

        private double TakeInPart(Route route, int node)
        {
            throw new NotImplementedException();
        }

        private int AddTime(in int carNode, int node)
        {
            throw new NotImplementedException();
        }
    }
}