using System.Collections.Generic;
using System.Linq;
using System;
using System.Numerics;

namespace Geometry
{
    public abstract class CircuitNodeBase : ICircuitNode
    {
        private HashSet<ICircuitNode> _connectedNodes;
        public IEnumerable<ICircuitNode> ConectedNodes =>
            _connectedNodes.Select(n => n);

        public IEnumerable<Vector3> Vertices => VerticesProvider();

        public CircuitNodeBase()
        {
            _connectedNodes = new HashSet<ICircuitNode>();
        }

        public void ConnectToNode(ICircuitNode other)
        {
            if(_connectedNodes.Contains(other))
                throw new InvalidOperationException("Node aready connected.");

            _connectedNodes.Add(other);
        }

        public void DisconnectNode(ICircuitNode other)
        {
            if(!_connectedNodes.Contains(other))
                throw new InvalidOperationException("Node doesn't connected.");

            _connectedNodes.Remove(other);
        }

        public IEnumerable<Poly> GetConnectionPolys(ICircuitNode other)
        {
            return ConnectionPolysProvider(other);
        }

        public IEnumerable<Poly> DeployCircuitGraphConnections()
        {
            var queue = new Queue<Tuple<ICircuitNode, ICircuitNode>>();
            var visisted = new HashSet<ICircuitNode>();

            foreach (var connected in ConectedNodes)
                queue.Enqueue(Tuple.Create((ICircuitNode)this, connected));

            visisted.Add((ICircuitNode)this);

            while(queue.Count != 0)
            {
                var current = queue.Dequeue();

                var from = current.Item1;
                var to = current.Item2;

                if(visisted.Contains(to)) continue;

                foreach(var poly in from.GetConnectionPolys(to))
                    yield return poly;

                foreach(var connected in to.ConectedNodes)
                    queue.Enqueue(Tuple.Create(to, connected));

                visisted.Add(to);
            }
        }

        protected virtual IEnumerable<Poly> ConnectionPolysProvider(ICircuitNode other)
        {
            //Default connection provider
            var verticesFirst = Vertices.ToArray();
            var verticesSecond = other.Vertices.ToArray();

            if(verticesFirst.Length == verticesSecond.Length)
                return ConnectionHelper(verticesFirst, verticesSecond, 0, 0, verticesFirst.Length);

            //Swaping that second always should be longer than first
            if(verticesFirst.Length > verticesSecond.Length)
            {
                var temp = verticesFirst;
                verticesFirst = verticesSecond;
                verticesSecond = temp;
            }

            IEnumerable<Poly> setOfPolys;

            //Divide in two groups - symmetric connected and side groups 
            var diff = verticesSecond.Length - verticesFirst.Length;
            var halfDiff = diff / 2;

            //Connecting side groups 
            var firstSet = diff % 2 == 0 ?
                verticesFirst.Take(1).Concat(verticesSecond.Take(halfDiff + 1))
                                    : 
                verticesFirst.Take(1).Concat(verticesSecond.Take(halfDiff + 2));

            setOfPolys =  VectorMath.TriangulateOrderedVertexSet(firstSet);

            var secondSet = diff == 1 ?
                (IEnumerable<Vector3>) new Vector3[]{}
                                    :
                verticesFirst.TakeLast(1).Concat(verticesSecond.TakeLast(halfDiff + 1));

            setOfPolys = setOfPolys.Concat(VectorMath.TriangulateOrderedVertexSet(secondSet));

            //Connecting center vertices 
            var count = diff == 1 ? verticesFirst.Length - 1 : verticesFirst.Length - 2;
            var start = diff % 2 == 0 ? halfDiff : halfDiff + 1;
            var centerConnection = ConnectionHelper(verticesFirst, verticesSecond,
                1, start, count);

            return setOfPolys.Concat(centerConnection);
        }


        private IEnumerable<Poly> ConnectionHelper(Vector3[] verticesFirst, Vector3[] verticesSecond,
            int startFirst, int startSecond, int count)
        {
            for (int i = 0; i < count - 1; i++)
            {
                yield return new Poly(verticesFirst[i + startFirst], verticesSecond[i + startSecond], verticesSecond[i + 1 + startSecond]);
                yield return new Poly(verticesFirst[i + startFirst], verticesFirst[i + 1 + startFirst], verticesSecond[i + 1 + startSecond]);
            }

            yield return new Poly(verticesFirst.Last(), verticesSecond.Last(), verticesSecond.First());
            yield return new Poly(verticesFirst.Last(), verticesFirst.First(), verticesSecond.First());
        }

        protected abstract IEnumerable<Vector3> VerticesProvider();
    }
}