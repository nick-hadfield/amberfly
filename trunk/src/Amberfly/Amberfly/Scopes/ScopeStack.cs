using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;
using System.Web;

namespace Amberfly.Scopes
{
    /// <summary>
    /// A stack of scopes/contexts.
    /// </summary>
    /// <typeparam name="SCOPE">The type of scopes/contexts to be stored on the stack.</typeparam>
    public class ScopeStack<SCOPE> : IScopeStack<SCOPE> where SCOPE : class
    {
        /// <summary>
        /// Don't allow a default constuctor.
        /// </summary>
        protected ScopeStack() { }

        /// <summary>
        /// Initialises the scope stack.
        /// </summary>
        /// <param name="key">Identifies which stack to retrieve.</param>
        public ScopeStack(string key)
        {
            _key = typeof(SCOPE).GUID.ToString() + key;
        }

        /// <summary>
        /// Identifies which stack we wish to retrieve.
        /// </summary>
        private string _key;

        private static readonly Object _lock = new Object();

        [ThreadStatic]
        private static IDictionary<string, Stack<SCOPE>> _stacks;

        /// <summary>
        /// Gets the current stack.
        /// </summary>
        /// <value>The current stack.</value>
        protected Stack<SCOPE> Stack
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                HttpContext current = HttpContext.Current;
                if (current == null)
                {
                    // ThreadScopeStack //
                    lock (_lock)
                    {
                        if (_stacks == null)
                        {
                            _stacks = new Dictionary<string, Stack<SCOPE>>();
                        }
                        if (!_stacks.ContainsKey(_key))
                        {
                            _stacks.Add(_key, new Stack<SCOPE>());
                        }
                        return _stacks[_key];
                    }
                }

                // WebScopeStack //
                IDictionary<string, Stack<SCOPE>> stacks = (IDictionary<string, Stack<SCOPE>>)current.Items[typeof(IDictionary<string, Stack<SCOPE>>).ToString()];
                if (null == stacks)
                {
                    stacks = new Dictionary<string, Stack<SCOPE>>();
                    current.Items[typeof(IDictionary<string, Stack<SCOPE>>).ToString()] = stacks;
                }
                if (!stacks.ContainsKey(_key))
                {
                    stacks.Add(_key, new Stack<SCOPE>());
                }
                return stacks[_key];
            }
        }

        /// <summary>
        /// Gets the inner most scope.
        /// </summary>
        public SCOPE Peek()
        {
            if (0 == Stack.Count) return null;
            return Stack.Peek();
        }

        /// <summary>
        /// Adds a scope to the scope stack.
        /// </summary>
        /// <param name="scope"></param>
        public void Push(SCOPE scope)
        {
            Stack.Push(scope);
        }

        /// <summary>
        /// Pops a scope from the scope stack.
        /// </summary>
        /// <returns></returns>
        public SCOPE Pop()
        {
            return Stack.Pop();
        }
    }
}
