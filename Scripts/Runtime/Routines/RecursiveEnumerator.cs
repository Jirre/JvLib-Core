using System;
using System.Collections;
using System.Collections.Generic;

namespace JvLib.Routines
{
    public struct RecursiveEnumerator : IEnumerator
    {
        private IEnumerator _enumerator;
        private Stack<IEnumerator> _stack;

        public RecursiveEnumerator(IEnumerator enumerator)
        {
            this._enumerator = enumerator;
            _stack = null;
            Current = null;
        }

        void IEnumerator.Reset()
        {
            throw new NotSupportedException();
        }

        public object Current { get; private set; }

        public bool MoveNext()
        {
            while (_enumerator != null)
            {
                while (_enumerator.MoveNext())
                {
                    Current = _enumerator.Current;
                    if (Current is IEnumerator e)
                    {
                        _stack ??= new Stack<IEnumerator>(4);
                        _stack.Push(_enumerator);
                        _enumerator = e;
                    }
                    else
                    {
                        return true;
                    }
                }
                if (_stack != null && _stack.Count > 0)
                    _enumerator = _stack.Pop();
                else
                    _enumerator = null;
            }
            return false;
        }
    }
}
