using System.Collections.Generic;

namespace Geometry
{
    public abstract class RecursiveGeneratorBase : IGenerator
    {
        public abstract IRecursiveHandler RecursiveHandler { get; }
        public int RecursiveMaxDepth { get; private set; }
        public bool GenerateOnlyLeaf { get; }

        public RecursiveGeneratorBase(int recursiveMaxDepth, bool generateOnlyLeaf = true)
        {
            RecursiveMaxDepth = recursiveMaxDepth;
            GenerateOnlyLeaf = generateOnlyLeaf;
        }

        public IEnumerable<Poly> Generate()
        {
            return PolysProvider();
        }

        public virtual IEnumerable<Poly> PolysProvider()
        {
            var stack = new Stack<IRecursiveHandler>();
            stack.Push(RecursiveHandler);

            while (stack.Count != 0)
            {
                var currentHandler = stack.Pop();
                
                if(currentHandler.RecursiveDepth == RecursiveMaxDepth || !GenerateOnlyLeaf)
                {
                    foreach (var poly in currentHandler.PolysProvider())
                        yield return poly;

                    if(GenerateOnlyLeaf) continue;
                }

                foreach (var handler in currentHandler.HandlersProvider())
                    stack.Push(handler);
            }
        }
    }
}