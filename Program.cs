using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Numerics;

namespace Geometry
{
    class Program
    {
        ///Simple usage
        static void Main(string[] args)
        {
            //Creating thor
            var thorGen = new ThorGenerator(Vector3.Zero, 100, 20, 100, 20);
            var polys = thorGen.Generate().ToList();
            var model = ModelTools.MakeModelFromPolys(Vector3.Zero, polys);
            model.SaveToObjFile("./Models/Thor.obj");

            //Creating tree
            var treeGen = new TreeGenerator(Vector3.Zero, 100, 100, 20, 10f, 2000);
            polys = treeGen.Generate().ToList();
            model = ModelTools.MakeModelFromPolys(Vector3.Zero, polys);
            model.SaveToObjFile("./Models/Tree.obj");
        }
    }
}
