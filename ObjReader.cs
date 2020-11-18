using System.Numerics;
using System.IO;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;

namespace Geometry
{
    public class ObjReader
    {
        static readonly char[] tokenSymbols = new char[] {'v', 'f'};
        List<Vector3> _vertices;
        List<Vector3> _normals;
        List<Vector2> _textureCoords;
        List<int> _indexes;
        List<int> _normalsIndexes;
        List<int> _textureIndexes;
        StreamReader reader;

        public Model ReadModelFromObjFile(string filePath)
        {
            Initialize(filePath);

            while (!reader.EndOfStream)
            {
                reader.SkipToSymbolsNoExcept(tokenSymbols);

                var type = reader.Read(2);
                try
                {
                    if (type[0] == 'v')
                    {
                        switch (type[1])
                        {
                            case 't':
                                ReadTextureCoord();
                                break;

                            case 'n':
                                ReadNormal();
                                break;

                            default:
                                ReadVertex();
                                break;
                        }
                    }
                    else if (type[0] == 'f')
                    {
                        ReadIndexes();
                    }
                }
                catch(Exception){}
            }

            return new Model(_vertices.ToArray(), _normals.ToArray(), _textureCoords.ToArray(),
                _indexes.ToArray(), _normalsIndexes.ToArray(), _textureIndexes.ToArray(),
                Pivot.BasePivot(Vector3.Zero), null);
        }

        private void Initialize(string filePath)
        {
            reader = new StreamReader(File.OpenRead(filePath));
            _indexes = new List<int>();
            _textureIndexes = new List<int>();
            _normalsIndexes = new List<int>();
            _vertices = new List<Vector3>();
            _normals = new List<Vector3>();
            _textureCoords = new List<Vector2>();
        }

        private Vector2 ReadVector2()
        {
            return new Vector2(reader.ReadFloat32(), reader.ReadFloat32());
        }

        private Vector3 ReadVector3()
        {
            return new Vector3(reader.ReadFloat32(), reader.ReadFloat32(), reader.ReadFloat32());
        }

        private void ReadVertex()
        {
            _vertices.Add(ReadVector3());
        }

        private void ReadNormal()
        {
            _normals.Add(ReadVector3());
        }

        private void ReadTextureCoord()
        {
            _textureCoords.Add(ReadVector2());
        }

        private void ReadIndexes()
        {
            var line = reader.ReadToSymbols(tokenSymbols);
            var sublines = line.Split(' ');

            var indexSets = sublines.Select(sl => sl.Split('/')
                .Select(l => int.Parse(l, CultureInfo.InvariantCulture) - 1).ToArray())
                .ToArray();

            //This sould be refactored

            for (int i = 0; i < indexSets.Length - 1; i++)
            {
                _indexes.Add(indexSets[0][0]);
                if(indexSets[0].Length == 2) _textureIndexes.Add(indexSets[0][1]);
                if(indexSets[0].Length == 3) _normalsIndexes.Add(indexSets[0][2]);

                _indexes.Add(indexSets[i][0]);
                if (indexSets[0].Length == 2) _textureIndexes.Add(indexSets[i][1]);
                if (indexSets[0].Length == 3) _normalsIndexes.Add(indexSets[i][2]);

                _indexes.Add(indexSets[i + 1][0]);
                if (indexSets[0].Length == 2) _textureIndexes.Add(indexSets[i + 1][1]);
                if (indexSets[0].Length == 3) _normalsIndexes.Add(indexSets[i + 1][2]);
            }
        }
    }
}