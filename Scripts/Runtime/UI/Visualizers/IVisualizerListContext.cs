using System.Collections.Generic;

namespace JvLib.UI.Visualizers
{
    public interface IVisualizerListContext<C>
    {
        List<C> ListEntries { get; }
    }
}
