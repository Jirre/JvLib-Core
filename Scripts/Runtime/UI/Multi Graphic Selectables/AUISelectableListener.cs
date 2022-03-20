using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JvLib.UI
{
    public abstract class AUISelectableTarget : MonoBehaviour
    {
        internal abstract void Transition(AUISelectable.SelectionState pState, bool pInstant);
        internal abstract void Clear();
    }
}