using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JvLib.UI.Visualizers
{
    public abstract class UIVisualizerList<C, E> : UIVisualizer<C> 
        where C : IVisualizerListContext<E>
    {
        protected override void OnContextUpdate(C pContext)
        {
            OnPopulateList(pContext);
        }

        protected abstract void OnPopulateList(C pContent);
    }
}

