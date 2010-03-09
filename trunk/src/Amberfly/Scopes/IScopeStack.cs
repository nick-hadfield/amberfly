using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amberfly.Scopes
{
    /// <summary>
    /// A stack of scopes/contexts.
    /// </summary>
    /// <typeparam name="SCOPE">The type of scopes/contexts to be stored on the stack.</typeparam>
    public interface IScopeStack<SCOPE>
    {
        /// <summary>
        /// Gets the inner most scope.
        /// </summary>
        SCOPE Peek();

        /// <summary>
        /// Adds a scope to the scope stack.
        /// </summary>
        /// <param name="scope"></param>
        void Push(SCOPE scope);

        /// <summary>
        /// Pops a scope from the scope stack.
        /// </summary>
        /// <returns></returns>
        SCOPE Pop();
    }
}
