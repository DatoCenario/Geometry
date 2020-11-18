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
            var tree = TreeGenerator.GenerateTree(Vector3.Zero, 20, 100, 20000, 30);
            tree.SaveToObjFile("./Models/tree2.obj");

            var rand = new Random();
            var randomPoints = Enumerable.Range(0, 1000)
                .Select(p => new Vector3(rand.Next(0, 1000),rand.Next(0, 1000),rand.Next(0, 1000)))
                .ToArray();

            var convexHull = ModelTools.GetModelConvexHull(randomPoints);

            convexHull.SaveToObjFile("./Models/convex.obj");

            var funct = new Function(Vector3.Zero, (a,b) => 
                (float)Math.Sin(10 * (a * b) / (a + b)) * 2, 0, 0, 10, 10, 0.1f);
            funct.SaveToObjFile("./Models/function.obj");
            var thor = new Thor(Vector3.Zero, 100, 40, 2000, 600);
            thor.SaveToObjFile("./Models/thor.obj");
            var cube = new Cube(Vector3.Zero, 100);
            cube.SaveToObjFile("./Models/model.obj");
            Console.Read();
        }
    }
}
