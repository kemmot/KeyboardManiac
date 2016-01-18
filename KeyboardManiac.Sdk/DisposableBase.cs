using System;

namespace KeyboardManiac.Sdk
{
    /// <summary>
    /// A standard implementation of the <see cref="IDisposable"/> interface
    /// that can be used if no other base class is required.
    /// </summary>
    public class DisposableBase : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DisposableBase"/> class.
        /// </summary>
        protected DisposableBase()
        {
        }

        /// <summary>
        /// Finaliser function.
        /// </summary>
        ~DisposableBase()
        {
            Dispose(false);
        }


        /// <summary>
        /// Gets a value indicating whether this object has been disposed.
        /// </summary>
        protected bool IsDisposed { get; private set; }


        /// <summary>
        /// Throws an exception if this object has been disposed.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Thrown if this object has been disposed.</exception>
        protected void CheckDisposed()
        {
            if (IsDisposed) throw new ObjectDisposedException("The object has been disposed.");
        }

        /// <summary>
        /// Disposes the object.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Cleans up the object.
        /// </summary>
        /// <param name="disposing">If true then called by Dispose function.</param>
        virtual protected void Dispose(bool disposing)
        {
            lock (this)
            {
                if (!IsDisposed)
                {
                    if (disposing)
                    {
                        // Free other state (managed objects).
                        DisposeManagedResources();
                    }

                    // Free your own state (unmanaged objects).
                    // Set large fields to null.
                    DisposeUnmanagedResources();

                    IsDisposed = true;
                }
            }
        }

        /// <summary>
        /// Dispose managed resources.
        /// </summary>
        virtual protected void DisposeManagedResources()
        {
        }

        /// <summary>
        /// Disposes the unmanaged resources.
        /// </summary>
        virtual protected void DisposeUnmanagedResources()
        {
        }
    }
}
