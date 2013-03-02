using System;
using Retlang.Core;

namespace Retlang
{
    public class ExecutionContext : IExecutionContext
    {
        private readonly Action<Action> _enqueue;

        public ExecutionContext(Action<Action> enqueue)
        {
            _enqueue = enqueue;
        }

        public void Enqueue(Action action)
        {
            _enqueue(action);
        }
    }
}

