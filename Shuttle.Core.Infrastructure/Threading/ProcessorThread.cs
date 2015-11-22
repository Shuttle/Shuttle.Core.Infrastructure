using System.Threading;

namespace Shuttle.Core.Infrastructure
{
    public class ProcessorThread : IThreadState
    {
        private readonly ILog _log;
        private readonly string _name;
        private readonly IProcessor _processor;

        private static readonly int ThreadJoinTimeoutInterval =
            ConfigurationItem<int>.ReadSetting("ThreadJoinTimeoutInterval", 1000).GetValue();

        private volatile bool _active;

        private Thread _thread;

        public ProcessorThread(string name, IProcessor processor)
        {
            _name = name;
            _processor = processor;

            _log = Log.For(this);
        }

        public bool Active
        {
            get { return _active; }
        }

        public void Start()
        {
            if (_active)
            {
                return;
            }

            _thread = new Thread(Work) {Name = _name};

            _thread.SetApartmentState(ApartmentState.MTA);
            _thread.IsBackground = true;
            _thread.Priority = ThreadPriority.Normal;

            _active = true;

            _thread.Start();

            if (Log.IsTraceEnabled)
            {
                _log.Trace(string.Format(InfrastructureResources.ProcessorThreadStarting, _thread.ManagedThreadId,
                    _processor.GetType().FullName));
            }

            while (!_thread.IsAlive && _active)
            {
            }

            if (_active && Log.IsTraceEnabled)
            {
                _log.Trace(string.Format(InfrastructureResources.ProcessorThreadActive, _thread.ManagedThreadId,
                    _processor.GetType().FullName));
            }
        }

        public void Stop()
        {
            if (Log.IsTraceEnabled)
            {
                _log.Trace(string.Format(InfrastructureResources.ProcessorThreadStopping, _thread.ManagedThreadId,
                    _processor.GetType().FullName));
            }

            _active = false;

            if (_thread.IsAlive)
            {
                _thread.Join(ThreadJoinTimeoutInterval);
            }
        }

        private void Work()
        {
            while (_active)
            {
                if (Log.IsVerboseEnabled)
                {
                    _log.Verbose(string.Format(InfrastructureResources.ProcessorExecuting, _thread.ManagedThreadId,
                        _processor.GetType().FullName));
                }

                _processor.Execute(this);
            }

            if (Log.IsTraceEnabled)
            {
                _log.Trace(string.Format(InfrastructureResources.ProcessorThreadStopped, _thread.ManagedThreadId,
                    _processor.GetType().FullName));
            }
        }

        internal void Deactivate()
        {
            _active = false;
        }
    }
}