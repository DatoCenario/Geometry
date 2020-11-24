using System.Collections.Generic;

namespace Geometry
{
    public interface IRecursiveHandler
    {
        IEnumerable<Poly> PolysProvider();
        IEnumerable<IRecursiveHandler> HandlersProvider();
        int RecursiveDepth { get; }
    }
}