using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Numerics;

namespace Geometry
{
    class Program
    {
        static void Main(string[] args)
        {
            // var gen = new ThorGenerator(Vector3.Zero, 1000, 200, 1000, 200);
            // var polys = gen.Generate().ToList();
            // var model = ModelTools.MakeModelFromPolys(Vector3.Zero, polys);
            // model.SaveToObjFile("./Models/Thor.obj");
            // var node1 = new CircleCircuitNode(Pivot.BasePivot(Vector3.Zero), 100, 20);
            // var node2 = new CircleCircuitNode(Pivot.BasePivot(new Vector3(0,0,100)), 200, 10);
            // node1.ConnectToNode(node2);
            // var p = node1.DeployCircuitGraphConnections().ToList();
            // var model = ModelTools.MakeModelFromPolys(Vector3.Zero, p);
            // model.SaveToObjFile("./Models/Test.obj");

            // var treesPolys = Enumerable.Range(0, 5)
            //     .SelectMany(t => Enumerable.Range(0, 5)
            //     .SelectMany(p => GetRandomGenerator().Generate()))
            //     .ToList();

            // var model = ModelTools.MakeModelFromPolys(Vector3.Zero, treesPolys);
            // model.Rotate((float)Math.PI / 2, AxisType.XAxis);

            // model.SaveToObjFile("./Models/Forest.obj");

            var gen = new TreeGenerator(70, 100, 40, 15f, 3000, Vector3.Zero);
            var polys = gen.Generate().ToList();
            var treeModel = ModelTools.MakeModelFromPolys(Vector3.Zero, polys);
            treeModel.Rotate((float)Math.PI / 2, AxisType.XAxis);
            treeModel.SaveToObjFile("./Models/myTree.obj");
        }

        static Random rand = new Random();

        static TreeGenerator GetRandomGenerator()
        {
            return new TreeGenerator(70, rand.Next(50, 150), rand.Next(15, 60), 6f, rand.Next(1500, 4000),
                new Vector3(rand.Next(0, 2000), 0, rand.Next(0, 2000)));
        }
    }
}
