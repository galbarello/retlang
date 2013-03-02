using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Retlang.Core;

namespace Retlang.Fibers
{
    ///<summary>
    /// Allows interaction with an IExecutionContext. Useful for interacting with an outside
    /// threading model, such as that of a UI toolkit.
    ///</summary>
    public abstract class ContextFiber : BaseFiber
    {
        private readonly object _lock = new object();
        private readonly IExecutionContext _executionContext;
        private readonly IExecutor _executor;
        private readonly List<Action> _queue = new List<Action>();

        public ContextFiber(Action<Action> enqueue, IExecutor executor)
            : this(new ExecutionContext(enqueue), executor)
        {

        }

        public ContextFiber(IExecutionContext executionContext, IExecutor executor)
        {
            _executionContext = executionContext;
            _executor = executor;
        }

        /// <summary>
        /// <see cref="IFiber.Start()"/>
        /// </summary>
        public override void Start()
        {
            lock (_lock)
            {
                base.Start();

                var actions = _queue.ToList();
                _queue.Clear();
                if (actions.Count > 0)
                {
                    _executionContext.Enqueue(() => _executor.Execute(actions));
                }
            }
        }

        /// <summary>
        /// Enqueue a single action.
        /// </summary>
        /// <param name="action"></param>
        public override void Enqueue(Action action)
        {
            if (_state == ExecutionState.Stopped)
            {
                return;
            }

            switch (_state)
            {
                case ExecutionState.Created:
                    lock (_lock)
                    {
                        if (_state == ExecutionState.Created)
                        {
                            _queue.Add(action);
                        }
                        else
                        {
                            Enqueue(action);
                        }
                    }
                    break;
                default:
                    _executionContext.Enqueue(() => _executor.Execute(action));
                    break;
            }
        }
    }
}
