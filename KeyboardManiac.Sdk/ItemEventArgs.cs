using System;

namespace KeyboardManiac.Sdk
{
    /// <summary>
    /// The generic item event argument.
    /// </summary>
    /// <typeparam name="T">The type of the item the arguments are concerning.</typeparam>
    public class ItemEventArgs<T> : EventArgs
    {
        private readonly T m_Item;

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemEventArgs"/> class.
        /// </summary>
        /// <param name="item">The item the event arguments are concerning.</param>
        public ItemEventArgs(T item)
        {
            m_Item = item;
        }

        /// <summary>
        /// Gets the item the event arguments are concerning.
        /// </summary>
        public T Item { get { return m_Item; } }
    }
}
