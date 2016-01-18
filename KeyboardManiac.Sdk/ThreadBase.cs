using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using log4net;

namespace KeyboardManiac.Sdk
{
    abstract public class ThreadBase : DisposableBase
    {
        /// <summary>
        /// Raised when the thread starts.
        /// </summary>
        public event EventHandler Started;
        /// <summary>
        /// Raised when the thread stops.
        /// </summary>
        public event EventHandler Stopped;

        private static readonly ILog Logger = LogManager.GetLogger(typeof(ThreadBase));
        private readonly ManualResetEvent m_StoppingEvent = new ManualResetEvent(false);
        private readonly Thread m_Thread;

        protected ThreadBase()
        {
            m_Thread = new Thread(ThreadStart);
        }

        public bool IsAlive { get { return m_Thread.IsAlive; } }
        public bool IsBackground { get { return m_Thread.IsBackground; } set { m_Thread.IsBackground = value; } }
        protected bool IsStopping { get { return m_StoppingEvent.WaitOne(0); } }
        public int ManagedThreadId { get { return m_Thread.ManagedThreadId; } }
        public string Name { get { return m_Thread.Name; } set { m_Thread.Name = value; } }

        public void Start()
        {
            m_Thread.Start();
        }

        private void ThreadStart()
        {
            Logger.Debug("Thread started");
            OnStarted(new EventArgs());
            try
            {
                InnerStart();
            }
            catch (ThreadAbortException)
            {
                Logger.Debug("Thread aborted");
                Thread.ResetAbort();
            }
            catch (Exception ex)
            {
                Logger.Error("Thread terminated by exception", ex);
            }
            Logger.Debug("Thread stopped");
            OnStopped(new EventArgs());
        }

        abstract protected void InnerStart();

        public bool Join(int millisecondsTimeout, bool signal = false, bool abort = false)
        {
            if (signal)
            {
                PreJoin();
            }

            bool result = m_Thread.Join(millisecondsTimeout);
            if (!result && abort)
            {
                m_Thread.Abort();
                result = m_Thread.Join(millisecondsTimeout);
            }
            return result;
        }

        public void Join(bool signal = false)
        {
            if (signal)
            {
                PreJoin();
            }
            m_Thread.Join();
        }

        private void PreJoin()
        {
            m_StoppingEvent.Set();
            DoPreJoin();
        }

        virtual protected void DoPreJoin()
        {
        }

        protected override void DisposeManagedResources()
        {
            base.DisposeManagedResources();
            
            m_StoppingEvent.Dispose();
        }

        /// <summary>
        /// Raises the <see cref="Started"/> event.
        /// </summary>
        /// <param name="e">The arguments to raise the event with.</param>
        virtual protected void OnStarted(EventArgs e)
        {
            if (Started != null) Started(this, e);
        }

        /// <summary>
        /// Raises the <see cref="Stopped"/> event.
        /// </summary>
        /// <param name="e">The arguments to raise the event with.</param>
        virtual protected void OnStopped(EventArgs e)
        {
            if (Stopped != null) Stopped(this, e);
        }
    }
}
