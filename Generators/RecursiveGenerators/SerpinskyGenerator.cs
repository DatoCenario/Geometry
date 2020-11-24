namespace Geometry
{
    public class SerpinskyGenerator : RecursiveGeneratorBase
    {
        public SerpinskyGenerator(IRecursiveHandler recursiveHandler, int recursiveMaxDepth, bool generateOnlyLeaf = true) 
            : base(recursiveMaxDepth, generateOnlyLeaf)
        {
            RecursiveHandler = recursiveHandler;
        }

        public override IRecursiveHandler RecursiveHandler { get; }
    }
}