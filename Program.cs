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
            //Creating cube (YEA, this is the best thing i've ever done)
            var cube = ModelTools.MakeModelFromPolys(Vector3.Zero, 
                new CubeGenerator(Vector3.Zero, 100).Generate().ToList());
            cube.SaveToObjFile("./Models/Cube.obj");

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

            //Creating serpinsky pyramid
            IRecursiveHandler handler = new SerpinskyPyramidRecursiveHandler(Vector3.Zero, 100, 100, 0);
            var pyrGen = new SerpinskyGenerator(handler, 5);
            polys = pyrGen.Generate().ToList();
            model = ModelTools.MakeModelFromPolys(Vector3.Zero, polys);
            model.SaveToObjFile("./Models/SP.obj");

            //Creating serpinsky cube
            handler = new SerpinskyCubeRecursiveHandler(Vector3.Zero, 100, 0);
            var cubeGen = new SerpinskyGenerator(handler, 5);
            polys = cubeGen.Generate().ToList();
            model = ModelTools.MakeModelFromPolys(Vector3.Zero, polys);
            model.SaveToObjFile("./Models/SC.obj");
        }
    }
}
