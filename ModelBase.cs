using System.Collections.Generic;
using System.Numerics;
using System.Linq;
using System;
using System.IO;
using System.Text;

namespace Geometry
{
    public abstract class ModelBase :
        IVertexPrimitive,
        ITexturedPrimitive,
        IPolyNormalizedPrimitive,
        IDynamicPrimitive
    {
        protected Vector3[] _vertices;
        protected Vector3[] _globalVertices;
        protected Vector3[] _normals;
        protected Vector2[] _textureCoords;
        protected int[] _indexes;
        protected int[] _normalsIndexes;
        protected int[] _textureIndexes;

        public ModelBase()
        {
            _indexes = new int[] { };
            _vertices = new Vector3[] { };
            _globalVertices = new Vector3[] { };
            _textureIndexes = new int[] { };
            _textureCoords = new Vector2[] { };
            _normals = new Vector3[] { };
            _normalsIndexes = new int[] { };
        }

        public ModelBase(
            Vector3[] vertices,
            Vector3[] normals,
            Vector2[] texCoords,
            int[] indexes,
            int[] normalsIndexes,
            int[] textureIndexes,
            Pivot pivot,
            ITexture texture)
        {
            if(vertices == null) throw new ArgumentException(nameof(vertices));
            if(indexes == null) throw new ArgumentException(nameof(indexes));
            if(texCoords == null) throw new ArgumentException(nameof(texCoords));
            if(textureIndexes == null) throw new ArgumentException(nameof(textureIndexes));
            if(normals == null) throw new ArgumentException(nameof(normals));
            if(normalsIndexes == null) throw new ArgumentException(nameof(normalsIndexes));
            if(pivot == null) throw new ArgumentException(nameof(pivot));
            if(texture == null) throw new ArgumentException(nameof(texture));

            if(indexes.Length != normalsIndexes.Length || indexes.Length != textureIndexes.Length)
                throw new ArgumentException("Indexes lenghts should be common.");

            ValidateNoIndexOutOfRange(vertices, indexes);
            ValidateNoIndexOutOfRange(normals, normalsIndexes);
            ValidateNoIndexOutOfRange(texCoords, textureIndexes);

            _vertices = vertices;
            _normals = normals;
            _textureCoords = texCoords;
            _indexes = indexes;
            _textureIndexes = textureIndexes;
            _normalsIndexes = normalsIndexes;
            Pivot = pivot;
            Texture = texture;
            
            UpdateGlobalVertices();
        }

        public ModelBase(Vector3 center, Vector3[] vertices, int[] indexes)
        {
            if (vertices == null) throw new ArgumentException(nameof(vertices));
            if (indexes == null) throw new ArgumentException(nameof(indexes));

            ValidateNoIndexOutOfRange(vertices, indexes);

            _vertices = vertices;
            _indexes = indexes;

            Pivot = Pivot.BasePivot(center);

            UpdateGlobalVertices();
            CreateModelNormals();
        }

        public int PolygonsCount { get; }

        public Pivot Pivot { get; protected set; }

        public IEnumerable<Vector3> Normals => _normals.Select(n => n);


        public IEnumerable<int> NormalsIndexes => _normalsIndexes.Select(i => i);


        public int NormalsCount => _normals.Length;


        public int NormalsIndexesCount => _normalsIndexes.Length;


        public IEnumerable<Vector3> Vertices => _vertices.Select(v => v);


        public IEnumerable<Vector3> GlobalVertices => _globalVertices.Select(v => v);


        public IEnumerable<int> Indexes => _indexes.Select(i => i);


        public int VerticesCount => _vertices.Length;


        public int IndexesCount => _indexes.Length;


        public ITexture Texture { get; }


        public IEnumerable<Vector2> TextureCoords => _textureCoords.Select(t => t);


        public IEnumerable<int> TextureIndexes => _textureIndexes.Select(t => t);


        public void Move(Vector3 vector)
        {
            for (int i = 0; i < VerticesCount; i++)
            {
                _vertices[i] += vector;
                _globalVertices[i] += vector;
            }

            Pivot.Move(vector);
        }

        public void Rotate(float angle, AxisType axisType)
        {
            Pivot.Rotate(angle, axisType);

            for (int i = 0; i < VerticesCount; i++)
                _globalVertices[i] = Pivot.ToGlobalCoords(_vertices[i]);
        }

        public void Scale(float coef)
        {
            for (int i = 0; i < VerticesCount; i++)
            {
                _vertices[i] *= coef;
                _globalVertices[i] = Pivot.ToGlobalCoords(_vertices[i]);
            }
        }

        protected void UpdateGlobalVertices()
        {
            _globalVertices = new Vector3[_vertices.Length];
            for (int i = 0; i < _vertices.Length; i++)
            {
                _globalVertices[i] = Pivot.ToGlobalCoords(_vertices[i]);
            }
        }

        public IEnumerable<Poly> GetPolys()
        {
            for (int i = 0; i < _indexes.Length; i+=3)
            {
                yield return new Poly(
                    _vertices[_indexes[i]],
                    _vertices[_indexes[i + 1]],
                    _vertices[_indexes[i + 2]]);
            }   
        }

        public void SaveToObjFile(string filePath)
        {
            if (!filePath.EndsWith(".obj"))
                throw new ArgumentException($"File path {filePath} must end with .obj");

            var writer = new StreamWriter(File.Open(filePath, FileMode.Create));

            WriteVertices(_vertices, "v", writer);
            if(_normals != null) WriteVertices(_normals, "vn", writer);
            if (_textureCoords != null)  WriteVertices(_textureCoords, "vt", writer);

            if(_normalsIndexes == null || _normalsIndexes.Length == 0)
            {
                if(_textureIndexes == null || _textureIndexes.Length == 0) 
                    WriteIndexesVertices(writer);
                else WriteIndexesTexture(writer);
            }
            else
            {
                if (_textureIndexes == null || _textureIndexes.Length == 0) 
                    WriteIndexesNormals(writer);
                else WriteIndexesFull(writer);
            }

            writer.Close();
        }

        protected void WriteVertices(Vector3[] vertices, string prefics, StreamWriter writer)
        {
            foreach (var v in vertices)
            {
                writer.WriteLine($"{prefics} {v.Print()}");
            }
        }

        protected void WriteVertices(Vector2[] vertices, string prefics, StreamWriter writer)
        {
            foreach (var v in vertices)
            {
                writer.WriteLine($"{prefics} {v.Print()}");
            }
        }

        protected void WriteIndexesFull(StreamWriter writer)
        {
            for (int i = 0; i < _indexes.Length; i += 3)
            {
                var sb = new StringBuilder("f ");
                sb.Append($"{_indexes[i] + 1}/{_textureIndexes[i] + 1}/{_normalsIndexes[i] + 1} ");
                sb.Append($"{_indexes[i + 1] + 1}/{_textureIndexes[i + 1] + 1}/{_normalsIndexes[i + 1] + 1} ");
                sb.Append($"{_indexes[i + 2] + 1}/{_textureIndexes[i + 2] + 1}/{_normalsIndexes[i + 2] + 1}");
                writer.WriteLine(sb.ToString());
            }
        }

        protected void WriteIndexesNormals(StreamWriter writer)
        {
            for (int i = 0; i < _indexes.Length; i += 3)
            {
                var sb = new StringBuilder("f ");
                sb.Append($"{_indexes[i] + 1}//{_normalsIndexes[i] + 1} ");
                sb.Append($"{_indexes[i + 1] + 1}//{_normalsIndexes[i + 1] + 1} ");
                sb.Append($"{_indexes[i + 2] + 1}//{_normalsIndexes[i + 2] + 1}");
                writer.WriteLine(sb.ToString());
            }
        }

        protected void WriteIndexesTexture(StreamWriter writer)
        {
            for (int i = 0; i < _indexes.Length; i += 3)
            {
                var sb = new StringBuilder("f ");
                sb.Append($"{_indexes[i] + 1}/{_textureIndexes[i] + 1} ");
                sb.Append($"{_indexes[i + 1] + 1}/{_textureIndexes[i + 1] + 1} ");
                sb.Append($"{_indexes[i + 2] + 1}/{_textureIndexes[i + 2] + 1}");
                writer.WriteLine(sb.ToString());
            }
        }

        protected void WriteIndexesVertices(StreamWriter writer)
        {
            for (int i = 0; i < _indexes.Length; i += 3)
            {
                var sb = new StringBuilder("f ");
                sb.Append($"{_indexes[i] + 1} ");
                sb.Append($"{_indexes[i + 1] + 1} ");
                sb.Append($"{_indexes[i + 2] + 1} ");
                writer.WriteLine(sb.ToString());
            }
        }
        
        public void CreateModelNormals()
        {
            var normal_index = new Dictionary<Vector3, int>();
            var normals = new List<Vector3>();
            var indexes = new List<int>();

            foreach (var poly in GetPolys())
            {
                var normal = VectorMath.GetNormal(poly);

                if(float.IsNaN(normal.X * normal.Y * normal.Z)) normal = Vector3.Zero;

                if(!normal_index.TryGetValue(normal, out var index))
                {
                    normals.Add(normal);
                    normal_index[normal] = index = normals.Count - 1;
                }

                indexes.AddRange(new[] { index, index, index });
            }

            _normals = normals.ToArray();
            _normalsIndexes = indexes.ToArray();
        }

        protected void ValidateNoIndexOutOfRange(Array vertices, int[] indexes)
        {
            if (indexes.Length % 3 != 0) throw new ArgumentException("Invalid indexes size");

            var unreachable = indexes.Cast<int?>()
                .FirstOrDefault(i => i >= vertices.Length);

            if (unreachable != null)
                throw new ArgumentException($"Unreachable vertex occured at index {unreachable}");
        }
    }
}
