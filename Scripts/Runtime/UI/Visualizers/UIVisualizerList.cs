namespace JvLib.UI.Visualizers
{
    /// <typeparam name="C">List Context Type</typeparam>
    /// <typeparam name="E">Entry Context Type</typeparam>
    public abstract class UIVisualizerList<C, E> : UIVisualizer<C> 
        where C : IVisualizerListContext<E>
    {
        protected override void OnContextUpdate(C pContext)
        {
            OnPopulateList(pContext);
        }

        protected abstract void OnPopulateList(C pContext);
    }
}

